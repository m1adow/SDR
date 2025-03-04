namespace SDR.Models;

public static class GraphicsTools
{
    public static float Interpolate(float value, double min, double max, double canvasAxis)
        => (float)((value - min) * canvasAxis / (max - min));
}