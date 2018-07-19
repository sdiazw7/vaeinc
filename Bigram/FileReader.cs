using System;
using System.IO;

namespace BigramParser
{
    public class FileReader 
    {
        /// <summary>
        /// Reads an input file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>string</returns>
        public string Read(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("The file path must not be empty");
            }

            String line = string.Empty;
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string
                    line = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw new Exception("The file could not be read: " + e.Message);
            }
            return line;
        }
    }

}
