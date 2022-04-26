using System;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QuadrantsImageComparerLib.Helpers;

namespace QicRecVisualizer.Views.QuadrantsControls.Adapters
{
    internal sealed class DiffDeltaDisplayAdapter : ViewModelBase
    {
        public bool IsTheSameQuadrantCount { get; }
        public MatrixAdapter[] RgbMatrices { get; }
        private readonly DifferenceDiff _currentDiff;
        private int _currentMaxDelta;
        private double _thresholdValue = -1;
        private int _thresholdValueInt;
        private bool _isValid;

        public DiffDeltaDisplayAdapter(LoadedDiffFileAdapter referenceDiffFile, LoadedDiffFileAdapter comparisonDiffFile)
        {
            _currentDiff = referenceDiffFile.DiffDto.IsValidAgainstWithDifference(comparisonDiffFile.DiffDto);
            IsValid = _currentDiff.IsValidWithCurrentThreshold;
            IsTheSameQuadrantCount = !_currentDiff.IsNotTheSameQuadrantCount;

            // display matrix:
            RgbMatrices = new[]
            {
                new MatrixAdapter("Red", _currentDiff.Red, _currentDiff.CurrentThreshold),
                new MatrixAdapter("Green", _currentDiff.Green, _currentDiff.CurrentThreshold),
                new MatrixAdapter("Blue", _currentDiff.Blue, _currentDiff.CurrentThreshold),
            };
            ThresholdValue = _currentDiff.CurrentThreshold;
        }

        private int ComputeMaxDelta()
        {
            var max = 0;
            max = ComputeMaxDeltaFromChannel(_currentDiff.Red.GetValues(), max);
            max = ComputeMaxDeltaFromChannel(_currentDiff.Green.GetValues(), max);
            max = ComputeMaxDeltaFromChannel(_currentDiff.Blue.GetValues(), max);
            return Math.Max(max - _thresholdValueInt, 0);
        }

        private static int ComputeMaxDeltaFromChannel(int[] channelValues, int currentMax)
        {
            foreach (var channelValue in channelValues)
            {
                if (channelValue > currentMax)
                {
                    currentMax = channelValue;
                }
            }

            return currentMax;
        }

        public int CurrentMaxDelta
        {
            get => _currentMaxDelta;
            private set
            {
                if (SetProperty(ref _currentMaxDelta, value))
                {
                    IsValid = value <= 0;
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public double ThresholdValue
        {
            get => _thresholdValue;
            set
            {
                if (SetProperty(ref _thresholdValue, value))
                {
                    _thresholdValueInt = (int)_thresholdValue;
                    foreach (var matrixAdapter in RgbMatrices)
                    {
                        matrixAdapter.RefreshCells(_thresholdValueInt);
                    }

                    CurrentMaxDelta = ComputeMaxDelta();
                }
            }
        }
    }
}