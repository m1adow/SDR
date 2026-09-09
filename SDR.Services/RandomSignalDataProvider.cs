using Microsoft.Extensions.Options;
using SDR.Models;
using SDR.Models.Interfaces;
using SDR.Models.Settings;
using System;
using System.Collections.Generic;
using System.Timers;

namespace SDR.Services;

public class RandomSignalDataProvider : IRandomSignalDataProvider
{
    private const int LowZoneDivisor = 6;
    private const double LowChangeDivisor = 1.5;
    private const int MaxRandomMultiplier = 8;
    private const int MinRandomMultiplier = 2;
    private const int IntermediateRandomMultiplier = 4;

    private readonly (float min, float max) frequencyRange;
    private readonly (float min, float max) strengthRange;
    private readonly int count;
    private readonly Random random;
    private readonly Timer timer;
    private readonly Dictionary<float, Signal> signals;

    public RandomSignalDataProvider(IOptions<SignalSettings> signalSettings)
    {
        this.frequencyRange = (signalSettings.Value.FrequencyMin, signalSettings.Value.FrequencyMax);
        this.strengthRange = (signalSettings.Value.StrengthMin, signalSettings.Value.StrengthMax);
        this.count = signalSettings.Value.Count;
        this.random = new Random();
        this.signals = new Dictionary<float, Signal>(count);

        timer = new Timer(TimeSpan.FromSeconds(1f / signalSettings.Value.Frequency));
        timer.Elapsed += OnTimerElapsed;
    }

    public event Action<Signal>? SignalReceived;

    //So in for loops we better be handling floating point errors with epsilon, but we will skip X﹏X
    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        float lowStrengthZone = (float)count / LowZoneDivisor;
        var step = (frequencyRange.max - frequencyRange.min) / count;
        var currentFrequency = frequencyRange.min;

        for (; currentFrequency < frequencyRange.min + step * lowStrengthZone; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)(strengthRange.min / LowChangeDivisor));
        }

        for (; currentFrequency < frequencyRange.max - step * lowStrengthZone; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)strengthRange.max);
        }

        for (; currentFrequency < frequencyRange.max; currentFrequency += step)
        {
            GenerateSignal(currentFrequency, (int)strengthRange.min, (int)(strengthRange.min / LowChangeDivisor));
        }
    }

    private void GenerateSignal(float currentFrequency, int strengthMin, int strengthMax)
    {
        if (signals.TryGetValue(currentFrequency, out var signal))
        {
            var add = random.Next(0, 2) == 0;
            var strength = signal.Strength + (add ? random.NextSingle() : -random.NextSingle()) * random.Next(MinRandomMultiplier, MaxRandomMultiplier);
            if (strength > strengthMax || strength < strengthMin)
            {
                var offset = random.NextSingle() * random.Next(IntermediateRandomMultiplier, MaxRandomMultiplier);
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