using System;
using QicRecVisualizer.Views.QuadrantsControls.Adapters;

namespace QicRecVisualizer.Views.QuadrantsControls.RelatedVm
{
    
    internal static class SearchFilters
    {
        public static bool FilterParts(object item, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // if no filter, show all.
                return true;
            }

            if (item is LoadedDiffFileAdapter loadedDiffFileAdapter)
            {
                // contains insensitive
                return  loadedDiffFileAdapter.DiffName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }
    }
}