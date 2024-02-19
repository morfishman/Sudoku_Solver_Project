using System;

namespace SudokuSolver.IOUtilities.Input
{
    public static class InputProviderFactory
    {
        /// <summary>
        /// facrory that creats objects at runtime
        /// </summary>
        /// <typeparam name="T">type that 'IInputProvider' returns</typeparam>
        /// <param name="type">type of 'IInputProvider'</param>
        /// <param name="filePath">filePath in a case of file provider</param>
        /// <returns>IInputProvider</returns>
        /// <exception cref="System.ArgumentException">Invalid input params.</exception>
        public static IInputProvider<T> GetInputProvider<T>(InputProviderType type, string filePath = null)
        {
            switch (type)
            {
                case InputProviderType.Console:
                    if (typeof(T) == typeof(string))
                    {
                        return (IInputProvider<T>)(new ConsoleInputProvider());
                    }
                    break; // Add break statement here

                case InputProviderType.TextFile:
                    if (string.IsNullOrEmpty(filePath))
                    {
                        throw new ArgumentException("File path cannot be null or empty for TextFileProvider.");
                    }
                    if (typeof(T) == typeof(string))
                    {
                        return (IInputProvider<T>)(new TextFileProvider(filePath));
                    }
                    break; // Add break statement here
                default:
                    throw new ArgumentException("Invalid input provider type.");
            }
            throw new ArgumentException("Invalid input provider type.");
        }
    }
}
