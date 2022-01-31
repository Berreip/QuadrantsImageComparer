using System.Windows;
using System.Windows.Controls;
using QicRecVisualizer.Views.RecValidation.Adapters.PanelTabs;

namespace QicRecVisualizer.Views.DataTemplates
{
    public sealed class MainPanelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImagePanel { get; set; }
        public DataTemplate ResultPanel { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case ResultPanelAdapter _:
                    return ResultPanel;
                case ImageAoiPanelAdapter _:
                    return ImagePanel;
                default:
                    return null;
            }
        }
    }
}