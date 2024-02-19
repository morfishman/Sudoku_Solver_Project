using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver.SudokuBoard.BoardCell
{
    public class BitwiseBoardCell: IBoardCell
    {
        private byte[] options; 
        private bool permint;
        private int currentValue;
        private int optionsAmount;


        /// <summary>
        /// Initializes a new instance of the BitwiseBoardCell class.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="permint">is it changeble</param>
        /// <param name="numOptions">The number of options.</param>
        /// <exception cref="ArgumentException">Thrown when the number of options is less than or equal to zero.</exception>
        public BitwiseBoardCell(int currentValue, bool permint, int numOptions)
        {
            if (numOptions <= 0)
            {
                throw new ArgumentException("Number of options must be greater than zero.");
            }

            this.permint = permint;
            this.currentValue = currentValue;

            int numBytes = (numOptions + 7) / 8;
            this.options = new byte[numBytes];
            this.optionsAmount = 0;

            if (!permint)
            {
                for (int i = 1; i <= numOptions; i++)
                {
                    RetriveOption(i);
                }
            }
        }


        /// <summary>
        /// Sets to 1 the specified option in the Options byte array.
        /// </summary>
        /// <param name="option">The option to set.</param>
        /// <returns>status code if added</returns>
        public bool RetriveOption(int option)
        {
            option -= 1;
            bool returnFlag = true;
            int byteIndex = option / 8; 
            int bitIndex = option % 8;
            byte mask = (byte)(1 << bitIndex);
            if((options[byteIndex] & mask) != 0)
            {
                returnFlag = false;
            }
            else
            {
                options[byteIndex] |= mask;
                this.optionsAmount++;
            }
            return returnFlag;
        }


        /// <summary>
        /// Checks if the specified option exists.
        /// </summary>
        /// <param name="option">The option to check.</param>
        /// <returns>True if the option exists, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified option is negative or zero.</exception>
        public bool IsOptionExists(int option)
        {
            if (option <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(option), "Option must be non-negative.");
            }
            option -= 1;
            int byteIndex = option / 8;
            int bitIndex = option % 8;

            if (byteIndex >= options.Length)
            {
                return false; 
            }

            byte mask = (byte)(1 << bitIndex);
            return (options[byteIndex] & mask) != 0;
        }


        /// <summary>
        /// Removes the specified option.
        /// </summary>
        /// <param name="option">The option to remove.</param>
        /// <returns>status code if Removed</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified option index is negative or zero.</exception>
        public bool RemoveOption(int option)
        {
            option -= 1;
            bool returnFlag = true;
            int byteIndex = option / 8;
            int bitIndex = option % 8;

            byte mask = (byte)(1 << bitIndex);

            if ((options[byteIndex] & mask) == 0)
            {
                returnFlag = false;
            }
            else
            {
                mask = (byte)~mask;
                options[byteIndex] &= mask;
                this.optionsAmount--;
            }
            return returnFlag;
        }

        /// <summary>
        /// Sets the current value of the cell.
        /// </summary>
        /// <param name="value">The new value to set.</param>
        public void SetCurrentValue(int value)
        {
            this.currentValue = value;
        }


        /// <summary>
        /// Gets the status of the cell.
        /// </summary>
        /// <returns>True if the cell changable, otherwise false.</returns>
        public bool IsPermint()
        {
            return this.permint;
        }


        /// <summary>
        /// Gets the amount of options in each cell.
        /// </summary>
        /// <returns>True if the cell changable, otherwise false.</returns>
        public int CountOptions()
        {
            return this.optionsAmount;
        }

        public int GetCurrentValue()
        {
            return this.currentValue;
        }



        public byte[] GetOptions()
        {
            return this.options;
        }

        public void RemoveOptions(byte[] options)
        {
            this.optionsAmount = 0;
            for (int i = 0; i < options.Length; i++)
            {
                this.options[i] &= (byte)~options[i];
                for (int j = 0; j < 8; j++)
                {
                    if (((this.options[i] >> j) & 1) == 1)
                    {
                        this.optionsAmount++;
                    }
                }
            }
            
          

        }

        public void SetOptions(byte[] options)
        {
            this.optionsAmount = 0;
            for (int i = 0; i < options.Length; i++)
            {
                this.options[i] = options[i];
                for (int j = 0; j < 8; j++)
                {
                    if (((this.options[i] >> j) & 1) == 1)
                    {
                        this.optionsAmount++;
                    }
                }
            }
        }
    }
}
