using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.IOUtilities.FileInputProviders
{
    interface IFileInputProvider<fileType> : IInputProvider<fileType>
    {
        string FilePath { set; }
    }
} 
