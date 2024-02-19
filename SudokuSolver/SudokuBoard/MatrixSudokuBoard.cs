using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard.BoardCell;


namespace SudokuSolver.SudokuBoard
{
    public class MatrixSudokuBoard : ISudokuBoard
    {

        private List<List<IBoardCell>> sudokuBoard;
        private int boardSize;


        /// <summary>
        /// Initializes a new instance of the MatrixSudokuBoard class.
        /// </summary>
        /// <param name="sudokuBoard">the board instance</param>
        /// <param name="BoardSize">the sudoku size</param>
        /// <exception cref="ArgumentException">Thrown when the matrix empty or not same cols as rows.</exception>
        public MatrixSudokuBoard(List<List<IBoardCell>> sudokuBoard)
        {
            if(sudokuBoard is null)
            {
                throw new ArgumentException("Sudoku Board cant be null");
            }
            else if (sudokuBoard.Count != sudokuBoard[0].Count)
            {
                throw new ArgumentException("Sudoku Board cols must be same as rows");
            }
            this.sudokuBoard = sudokuBoard;
            this.boardSize = sudokuBoard.Count;
            
        }

        /// <summary>
        /// returns the size of the matrix board
        /// </summary> 
        public int BoardSize()
        {
            return boardSize;
        }

        /// <summary>
        /// returns IBoardCell corispond to the [row][col]
        /// </summary>
        /// <param name="col">the board col</param>
        /// <param name="row">the sudoku row</param>
        public IBoardCell GetCellByIndex(int row, int col)
        {
            return sudokuBoard[row][col];
        }


    }
}
