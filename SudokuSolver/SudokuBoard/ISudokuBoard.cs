using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard.BoardCell;


namespace SudokuSolver.SudokuBoard
{
    public interface ISudokuBoard
    {
        int BoardSize();
        IBoardCell GetCellByIndex(int row, int col);
    }
}
