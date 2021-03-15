using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;
            int last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }
        
        public static T GetRandom<T>(this List<T> list, int fromIndex = 0)
        {
            T result = default;

            int length = list.Count;

            return fromIndex < length - 1 ? list[Random.Range(fromIndex, length)] : result;
        }

        public static T GetRandom<T>(this T[] array, int fromIndex = 0)
        {
            T result = default;

            int length = array.Length;

            if (length == 1)
            {
                return array[0];
            }

            return fromIndex < length - 1 ? array[Random.Range(fromIndex, length)] : result;
        }
    }
}