using System.Drawing;

namespace QicRecVisualizer.Services.Helpers
{
    public static class MatrixColorProvider
    {
        public static Color GetColor(int absDifference)
        {
            if (absDifference <= 0)
            {
                return Color.FromArgb(199, 237, 199);
            }

            if (absDifference == 1)
            {
                return Color.FromArgb(102, 237, 90);
            }

            if (absDifference == 2)
            {
                return Color.FromArgb(80, 232, 53);
            }

            if (absDifference == 3)
            {
                return Color.FromArgb(158, 206, 68);
            }
            
            if (absDifference == 4)
            {
                return Color.LightPink;
            }
            if (absDifference <= 10)
            {
                return Color.Orange;
            }
            if (absDifference <= 20)
            {
                return Color.FromArgb(198, 125, 0);
            }
            if (absDifference <= 30)
            {
                return Color.IndianRed;
            }
            if (absDifference <= 60)
            {
                return Color.Crimson;
            }
            
            if (absDifference <= 90)
            {
                return Color.DarkRed;
            }
            return Color.Black;
        }
    }
}