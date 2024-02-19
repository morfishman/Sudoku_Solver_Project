using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard.BoardCell;

namespace SudokuSolver.DataParsers
{
    public class StringToMatrixParser: IDataParser<string,List<List<IBoardCell>>>
    {
        private string dataToParse;

        public StringToMatrixParser(string dataToParse)
        {
            this.dataToParse = dataToParse;
        }


        /// <summary>
        /// Parses the input data and returns a list of lists of IBoardCell representing the Sudoku board.
        /// </summary>
        /// <returns>A list of lists of IBoardCell representing the Sudoku board.</returns>
        /// <exception cref="ArgumentException">Thrown when the input data length does not form a perfect square, or when an invalid number is found in the board.</exception>
        public List<List<IBoardCell>> ParseData()
        {
            double lengthTest = Math.Sqrt((double)dataToParse.Length);
            if(lengthTest%1 != 0)
            {
                throw new ArgumentException("not valid board");
            }
            int sudokuSize = (int)lengthTest;
            List<List<IBoardCell>> boardCells = new List<List<IBoardCell>>(sudokuSize);
            int iterator = 0;
            for (int i = 0; i < sudokuSize; i++)
            {
                boardCells[i] = new List<IBoardCell>(sudokuSize);
                for (int j = 0; j < sudokuSize; j++)
                {
                    int number = dataToParse[iterator] - '0';
                    if(number > sudokuSize)
                    {
                        throw new ArgumentException(String.Format("not valid number in board {0}", number));
                    }
                    bool permint = (number != IBoardCell.BOARD_CELL_EMPTY) ? true : false;
                    boardCells[i][j] = new BitwiseBoardCell(number, permint, sudokuSize);
                }
            }
            return boardCells;
        }
    }
}
