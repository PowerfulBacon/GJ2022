using GJ2022.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    /// <summary>
    /// Handles claiming of entities in a thread safe manner
    /// </summary>
    public static class ThreadSafeClaimManager
    {

        private static object lockObject = new object();

        private static Dictionary<Entity, Entity> claims = new Dictionary<Entity, Entity>();

        private static int totalActionsReserved = 0;
        private static int currentAction = 1;

        public static bool HasClaim(Entity entity)
        {
            return claims.ContainsKey(entity);
        }

        public static Entity GetClaimedItem(Entity entity)
        {
            return HasClaim(entity) ? claims[entity] : null;
        }

        public static void ReleaseClaimBlocking(Entity entity)
        {
            int queueId = GetQueueId();
            while (!IsReady(queueId)) { Thread.Yield(); }
            if (claims.ContainsKey(entity))
            {
                claims[entity].IsClaimed = false;
                claims.Remove(entity);
            }
            Complete();
        }

        public static bool ReserveClaimBlocking(Entity entity, Entity target)
        {
            int queueId = GetQueueId();
            int sanity = 0;
            while (!IsReady(queueId))
            {
                if (sanity++ > 1000)
                {
                    Log.WriteLine($"Reserve claim ID : {queueId} has been waiting for {sanity} ticks without success.");
                    throw new Exception("");
                }
                Thread.Yield();
            }
            //Unclaim existing
            if (claims.ContainsKey(entity))
            {
                claims[entity].IsClaimed = false;
                claims.Remove(entity);
            }
            //Someone claimed the target before us.
            if (target.IsClaimed)
            {
                Log.WriteLine("Failed to reserve");
                Complete();
                return false;
            }
            //Claim new
            claims.Add(entity, target);
            target.IsClaimed = true;
            //Complete action
            Complete();
            //We claimed the target
            return true;
        }

        private static int GetQueueId()
        {
            lock (lockObject)
            {
                return Interlocked.Increment(ref totalActionsReserved);
            }
        }

        private static bool IsReady(int id)
        {
            if (currentAction > id)
            {
                throw new Exception("Thread lock detected in claim manager");
            }
            return id == currentAction;
        }

        private static void Complete()
        {
            Interlocked.Increment(ref currentAction);
        }

    }
}
