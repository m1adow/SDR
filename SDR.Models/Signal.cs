using System;

namespace SDR.Models;

public readonly struct Signal(float frequency, float strength)
{
    public float Frequency { get; } = frequency;
    public float Strength { get; } = strength;
    public DateTime Time { get; } = DateTime.Now;
}