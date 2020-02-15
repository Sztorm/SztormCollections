using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Sztorm.Collections.Tests
{
    public static class TestsUtils
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

        public static void WriteTable<T>(T[,] array, string format)
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

        public static void WriteTable<T>(Array2D<T> array, string format)
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
    }
}
