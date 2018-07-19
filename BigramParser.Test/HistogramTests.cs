﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BigramParser.Models;

namespace BigramParser.Test
{

    [TestFixture]
    public class HistogramTests
    {
        Histogram histogram;
        Func<WordPair, object> wpSelector;  // a transform function to apply to WordPair collection

        [SetUp]
        public void TestSetup()
        {
            histogram = new Histogram();
            wpSelector = i => new { i.Text, i.Count };  
        }

        [Test]
        public void SplitString_InputWithComma_SplitsWordsByComma()
        {
            List<string> expected = new List<string> { "the", "quick" }; 
            List<string> actual = histogram.SplitString("the,quick");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitString_InputWithUppercase_IgnoresUppercase()
        {
            List<string> expected = new List<string> { "the", "quick", "brown", "fox" };
            List<string> actual = histogram.SplitString("The Quick brown Fox");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitString_InputWithPeriod_IgnoresPeriod()
        {
            List<string> expected = new List<string> { "the", "quick", "brown", "fox" };
            List<string> actual = histogram.SplitString("The quick brown fox.");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitString_InputOneWord_ReturnsOneCollectionItem()
        {
            List<string> expected = new List<string> { "fox" };
            List<string> actual = histogram.SplitString("fox");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WordPairGroups_InputTwoIdenticalWords_ReturnsOneCollectionItemWithOneCount()
        {
            IEnumerable<WordPair> expected = new WordPair[] { new WordPair() { Text = "quick quick", Count = 1 } };

            List<string> words = histogram.SplitString("quick quick");
            IEnumerable<WordPair> actual = histogram.GetWordPairGroups(words);

            // Could not compare the two lists directly using Assert.AreEqual. 
            // Used transform function as a workaround.            
            Assert.That(expected.Select(wpSelector), Is.EquivalentTo(actual.Select(wpSelector)));
        }

        [Test]
        public void WordPairGroups_InputThreeIdenticalWords_ReturnsOneCollectionItemWithTwoCounts()
        {
            IEnumerable<WordPair> expected = new WordPair[] { new WordPair() { Text = "quick quick", Count = 2 } };

            List<string> words = histogram.SplitString("quick quick quick");
            IEnumerable<WordPair> actual = histogram.GetWordPairGroups(words);

            Assert.That(expected.Select(wpSelector), Is.EquivalentTo(actual.Select(wpSelector)));
        }

        [Test]
        public void WordPairGroups_InputTwoDifferentWords_ReturnsOneCollectionItemWithOneCount()
        {
            IEnumerable<WordPair> expected = new WordPair[] { new WordPair() { Text = "quick brown", Count = 1 } };

            List<string> words = histogram.SplitString("quick brown");
            IEnumerable<WordPair> actual = histogram.GetWordPairGroups(words);

            Assert.That(expected.Select(wpSelector), Is.EquivalentTo(actual.Select(wpSelector)));
        }

        [Test]
        public void WordPairGroups_InputThreeDifferentWords_ReturnsTwoCollectionItemsWithOneCountEach()
        {
            IEnumerable<WordPair> expected = new WordPair[] {
                new WordPair() { Text = "quick brown", Count = 1 },
                new WordPair() { Text = "brown fox", Count = 1 }
            };

            List<string> words = histogram.SplitString("quick brown fox");
            IEnumerable<WordPair> actual = histogram.GetWordPairGroups(words);

            Assert.That(expected.Select(wpSelector), Is.EquivalentTo(actual.Select(wpSelector)));
        }

        [Test]
        public void WordPairGroups_ExpectedAndActualSame()
        {
            IEnumerable<WordPair> expected = new WordPair[] {
                new WordPair() { Text = "the quick", Count = 2 },
                new WordPair() { Text = "quick brown", Count = 1 },
                new WordPair() { Text = "brown fox", Count = 1 },
                new WordPair() { Text = "fox and", Count = 1 },
                new WordPair() { Text = "and the", Count = 1 },
                new WordPair() { Text = "quick blue", Count = 1 },
                new WordPair() { Text = "blue hare", Count = 1 }
            };

            List<string> words = histogram.SplitString("The quick brown fox and the quick blue hare.");
            IEnumerable<WordPair> actual = histogram.GetWordPairGroups(words);

            Assert.That(expected.Select(wpSelector), Is.EquivalentTo(actual.Select(wpSelector)));
        }

    }
}
