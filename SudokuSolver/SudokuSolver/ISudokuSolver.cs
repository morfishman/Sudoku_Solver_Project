using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuSolver
{
    public interface ISudokuSolver
    {
        bool Solve();
        string ToString();
    }
}
