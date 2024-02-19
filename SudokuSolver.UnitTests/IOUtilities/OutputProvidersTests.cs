using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SudokuSolver.IOUtilities.Output;

namespace SudokuSolver.UnitTests.IOUtilities
{
    [TestClass]
    public class OutputProvidersTests
    {
        [TestMethod]
        public void ConsoleOutputProvider_WriteToConsole_DataWrittenToConsole()
        {
            // Arrange
            string output = "Test output";
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                var consoleOutputProvider = new ConsoleOutputProvider(output);
                consoleOutputProvider.WriteOutput();
                string result = stringWriter.ToString().Trim(); 

                // Assert
                Assert.AreEqual(output, result);
            }
        }


    }
}
