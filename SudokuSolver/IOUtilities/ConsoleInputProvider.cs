using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SudokuSolver.IOUtilities
{
    public class ConsoleInputProvider : IInputProvider<string>
    {
        private static ConsoleInputProvider instance;
        private ConsoleInputProvider() { }

        /// <summary>
        /// singletone implimintation
        /// </summary>
        /// <returns> A object of ConsoleInputProvider</returns>
        public static ConsoleInputProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleInputProvider();
                }
                return instance;
            }
        }

        /// <summary>
        /// Reads a line of characters from the standard input stream.
        /// </summary>
        /// <returns>A string containing the line read from the input stream.</returns>
        /// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="System.Int32.MaxValue"/>.</exception>
        public string ReadInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Test the class 
        /// </summary>
        /// <param name="args"></param>
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Enter your name:");
        //    string name = ConsoleInputProvider.Instance.ReadInput();
        //    Console.WriteLine($"Hello, {name}!");
        //}
    }
}
