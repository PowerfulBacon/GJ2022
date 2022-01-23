using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Utility.Helpers
{
    static class ListPicker
    {

        private static Random random = new Random();

        public static T Pick<T>(IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default;
            return list.ElementAt(random.Next(0, list.Count()));
        }

    }
}
