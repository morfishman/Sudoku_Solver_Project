using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace SudokuSolver.IOUtilities.Input
{
    public class TextFileProvider : IInputProvider<string>
    {
        private string filePath;
        private bool isSet;
        

        /// <summary>
        /// Default C'tor implimintation
        /// </summary>
        /// <returns> A object of TextFileProvider</returns>
        public TextFileProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }
            filePath = path;
            isSet = true;
        }

       
        /// <summary>
        /// Reads all characters from the file stream.
        /// </summary>
        /// <returns>A string containing the characters from the file stream.</returns>
        /// <exception cref="System.InvalidOperationException">File path is not set.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Wrong file path</exception>
        public string ReadInput()
        {
            if (!isSet)
            {
                throw new InvalidOperationException("File path is not set.");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
            return File.ReadAllText(filePath);
        }
    }
}
