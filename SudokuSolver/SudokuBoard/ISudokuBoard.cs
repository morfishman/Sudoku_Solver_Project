using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard.BoardCell;


namespace SudokuSolver.SudokuBoard
{
    public interface ISudokuBoard
    {
        public int Board_Size();
        IBoardCell Get_Cell_By_Index(int row, int col);
    }
}
