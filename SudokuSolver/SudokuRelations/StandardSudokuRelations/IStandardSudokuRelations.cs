using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuRelations.StandardSudokuRelations
{
    public interface IStandardSudokuRelations: ISudokuRelations
    {
        HashSet<(int, int)> GetRowRelations(int row, int col);
        HashSet<(int, int)> GetColumnRelations(int row, int col);
        HashSet<(int, int)> GetBoxRelations(int row, int col);
        HashSet<(int, int)> GetOnlyColumnRelations(int col);
        HashSet<(int, int)> GetOnlyRowRelations(int row);
        HashSet<(int, int)> GetOnlyBoxRelations(int boxIndex);


    }
}
