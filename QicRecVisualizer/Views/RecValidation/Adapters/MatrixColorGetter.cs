using System.Windows.Media;

namespace QicRecVisualizer.Views.RecValidation.Adapters
{
    internal static class MatrixColorGetter
    {
        public static Brush GetCellColor(int absDifference)
        {
            if (absDifference <= 0)
            {
                return Brushes.LightGreen;
            }

            if (absDifference <= 10)
            {
                return Brushes.LightPink;
            }
            if (absDifference <= 20)
            {
                return Brushes.Orange;
            }
            if (absDifference <= 30)
            {
                return Brushes.IndianRed;
            }
            return Brushes.DarkRed;
        }
    }
}