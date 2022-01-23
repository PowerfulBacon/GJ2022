using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.GlobalDataComponents
{
    /// <summary>
    /// Could be optimised using a binary insertion, but this is fine since hooks aren't added and removed often.
    /// </summary>
    static class MouseHookTracker
    {

        public const int BUILDMODE_PRIORITY = 100;

        private static string ActiveHook;

        private static Dictionary<string, int> MouseHooks = new Dictionary<string, int>();

        public static void AddHook(string key, int priority)
        {
            MouseHooks.Add(key, priority);
            CheckHooks();
        }

        public static void RemoveHook(string key)
        {
            MouseHooks.Remove(key);
            CheckHooks();
        }

        private static void CheckHooks()
        {
            if (MouseHooks.Count == 0)
            {
                ActiveHook = string.Empty;
                return;
            }
            //Set default
            int bestPriority = MouseHooks.Values.ElementAt(0);
            ActiveHook = MouseHooks.Keys.ElementAt(0);
            //Check
            for (int i = 1; i < MouseHooks.Count; i++)
            {
                if (MouseHooks.Values.ElementAt(i) > bestPriority)
                {
                    bestPriority = MouseHooks.Values.ElementAt(i);
                    ActiveHook = MouseHooks.Keys.ElementAt(i);
                }
            }
        }

        /// <summary>
        /// Returns true if the specified key hook has mouse control.
        /// </summary>
        public static bool HasMouseControl(string key)
        {
            return ActiveHook == string.Empty || ActiveHook == key;
        }

    }
}
