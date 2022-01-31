﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using QicRecVisualizer.WpfCore;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal sealed class MatrixAdapter : ViewModelBase
    {
        private readonly Array2D _array2D;
        public string MatrixDisplayName { get; }
        public CellAdapter[] MatrixCells { get; }

        public int ColumnsCount { get; }
        public int RowsCount { get; }

        public MatrixAdapter(string displayName, Array2D array2D, int threshold)
        {
            _array2D = array2D;
            MatrixDisplayName = displayName;
            ColumnsCount = array2D.Columns;
            RowsCount = array2D.Rows;
            MatrixCells = array2D.GetValues().Select(v => new CellAdapter(v, threshold)).ToArray();
        }

        public void RefreshCells(int newThreshold)
        {
            var isValidMatrix = true;
            foreach (var cell in MatrixCells)
            {
                if (!cell.RefreshColor(newThreshold))
                {
                    isValidMatrix = false;
                }
            }

            IsValidMatrix = isValidMatrix;
        }

        private bool _isValidMatrix;

        public bool IsValidMatrix
        {
            get => _isValidMatrix;
            set => SetProperty(ref _isValidMatrix, value);
        }
    }

    internal sealed class CellAdapter : ViewModelBase
    {
        private readonly int _absValue;
        private Brush _cellColor;
        private bool _isValidCell;
        public int CurrentCellValue { get; }

        public CellAdapter(int currentCellValue, int initialThreshold)
        {
            CurrentCellValue = currentCellValue;
            _absValue = Math.Abs(currentCellValue);
            RefreshColor(initialThreshold);
        }

        public Brush CellColor
        {
            get => _cellColor;
            private set => SetProperty(ref _cellColor, value);
        }

        public bool IsValidCell
        {
            get => _isValidCell;
            private set => SetProperty(ref _isValidCell, value);
        }

        /// <summary>
        /// Refresh cell color and return true if the cell is valid
        /// </summary>
        public bool RefreshColor(int threshold)
        {
            var absDifference = _absValue - threshold;
            CellColor = MatrixColorGetter.GetCellColor(absDifference);
            IsValidCell = absDifference <= 0;
            return IsValidCell;
        }
    }
}