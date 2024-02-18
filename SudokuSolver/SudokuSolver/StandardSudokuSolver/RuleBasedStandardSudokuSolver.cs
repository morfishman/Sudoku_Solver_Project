using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard;
using SudokuSolver.SudokuRelations.StandardSudokuRelations;
using SudokuSolver.SudokuBoard.BoardCell;


namespace SudokuSolver.SudokuSolver.StandardSudokuSolver
{
    public class RuleBasedStandardSudokuSolver: IStandardSudokuSolver
    {
        private IStandardSudokuRelations Relations;
        private ISudokuBoard Board;
        private Dictionary<int,List<(int, int)>> MinimumList;
        private int BoardArg;
        private int emptyCells;


        public RuleBasedStandardSudokuSolver(IStandardSudokuRelations relations, ISudokuBoard board)
        {
            this.emptyCells = 0;
            this.Board = board;
            this.Relations = relations;
            this.BoardArg = board.Board_Size();
            this.MinimumList = new Dictionary<int,List<(int, int)>>(this.BoardArg+1);
            for (int optionCounter = 0; optionCounter <= this.BoardArg; optionCounter++)
            {
                this.MinimumList.Add(optionCounter, new List<(int, int)>(this.BoardArg*this.BoardArg));
            }
            for (int row = 0; row < this.BoardArg; row++)
            {
                for (int col = 0; col < this.BoardArg; col++)
                {
                    IBoardCell tempCell = this.Board.Get_Cell_By_Index(row, col);
                    int optionCount = (tempCell.Is_Permint())?0: this.BoardArg;
                    this.emptyCells += (optionCount > 0) ? 1 : 0;
                    this.MinimumList[optionCount].Add((row, col));
                }
            }

        }

        /// <summary>
        /// Finds the coordinates of the cell with the minimum number of options.
        /// </summary>
        /// <returns>The coordinates of the cell with the minimum number of options.</returns>
        private (int, int) Find_Minimum_Cell_Cords()
        {
            foreach (var Option in MinimumList)
            {
                if (Option.Value.Count > 0)
                {
                    return Option.Value[0];
                }
            }
            return (-1, -1);
        }


        /// <summary>
        /// Solves the Sudoku puzzle recursively using backtracking.
        /// </summary>
        /// <returns>True if the puzzle is solved, otherwise false.</returns>
        public bool Solve()
        {
            if(this.emptyCells == 0)
            {
                return true;
            }
            (int,int) cords = Find_Minimum_Cell_Cords();

            HashSet<(int, int)> neighbers = this.Relations.GetAllRelations(cords.Item1, cords.Item2);
            IBoardCell minimumCell = this.Board.Get_Cell_By_Index(cords.Item1, cords.Item2);
            
            if (minimumCell.Is_Permint())
            {
                UpdateCells(neighbers, minimumCell.Get_Current_Value());
                this.MinimumList[0].Remove(cords);
                return Solve();
            }
            
            for (int option = 1; option <= this.BoardArg; option++)
            {
                if (minimumCell.Is_Option_Exists(option))
                {
                    HashSet<(int, int)> updatedCells = UpdateCells(neighbers, option);
                    this.MinimumList[minimumCell.Count_Options()].Remove(cords);
                    minimumCell.Set_Current_Value(option);
                    bool statusCode = Solve();
                    if (statusCode)
                    {
                        return true;
                    }
                    else
                    {
                        minimumCell.Remove_Option(option);
                        if (minimumCell.Count_Options()  < 1)
                        {
                            return false;
                        }
                        RetriveCells(updatedCells, option);
                        minimumCell.Set_Current_Value(IBoardCell.BOARD_CELL_EMPTY);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the cells in the specified set by removing the given option.
        /// </summary>
        /// <param name="cellsToUpdate">The set of cell coordinates to update.</param>
        /// <param name="option">The option to remove from the cells.</param>
        /// <returns>A set containing the coordinates of updated cells.</returns>
        private HashSet<(int,int)> UpdateCells(HashSet<(int,int)> cells_to_update,int option)
        {
            HashSet<(int, int)> updatedCells= new HashSet<(int, int)>(cells_to_update.Count);
            foreach (var cell in cells_to_update)
            {
                IBoardCell cellToChagne = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
                bool statusCode = cellToChagne.Remove_Option(option);
                if (statusCode)
                {
                    updatedCells.Add(cell);
                }
            }
            return updatedCells;
        }

        /// <summary>
        /// Retrieves the given option in the cells specified by the set.
        /// </summary>
        /// <param name="cellsToUpdate">The set of cell coordinates to update.</param>
        /// <param name="option">The option to retrieve in the cells.</param>
        private void RetriveCells(HashSet<(int, int)> cells_to_update, int option)
        {
            foreach (var cell in cells_to_update)
            {
                IBoardCell cellToChagne = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
                cellToChagne.Retrive_Option(option);
            }
        }

        /// <summary>
        /// Returns a string representation of the Sudoku board.
        /// </summary>
        /// <returns>A string representing the Sudoku board.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.BoardArg; i++)
            {
                // Add horizontal line after every third row
                if (i > 0 && i % Math.Sqrt(this.BoardArg) == 0)
                {
                    sb.AppendLine(new string('-', (int)(this.BoardArg * 4 + Math.Sqrt(this.BoardArg) - 1)));
                }

                for (int j = 0; j < this.BoardArg; j++)
                {
                    // Add vertical line after every third column
                    if (j > 0 && j % Math.Sqrt(this.BoardArg) == 0)
                    {
                        sb.Append("| ");
                    }

                    sb.Append($"{this.Board.Get_Cell_By_Index(i, j)} ");

                    // Add newline after every row
                    if (j == this.BoardArg - 1)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }

    }
}
