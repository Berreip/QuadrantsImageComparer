using System;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal sealed class TabHeaderAdapter : ViewModelBase
    {
        private bool _isSelected;
        public string DisplayHeader { get; }
        private readonly Action<TabHeaderAdapter> _onSelectedTabChanged;

        public TabHeaderAdapter(string displayHeader, Action<TabHeaderAdapter> onSelectedTabChanged)
        {
            DisplayHeader = displayHeader;
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
    }
}