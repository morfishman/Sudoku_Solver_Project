using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.IOUtilities
{
    interface IInputProvider<inputType>
    {
        inputType ReadInput();
        static IInputProvider<inputType> Instance;
    }
}
