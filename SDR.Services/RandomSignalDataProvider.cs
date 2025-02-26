using SDR.Models;
using SDR.Models.Interfaces;
using System;
using System.Timers;

namespace SDR.Services;

public class RandomSignalDataProvider : IRandomSignalDataProvider
{
    private readonly (byte min, byte max) frequencyRange;
    private readonly (sbyte min, sbyte max) strengthRange;
    private readonly int count;
    private readonly Random random;
    private readonly Timer timer;

    public RandomSignalDataProvider((byte min, byte max) frequencyRange, (sbyte min, sbyte max) strengthRange, int count, int frequency)
    {
        this.frequencyRange = frequencyRange;
        this.strengthRange = strengthRange;
        this.count = count;
        this.random = new Random();

        timer = new Timer(TimeSpan.FromSeconds(frequency / 1d));
        timer.Elapsed += OnTimerElapsed;
        timer.Start();
    }

    public event Action<Signal>? SignalReceived;

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        for (int i = 0; i < count; i++)
        {
            SignalReceived?.Invoke(new Signal((byte)random.Next(frequencyRange.min, frequencyRange.max), (sbyte)random.Next(strengthRange.min, strengthRange.max)));
        }
    }

    public void StartReceiving()
        => timer.Start();

    public void StopReceiving()
        => timer.Stop();
}