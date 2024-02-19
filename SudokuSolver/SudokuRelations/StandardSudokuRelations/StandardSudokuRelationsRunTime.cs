using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;



namespace SudokuSolver.SudokuRelations.StandardSudokuRelations
{
    public class StandardSudokuRelationsRunTime : IStandardSudokuRelations
    {
        private int size;
        private Dictionary<int, HashSet<(int, int)>> rowRelations;
        private Dictionary<int, HashSet<(int, int)>> colRelations;
        private Dictionary<int, HashSet<(int, int)>> boxRelations;
        private Dictionary<(int, int), HashSet<(int, int)>> rowRelationsByIndex;
        private Dictionary<(int, int), HashSet<(int, int)>> colRelationsByIndex;
        private Dictionary<(int, int), HashSet<(int, int)>> boxRelationsByIndex;
        private Dictionary<(int, int), HashSet<(int, int)>> allRelationsByIndex;

        public StandardSudokuRelationsRunTime(int size)
        {
            this.size = size;
            InitializeRelations();
        }

        /// <summary>
        /// Initializes the row, column, and box relations dictionaries.
        /// </summary>
        private void InitializeRelations()
        {
            rowRelations = new Dictionary<int, HashSet<(int, int)>>();
            colRelations = new Dictionary<int, HashSet<(int, int)>>();
            boxRelations = new Dictionary<int, HashSet<(int, int)>>();
            rowRelationsByIndex = new Dictionary<(int, int), HashSet<(int, int)>>();
            colRelationsByIndex = new Dictionary<(int, int), HashSet<(int, int)>>();
            boxRelationsByIndex = new Dictionary<(int, int), HashSet<(int, int)>>();
            allRelationsByIndex = new Dictionary<(int, int), HashSet<(int, int)>>();

            for (int i = 0; i < size; i++)
            {
                rowRelations[i] = new HashSet<(int, int)>(GetRowRelations(i));
                colRelations[i] = new HashSet<(int, int)>(GetColumnRelations(i));
            }

            int boxSize = (int)Math.Sqrt(size);
            for (int startRow = 0; startRow < size; startRow += boxSize)
            {
                for (int startCol = 0; startCol < size; startCol += boxSize)
                {
                    int boxIndex = startRow / boxSize * boxSize + startCol / boxSize;
                    boxRelations[boxIndex] = new HashSet<(int, int)>(GetBoxRelations(startRow, startCol, boxSize));
                }
            }
        }

        /// <summary>
        /// Gets the coordinates of all cells in the specified row.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>An enumerable collection of cell coordinates.</returns>
        private IEnumerable<(int, int)> GetRowRelations(int row)
        {
            for (int col = 0; col < size; col++)
            {
                yield return (row, col);
            }
        }


        /// <summary>
        /// Gets the coordinates of all cells in the specified column.
        /// </summary>
        /// <param name="col">The column index.</param>
        /// <returns>An enumerable collection of cell coordinates.</returns>
        private IEnumerable<(int, int)> GetColumnRelations(int col)
        {
            for (int row = 0; row < size; row++)
            {
                yield return (row, col);
            }
        }


        /// <summary>
        /// Gets the coordinates of all cells in the specified box.
        /// </summary>
        /// <param name="startRow">The starting row index of the box.</param>
        /// <param name="startCol">The starting column index of the box.</param>
        /// <param name="boxSize">The size of the box (number of rows/columns).</param>
        /// <returns>An enumerable collection of cell coordinates.</returns>
        private IEnumerable<(int, int)> GetBoxRelations(int startRow, int startCol, int boxSize)
        {
            for (int row = startRow; row < startRow + boxSize; row++)
            {
                for (int col = startCol; col < startCol + boxSize; col++)
                {
                    yield return (row, col);
                }
            }
        }


        /// <summary>
        /// Gets the coordinates of cells in the same row as the specified cell, excluding the specified cell itself.
        /// </summary>
        /// <param name="row">The row index of the specified cell.</param>
        /// <param name="col">The column index of the specified cell.</param>
        /// <returns>A hash set containing the coordinates of cells in the same row as the specified cell, excluding the specified cell itself.</returns>
        public HashSet<(int, int)> GetRowRelations(int row, int col)
        {
            if (!rowRelationsByIndex.ContainsKey((row, col)))
            {
                rowRelationsByIndex[(row, col)] = new HashSet<(int, int)>(rowRelations[row].Where(rc => rc.Item2 != col));
            }
            return rowRelationsByIndex[(row, col)];
        }


