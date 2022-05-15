using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter String");
                string input = Console.ReadLine().Trim().ToLower();

                List<string> inputParts = input.Split(' ').ToList();

                int gridSize = 0;
                bool successfull = int.TryParse(inputParts[0], out gridSize);
                if (successfull)
                {
                    Console.WriteLine("Grid size is: {0} x {0}", gridSize);
                    Console.WriteLine("Available characters are: {0}", inputParts[1]);
                    CreateGrid wordGrid = new CreateGrid(gridSize, inputParts[1]);
                }
                else
                {
                    Console.WriteLine("Invalid grid size value entered {0}", inputParts[1]);
                }
            }
        }
    }
}
