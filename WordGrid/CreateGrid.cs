using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace WordGrid
{
    class CreateGrid
    {
        #region Variables
        private static string FILEPATH = "https://norvig.com/ngrams/enabl";

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
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            GetWordList(FILEPATH);

            //Initialise search indices of word index
            bool complete = false;

            Console.WriteLine("Starting grid generation...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<List<string>> wordLists = new List<List<string>>();
            wordLists.Add(new List<string>(_wordList));
            while (!complete)
            {
                if (wordLists[wordLists.Count - 1].Count != 0)
                {
                    _wordGrid.Add(wordLists[wordLists.Count - 1][0]);
                    wordLists[wordLists.Count - 1].RemoveAt(0);

                    if (_wordGrid.Count < _gridSize)
                    {
                        //Create new word list with only valid letters from previously selected word
                        wordLists.Add(_wordList.Where(x => x[0] == _wordGrid[0][_wordGrid.Count]).ToList());
                        for (int i = 1; i < _wordGrid.Count; i++)
                        {
                            wordLists[wordLists.Count - 1] = wordLists[wordLists.Count - 1].Where(x => x[i] == _wordGrid[i][_wordGrid.Count]).ToList();
                        }
                    }
                    else
                    {
                        //Get character list from characters in valid grid
                        string compare = string.Join(string.Empty, _wordGrid);
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
                        }
                    }
                }
                else
                {
                    if (wordLists.Count == 1)
                    {
                        throw new GridNotFoundException("No valid word grid found for provided characters");
                    }
                    _wordGrid.RemoveAt(_wordGrid.Count - 1);
                    wordLists.RemoveAt(wordLists.Count - 1);
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
        private void GetWordList(string url)
        {
            try
            {
                //Extract valid words from word list
                Console.WriteLine("Extracting valid words from {0}", url);
                int total = 0;
                WebClient client = new WebClient();
                Stream inputStream = client.OpenRead(url);
                StreamReader streamReader = new StreamReader(inputStream);
                string readerLine;
                readerLine = streamReader.ReadLine();

                while (readerLine != null)
                {
                    if (readerLine.Length == _gridSize)
                    {
                        if (IsValid(readerLine, _characters))
                        {
                            _wordList.Add(readerLine);
                        }
                    }
                    total++;
                    readerLine = streamReader.ReadLine();
                }
                inputStream.Close();
                Console.WriteLine("{0} valid words out of {1}", _wordList.Count, total);
            }
            catch(WebException webEx)
            {
                throw new GridNotFoundException("Word list unavailable");
            }
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
