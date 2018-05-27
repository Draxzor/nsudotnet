using System;

namespace Ponomarenko.Nsudotnet.Perlin
{
    class Grid
    {
        private byte[] ImageBytes;

        private byte[,] ControlPoints;

        private Random Randomizer;

        private int Size;

        public Grid(int size, int rank)
        {
            Size = size;
            ImageBytes = new byte[size * size];
            // rank is a number of cells, so we need rank + 1 knots
            // we set size to rank + 2, because when we get closer to the edge of an image we might have blank space
            // because we already drawn a grid. So, we need 1 "invisible" knot, so that we can interpolate colors on the edge
            ControlPoints = new byte[rank + 2, rank + 2];
            Randomizer = new Random();
            int step = (int)Math.Ceiling((float)size / rank);

            for (int i = 0; i < rank + 2; i++)
            {
                for (int j = 0; j < rank + 2; j++)
                {
                    ControlPoints[i, j] = (byte)Randomizer.Next(50, 255);
                }
            }
            int globalY = 0;

            for (int j = 0; j < size; j ++)
            {
                int globalX = 0;
                globalY = j / step;
                int localY = j % step;
                float yCoef = (float)localY / step;    
                
                if (j == size - 1)
                {
                    globalY = rank - 1;
                    yCoef = 1.0F;
                }    

                for (int i = 0; i < size - 1; i ++) {
                    globalX = i / step;
                    //inside square
                    int localX = i % step;
                    float xCoef = (float)localX / step;
                    byte up = Intepolate(ControlPoints[globalX, globalY], ControlPoints[globalX + 1, globalY], xCoef);
                    byte bottom = Intepolate(ControlPoints[globalX, globalY + 1], ControlPoints[globalX + 1, globalY + 1], xCoef);
                    ImageBytes[j * size + i] = Intepolate(up, bottom, yCoef);
                }
                ImageBytes[j * size + size - 1] = Intepolate(ControlPoints[rank, globalY], ControlPoints[rank, globalY + 1], yCoef);
            }
        }

        private byte Intepolate(byte left, byte right, float coef)
        {
            return (byte)(left + (right - left) * coef);
        }

        public byte GetColor(int x, int y) { return ImageBytes[y * Size + x]; }
    }
}
