using System;
using System.Collections;
using System.Linq;

namespace Reflection
{
    public class Comparer
    {
        public static bool Compare<T>(T first, T second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
            {
                return true;
            }

            if (ReferenceEquals(first, null))
            {
                return false;
            }

            if (ReferenceEquals(second, null))
            {
                return false;
            }

            if (first.GetType() != second.GetType())
            {
                return false;
            }

            bool equalityFlag = true;
            var properties = first.GetType().GetProperties()
                .Where(p => p.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                var firstValue = property.GetValue(first);
                var secondValue = property.GetValue(second);

                if (property.PropertyType.IsPrimitive)
                {
                    equalityFlag &= CompareValues(firstValue, secondValue);   
                }
                else
                {
                    if (property.PropertyType.GetInterface("IEnumerable`1") != null ||
                        property.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        equalityFlag &= CompareEnumerables((IEnumerable)firstValue, (IEnumerable)secondValue);
                    }
                    else
                    {
                        if (property.PropertyType.IsClass)
                        {
                            equalityFlag &= Compare(firstValue, secondValue);
                        }
                    }
                }
            }

            return equalityFlag;
        }

        private static bool CompareEnumerables(IEnumerable first, IEnumerable second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
            {
                return true;
            }

            if (ReferenceEquals(first, null))
            {
                return false;
            }

            if (ReferenceEquals(second, null))
            {
                return false;
            }

            var firstValue = first.Cast<object>();
            var secondValue = second.Cast<object>();

            if (firstValue.Count() != secondValue.Count())
            {
                return false;
            }

            bool equalityFlag = true;
            var firstEnumerator = firstValue.GetEnumerator();
            var secondEnumerator = secondValue.GetEnumerator();

            for (int i = 0; i < firstValue.Count(); i++)
            {
                firstEnumerator.MoveNext();
                secondEnumerator.MoveNext();

                if (firstEnumerator.Current.GetType().IsPrimitive)
                {
                    equalityFlag &= CompareValues(firstEnumerator.Current, secondEnumerator.Current);
                }
                else
                {
                    equalityFlag &= Compare(firstEnumerator.Current, secondEnumerator.Current);
                }

            }

            return equalityFlag;
        }

        private static bool CompareValues(object firstValue, object secondValue)
        {
            IComparable firstComparer = firstValue as IComparable;

            if (!ReferenceEquals(firstComparer, null))
            {
                return firstComparer.CompareTo(secondValue) == 0;
            }
            else
            {
                return false;
            }
        }
    }
}
