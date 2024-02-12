using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.UnitTests.IOUtilities
{
    public interface IInputProvider<inputType>
    {
        inputType ReadInput();
    }
}
