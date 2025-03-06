using SDR.Models;
using SDR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Timers;

namespace SDR.Services;

public class RandomSignalDataProvider : IRandomSignalDataProvider
{
    private readonly (float min, float max) frequencyRange;
    private readonly (float min, float max) strengthRange;
    private readonly int count;
    private readonly Random random;
    private readonly Timer timer;
    private readonly Dictionary<float, Signal> signals;

    public RandomSignalDataProvider((float min, float max) frequencyRange, (float min, float max) strengthRange, int count, int frequency)
    {
        this.frequencyRange = frequencyRange;
        this.strengthRange = strengthRange;
        this.count = count;
        this.random = new Random();
        this.signals = new Dictionary<float, Signal>(count);

        timer = new Timer(TimeSpan.FromSeconds(1f / frequency));
        timer.Elapsed += OnTimerElapsed;
    }

    public event Action<Signal>? SignalReceived;

    //So in for loops we better be handling floating point errors with epsilon, but we will skip X﹏X
    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        float lowStrengthZone = (float)count / 6;
        var step = (frequencyRange.max - frequencyRange.min) / count;
        var currentFrequency = frequencyRange.min;

        for (; currentFrequency < frequencyRange.min + step * lowStrengthZone; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)(strengthRange.min / 1.5));
        }

        for (; currentFrequency < frequencyRange.max - step * lowStrengthZone; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)strengthRange.max);
        }

        for (; currentFrequency < frequencyRange.max; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)(strengthRange.min / 1.5));
        }
    }

    private void GenerateSignal(float currentFrequency, int strengthMin, int strengthMax)
    {
        if (signals.TryGetValue(currentFrequency, out var signal))
        {
            var add = random.Next(0, 2) == 0;
            var strength = signal.Strength + (add ? random.NextSingle() : -random.NextSingle()) * random.Next(2, 8);
            if (strength > strengthMax || strength < strengthMin)
            {
                var offset = random.NextSingle() * random.Next(4, 8);
                strength += strength > strengthMax ? -offset : offset;
            }
            signal = new Signal(currentFrequency, strength);
        }
        else
        {
            signal = new Signal(currentFrequency, random.Next(strengthMin, strengthMax) + random.NextSingle());
        }
        signals[currentFrequency] = signal;
        SignalReceived?.Invoke(signal);
    }

    public void StartReceiving()
        => timer.Start();

    public void StopReceiving()
        => timer.Stop();
}