        /// <summary>
        /// Gets the coordinates of cells in the same column as the specified cell, excluding the specified cell itself.
        /// </summary>
        /// <param name="row">The row index of the specified cell.</param>
        /// <param name="col">The column index of the specified cell.</param>
        /// <returns>A hash set containing the coordinates of cells in the same column as the specified cell, excluding the specified cell itself.</returns>
        public HashSet<(int, int)> GetColumnRelations(int row, int col)
        {
            if (!colRelationsByIndex.ContainsKey((row, col)))
            {
                colRelationsByIndex[(row, col)] = new HashSet<(int, int)>(colRelations[col].Where(rc => rc.Item1 != row));
            }
            return colRelationsByIndex[(row, col)];
        }


        /// <summary>
        /// Gets the coordinates of cells in the same box as the specified cell, excluding the specified cell itself.
        /// </summary>
        /// <param name="row">The row index of the specified cell.</param>
        /// <param name="col">The column index of the specified cell.</param>
        /// <returns>A hash set containing the coordinates of cells in the same box as the specified cell, excluding the specified cell itself.</returns>
        public HashSet<(int, int)> GetBoxRelations(int row, int col)
        {
            int boxSize = (int)Math.Sqrt(size);
            int boxRowIndex = row / boxSize;
            int boxColIndex = col / boxSize;
            int boxIndex = boxRowIndex * boxSize + boxColIndex;

            if (!boxRelationsByIndex.ContainsKey((row, col)))
            {
                boxRelationsByIndex[(row, col)] = new HashSet<(int, int)>(boxRelations[boxIndex]
                                                    .Where(rc => rc.Item1 != row || rc.Item2 != col));
            }
            return boxRelationsByIndex[(row, col)];
        }

        /// <summary>
        /// Retrieves all unique relations for the specified cell, excluding the coordinates of the cell itself.
        /// </summary>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="col">The column index of the cell.</param>
        /// <returns>A set containing all unique relations for the specified cell.</returns>
        public HashSet<(int, int)> GetAllRelations(int row, int col)
        {
            if (!allRelationsByIndex.ContainsKey((row, col)))
            {
                HashSet<(int, int)> allRelations = new HashSet<(int, int)>();

                allRelations.UnionWith(GetRowRelations(row, col));
                allRelations.UnionWith(GetColumnRelations(row, col));
                allRelations.UnionWith(GetBoxRelations(row, col));

                allRelationsByIndex[(row, col)] = allRelations;
            }
            return allRelationsByIndex[(row, col)];
        }

        public HashSet<(int, int)> GetOnlyCollomRelations(int col)
        {
            return this.colRelations[col];
        }

        public HashSet<(int, int)> GetOnlyRowRelations(int row)
        {
            return this.rowRelations[row];
        }

        public HashSet<(int, int)> GetOnlyBoxRelations(int boxIndex)
        {
            return this.boxRelations[boxIndex];
        }

        /// <summary>
        /// Main method to test the StandardSudokuRelationsRunTime class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        //public static void Main(string[] args)
        //{
        //    Stopwatch stopwatch = Stopwatch.StartNew();
        //    IStandardSudokuRelations sudokuRelations = new StandardSudokuRelationsRunTime(16);

        //    int row = 0;
        //    int col = 0;

        //    Console.WriteLine($"Row relations for cell ({row},{col}): {string.Join(", ", sudokuRelations.GetRowRelations(row, col))}");
        //    Console.WriteLine($"Column relations for cell ({row},{col}): {string.Join(", ", sudokuRelations.GetColumnRelations(row, col))}");
        //    Console.WriteLine($"Box relations for cell ({row},{col}): {string.Join(", ", sudokuRelations.GetBoxRelations(row, col))}");
        //    Console.WriteLine($"All relations for cell ({row},{col}): {string.Join(", ", sudokuRelations.GetAllRelations(row, col))}");
        //    Console.WriteLine($"All relations for box: {string.Join(", ", sudokuRelations.GetOnlyBoxRelations(2))}");

        //    stopwatch.Stop();
        //    Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

        //}
    }
}

