using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WordGrid
{
    class CreateGrid
    {
        #region Variables
        private static string FILEPATH = "../../../enable1.txt";

        private int _gridSize;
        private string _characters;
        private List<string> _wordList = new List<string>();
        private List<string> _wordGrid = new List<string>();
        private TimeSpan _timeCompleted;
        #endregion


        public CreateGrid(int gridSize, string characters)
        {
            _gridSize = gridSize;

            List<char> chars = characters.ToList();
            chars.Sort();
            _characters = new string(chars.ToArray());
        }

        #region public members
        public void GenerateGrid()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            //Extract valid words from word list
            Console.WriteLine("Extracting valid words from {0}", FILEPATH);
            int total = 0;
            foreach (string line in File.ReadLines(@FILEPATH, Encoding.UTF8))
            {
                if (line.Length == _gridSize)
                {
                    if (IsValid(line, _characters))
                    {
                        _wordList.Add(line);
                    }
                }
                total++;
            }
            Console.WriteLine("{0} valid words out of {1}", _wordList.Count, total);

            //Initialise search indices of word index
            bool complete = false;
            List<int> wordListIndex = new List<int>();
            wordListIndex.Add(0);

            Console.WriteLine("Starting grid generation...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!complete)
            {
                if (wordListIndex[wordListIndex.Count - 1] >= _wordList.Count)
                {
                    if(wordListIndex.Count == 1)
                    {
                        throw new GridNotFoundException("No valid word grid found for provided characters");
                    }
                    _wordGrid.RemoveAt(_wordGrid.Count - 1);
                    wordListIndex.RemoveAt(wordListIndex.Count - 1);
                    wordListIndex[wordListIndex.Count - 1] += 1;
                }
                else if (CheckValid(_wordList[wordListIndex[wordListIndex.Count - 1]]))
                {
                    _wordGrid.Add(_wordList[wordListIndex[wordListIndex.Count - 1]]);
                    wordListIndex.Add(0);
                }
                else
                {
                    wordListIndex[wordListIndex.Count - 1] += 1;
                }

                if (wordListIndex.Count > _gridSize)
                {
                    wordListIndex.RemoveAt(wordListIndex.Count - 1);

                    //Get character list from characters in valid grid
                    string compare = string.Join(string.Empty,_wordGrid);
                    List<char> chars = compare.ToList();
                    chars.Sort();
                    compare = new string(chars.ToArray());

                    //See if grid has valid characters
                    if (compare == _characters)
                    {
                        complete = true;
                    }
                    else
                    {
                        _wordGrid.RemoveAt(_wordGrid.Count - 1);

                        if (wordListIndex[wordListIndex.Count - 1] >= _wordList.Count)
                        {
                            _wordGrid.RemoveAt(_wordGrid.Count - 1);
                            wordListIndex.RemoveAt(wordListIndex.Count - 1);
                        }
                        wordListIndex[wordListIndex.Count - 1] += 1;
                    }
                }
            }
            sw.Stop();
            _timeCompleted = sw.Elapsed;
            Console.WriteLine("Grid Generation Completed");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public List<string> GetWordGrid()
        {
            return _wordGrid;
        }

        public TimeSpan GetTimeCompleted()
        {
            return _timeCompleted;
        }
        #endregion

        #region private members
        private bool CheckValid(string word)
        {
            for(int i = 0; i < _wordGrid.Count; i++)
            {
                if(!(_wordGrid[i][_wordGrid.Count] == word[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValid(string word, string check)
        {
            foreach (char character in word)
            {
                if (!check.Contains(character))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }

    public class GridNotFoundException : Exception
    {
        public GridNotFoundException()
        {
        }

        public GridNotFoundException(string message)
            : base(message)
        {
        }

        public GridNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
