using System;

namespace SDR.Models.Interfaces;

public interface ISignalDataProvider
{
    event Action<Signal> SignalReceived;
    void StartReceiving();
    void StopReceiving();
}