using System;
using System.Collections.Generic;
using BigramParser.Models;
using VAE.CLI.Flags;

namespace BigramParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser parser = new Parser();
            FlagValue<string> file = parser.AddStringFlag("File", "", "File to read");
            parser.Parse(args);

            if (file == null)
            {
                throw new Exception("Unable to get file information");
            }

            try
            {
                FileReader fr = new FileReader();
                string line = fr.Read(file.Value);
                Histogram histogram = new Histogram();
                IEnumerable<WordPair> wordPairs = histogram.CreateHistogram(line);
                foreach (var item in wordPairs)
                {
                    Console.WriteLine(@"""{0}"" {1}", item.Text, item.Count);
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

    }
}

