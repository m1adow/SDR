using Windows.UI;

namespace SDR.Models;

public static class GraphicsTools
{
    public static float Interpolate(float value, double min, double max, double canvasAxis)
        => (float)((value - min) * canvasAxis / (max - min));

    public static Color LerpColor(Color a, Color b, float amt)
        => Color.FromArgb
        (
            255,
            (byte)(a.R + (b.R - a.R) * amt),
            (byte)(a.G + (b.G - a.G) * amt),
            (byte)(a.B + (b.B - a.B) * amt)
        );
}