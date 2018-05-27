using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponomarenko.Nsudotnet.NumberGuesser
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            try
            {
                game.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
