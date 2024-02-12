using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SudokuSolver.UnitTests.IOUtilities
{
    public class BineryFileProvider : IInputProvider<byte[]>
    {
        private string filePath;
        private bool isSet;
        /// <summary>
        /// Default C'tor implimintation
        /// </summary>
        /// <returns> A object of BineryFileProvider</returns>
        private BineryFileProvider(string path)
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
        public byte[] ReadInput()
        {
            if (!isSet)
            {
                throw new InvalidOperationException("File path is not set.");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }


            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                if (bytesRead != fileStream.Length)
                {
                    throw new FileLoadException("File read is not complete");
                }
                return buffer;
            }
        }
    }
}
