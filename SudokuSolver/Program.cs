using System;
using System.Collections.Generic;
using SudokuSolver.IOUtilities.Input;
using SudokuSolver.IOUtilities.Output;
using SudokuSolver.DataParsers;
using SudokuSolver.SudokuBoard.BoardCell;
using SudokuSolver.SudokuBoard;
using SudokuSolver.SudokuRelations.StandardSudokuRelations;
using SudokuSolver.SudokuSolver.StandardSudokuSolver;
using System.Diagnostics;

namespace SudokuSolver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IManue manue = new GeneralManue(new ConsoleInputProvider(), "C:\\Users\\User\\source\\repos\\SudokuSolver\\SudokuSolver\\CurBoard.txt");
            manue.Wellcom();
        }
    }
}
