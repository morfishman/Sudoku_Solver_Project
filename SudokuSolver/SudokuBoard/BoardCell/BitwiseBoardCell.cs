using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuBoard.BoardCell
{
    public class BitwiseBoardCell: IBoardCell
    {
        private byte[] Options; 
        private bool Permint;
        private int Current_Value;


        /// <summary>
        /// Initializes a new instance of the BitwiseBoardCell class.
        /// </summary>
        /// <param name="Current_Value">The current value.</param>
        /// <param name="Permint">is it changeble</param>
        /// <param name="numOptions">The number of options.</param>
        /// <exception cref="ArgumentException">Thrown when the number of options is less than or equal to zero.</exception>
        public BitwiseBoardCell(int Current_Value, bool Permint, int numOptions)
        {
            if (numOptions <= 0)
            {
                throw new ArgumentException("Number of options must be greater than zero.");
            }

            this.Permint = Permint;
            this.Current_Value = Current_Value;

            int numBytes = (numOptions + 7) / 8;
            this.Options = new byte[numBytes];

            if (!Permint)
            {
                for (int i = 0; i < numOptions; i++)
                {
                    Retrive_Option(i);
                }
            }
        }


        /// <summary>
        /// Sets to 1 the specified option in the Options byte array.
        /// </summary>
        /// <param name="Option">The option to set.</param>
        public void Retrive_Option(int Option)
        {
            Option -= 1;
            int byteIndex = Option / 8; 
            int bitIndex = Option % 8;

            byte mask = (byte)(1 << bitIndex); 
            Options[byteIndex] |= mask; 
        }


        /// <summary>
        /// Checks if the specified option exists.
        /// </summary>
        /// <param name="Option">The option to check.</param>
        /// <returns>True if the option exists, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified option is negative or zero.</exception>
        public bool Is_Option_Exists(int Option)
        {
            if (Option <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Option), "Option must be non-negative.");
            }
            Option -= 1;
            int byteIndex = Option / 8;
            int bitIndex = Option % 8;

            if (byteIndex >= Options.Length)
            {
                return false; 
            }

            byte mask = (byte)(1 << bitIndex);
            return (Options[byteIndex] & mask) != 0;
        }


        /// <summary>
        /// Removes the specified option.
        /// </summary>
        /// <param name="Option">The option to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified option index is negative or zero.</exception>
        public void Remove_Option(int Option)
        {
            Option -= 1;
            int byteIndex = Option / 8;
            int bitIndex = Option % 8;

            byte mask = (byte)~(1 << bitIndex);
            Options[byteIndex] &= mask;
        }

        /// <summary>
        /// Sets the current value of the cell.
        /// </summary>
        /// <param name="Value">The new value to set.</param>
        public void Set_Current_Value(int Value)
        {
            this.Current_Value = Value;
        }


        /// <summary>
        /// Gets the status of the cell.
        /// </summary>
        /// <returns>True if the cell changable, otherwise false.</returns>
        public bool Is_Permint()
        {
            return this.Permint;
        }


    }
}
