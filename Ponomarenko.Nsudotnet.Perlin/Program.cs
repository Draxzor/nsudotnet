using System;
using System.Drawing;

namespace Ponomarenko.Nsudotnet.Perlin
{
    class Program
    {
        static void Main(string[] args)
        {
            int size;

            if (!Int32.TryParse(args[0], out size))
                throw new ArgumentException("First argument is not a number");
            else if (size <= 0)
                throw new ArgumentException("Size should be greater than zero");

            string fileName = args[1];
            int initialRank = 5;
            //int size = 750;
            float a = 0.5F, b = 0.25F, c = 0.15F;
            Bitmap bitmap = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int[,] colors = new int[3, size * size];
            int rank = initialRank;
            for (int k = 0; k < 3; k++) {
                Grid grid1 = new Grid(size, initialRank);
                Grid grid2 = new Grid(size, initialRank * 2);
                Grid grid3 = new Grid(size, initialRank * 4);

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int color =  (int)(a * grid1.GetColor(j, i) + b *grid2.GetColor(j, i) + c *grid3.GetColor(j, i));
                        if (color > 255) color = 255;
                        if (color < 0) color = 0;
                        colors[k, i * size + j] = color;
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb(colors[0, i * size + j], colors[1, i * size + j], colors[2, i * size + j]));
                }
            }

            bitmap.Save(fileName);
        }
    }
}
