using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuRelations
{
    public interface ISudokuRelations
    {
        HashSet<(int,int)> GetAllRelations(int row, int col);
    }
}
