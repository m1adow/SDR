using SDR.Models;
using SDR.Models.Interfaces;
using System;
using System.Timers;

namespace SDR.Services;

public class RandomSignalDataProvider : IRandomSignalDataProvider
{
    private readonly (float min, float max) frequencyRange;
    private readonly (float min, float max) strengthRange;
    private readonly int count;
    private readonly Random random;
    private readonly Timer timer;

    public RandomSignalDataProvider((float min, float max) frequencyRange, (float min, float max) strengthRange, int count, int frequency)
    {
        this.frequencyRange = frequencyRange;
        this.strengthRange = strengthRange;
        this.count = count;
        this.random = new Random();

        timer = new Timer(TimeSpan.FromSeconds(1f / frequency));
        timer.Elapsed += OnTimerElapsed;
        timer.Start();
    }

    public event Action<Signal>? SignalReceived;

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        var currentFrequency = frequencyRange.min;
        var frequencyStep = (frequencyRange.max - frequencyRange.min) / count;
        for (int i = 0; i < count; i++)
        {
            var signal = new Signal(currentFrequency, random.Next((int)strengthRange.min, (int)strengthRange.max) + random.NextSingle());
            SignalReceived?.Invoke(signal);
            currentFrequency += frequencyStep;
        }
    }

    public void StartReceiving()
        => timer.Start();

    public void StopReceiving()
        => timer.Stop();
}