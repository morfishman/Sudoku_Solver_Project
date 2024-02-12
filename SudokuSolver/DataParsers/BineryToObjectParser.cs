using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace SudokuSolver.DataParsers
{
    public class BineryToObjectParser<Treturnobj> : IDataParser<byte[], Treturnobj>
    {
        private byte[] dataToParse;

        BineryToObjectParser(byte[] dataToParse)
        {
            this.dataToParse = dataToParse;
        }


        /// <summary>
        /// Deserializes a byte array into an object of type Treturnobj.
        /// </summary>
        /// <param name="dataToParse">The byte array to deserialize.</param>
        /// <typeparam name="Treturnobj">The type of object to deserialize into.</typeparam>
        /// <returns>The deserialized object of type Treturnobj.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if dataToParse is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Thrown if there is an issue during deserialization.</exception>
        /// <exception cref="System.InvalidCastException">Thrown if the deserialized object is not of the expected type Treturnobj.</exception>
        public Treturnobj ParseData()
        {
            if (dataToParse == null)
                throw new ArgumentNullException(nameof(dataToParse));

            using (MemoryStream memoryStream = new MemoryStream(dataToParse))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object deserializedObject = formatter.Deserialize(memoryStream);

                // Check if the deserialized object is of the expected type
                if (deserializedObject is Treturnobj)
                {
                    return (Treturnobj)deserializedObject;
                }
                else
                {
                    throw new InvalidCastException($"Deserialized object is not of type {typeof(Treturnobj)}");
                }
            }

        }
    }
}
