using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            bool active = true;
            while (active)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter grid size and available word grid characters");
                Console.WriteLine("(Type \"exit\" to close)");
                string input = Console.ReadLine().Trim().ToLower();

                if (input != "exit")
                {
                    List<string> inputParts = input.Split(' ').ToList();

                    if (inputParts.Count == 2)
                    {
                        int gridSize = 0;
                        bool successfull = int.TryParse(inputParts[0], out gridSize);
                        if (successfull)
                        {
                            Console.WriteLine("Grid size is: {0} x {0}", gridSize);
                            Console.WriteLine("Available characters are: {0}", inputParts[1]);
                            CreateGrid wordGrid = new CreateGrid(gridSize, inputParts[1]);
                            try
                            {
                                wordGrid.GenerateGrid();

                                TimeSpan time = wordGrid.GetTimeCompleted();
                                Console.WriteLine("Grid found in {0}:{1}:{2}", time.Minutes, time.Seconds, time.Milliseconds);
                                Console.ForegroundColor = ConsoleColor.Green;
                                foreach (string word in wordGrid.GetWordGrid())
                                {
                                    Console.WriteLine(word);
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            catch (GridNotFoundException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("EXCEPTION: {0}", ex.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid grid size value entered \"{0}\"", inputParts[1]);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\"{0}\" is an invalid format, please enter the grid size followed by available characters\n" +
                                          "Example: 4 eeeeddoonnnsssrv", input);
                    }
                }
                else
                {
                    active = false;
                }

            }
        }
    }
}
