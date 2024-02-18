using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard.BoardCell;


namespace SudokuSolver.SudokuBoard
{
    public class MatrixSudokuBoard : ISudokuBoard
    {

        private List<List<IBoardCell>> SudokuBoard;
        private int BoardSize;


        /// <summary>
        /// Initializes a new instance of the MatrixSudokuBoard class.
        /// </summary>
        /// <param name="SudokuBoard">the board instance</param>
        /// <param name="BoardSize">the sudoku size</param>
        /// <exception cref="ArgumentException">Thrown when the matrix empty or not same cols as rows.</exception>
        public MatrixSudokuBoard(List<List<IBoardCell>> SudokuBoard)
        {
            if(SudokuBoard is null)
            {
                throw new ArgumentException("Sudoku Board cant be null");
            }
            else if (SudokuBoard.Count != SudokuBoard[0].Count)
            {
                throw new ArgumentException("Sudoku Board cols must be same as rows");
            }
            this.SudokuBoard = SudokuBoard;
            this.BoardSize = SudokuBoard.Count;
            
        }

        /// <summary>
        /// returns the size of the matrix board
        /// </summary> 
        public int Board_Size()
        {
            return BoardSize;
        }

        /// <summary>
        /// returns IBoardCell corispond to the [row][col]
        /// </summary>
        /// <param name="col">the board col</param>
        /// <param name="row">the sudoku row</param>
        public IBoardCell Get_Cell_By_Index(int row, int col)
        {
            return SudokuBoard[row][col];
        }
    }
}
