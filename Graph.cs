using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Technical_Solution
{
    internal class Graph
    {
        public static List<(int x, int y)> WhitePixels(Bitmap image)
        {
            List<(int x, int y)> whitePixels = new List<(int x, int y)>();
            for (int imageRow = 1; imageRow < image.Height - 1; imageRow++)
            {
                for (int imageColumn = 1; imageColumn < image.Width - 1; imageColumn++)
                {
                    Color pixelColor = image.GetPixel(imageColumn, imageRow);
                    if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                    {
                        whitePixels.Add((imageColumn, imageRow));
                    }
                }
            }
            Console.WriteLine($"[DEBUG] ExtractNodes found {whitePixels.Count} white pixels.");
            return whitePixels;
        }

        public static Dictionary<(int x, int y), List<(int, int)>> CreateAdjacencyList(List<(int, int)> whitePixels)
        {
            HashSet<(int, int)> nodeSet = new HashSet<(int, int)>(whitePixels);
            Dictionary<(int x, int y), List<(int, int)>> adjacencyList = new Dictionary<(int x, int y), List<(int, int)>>();
            int[] horizontalDirections = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] verticalDirections = { -1, -1, -1, 0, 0, 1, 1, 1 };
            foreach (var whitePixel in whitePixels)
            {
                int horizontalPosition = whitePixel.Item1;
                int verticalPosition = whitePixel.Item2;
                List<(int, int)> neighbours = new List<(int, int)>();
                for (int i = 0; i < horizontalDirections.Length; i++)
                {
                    int newHorizontalPosition = horizontalPosition + horizontalDirections[i];
                    int newVerticalPosition = verticalPosition + verticalDirections[i];
                    var neighbour = (newHorizontalPosition, newVerticalPosition);
                    if (nodeSet.Contains(neighbour))
                    {
                        neighbours.Add(neighbour);
                    }
                }
                if (neighbours.Count > 2)
                {
                    adjacencyList[whitePixel] = neighbours;
                }
            }

            Console.WriteLine($"[DEBUG] Built adjacency list with {adjacencyList.Count} nodes.");
            return adjacencyList;
        }

        public static void PrintAdjacencyList(Dictionary<(int x, int y), List<(int, int)>> adjacencyList, int maxToPrint = 20)
        {
            Console.WriteLine($"[DEBUG] Total nodes in adjacency list: {adjacencyList.Count}");

            if (adjacencyList.Count == 0)
            {
                Console.WriteLine("[DEBUG] The adjacency list is empty. Check if ExtractNodes() found any white pixels.");
                return;
            }

            int printed = 0;
            foreach (var kvp in adjacencyList)
            {
                var node = kvp.Key;
                var neighbors = kvp.Value;

                Console.Write($"({node.x}, {node.y}) -> ");

                if (neighbors.Count > 0)
                {
                    Console.WriteLine(string.Join(", ", neighbors.Select(n => $"({n.Item1}, {n.Item2})")));
                }
                else
                {
                    Console.WriteLine("[]");
                }

                printed++;
                if (printed >= maxToPrint)
                {
                    Console.WriteLine($"[DEBUG] Showing only first {maxToPrint} nodes...");
                    break;
                }
            }
        }
    }
}

