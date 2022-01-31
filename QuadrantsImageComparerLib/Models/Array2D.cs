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

        public bool EqualsArray(int[,] quadrantInfoRed)
        {
            if (quadrantInfoRed == null)
            {
                return false;
            }
            if (quadrantInfoRed.GetLength(0) != Rows)
            {
                return false;
            }
            if (quadrantInfoRed.GetLength(1) != Columns)
            {
                return false;
            }
            
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    if (_array[i, j] != quadrantInfoRed[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}