using QicRecVisualizer.Views.RecValidation.Adapters;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Services.Helpers
{
    internal static class WpfMatrixHelper
    {
        public static CellAdapter[] ComputeCellAdaptersFromArray2d(this Array2D array2D, int threshold)
        {
            var matrix = new CellAdapter[array2D.Rows*array2D.Columns];
            for (var i = 0; i < array2D.Rows; i++)
            {
                var columnsDelta = i * array2D.Columns;
                for (var j = 0; j < array2D.Columns; j++)
                {
                    matrix[columnsDelta + j] = new CellAdapter(array2D.GetValue(i, j), threshold, i, j);
                }
            }
            return matrix;
        }
    }
}