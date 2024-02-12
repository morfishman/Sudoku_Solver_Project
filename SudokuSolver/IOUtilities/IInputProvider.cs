using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.IOUtilities
{
    public interface IInputProvider<inputType>
    {
        inputType ReadInput();
    }
}
