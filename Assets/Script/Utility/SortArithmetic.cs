using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT
{

    public static class SortArithmetic
    {
        public static class QuickSortArithmetic
        {
            public static void QuickSort(List<int> list, int left, int right)
            {
                if (left < right)
                {
                    int i = Division(list, left, right);
                    QuickSort(list, i + 1, right);
                    QuickSort(list, left, i - 1);
                }
            }

            private static int Division(List<int> list, int left, int right)
            {
                while (left < right)
                {
                    int num = list[left]; 
                    if (num > list[left + 1])
                    {
                        list[left] = list[left + 1];
                        list[left + 1] = num;
                        left++;
                    }
                    else
                    {
                        int temp = list[right];
                        list[right] = list[left + 1];
                        list[left + 1] = temp;
                        right--;
                    }
                }
                return left;
            }
        }

    }


}
