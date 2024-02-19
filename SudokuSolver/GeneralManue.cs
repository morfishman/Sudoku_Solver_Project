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
    public class GeneralManue : IManue
    {
        private readonly IInputProvider<string> inputProvider;
        private readonly string filePath;

        public GeneralManue(IInputProvider<string> inputProvider, string filePath)
        {
            this.inputProvider = inputProvider;
            this.filePath = filePath;
        }

        public void Wellcom()
        {
            Console.WriteLine("Welcome to Sudoku Solver!\n");
            Console.WriteLine("Choose an option:\n");
            Console.WriteLine("1. Solve Sudoku\n");
            Console.WriteLine("2. Exit\n");

            int choice = GetChoice();
            HandleChoice(choice);
        }

        private int GetChoice()
        {
            int choice;
            while (!int.TryParse(inputProvider.ReadInput(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.\n");
            }
            return choice;
        }

        private void HandleChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    SolveSudoku();
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.\n");
                    Wellcom(); // Re-display the menu if the choice is invalid
                    break;
            }
        }

        private void SolveSudoku()
        {
            // Implement the Sudoku solving logic here
            Console.WriteLine("Enter the Sudoku board:");
            string temp = inputProvider.ReadInput();
            if(temp.Length > 25 * 25)
            {
                Console.WriteLine("Cant Solve This Board!");
                Wellcom();
            }
            IDataParser<string, List<List<IBoardCell>>> dataParser = new StringToMatrixParser(temp);
            try
            {
                List<List<IBoardCell>> board = dataParser.ParseData();
                ISudokuBoard sudokuBoard = new MatrixSudokuBoard(board);
                IStandardSudokuRelations sudokuRelations = new StandardSudokuRelationsRunTime(sudokuBoard.BoardSize());
                IStandardSudokuSolver sudokuSolver = new RuleBasedStandardSudokuSolver(sudokuRelations, sudokuBoard);
                var watch = new Stopwatch();
                watch.Start();
                bool solved = sudokuSolver.Solve();
                watch.Stop();
                if (!solved)
                {
                    Console.WriteLine("Cant Solve This Board!");
                }
                else
                {

                    string buffer = sudokuSolver.ToString();
                    Console.WriteLine(buffer);
                    IOutputProvider fileWriter = new TextFileOutputProvider(filePath, buffer);
                    fileWriter.WriteOutput();

                }
                Console.WriteLine($"Elapsed time: {watch.Elapsed}");
                Wellcom();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine($"Exeption: {e.Message}");
                Wellcom();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exeption: {e.Message}");
                Wellcom();
            }
     
            // You'll need to mplement the Sudoku solving algorithm or method here
        }
    }
}
