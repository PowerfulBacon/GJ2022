using GJ2022.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Managers
{
    /// <summary>
    /// Handles claiming of entities in a thread safe manner
    /// </summary>
    public static class ThreadSafeClaimManager
    {

        private static Dictionary<Entity, Entity> claims = new Dictionary<Entity, Entity>();

        private static volatile bool executing = false;

        private static volatile int totalActionsReserved = 0;
        private static volatile int currentAction = 1;

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
            while (!IsReady(queueId)) { }
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
            while (!IsReady(queueId)) { }
            //Unclaim existing
            if (claims.ContainsKey(entity))
            {
                claims[entity].IsClaimed = false;
                claims.Remove(entity);
            }
            //Someone claimed the target before us.
            if (target.IsClaimed)
            {
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
            return totalActionsReserved++;
        }

        private static bool IsReady(int id)
        {
            return id == currentAction;
        }

        private static void Complete()
        {
            currentAction++;
        }

    }
}
