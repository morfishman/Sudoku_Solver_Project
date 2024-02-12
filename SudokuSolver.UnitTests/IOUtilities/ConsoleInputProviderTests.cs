using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace SudokuSolver.UnitTests.IOUtilities
{
    [TestClass]
    public class ConsoleInputProviderTests
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
