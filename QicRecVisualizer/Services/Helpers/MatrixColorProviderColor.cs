using System.Drawing;

namespace QicRecVisualizer.Services.Helpers
{
    public static class MatrixColorProvider
    {
        public static Color GetColor(int absDifference)
        {
            if (absDifference <= 0)
            {
                return Color.LightGreen;
            }

            if (absDifference == 1)
            {
                return Color.FromArgb(102, 237, 90);
            }

            if (absDifference == 2)
            {
                return Color.FromArgb(127, 234, 82);
            }

            if (absDifference == 3)
            {
                return Color.FromArgb(152, 234, 84);
            }
            
            if (absDifference == 4)
            {
                return Color.FromArgb(179, 234, 77);
            }
            if (absDifference <= 10)
            {
                return Color.LightPink;
            }
            if (absDifference <= 20)
            {
                return Color.Orange;
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