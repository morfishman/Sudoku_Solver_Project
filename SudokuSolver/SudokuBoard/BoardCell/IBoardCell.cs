using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuBoard.BoardCell
{
    public interface IBoardCell
    {
        bool Is_Option_Exists(int Option);
        void Remove_Option(int Option);
        void Retrive_Option(int Option);
        void Set_Current_Value(int Value);
        bool Is_Permint();
    }
}
