using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Sztorm.Collections.Tests
{
    public static class TestUtils
    {
        public static string CreateShortMethodSignature(
            string methodName, params Type[] parameterTypes)
        {
            var sb = new StringBuilder();
            int lastIndex = parameterTypes.Length - 1;

            sb.Append(methodName);
            sb.Append('(');

            for (int i = 0; i < lastIndex; i++)
            {
                sb.Append(parameterTypes[i].Name);
                sb.Append(", ");
            }
            sb.Append(parameterTypes[lastIndex].Name);
            sb.Append(')');

            return sb.ToString();
        }

        public static void WriteTable<T>(T[,] array, string format = "")
        {
            int len0 = array.GetLength(0);
            int lastColIndex = array.GetLength(1) - 1;

            for (int i = 0; i < len0; i++)
            {
                for (int j = 0; j < lastColIndex; j++)
                {
                    Console.Write("{0:" + format + "}, ", array[i, j]);
                }
                Console.WriteLine("{0:" + format + "}", array[i, lastColIndex]);
            }
        }

        public static void WriteTable<T>(Array2D<T> array, string format = "")
        {
            int len1 = array.Length1;
            int lastColIndex = array.Columns - 1;

            for (int i = 0; i < len1; i++)
            {
                for (int j = 0; j < lastColIndex; j++)
                {
                    Console.Write("{0:" + format + "}, ", array[i, j]);
                }
                Console.WriteLine("{0:" + format + "}", array[i, lastColIndex]);
            }
        }

        public static void WriteTable<T>(List2D<T> list, string format = "")
        {
            if (list.IsEmpty || list == null)
            {
                return;
            }
            int len1 = list.Length1;
            int lastColIndex = list.Columns - 1;

            for (int i = 0; i < len1; i++)
            {
                for (int j = 0; j < lastColIndex; j++)
                {
                    Console.Write("{0:" + format + "}, ", list[i, j]);
                }
                Console.WriteLine("{0:" + format + "}", list[i, lastColIndex]);
            }
        }

        public static int[,] IncrementedSystemInt2DArray(int rows, int columns)
        {
            var result = new int[rows, columns];
            int counter = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = counter++;
                }
            }
            return result;
        }

        public static Array2D<int> IncrementedIntArray2D(int rows, int columns)
        {
            var result = new Array2D<int>(rows, columns);
            int counter = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = counter++;
                }
            }
            return result;
        }

        public readonly struct AlwaysTruePredicate<T> : IPredicate<T>
        {
            public bool Invoke(T obj) => true;
        }

        public readonly struct AlwaysFalsePredicate<T> : IPredicate<T>
        {
            public bool Invoke(T obj) => false;
        }

        public struct SumIntAction : IAction<int>
        {
            public int Sum { get; private set; }

            public void Invoke(int number) => Sum += number;
        }

        public struct ConcatStringsAction : IAction<string>
        {
            private StringBuilder sb;

            public ConcatStringsAction(int approximateLength)
                => sb = new StringBuilder(approximateLength);

            public string Result => sb.ToString();

            public void Invoke(string text) => sb.Append(text);
        }
    }
}
