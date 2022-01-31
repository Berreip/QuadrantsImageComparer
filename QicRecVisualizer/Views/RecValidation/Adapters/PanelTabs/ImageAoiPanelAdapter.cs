using QicRecVisualizer.Views.RecValidation.RelatedVm;
using QicRecVisualizer.WpfCore;

namespace QicRecVisualizer.Views.RecValidation.Adapters.PanelTabs
{
    internal sealed class ImageAoiPanelAdapter : ViewModelBase, IDisplayPanelAdapter
    {
        public IImageDisplayer ImageDisplayer { get; }

        public ImageAoiPanelAdapter(IImageDisplayer imageDisplayer)
        {
            ImageDisplayer = imageDisplayer;
        }
    }
}