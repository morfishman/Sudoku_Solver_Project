using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SudokuSolver.IOUtilities.Output
{
    public class TextFileOutputProvider: IOutputProvider
    {
        private string filePath;
        private string dataToWrite;


        /// <summary>
        /// Initializes a new instance of the TextFileOutputProvider class with the specified file path and data to write.
        /// </summary>
        /// <param name="filePath">The path to the file where the output will be written.</param>
        /// <param name="dataToWrite">The data to write to the file.</param>
        public TextFileOutputProvider(string filePath, string dataToWrite)
        {
            this.dataToWrite = dataToWrite;
            this.filePath = filePath;
        }

        /// <summary>
        /// Write the data to the initialized filePath
        /// </summary>
        ///<exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
        public void WriteOutput()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
            File.WriteAllText(filePath, dataToWrite);
        }

    }
}
