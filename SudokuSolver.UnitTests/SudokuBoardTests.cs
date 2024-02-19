using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.DataParsers;
using SudokuSolver.SudokuBoard;
using SudokuSolver.SudokuBoard.BoardCell;

namespace SudokuSolver.UnitTests
{
    [TestClass]
    public class SudokuBoardTests
    {
        [TestMethod]
        // Add more test cases here
        public void MatrixSudokuBoard_InitializeWithValidInput_CreatesCorrectBoard()
        {

            // Arrange
            string input = "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
            List<List<IBoardCell>> boardCells = (new StringToMatrixParser(input)).ParseData();


            // Act
            ISudokuBoard sudokuBoard = new MatrixSudokuBoard(boardCells);

            // Assert
            Assert.IsNotNull(sudokuBoard);
            Assert.AreEqual(9, sudokuBoard.BoardSize());
            // Add more assertions to verify the board's properties
        }

        [TestMethod]
        public void MatrixSudokuBoard_GetCellValue_ReturnsCorrectValue()
        {
            // Arrange
            string input = "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
            List<List<IBoardCell>> boardCells = (new StringToMatrixParser(input)).ParseData();

            ISudokuBoard sudokuBoard = new MatrixSudokuBoard(boardCells);

            // Act
            int cellValue = sudokuBoard.GetCellByIndex(0,0).GetCurrentValue();

            // Assert
            Assert.AreEqual(5, cellValue);
        }


        [TestMethod]
        public void BitwiseBoardCell_RemoveOption_ReturnsCorrectValue()
        {
            // Arrange
            string input = "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
            List<List<IBoardCell>> boardCells = (new StringToMatrixParser(input)).ParseData();

            ISudokuBoard sudokuBoard = new MatrixSudokuBoard(boardCells);

            // Act
            bool deleted = sudokuBoard.GetCellByIndex(0, 2).RemoveOption(3);

            // Assert
            Assert.AreEqual(true, deleted);

            // Act
            deleted = sudokuBoard.GetCellByIndex(0, 2).RemoveOption(3);

            // Assert
            Assert.AreEqual(false, deleted);
        }




    }
}
