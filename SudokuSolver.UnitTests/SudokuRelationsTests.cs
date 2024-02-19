using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.SudokuRelations;
using SudokuSolver.SudokuRelations.StandardSudokuRelations;
using System.Linq;

namespace SudokuSolver.UnitTests
{
    [TestClass]
    public class StandardSudokuRelationsTests
    {
        

        [DataTestMethod]
        [DataRow(9)]
        [DataRow(16)]
        [DataRow(25)]
        [DataRow(36)]
        public void GetOnlyRowRelations_ReturnsCorrectRowRelations(int BoardSize)
        {
            // Arrange
            IStandardSudokuRelations relations = new StandardSudokuRelationsRunTime(BoardSize);
            for (int i = 0; i < BoardSize; i++)
            {
                HashSet<(int, int)> temp = relations.GetOnlyRowRelations(i);
                HashSet<(int, int)> expectedRowRelations = new HashSet<(int, int)>();
                for (int j = 0; j < BoardSize; j++)
                {
                    expectedRowRelations.Add((i, j));
                }
                Assert.IsTrue(temp.OrderBy(t => t).SequenceEqual(expectedRowRelations.OrderBy(t => t)));
            }
        }


        [DataTestMethod]
        [DataRow(9)]
        [DataRow(16)]
        [DataRow(25)]
        [DataRow(36)]
        public void GetOnlyCollomRelations_ReturnsCorrectCollomRelations(int BoardSize)
        {
            // Arrange
            IStandardSudokuRelations relations = new StandardSudokuRelationsRunTime(BoardSize);
            for (int i = 0; i < BoardSize; i++)
            {
                HashSet<(int, int)> temp = relations.GetOnlyCollomRelations(i);
                HashSet<(int, int)> expectedCollomRelations = new HashSet<(int, int)>();
                for (int j = 0; j < BoardSize; j++)
                {
                    expectedCollomRelations.Add((j, i));
                }
                Assert.IsTrue(temp.OrderBy(t => t).SequenceEqual(expectedCollomRelations.OrderBy(t => t)));
            }
        }


       


    }
}
