using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BigramParser.Models;

namespace BigramParser
{
    public class Histogram
    {
        /// <summary>
        /// Lists how many times a particular bigram (two adjacent words) occurred in the text.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>IEnumerable<WordPair></returns>
        public IEnumerable<WordPair> CreateHistogram(String input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("The string input must not be empty");
            }

            IEnumerable<WordPair> wordPairGroups = null;            

            try
            {
                List<string> words = SplitString(input);
                wordPairGroups = GetWordPairGroups(words);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create histogram: " + e.Message);
            }

            return wordPairGroups;
        }

        public List<string> SplitString(String input)
        {
            return Regex.Split(input.ToLower(), @"\W+").Where(s => s != String.Empty).ToList();
        }

        public IEnumerable<WordPair> GetWordPairGroups(List<string> words)
        {
            IOrderedEnumerable<WordPair> wordPairGroups = null;

            // Pair up adjacent words and add to a collection
            IEnumerable<string> wordPairs = words.Take(words.Count() - 1)
                .Select((w, i) => w + " " + words[i + 1]);

            // Group by adjacent word pairs
            wordPairGroups = wordPairs.GroupBy(x => x)
                .Select(x => new WordPair
                {
                    Text = x.Key,
                    Count = x.Count()
                })
            .OrderByDescending(x => x.Count);

            return wordPairGroups;
        }

    }
}
