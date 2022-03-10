using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Utility.MathConstructs
{

    internal class BinaryListElement<T>
    {
        public int key;
        public T value;

        public BinaryListElement(int key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public class BinaryList<T>
    {

        public delegate bool BinaryListValidityCheckDelegate(T input);

        internal List<BinaryListElement<T>> binaryListElements = new List<BinaryListElement<T>>();

        public T TakeFirst()
        {
            T taken = binaryListElements.First().value;
            binaryListElements.RemoveAt(0);
            return taken;
        }

        public T First()
        {
            return binaryListElements.First().value;
        }

        public int Length()
        {
            return binaryListElements.Count;
        }

        public int Add(int key, T toAdd, int start = 0, int _end = -1)
        {
            //No elements, just add
            if (binaryListElements.Count == 0)
            {
                binaryListElements.Add(new BinaryListElement<T>(key, toAdd));
                return -1;
            }
            //We need to locate the point which is less than the key, and where the key is less than the next value
            //f(i) < key < f(i + 1)
            int end = _end;
            if (end == -1)
                end = binaryListElements.Count - 1;
            //Get the midpoint
            int midPoint = (start + end) / 2;
            //Midpoint has converted
            if (start >= end)
            {
                //Check if the midpoint is too small or too large
                BinaryListElement<T> convergedPoint = binaryListElements.ElementAt(midPoint);
                if (convergedPoint.key > key)
                    binaryListElements.Insert(midPoint, new BinaryListElement<T>(key, toAdd));
                else
                    binaryListElements.Insert(midPoint + 1, new BinaryListElement<T>(key, toAdd));
                return -1;
            }
            //Locate the element at the midpoint
            BinaryListElement<T> current = binaryListElements.ElementAt(midPoint);
            //Perform next search
            if (current.key > key)
                return Add(key, toAdd, start, Math.Max(midPoint - 1, 0));
            else
                return Add(key, toAdd, midPoint + 1, end);
        }

        public void Remove(int key)
        {
            int elementIndex = IndexOf(key);
            if (elementIndex == -1)
                return;
            binaryListElements.RemoveAt(elementIndex);
        }

        public T _ElementAt(int index)
        {
            return binaryListElements.ElementAt(index).value;
        }

        public T ElementWithKey(int key)
        {
            int index = IndexOf(key);
            if (index == -1)
                return default;
            else
                return binaryListElements.ElementAt(index).value;
        }

        private int IndexOf(int i, int start = 0, int _end = -1)
        {
            if (binaryListElements.Count == 0)
                return -1;
            int end = _end;
            if (end == -1)
                end = binaryListElements.Count - 1;
            //Get the midpoint
            int midPoint = (start + end) / 2;
            //Locate the element at the midpoint
            BinaryListElement<T> located = binaryListElements.ElementAt(midPoint);
            //Perform checks
            if (located.key == i)
                return midPoint;
            //Check if we exhausted the list
            if (start >= end)
                return -1;
            //Perform next search
            if (located.key > i)
                return IndexOf(i, start, Math.Max(midPoint - 1, 0));
            else
                return IndexOf(i, midPoint + 1, end);
        }

        public bool ElementsInRange(int min, int max, int start = 0, int _end = -1, BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            //Check the X-Axis
            if (binaryListElements.Count == 0)
                return false;
            int end = _end;
            if (end == -1)
                end = binaryListElements.Count - 1;
            //Get the midpoint
            int midPoint = (start + end) / 2;
            //Locate the element at the midpoint
            BinaryListElement<T> located = binaryListElements.ElementAt(midPoint);
            //Perform checks
            if (located.key >= min && located.key <= max && (conditionalCheck?.Invoke(located.value) ?? true))
                return true;
            //Check if we exhausted the list
            if (start >= end)
                return false;
            //Perform next search
            if (located.key > max)
                return ElementsInRange(min, max, start, Math.Max(midPoint - 1, 0));
            else
                return ElementsInRange(min, max, midPoint + 1, end);
        }

    }

    public class PositionBasedBinaryList<T>
    {

        private BinaryList<BinaryList<T>> list = new BinaryList<BinaryList<T>>();

        public bool HasElements => list.Length() > 0;

        public T TakeFirst()
        {
            T taken = list.First().TakeFirst();
            if (list.First().Length() == 0)
                list.TakeFirst();
            return taken;
        }

        public T First()
        {
            return list.First().First();
        }

        public void Add(int x, int y, T element)
        {
            //Locate the X element
            BinaryList<T> located = list.ElementWithKey(x);
            //X element contains nothing, initialize it
            if (located == null)
            {
                BinaryList<T> createdList = new BinaryList<T>();
                located = createdList;
                list.Add(x, located);
            }
            //Add the y element
            located.Add(y, element);
        }

        public void Remove(int x, int y)
        {
            //Locate the X element
            BinaryList<T> located = list.ElementWithKey(x);
            //X element contains nothing, initialize it
            if (located == null)
                return;
            located.Remove(y);
            if (located.Length() == 0)
                list.Remove(x);
        }

        public T Get(int x, int y)
        {
            //Locate the X element
            BinaryList<T> located = list.ElementWithKey(x);
            //X element contains nothing, initialize it
            if (located == null)
                return default;
            return located.ElementWithKey(y);
        }

        /// <summary>
        /// Checks if any elements exist within the specified range.
        /// </summary>
        public bool ElementsInRange(int minX, int minY, int maxX, int maxY, int start = 0, int _end = -1, BinaryList<T>.BinaryListValidityCheckDelegate conditionalCheck = null)
        {
            //Check the X-Axis
            if (list.binaryListElements.Count == 0)
                return false;
            int end = _end;
            if (end == -1)
                end = list.binaryListElements.Count - 1;
            //Get the midpoint
            int midPoint = (start + end) / 2;
            //Locate the element at the midpoint
            BinaryListElement<BinaryList<T>> located = list.binaryListElements.ElementAt(midPoint);
            //Perform checks
            if (located.key >= minX && located.key <= maxX)
            {
                if (located.value.ElementsInRange(minY, maxY, 0, -1, conditionalCheck))
                    return true;
            }
            //Check if we exhausted the list
            if (start >= end)
                return false;
            //Perform next search
            if (located.key > maxX)
                return ElementsInRange(minX, minY, maxX, maxY, start, Math.Max(midPoint - 1, 0));
            else
                return ElementsInRange(minX, minY, maxX, maxY, midPoint + 1, end);
        }

        public override string ToString()
        {
            string output = "{\n";
            foreach (BinaryListElement<BinaryList<T>> xAxis in list.binaryListElements)
            {
                int x = xAxis.key;
                foreach (BinaryListElement<T> thing in xAxis.value.binaryListElements)
                {
                    int y = thing.key;
                    if (thing.value is IEnumerable)
                    {
                        output += $"\t({x}, {y}) = (";
                        IEnumerator enumerator = (thing.value as IEnumerable).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            output += $"{enumerator.Current},";
                        }
                        output += $")\n";
                    }
                    else
                        output += $"\t({x}, {y}) = ({thing.value})\n";
                }
            }
            return output + "\n}";
        }
    }
}
