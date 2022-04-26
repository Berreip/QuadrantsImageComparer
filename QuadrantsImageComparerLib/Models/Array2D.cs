using System;

namespace QuadrantsImageComparerLib.Models
{
    public sealed class Array2D
    {
        private readonly int[,] _array;

        public Array2D(int[,] array)
        {
            _array = array;
            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
        }

        public int Rows { get; }
        public int Columns { get; }

        public int GetValue(int rowIndex, int columnIndex)
        {
            return _array[rowIndex, columnIndex];
        }

        public int[] GetValues()
        {
            var concatenate = new int[Rows*Columns];
            for (var i = 0; i < Rows; i++)
            {
                var columnsDelta = i * Columns;
                for (var j = 0; j < Columns; j++)
                {
                    concatenate[columnsDelta + j] = _array[i, j];
                }
            }

            return concatenate;
        }

        public int[,] GetMatrix()
        {
            return (int[,])_array.Clone();
        }

        /// <summary>
        /// Returns true if array2d are equals with a tolerance (if threshold == 0 => strictly equals) 
        /// </summary>
        public bool EqualsArray(int[,] quadrantInfoInput, int threshold)
        {
            if (quadrantInfoInput == null)
            {
                return false;
            }
            if (quadrantInfoInput.GetLength(0) != Rows)
            {
                return false;
            }
            if (quadrantInfoInput.GetLength(1) != Columns)
            {
                return false;
            }
            
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    if (Math.Abs(_array[i, j] - quadrantInfoInput[i, j]) > threshold)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// Returns true if array2d are equals with a tolerance (if threshold == 0 => strictly equals)
        /// and out the delta diff matrix (without taking into account the threshold)
        /// </summary>
        public bool EqualsArrayAndGetDifference(int[,] quadrantInfoInput, int threshold, out int[,] delta)
        {
            if (quadrantInfoInput == null)
            {
                delta = default;
                return false;
            }
            if (quadrantInfoInput.GetLength(0) != Rows)
            {
                delta = default;
                return false;
            }
            if (quadrantInfoInput.GetLength(1) != Columns)
            {
                delta = default;
                return false;
            }

            delta = new int[Rows, Columns];
            var arrayEquals = true;
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    var deltaCell = Math.Abs(_array[i, j] - quadrantInfoInput[i, j]);
                    
                    delta[i, j] = deltaCell;
                    
                    if (deltaCell > threshold)
                    {
                        arrayEquals = false;
                    }
                }
            }
            return arrayEquals;
        }
    }
}