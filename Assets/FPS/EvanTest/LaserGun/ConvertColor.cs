using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public static class ConvertColor
    {
        public static Color ConvertColorRGB(LaserColor currentColor)
        {
            //convert currentColor string to RGB value
            int r = 0;
            int g = 0;
            int b = 0;
            if (currentColor == LaserColor.RED)
            {
                r = 1;
                g = 0;
                b = 0;
            }
            if (currentColor == LaserColor.GREEN)
            {
                r = 0;
                g = 1;
                b = 0;
            }
            if (currentColor == LaserColor.BLUE)
            {
                r = 0;
                g = 0;
                b = 1;
            }
            if (currentColor == LaserColor.WHITE)
            {
                r = 1;
                g = 1;
                b = 1;
            }
            if (currentColor == LaserColor.YELLOW)
            {
                r = 1;
                g = 1;
                b = 0;
            }
            if (currentColor == LaserColor.MAGENTA)
            {
                r = 1;
                g = 0;
                b = 1;
            }

            if (currentColor == LaserColor.CYAN)
            {
                r = 0;
                g = 1;
                b = 1;
            }

            Color myColor = new Color(r, g, b, 1);
            return myColor;
        }
    }
}