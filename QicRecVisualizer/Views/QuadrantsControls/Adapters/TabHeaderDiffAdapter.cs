using System;
using QicRecVisualizer.Views.QuadrantsControls.RelatedVm;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Views.QuadrantsControls.Adapters
{
    internal sealed class TabHeaderDiffAdapter : ViewModelBase
    {
        private bool _isSelected;
        public string DisplayHeader { get; }
        private readonly Action<TabHeaderDiffAdapter> _onSelectedTabChanged;
        public DiffKind Kind { get; }
        public LoadedDiffFileAdapter DiffAdapter { get; }

        public TabHeaderDiffAdapter(DiffKind kind, LoadedDiffFileAdapter diffAdapter, Action<TabHeaderDiffAdapter> onSelectedTabChanged)
        {
            DisplayHeader = $"{kind.ToDisplayString()}{diffAdapter.DiffName}";
            Kind = kind;
            DiffAdapter = diffAdapter;
            _onSelectedTabChanged = onSelectedTabChanged;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    // only if set to true
                    if (value)
                    {
                        _onSelectedTabChanged(this);
                    }
                }
            }
        }

        public bool IsNotTheSame(LoadedDiffFileAdapter diffToDisplay)
        {
            return !ReferenceEquals(DiffAdapter, diffToDisplay);
        }
    }
}