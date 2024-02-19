using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuBoard.BoardCell
{
    public interface IBoardCell
    {
        const int BOARD_CELL_EMPTY = 0; 
        bool IsOptionExists(int Option);
        bool RemoveOption(int Option);
        bool RetriveOption(int Option);
        void SetCurrentValue(int Value);
        bool IsPermint();
        int CountOptions();
        int GetCurrentValue();
        byte[] GetOptions();
        void RemoveOptions(byte[] Options);
        void SetOptions(byte[] Options);
    }
}
