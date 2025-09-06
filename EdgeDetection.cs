using System;
using System.Drawing;

namespace Technical_Solution
{
    internal class EdgeDetection
    {
        public static Bitmap RoadDetection(Bitmap inputImage)
        {
            int inputWidth = inputImage.Width;
            int inputHeight = inputImage.Height;
            Bitmap outputImage = new Bitmap(inputWidth, inputHeight);
            for (int imageRow = 1; imageRow < inputHeight - 1; imageRow++)
            {
                for (int imageColumn = 1; imageColumn < inputWidth - 1; imageColumn++)
                {
                    Color pixel = inputImage.GetPixel(imageColumn, imageRow);
                    bool isRoad = pixel.R == 215 && pixel.G == 225 && pixel.B == 230;
                    if (isRoad)
                    {
                        outputImage.SetPixel(imageColumn, imageRow, Color.White);
                    }
                    else
                    {
                        outputImage.SetPixel(imageColumn, imageRow, Color.Black);
                    }
                }
            }
            for (int x = 0; x < inputWidth; x++)
            {
                outputImage.SetPixel(x, 0, Color.Black); outputImage.SetPixel(x, inputHeight - 1, Color.Black);
            }
            for (int y = 0; y < inputHeight; y++)
            {
                outputImage.SetPixel(0, y, Color.Black); outputImage.SetPixel(inputWidth - 1, y, Color.Black);
            }
            return RemoveNoise(outputImage);
        }

        public static Bitmap RemoveNoise(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            Bitmap denoisedImage = new Bitmap(width, height);
            for (int imageRow = 1; imageRow < height - 1; imageRow++)
            {
                for (int imageColumn = 1; imageColumn < width - 1; imageColumn++)
                {
                    Color pixel = image.GetPixel(imageColumn, imageRow);
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255)
                    {
                        int whiteNeighborCount = 0;
                        for (int neighbourRow = -1; neighbourRow <= 1; neighbourRow++)
                        {
                            for (int neighbourColumn = -1; neighbourColumn <= 1; neighbourColumn++)
                            {
                                if (neighbourColumn == 0 && neighbourRow == 0)
                                {
                                    continue;
                                }
                                Color neighborPixel = image.GetPixel(imageColumn + neighbourColumn, imageRow + neighbourRow);
                                if (neighborPixel.R == 255 && neighborPixel.G == 255 && neighborPixel.B == 255)
                                {
                                    whiteNeighborCount++;
                                }
                            }
                        }
                        if (whiteNeighborCount >= 2)
                        {
                            denoisedImage.SetPixel(imageColumn, imageRow, Color.White);
                        }
                        else
                        {
                            denoisedImage.SetPixel(imageColumn, imageRow, Color.Black);
                        }
                    }
                    else
                    {
                        denoisedImage.SetPixel(imageColumn, imageRow, Color.Black);
                    }
                }
            }
            return denoisedImage;
        }
    }
}