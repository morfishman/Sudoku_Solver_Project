using System;
using System.Collections.Generic;
using System.Text;
using SudokuSolver.SudokuBoard;
using SudokuSolver.SudokuRelations.StandardSudokuRelations;
using SudokuSolver.SudokuBoard.BoardCell;
using SudokuSolver.DataParsers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;



namespace SudokuSolver.SudokuSolver.StandardSudokuSolver
{
    public class RuleBasedStandardSudokuSolver: IStandardSudokuSolver
    {
        private IStandardSudokuRelations Relations;
        private ISudokuBoard Board;
        private Dictionary<int,List<(int, int)>> MinimumList;
        private int BoardArg;
        private int emptyCells;
        private int statusSolver;



        public RuleBasedStandardSudokuSolver(IStandardSudokuRelations relations, ISudokuBoard board)
        {
           
            statusSolver = - (board.Board_Size()/2);
            this.emptyCells = 0;
            this.Board = board;
            this.Relations = relations;
            this.BoardArg = board.Board_Size();
            if (!is_sudoku_valid())
            {
                throw new ArgumentException("sudoku is not valid");
            }
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


        private bool is_sudoku_valid()
        {
            for (int row = 0; row < this.BoardArg; row++)
            {
                for (int col = 0; col < this.BoardArg; col++)
                {
                    if(HasDuplicates(this.Relations.GetColumnRelations(row, col)) || HasDuplicates(this.Relations.GetRowRelations(row, col)) || HasDuplicates(this.Relations.GetBoxRelations(row, col)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private bool HasDuplicates(HashSet<(int,int)> list)
        {
            HashSet<int> set = new HashSet<int>();
            foreach (var item in list)
            {
                if (this.Board.Get_Cell_By_Index(item.Item1, item.Item2).Get_Current_Value() != IBoardCell.BOARD_CELL_EMPTY 
                    && !set.Add(this.Board.Get_Cell_By_Index(item.Item1,item.Item2).Get_Current_Value()))
                {
                    return true;
                }
            }
            return false;
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
            
            if (this.emptyCells == 0)
            {
                return true;
            }
            (int,int) cords = Find_Minimum_Cell_Cords();
            HashSet<(int, int)> neighbers = this.Relations.GetAllRelations(cords.Item1, cords.Item2);
            IBoardCell minimumCell = this.Board.Get_Cell_By_Index(cords.Item1, cords.Item2);
            
            if (minimumCell.Is_Permint())
            {
                this.statusSolver++;
                UpdateCells(neighbers, minimumCell.Get_Current_Value());
                this.MinimumList[0].Remove(cords);
                return Solve();
            }
            

            if(true && statusSolver >= 0)
            {
                // optimal naked pattern
                for (int i = 0; i < this.BoardArg; i++)
                {
                    HashSet<(int, int)> collom = this.Relations.GetOnlyColumnRelations(i);
                    HashSet<(int, int)> row = this.Relations.GetOnlyRowRelations(i);
                    HashSet<(int, int)> box = this.Relations.GetOnlyBoxRelations(i);
                    Dictionary<byte[], List<(int, int)>> collom_finds = Naked_Pattern_Finder(collom);
                    Dictionary<byte[], List<(int, int)>> row_finds = Naked_Pattern_Finder(row);
                    Dictionary<byte[], List<(int, int)>> box_finds = Naked_Pattern_Finder(box);
                    Impliment_Naked(collom_finds, collom);
                    Impliment_Naked(row_finds, row);
                    Impliment_Naked(box_finds, box);
                }
                //
                statusSolver = -1;
            }

            for (int option = 1; option <= this.BoardArg; option++)
            {
                if (minimumCell.Is_Option_Exists(option))
                {
                    HashSet<(int, int)> updatedCells = UpdateCells(neighbers, option);

                    // naked patterns
                    bool naked_flag = false;
                    Dictionary<(int, int), byte[]> retrive_collom_table = null;
                    Dictionary<(int, int), byte[]> retrive_row_table = null;
                    Dictionary<(int, int), byte[]> retrive_box_table = null;
                    if (true && statusSolver < -this.BoardArg)
                    {
                        naked_flag = true;
                        HashSet<(int, int)> collom = this.Relations.GetColumnRelations(cords.Item1, cords.Item2);
                        HashSet<(int, int)> row = this.Relations.GetRowRelations(cords.Item1, cords.Item2);
                        HashSet<(int, int)> box = this.Relations.GetBoxRelations(cords.Item1, cords.Item2);

                        Dictionary<byte[], List<(int, int)>> collom_finds = Naked_Pattern_Finder(collom);
                        Dictionary<byte[], List<(int, int)>> row_finds = Naked_Pattern_Finder(row);
                        Dictionary<byte[], List<(int, int)>> box_finds = Naked_Pattern_Finder(box);


                        retrive_collom_table = Impliment_Naked(collom_finds, collom);
                        retrive_row_table = Impliment_Naked(row_finds, row);
                        retrive_box_table = Impliment_Naked(box_finds, box);
                        statusSolver = -1;
                    }
                    //







                    this.MinimumList[minimumCell.Count_Options()].Remove(cords);
                    minimumCell.Set_Current_Value(option);
                    this.emptyCells--;
                    bool statusCode = Solve();
                    if (statusCode)
                    {
                        return true;
                    }
                    else
                    {
                        
                        // naked patterns
                        if (naked_flag)
                        {
                            Deimpliment_Naked(retrive_box_table);
                            Deimpliment_Naked(retrive_row_table);
                            Deimpliment_Naked(retrive_collom_table);
                        }
                        //
                        RetriveCells(updatedCells, option);
                        this.emptyCells++;
                        minimumCell.Set_Current_Value(IBoardCell.BOARD_CELL_EMPTY);
                    }
                }
            }
            minimumCell.Set_Current_Value(IBoardCell.BOARD_CELL_EMPTY);
            this.statusSolver--;
            return false;
        }


        private void UpdateMinimumList((int,int) cell)
        {
            IBoardCell cellToChagne = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
            this.MinimumList[cellToChagne.Count_Options() + 1].Remove((cell.Item1, cell.Item2));
            this.MinimumList[cellToChagne.Count_Options()].Add((cell.Item1, cell.Item2));
        }

        private void RetriveMinimumList((int, int) cell)
        {
            IBoardCell cellToChagne = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
            this.MinimumList[cellToChagne.Count_Options() - 1].Remove((cell.Item1, cell.Item2));
            this.MinimumList[cellToChagne.Count_Options()].Add((cell.Item1, cell.Item2));
        }

        /// <summary>
        /// Updates the cells in the specified set by removing the given option.
        /// </summary>
        /// <param name="cellsToUpdate">The set of cell coordinates to update.</param>
        /// <param name="option">The option to remove from the cells.</param>
        /// <returns>A set containing the coordinates of updated cells.</returns>
        private HashSet<(int, int)> UpdateCells(HashSet<(int, int)> cells_to_update, int option)
        {
            HashSet<(int, int)> updatedCells = new HashSet<(int, int)>(cells_to_update.Count);
            foreach (var cell in cells_to_update)
            {
                IBoardCell cellToChagne = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
                if (cellToChagne.Get_Current_Value() == IBoardCell.BOARD_CELL_EMPTY)
                {
                    bool statusCode = cellToChagne.Remove_Option(option);
                    if (statusCode)
                    {
                        updatedCells.Add(cell);
                        UpdateMinimumList(cell);
                    }
                }
            }
            return updatedCells;
        }


        /// <summary>
        /// Updates the cells in the specified set by removing the given option. using threading
        /// </summary>
        /// <param name="cellsToUpdate">The set of cell coordinates to update.</param>
        /// <param name="option">The option to remove from the cells.</param>
        /// <returns>A set containing the coordinates of updated cells.</returns>
        //private HashSet<(int, int)> UpdateCells(HashSet<(int, int)> cellsToUpdate, int option)
        //{
        //    HashSet<(int, int)> updatedCells = new HashSet<(int, int)>(cellsToUpdate.Count);
        //    object lockObject = new object();

        //    Parallel.ForEach(cellsToUpdate, cell =>
        //    {
        //        IBoardCell cellToChange = Board.Get_Cell_By_Index(cell.Item1, cell.Item2);
        //        if (cellToChange.Get_Current_Value() == IBoardCell.BOARD_CELL_EMPTY)
        //        {
        //            bool statusCode;
        //            lock (lockObject)
        //            {
        //                statusCode = cellToChange.Remove_Option(option);
        //            }
        //            if (statusCode)
        //            {
        //                lock (lockObject)
        //                {
        //                    updatedCells.Add(cell);
        //                    UpdateMinimumList(cell);
        //                }
        //            }
        //        }
        //    });

        //    return updatedCells;
        //}



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
                RetriveMinimumList(cell);
            }
        }






        private void Deimpliment_Naked(Dictionary<(int, int), byte[]>  dict)
        {
            foreach (var keyValuePair in dict)
            {
                IBoardCell cellToChagne = Board.Get_Cell_By_Index(keyValuePair.Key.Item1, keyValuePair.Key.Item2);
                this.MinimumList[cellToChagne.Count_Options()].Remove((keyValuePair.Key.Item1, keyValuePair.Key.Item2));
                cellToChagne.Set_Options(keyValuePair.Value);
                this.MinimumList[cellToChagne.Count_Options()].Add((keyValuePair.Key.Item1, keyValuePair.Key.Item2));
            }
        }


        

        private Dictionary<(int,int), byte[]> Impliment_Naked(Dictionary<byte[], List<(int, int)>> dict, HashSet<(int, int)> cells)
        {
            Dictionary<(int, int), byte[]> retunal = new Dictionary<(int, int), byte[]>();
            foreach (var keyValuePair in dict)
            {
                if(keyValuePair.Value.Count > 1 && keyValuePair.Value.Count < this.BoardArg)
                {
                    List<(int,int)> list = new List<(int, int)>(cells);
                    list.RemoveAll(element => keyValuePair.Value.Contains(element));
                    foreach (var item in list)
                    {
                        IBoardCell cellToChagne = Board.Get_Cell_By_Index(item.Item1, item.Item2);
                        retunal.Add(item, cellToChagne.Get_Options());
                        this.MinimumList[cellToChagne.Count_Options()].Remove((item.Item1, item.Item2));
                        cellToChagne.Remove_Options(keyValuePair.Key);
                        this.MinimumList[cellToChagne.Count_Options()].Add((item.Item1, item.Item2));
                    }
                    break;
                }
            }
            return retunal;
        }


        private Dictionary<byte[], List<(int, int)>> Naked_Pattern_Finder(HashSet<(int, int)> cells)
        {
            Dictionary<byte[], List<(int, int)>> optionDict = new Dictionary<byte[], List<(int, int)>>();
            
            foreach (var cord in cells)
            {
                AddToDictionary(ref optionDict, cord); 
            }

           

            return optionDict;
        }


        private void AddToDictionary(ref Dictionary<byte[], List<(int, int)>> optionDict, (int, int) cords)
        {
            
            if (this.Board.Get_Cell_By_Index(cords.Item1, cords.Item2).Get_Current_Value() == IBoardCell.BOARD_CELL_EMPTY)
            {
                // Get the options of the Sudoku cell at the specified coordinates
                byte[] options = this.Board.Get_Cell_By_Index(cords.Item1, cords.Item2).Get_Options();
                // Check if the dictionary already contains an entry with the same options
                if (!optionDict.ContainsKey(options))
                {
                    // If not, add a new entry with the options as the key and a new list containing the coordinates
                    optionDict[options] = new List<(int, int)> { cords };
                }
                else
                {
                    // If the dictionary already contains an entry with the same options, add the coordinates to the existing list
                    optionDict[options].Add(cords);
                }
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

                    sb.Append((char)((char)this.Board.Get_Cell_By_Index(i, j).Get_Current_Value()+'0'));

                    // Add newline after every row
                    if (j == this.BoardArg - 1)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }



        //public static void Main(string[] args)
        //{
        //    IStandardSudokuRelations relations = new StandardSudokuRelationsRunTime(9);
        //    IDataParser<string, List<List<IBoardCell>>> dataParser = new StringToMatrixParser("900800000000000500000000000020010003010000060000400070708600000000030100400000200");
        //    ISudokuBoard sudokuBoard = new MatrixSudokuBoard(dataParser.ParseData());
        //    ISudokuSolver sudokuSolver = new RuleBasedStandardSudokuSolver(relations, sudokuBoard);
        //    Stopwatch stopwatch = Stopwatch.StartNew();
        //    Console.WriteLine(sudokuSolver);
        //    sudokuSolver.Solve();
        //    stopwatch.Stop();
        //    Console.WriteLine(sudokuSolver);
        //    Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

        //}
    }
}
