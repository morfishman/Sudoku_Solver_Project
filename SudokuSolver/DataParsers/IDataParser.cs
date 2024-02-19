using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.DataParsers
{
    public interface IDataParser<Ttoparse,Ttoreturn>
    {
        Ttoreturn ParseData();
    }
}
