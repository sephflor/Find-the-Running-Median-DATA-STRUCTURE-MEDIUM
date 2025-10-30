using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{

    /*
     * Complete the 'runningMedian' function below.
     *
     * The function is expected to return a DOUBLE_ARRAY.
     * The function accepts INTEGER_ARRAY a as parameter.
     */

    public static List<double> runningMedian(List<int> a)
    {
         List<double> result = new List<double>();
        // Max heap for lower half (invert min heap by using negative values)
        PriorityQueue<int, int> maxHeap = new PriorityQueue<int, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        // Min heap for upper half
        PriorityQueue<int, int> minHeap = new PriorityQueue<int, int>();
        
        foreach (int num in a) {
            // Add to appropriate heap
            if (maxHeap.Count == 0 || num <= maxHeap.Peek()) {
                maxHeap.Enqueue(num, num);
            } else {
                minHeap.Enqueue(num, num);
            }
            
            // Balance heaps
            if (maxHeap.Count > minHeap.Count + 1) {
                int moved = maxHeap.Dequeue();
                minHeap.Enqueue(moved, moved);
            } else if (minHeap.Count > maxHeap.Count) {
                int moved = minHeap.Dequeue();
                maxHeap.Enqueue(moved, moved);
            }
            
            // Calculate median
            double median;
            if (maxHeap.Count == minHeap.Count) {
                median = (maxHeap.Peek() + minHeap.Peek()) / 2.0;
            } else {
                median = maxHeap.Peek();
            }
            
            result.Add(Math.Round(median, 1));
        }
        
        return result;
    }

    // Alternative implementation for .NET versions without PriorityQueue
    public static List<double> runningMedianAlternative(List<int> a) {
        List<double> result = new List<double>();
        List<int> sorted = new List<int>();
        
        foreach (int num in a) {
            // Insert maintaining sorted order (binary search for insertion point)
            int index = sorted.BinarySearch(num);
            if (index < 0) index = ~index;
            sorted.Insert(index, num);
            
            // Calculate median
            int count = sorted.Count;
            double median;
            if (count % 2 == 0) {
                median = (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            } else {
                median = sorted[count / 2];
            }
            
            result.Add(Math.Round(median, 1));
        }
        
        return result;
    }


    }

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int aCount = Convert.ToInt32(Console.ReadLine().Trim());

        List<int> a = new List<int>();

        for (int i = 0; i < aCount; i++)
        {
            int aItem = Convert.ToInt32(Console.ReadLine().Trim());
            a.Add(aItem);
        }

        List<double> result = Result.runningMedian(a);

        textWriter.WriteLine(String.Join("\n", result));

        textWriter.Flush();
        textWriter.Close();
    }
}
