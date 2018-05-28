using System;
using System.Collections.Generic;

namespace Ponomarenko.Nsudotnet.Perlin
{
    class Grid
    {
        private byte[,] _controlPoints;

        private Random _randomizer;

        private int _size;

        private int _step;

        private int _rank;

        public Grid(int size, int rank)
        {
            _rank = rank;
            _size = size;
            // rank is a number of cells, so we need rank + 1 knots
            // we set size to rank + 3, because of bicubical interpolation.
            // There must be something to interpolate when we get closer to the edges of an image.
            // We need 1 "invisible" row of knots on each edge, so that we can interpolate colors properly
            _controlPoints = new byte[_rank + 3, _rank + 3];
            _randomizer = new Random();
            _step = (int)Math.Ceiling((float)size / _rank);

            for (int i = 0; i < _rank + 3; i++)
            {
                for (int j = 0; j < _rank + 3; j++)
                {
                    _controlPoints[i, j] = (byte)_randomizer.Next(50, 255);
                }
            }
        }

        private byte Intepolate(byte left, byte right, float coef)
        {
            return (byte)(left + (right - left) * coef);
        }

        private byte CubicInterpolate(byte[] knotValues, float localCoord)
        {
            return (byte)(knotValues[1] + (-0.5 * knotValues[0] + 0.5 * knotValues[2]) * localCoord
        + (knotValues[0] - 2.5 * knotValues[1] + 2.0 * knotValues[2] - 0.5 * knotValues[3]) * localCoord * localCoord
        + (-0.5 * knotValues[0] + 1.5 * knotValues[1] - 1.5 * knotValues[2] + 0.5 * knotValues[3]) * localCoord * localCoord * localCoord);
        }

        private byte BicubicInterpolate(List<byte[]> knotValues, float localX, float localY)
        {
            byte[] tmp = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                tmp[i] = CubicInterpolate(knotValues[i], localY);
            }

            return CubicInterpolate(tmp, localX);
        }

        public byte GetColor(int x, int y)
        {
            int globalY = y / _step + 1;
            int localY = y % _step;
            float yCoef = (float)localY / _step;
            int globalX = x / _step + 1;
            int localX = x % _step;
            float xCoef = (float)localX / _step;
            List<byte[]> knotValues = new List<byte[]>(4);

            for (int i = 0; i < 4; i++)
            {
                knotValues.Add(new byte[4]);

                for (int j = 0; j < 4; j++)
                {
                    knotValues[i][j] = _controlPoints[globalX - 1 + i, globalY - 1 + j];
                }
            }

            return BicubicInterpolate(knotValues, localX, localY);
        }
    }
}
