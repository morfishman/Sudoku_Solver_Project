using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SudokuSolver.IOUtilities.Input;

namespace SudokuSolver.UnitTests
{
    [TestClass]
    public class InputProvidersTests
    {
        [TestMethod]
        public void CarReadFromConsole_NothingWrote_ReturnsEmptyString()
        {
            // Arrange
            string input = "Test input";
            using (var stringReader = new StringReader(input))
            {
                Console.SetIn(stringReader);

                // Act
                var consoleInputProvider = new ConsoleInputProvider();
                string result = consoleInputProvider.ReadInput();

                // Assert
                Assert.AreEqual(input, result);
            }
        }
    }
}
