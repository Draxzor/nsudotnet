using System;
using System.Collections.Generic;
using System.Linq;

namespace Ponomarenko.Nsudotnet.NumberGuesser
{
    class Game
    {
        private byte Number;

        private string UserName;

        private List<string> Insults;

        private List<byte> History;

        private Random Randomizer;

        public Game()
        {
            Randomizer = new Random();
            InitInsults();
            History = new List<byte>(1000);
        }

        private void MakeNumber()
        {
            Number = (byte)Randomizer.Next(101);
        }

        private void ReadUserName()
        {
            UserName = Console.ReadLine();
        }

        private void InitInsults()
        {
            Insults = new List<string> { "Go fuck youtrself, {0}!",
                                         "You think it's funny, fucking {0}??",
                                         "{0}, please, stop, you're just a meat-headed shit sack.",
                                         "You are such a dickhead, {0}!",
                                         "Fun fact: {0} means dumbass in Latin." };
        }

        private bool MakeGuess()
        {
            string numberString = Console.ReadLine();
            int number;

            if (!Int32.TryParse(numberString, out number))
            {
                if (numberString == "q")
                {
                    Console.WriteLine("Sorry");

                    Environment.Exit(0);
                }
                Console.WriteLine("You need to type a number, little piece of shit.");

                return false;
            }
            else if (number < 0 || number > 100)
            {
                Console.WriteLine("Of course it's not right, number should be in [0,100] range.");

                return false;
            }
            else if (number != Number)
            {
                if (History.Count == 1000)
                    throw new Exception("Too many tries");
                else
                    History.Add((byte)number);


                if (number > Number)
                    Console.WriteLine("Your number is larger.");
                else
                    Console.WriteLine("Your number is smaller.");

                return false;
            }
            else
            {
                return true;
            }
        }

        public void Start()
        {
            Console.WriteLine("Enter your name, loser.");
            ReadUserName();
            MakeNumber();
            Console.WriteLine("Now try to guess a number, that is in [0, 100] range.");
            int failuresCount = 0;
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (!MakeGuess())
                {
                    failuresCount++;
                }
                else
                {
                    TimeSpan playingTime = DateTime.Now - startTime;
                    Console.WriteLine("Congrats! You win this time.");
                    // failuresCount include those times when user input was incorrect
                    // if it's needed to count only correct inputs, just replace failuresCount with History.Count() 
                    Console.WriteLine($"You failed {failuresCount} times, and it took {(int)playingTime.TotalMinutes} minutes and {playingTime.Seconds} seconds. Here is your history:");

                    foreach (byte i in History)
                    {
                        string result = (i > Number) ? "larger" : "smaller";
                        Console.WriteLine($"{i} - {result}");
                    }

                    break;
                }

                if (failuresCount % 4 == 0)
                    Console.WriteLine(String.Format(Insults[Randomizer.Next(Insults.Count())], UserName));
            }
        }
    }
}
