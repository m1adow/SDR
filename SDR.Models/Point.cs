using SDR.Models.Interfaces;

namespace SDR.Models;

public class Point(float x, float y) : IPoint
{
    public float X => x;
    public float Y => y;
}