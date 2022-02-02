using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QicRecVisualizer.Services.Helpers;
using QicRecVisualizer.WpfCore;
using QicRecVisualizer.WpfCore.Controls;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal sealed class MatrixAdapter : ViewModelBase
    {
        private readonly Array2D _array2D;
        public string MatrixDisplayName { get; }
        public MatrixInfo CurrentMatrixInfo { get;  }

        public MatrixAdapter(string displayName, Array2D array2D, int threshold)
        {
            _array2D = array2D;
            MatrixDisplayName = displayName;
            CurrentMatrixInfo = new MatrixInfo(array2D.Rows, array2D.Columns);
            RefreshCells(threshold);
        }

        private BitmapImage _heatMapMatrix;

        public BitmapImage HeatMapMatrix
        {
            get => _heatMapMatrix;
            private set => SetProperty(ref _heatMapMatrix, value); 
        }
        
        public int[,] GetMatrix()
        {
            return _array2D.GetMatrix();
        }

        public void RefreshCells(int thresholdValueInt)
        {
            HeatMapMatrix = BitmapHeatMap.RefreshBitmap(_array2D, thresholdValueInt);
        }
    }
}