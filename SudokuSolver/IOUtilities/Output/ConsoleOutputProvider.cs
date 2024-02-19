using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.IOUtilities.Output
{
    public class ConsoleOutputProvider:IOutputProvider
    {
        private String dataToWrite;


        /// <summary>
        /// Initializes a new instance of the ConsoleOutputProvider class with the specified data to write.
        /// </summary>
        /// <param name="dataToWrite">The data to write to the console.</param>
        public ConsoleOutputProvider(string dataToWrite)
        {
            this.dataToWrite = dataToWrite;
        }

        /// <summary>
        /// Write the data to write to the console.
        /// </summary>
        public void WriteOutput()
        {
            Console.Write(dataToWrite);
        }
    }
}
