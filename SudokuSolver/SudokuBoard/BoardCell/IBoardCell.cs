using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuBoard.BoardCell
{
    public interface IBoardCell
    {
        const int BOARD_CELL_EMPTY = 0; 
        bool Is_Option_Exists(int Option);
        bool Remove_Option(int Option);
        bool Retrive_Option(int Option);
        void Set_Current_Value(int Value);
        bool Is_Permint();
        int Count_Options();
        int Get_Current_Value();
        byte[] Get_Options();
        void Remove_Options(byte[] Options);
        void Set_Options(byte[] Options);
    }
}
