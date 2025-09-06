using System.Collections.Generic;
using System.Drawing;

namespace Technical_Solution
{
    internal class ImageThinning
    {
        public static Bitmap ZhangSuenThinning(Bitmap binaryImage)
        {
            int width = binaryImage.Width;
            int height = binaryImage.Height;
            bool changed;

            bool[,] pixels = new bool[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    pixels[x, y] = binaryImage.GetPixel(x, y).ToArgb() == Color.White.ToArgb();

            do
            {
                changed = false;
                List<(int, int)> toRemove = new List<(int, int)>();

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        if (!pixels[x, y]) continue;

                        int bp1 = CountNeighbors(pixels, x, y);
                        int ap1 = CountTransitions(pixels, x, y);
                        if (bp1 >= 2 && bp1 <= 6 && ap1 == 1 &&
                            !(pixels[x, y - 1] && pixels[x + 1, y] && pixels[x, y + 1]) &&
                            !(pixels[x + 1, y] && pixels[x, y + 1] && pixels[x - 1, y]))
                        {
                            toRemove.Add((x, y));
                        }
                    }
                }

                if (toRemove.Count > 0)
                {
                    changed = true;
                    foreach (var p in toRemove)
                        pixels[p.Item1, p.Item2] = false;
                }

                toRemove.Clear();

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        if (!pixels[x, y]) continue;

                        int bp1 = CountNeighbors(pixels, x, y);
                        int ap1 = CountTransitions(pixels, x, y);
                        if (bp1 >= 2 && bp1 <= 6 && ap1 == 1 &&
                            !(pixels[x, y - 1] && pixels[x + 1, y] && pixels[x - 1, y]) &&
                            !(pixels[x, y - 1] && pixels[x, y + 1] && pixels[x - 1, y]))
                        {
                            toRemove.Add((x, y));
                        }
                    }
                }

                if (toRemove.Count > 0)
                {
                    changed = true;
                    foreach (var p in toRemove)
                        pixels[p.Item1, p.Item2] = false;
                }

            } while (changed);

            Bitmap thinned = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    thinned.SetPixel(x, y, pixels[x, y] ? Color.White : Color.Black);
                }
            }

            return thinned;
        }

        private static int CountNeighbors(bool[,] pixels, int x, int y)
        {
            int count = 0;
            if (pixels[x, y - 1]) count++;
            if (pixels[x + 1, y - 1]) count++;
            if (pixels[x + 1, y]) count++;
            if (pixels[x + 1, y + 1]) count++;
            if (pixels[x, y + 1]) count++;
            if (pixels[x - 1, y + 1]) count++;
            if (pixels[x - 1, y]) count++;
            if (pixels[x - 1, y - 1]) count++;
            return count;
        }

        private static int CountTransitions(bool[,] pixels, int x, int y)
        {
            int count = 0;
            if (!pixels[x, y - 1] && pixels[x + 1, y - 1]) count++;
            if (!pixels[x + 1, y - 1] && pixels[x + 1, y]) count++;
            if (!pixels[x + 1, y] && pixels[x + 1, y + 1]) count++;
            if (!pixels[x + 1, y + 1] && pixels[x, y + 1]) count++;
            if (!pixels[x, y + 1] && pixels[x - 1, y + 1]) count++;
            if (!pixels[x - 1, y + 1] && pixels[x - 1, y]) count++;
            if (!pixels[x - 1, y] && pixels[x - 1, y - 1]) count++;
            if (!pixels[x - 1, y - 1] && pixels[x, y - 1]) count++;
            return count;
        }
    }
}
