using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.DataParsers;
using SudokuSolver.SudokuBoard.BoardCell;

namespace SudokuSolver.UnitTests
{
    [TestClass]
    public class DataParsersTests
    {
        [DataTestMethod]
        [DataRow("1234567894567891237891234562345678915678912348912345673456789126078912345912345678")]
        [DataRow("530070000600195000098000060800060003400803001700020006060000280000419005000080079")]
        // Add more test cases here
        public void StringToMatrixParser_ParseData_ValidInput_ReturnsCorrectMatrix(string input)
        {
            // Arrange
            IDataParser<string, List<List<IBoardCell>>> parser = new StringToMatrixParser(input);
            int size = (int)Math.Sqrt((double)input.Length);

            try
            {
                // Act
                List<List<IBoardCell>> resultMatrix = parser.ParseData();
                int iterator = 0;
                // Assert
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        int value = resultMatrix[row][col].GetCurrentValue();
                        Assert.AreEqual((int)(char)(input[iterator++]-'0'), value, $"Unexpected value at position [{row},{col}]. Expected: {(int)input[iterator - 1]}, Actual: {value}");

                    }
                }
            }

            catch (Exception e)
            {
                // Assert
                Assert.AreEqual(e.GetType(),typeof(ArgumentException), $"Unexpected exception thrown: {e.Message}");
            }
        }
    }
}
