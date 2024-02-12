using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace SudokuSolver.IOUtilities.FileInputProviders
{
    public class TextFileProvider : IFileInputProvider<string>
    {
        private static TextFileProvider instance;
        private string filePath;
        private bool isSet;


        /// <summary>
        /// Update the 'filePath' property
        /// </summary>
        /// <exception cref="System.ArgumentException">if File path is null or empty</exception>
        public string FilePath
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("File path cannot be null or empty.");
                }
                filePath = value;
                isSet = true;
            }
        }

        /// <summary>
        /// Default C'tor implimintation
        /// </summary>
        /// <returns> A object of ConsoleInputProvider</returns>
        private TextFileProvider()
        {
            isSet = false;
            filePath = "";
        }

        /// <summary>
        /// singletone implimintation
        /// </summary>
        /// <returns> A object of ConsoleInputProvider</returns>
        public static TextFileProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TextFileProvider();
                }
                return instance;
            }
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

        /// <summary>
        /// Test the class 
        /// </summary>
        /// <param name="args"></param>
        //static void Main(string[] args)
        //{
        //    IInputProvider<string> textProvider = TextFileProvider.Instance;
        //    //textProvider.FilePath = @"C:\path\to\your\file";
        //    try
        //    {
        //        string text = textProvider.ReadInput();
        //        Console.WriteLine($"File content: {text}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}
    }
}
