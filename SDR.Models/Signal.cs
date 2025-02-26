using System;

namespace SDR.Models;

public readonly struct Signal(byte frequency, sbyte strength)
{
    public byte Frequency { get; } = frequency;
    public sbyte Strength { get; } = strength;
    public DateTime Time { get; } = DateTime.Now;
}