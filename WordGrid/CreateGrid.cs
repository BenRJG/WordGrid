using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConsoleApp1
{
    class CreateGrid
    {
        private int _gridSize;
        private string _characters;
        private List<string> _wordList = new List<string>();
        private List<string> _wordGrid = new List<string>();

        public CreateGrid(int gridSize, string characters)
        {
            _gridSize = gridSize;

            List<char> chars = characters.ToList();
            chars.Sort();
            _characters = new string(chars.ToArray());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            start();
            sw.Stop();

            Console.WriteLine("Final Word Grid found in {0} minutes, {1} seconds, {2} miliseconds:", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
            foreach (string word in _wordGrid)
            {

                Console.WriteLine(word);
            }
        }

        private void start()
        {
            //Extract valid words from word list
            int total = 0;
            foreach (string line in File.ReadLines(@"../../../enable1.txt", Encoding.UTF8))
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

            /*Console.WriteLine("Available words ({0} out of {1})", _wordList.Count, total);
            foreach (string word in _wordList)
            {
                Console.WriteLine(word);
            }*/

            bool complete = false;
            string[] grid = new string[_gridSize];


            List<int> wordListIndex = new List<int>();
            wordListIndex.Add(0);
            while (!complete)
            {
                if (wordListIndex[wordListIndex.Count - 1] >= _wordList.Count)
                {
                    _wordGrid.RemoveAt(_wordGrid.Count - 1);
                    wordListIndex.RemoveAt(wordListIndex.Count - 1);
                    wordListIndex[wordListIndex.Count - 1] += 1;
                }
                else if (CheckValid(_wordList[wordListIndex[wordListIndex.Count - 1]]))
                {
                    _wordGrid.Add(_wordList[wordListIndex[wordListIndex.Count - 1]]);
                    if (_wordGrid[0] == "bravado")
                    {

                    }
                    wordListIndex.Add(0);
                }
                else
                {
                    wordListIndex[wordListIndex.Count - 1] += 1;
                }

                if (wordListIndex.Count > _gridSize)
                {
                    wordListIndex.RemoveAt(wordListIndex.Count - 1);
                    string compare = string.Empty;
                    foreach (string word in _wordGrid)
                    {
                        compare = compare + word;
                    }
                    List<char> chars = compare.ToList();
                    chars.Sort();
                    compare = new string(chars.ToArray());

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
        }

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
    }
}
