using System;
using System.IO;
using QicRecVisualizer.Views.QuadrantsControls.RelatedVm;
using QicRecVisualizer.Views.RecValidation.Adapters;
using QicRecVisualizer.WpfCore;
using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Models;

namespace QicRecVisualizer.Views.QuadrantsControls.Adapters
{
    internal sealed class LoadedDiffFileAdapter : ViewModelBase
    {
        private readonly FileInfo _diffOriginalFile;
        private readonly Action<DiffKind, LoadedDiffFileAdapter> _onDiffSelected;
        public string DiffName { get; }
        public string DiffToolTip { get; }
        private int _firstDiffBorderThickness;
        private int _secondDiffBorderThickness;
        private bool _isSelectedAsFirstDiff;
        private bool _isSelectedAsSecondDiff;
        private int _threshold;
        private string _aoiResume;
        
        public LoadedDiffFileAdapter(FileInfo diffOriginalFile, QuadrantDiffDto diffDto, Action<DiffKind, LoadedDiffFileAdapter> onDiffSelected)
        {
            _diffOriginalFile = diffOriginalFile;
            DiffDto = diffDto;
            _onDiffSelected = onDiffSelected;
            DiffName = diffOriginalFile.Name;
            DiffToolTip = diffOriginalFile.FullName;
            Threshold = diffDto.Threshold;
                    
            AoiResume = $"AOI [COLUMS,ROWS] = [{diffDto.AoiInfo.QuadrantColumns},{diffDto.AoiInfo.QuadrantRows}] - L: {diffDto.AoiInfo.AoiLeftPercentage}% | T: {diffDto.AoiInfo.AoiTopPercentage}% | R: {diffDto.AoiInfo.AoiRightPercentage}% | B: {diffDto.AoiInfo.AoiBottomPercentage}%";
                    
            // display matrix:
            RgbMatrices = new[]
            {
                new MatrixAdapter("Red", new Array2D(diffDto.Red), diffDto.Threshold),
                new MatrixAdapter("Green", new Array2D(diffDto.Green), diffDto.Threshold),
                new MatrixAdapter("Blue", new Array2D(diffDto.Blue), diffDto.Threshold),
            };
        }

        public QuadrantDiffDto DiffDto { get; }
        
        public string AoiResume
        {
            get => _aoiResume;
            private set => SetProperty(ref _aoiResume, value);
        }

        private MatrixAdapter[] _rgbMatrices = Array.Empty<MatrixAdapter>();


        public MatrixAdapter[] RgbMatrices
        {
            get => _rgbMatrices;
            private set => SetProperty(ref _rgbMatrices, value);
        }

        public int Threshold
        {
            get => _threshold;
            private set => SetProperty(ref _threshold, value);
        }

        public bool IsSelectedAsFirstDiff
        {
            get => _isSelectedAsFirstDiff;
            set
            {
                if (SetProperty(ref _isSelectedAsFirstDiff, value))
                {
                    FirstDiffBorderThickness = value ? 1 : 0;
                    if (value)
                    {
                        _onDiffSelected.Invoke(DiffKind.ReferenceDiff, this);
                    }
                }
            }
        }

        public bool IsSelectedAsSecondDiff
        {
            get => _isSelectedAsSecondDiff;
            set
            {
                if (SetProperty(ref _isSelectedAsSecondDiff, value))
                {
                    SecondDiffBorderThickness = value ? 1 : 0; if (value)
                    {
                        _onDiffSelected.Invoke(DiffKind.ComparedDiff, this);
                    }
                }
            }
        }


        public int FirstDiffBorderThickness
        {
            get => _firstDiffBorderThickness;
            set => SetProperty(ref _firstDiffBorderThickness, value);
        }

        public int SecondDiffBorderThickness
        {
            get => _secondDiffBorderThickness;
            set => SetProperty(ref _secondDiffBorderThickness, value);
        }

        public bool IsSameFile(FileInfo file)
        {
            return file.FullName == _diffOriginalFile.FullName;
        }
    }
}