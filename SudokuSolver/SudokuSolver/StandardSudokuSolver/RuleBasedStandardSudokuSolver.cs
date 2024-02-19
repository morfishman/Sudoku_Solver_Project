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
        private IStandardSudokuRelations relations;
        private ISudokuBoard board;
        private Dictionary<int,List<(int, int)>> minimumList;
        private int boardArg;
        private int emptyCells;
        private int statusSolver;



        public RuleBasedStandardSudokuSolver(IStandardSudokuRelations relations, ISudokuBoard board)
        {
           
            statusSolver = - (board.BoardSize()/2);
            this.emptyCells = 0;
            this.board = board;
            this.relations = relations;
            this.boardArg = board.BoardSize();
            if (!IsSudokuValid())
            {
                throw new ArgumentException("sudoku is not valid");
            }
            this.minimumList = new Dictionary<int,List<(int, int)>>(this.boardArg+1);
            for (int optionCounter = 0; optionCounter <= this.boardArg; optionCounter++)
            {
                this.minimumList.Add(optionCounter, new List<(int, int)>(this.boardArg*this.boardArg));
            }
            for (int row = 0; row < this.boardArg; row++)
            {
                for (int col = 0; col < this.boardArg; col++)
                {
                    IBoardCell tempCell = this.board.GetCellByIndex(row, col);
                    int optionCount = (tempCell.IsPermint())?0: this.boardArg;
                    this.emptyCells += (optionCount > 0) ? 1 : 0;
                    this.minimumList[optionCount].Add((row, col));
                }
            }

        }


        private bool IsSudokuValid()
        {
            for (int row = 0; row < this.boardArg; row++)
            {
                for (int col = 0; col < this.boardArg; col++)
                {
                    if(HasDuplicates(this.relations.GetColumnRelations(row, col)) || HasDuplicates(this.relations.GetRowRelations(row, col)) || HasDuplicates(this.relations.GetBoxRelations(row, col)))
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
                if (this.board.GetCellByIndex(item.Item1, item.Item2).GetCurrentValue() != IBoardCell.BOARD_CELL_EMPTY 
                    && !set.Add(this.board.GetCellByIndex(item.Item1,item.Item2).GetCurrentValue()))
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
        private (int, int) FindMinimumCellCords()
        {
            foreach (var option in minimumList)
            {
                if (option.Value.Count > 0)
                {
                    return option.Value[0];
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
            (int,int) cords = FindMinimumCellCords();
            HashSet<(int, int)> neighbers = this.relations.GetAllRelations(cords.Item1, cords.Item2);
            IBoardCell minimumCell = this.board.GetCellByIndex(cords.Item1, cords.Item2);
            
            if (minimumCell.IsPermint())
            {
                this.statusSolver++;
                UpdateCells(neighbers, minimumCell.GetCurrentValue());
                this.minimumList[0].Remove(cords);
                return Solve();
            }
            

            if(true && statusSolver >= 0)
            {
                // optimal naked pattern
                for (int i = 0; i < this.boardArg; i++)
                {
                    HashSet<(int, int)> collom = this.relations.GetOnlyColumnRelations(i);
                    HashSet<(int, int)> row = this.relations.GetOnlyRowRelations(i);
                    HashSet<(int, int)> box = this.relations.GetOnlyBoxRelations(i);
                    Dictionary<byte[], List<(int, int)>> collomFinds = NakedPatternFinder(collom);
                    Dictionary<byte[], List<(int, int)>> rowFinds = NakedPatternFinder(row);
                    Dictionary<byte[], List<(int, int)>> boxFinds = NakedPatternFinder(box);
                    Impliment_Naked(collomFinds, collom);
                    Impliment_Naked(rowFinds, row);
                    Impliment_Naked(boxFinds, box);
                }
                //
                statusSolver = -1;
            }

            for (int option = 1; option <= this.boardArg; option++)
            {
                if (minimumCell.IsOptionExists(option))
                {
                    HashSet<(int, int)> updatedCells = UpdateCells(neighbers, option);

                    // naked patterns
                    bool nakedFlag = false;
                    Dictionary<(int, int), byte[]> retriveCollomTable = null;
                    Dictionary<(int, int), byte[]> retriveRowTable = null;
                    Dictionary<(int, int), byte[]> retriveBoxTable = null;
                    if (true && statusSolver < -this.boardArg)
                    {
                        nakedFlag = true;
                        HashSet<(int, int)> collom = this.relations.GetColumnRelations(cords.Item1, cords.Item2);
                        HashSet<(int, int)> row = this.relations.GetRowRelations(cords.Item1, cords.Item2);
                        HashSet<(int, int)> box = this.relations.GetBoxRelations(cords.Item1, cords.Item2);

                        Dictionary<byte[], List<(int, int)>> collomFinds = NakedPatternFinder(collom);
                        Dictionary<byte[], List<(int, int)>> rowFinds = NakedPatternFinder(row);
                        Dictionary<byte[], List<(int, int)>> boxFinds = NakedPatternFinder(box);


                        retriveCollomTable = Impliment_Naked(collomFinds, collom);
                        retriveRowTable = Impliment_Naked(rowFinds, row);
                        retriveBoxTable = Impliment_Naked(boxFinds, box);
                        statusSolver = -1;
                    }
                    //







                    this.minimumList[minimumCell.CountOptions()].Remove(cords);
                    minimumCell.SetCurrentValue(option);
                    this.emptyCells--;
                    bool statusCode = Solve();
                    if (statusCode)
                    {
                        return true;
                    }
                    else
                    {
                        
                        // naked patterns
                        if (nakedFlag)
                        {
                            Deimpliment_Naked(retriveBoxTable);
                            Deimpliment_Naked(retriveRowTable);
                            Deimpliment_Naked(retriveCollomTable);
                        }
                        //
                        RetriveCells(updatedCells, option);
                        this.emptyCells++;
                        minimumCell.SetCurrentValue(IBoardCell.BOARD_CELL_EMPTY);
                    }
                }
            }
            minimumCell.SetCurrentValue(IBoardCell.BOARD_CELL_EMPTY);
            this.statusSolver--;
            return false;
        }


        private void UpdateMinimumList((int,int) cell)
        {
            IBoardCell cellToChagne = board.GetCellByIndex(cell.Item1, cell.Item2);
            this.minimumList[cellToChagne.CountOptions() + 1].Remove((cell.Item1, cell.Item2));
            this.minimumList[cellToChagne.CountOptions()].Add((cell.Item1, cell.Item2));
        }

        private void RetriveMinimumList((int, int) cell)
        {
            IBoardCell cellToChagne = board.GetCellByIndex(cell.Item1, cell.Item2);
            this.minimumList[cellToChagne.CountOptions() - 1].Remove((cell.Item1, cell.Item2));
            this.minimumList[cellToChagne.CountOptions()].Add((cell.Item1, cell.Item2));
        }

        /// <summary>
        /// Updates the cells in the specified set by removing the given option.
        /// </summary>
        /// <param name="cellsToUpdate">The set of cell coordinates to update.</param>
        /// <param name="option">The option to remove from the cells.</param>
        /// <returns>A set containing the coordinates of updated cells.</returns>
        private HashSet<(int, int)> UpdateCells(HashSet<(int, int)> cellsToUpdate, int option)
        {
            HashSet<(int, int)> updatedCells = new HashSet<(int, int)>(cellsToUpdate.Count);
            foreach (var cell in cellsToUpdate)
            {
                IBoardCell cellToChagne = board.GetCellByIndex(cell.Item1, cell.Item2);
                if (cellToChagne.GetCurrentValue() == IBoardCell.BOARD_CELL_EMPTY)
                {
                    bool statusCode = cellToChagne.RemoveOption(option);
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
        private void RetriveCells(HashSet<(int, int)> cellsToUpdate, int option)
        {
            foreach (var cell in cellsToUpdate)
            {
                IBoardCell cellToChagne = board.GetCellByIndex(cell.Item1, cell.Item2);
                cellToChagne.RetriveOption(option);
                RetriveMinimumList(cell);
            }
        }






        private void Deimpliment_Naked(Dictionary<(int, int), byte[]>  dict)
        {
            foreach (var keyValuePair in dict)
            {
                IBoardCell cellToChagne = board.GetCellByIndex(keyValuePair.Key.Item1, keyValuePair.Key.Item2);
                this.minimumList[cellToChagne.CountOptions()].Remove((keyValuePair.Key.Item1, keyValuePair.Key.Item2));
                cellToChagne.SetOptions(keyValuePair.Value);
                this.minimumList[cellToChagne.CountOptions()].Add((keyValuePair.Key.Item1, keyValuePair.Key.Item2));
            }
        }


        

        private Dictionary<(int,int), byte[]> Impliment_Naked(Dictionary<byte[], List<(int, int)>> dict, HashSet<(int, int)> cells)
        {
            Dictionary<(int, int), byte[]> retunal = new Dictionary<(int, int), byte[]>();
            foreach (var keyValuePair in dict)
            {
                if(keyValuePair.Value.Count > 1 && keyValuePair.Value.Count < this.boardArg)
                {
                    List<(int,int)> list = new List<(int, int)>(cells);
                    list.RemoveAll(element => keyValuePair.Value.Contains(element));
                    foreach (var item in list)
                    {
                        IBoardCell cellToChagne = board.GetCellByIndex(item.Item1, item.Item2);
                        retunal.Add(item, cellToChagne.GetOptions());
                        this.minimumList[cellToChagne.CountOptions()].Remove((item.Item1, item.Item2));
                        cellToChagne.RemoveOptions(keyValuePair.Key);
                        this.minimumList[cellToChagne.CountOptions()].Add((item.Item1, item.Item2));
                    }
                    break;
                }
            }
            return retunal;
        }


        private Dictionary<byte[], List<(int, int)>> NakedPatternFinder(HashSet<(int, int)> cells)
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
            
            if (this.board.GetCellByIndex(cords.Item1, cords.Item2).GetCurrentValue() == IBoardCell.BOARD_CELL_EMPTY)
            {
                // Get the options of the Sudoku cell at the specified coordinates
                byte[] options = this.board.GetCellByIndex(cords.Item1, cords.Item2).GetOptions();
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

            for (int i = 0; i < this.boardArg; i++)
            {
                // Add horizontal line after every third row
                if (i > 0 && i % Math.Sqrt(this.boardArg) == 0)
                {
                    sb.AppendLine(new string('-', (int)(this.boardArg * 4 + Math.Sqrt(this.boardArg) - 1)));
                }

                for (int j = 0; j < this.boardArg; j++)
                {
                    // Add vertical line after every third column
                    if (j > 0 && j % Math.Sqrt(this.boardArg) == 0)
                    {
                        sb.Append("| ");
                    }

                    sb.Append((char)((char)this.board.GetCellByIndex(i, j).GetCurrentValue()+'0'));

                    // Add newline after every row
                    if (j == this.boardArg - 1)
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
