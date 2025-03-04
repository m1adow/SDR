using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SDR.Models;
using SDR.Models.Interfaces;

namespace SDR.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly ISignalDataProvider signalDataProvider;

    public DataViewModel(ISignalDataProvider signalDataProvider, int maxSignalsCount)
    {
        signalDataProvider.SignalReceived += OnSignalDataProviderSignalReceived;
        this.signalDataProvider = signalDataProvider;
        Signals = new UniqueReplacementNotifyCollection<float, float>(maxSignalsCount);
    }

    public UniqueReplacementNotifyCollection<float, float> Signals { get; }

    private void OnSignalDataProviderSignalReceived(Signal signal)
        => Signals.Add(signal.Frequency, signal.Strength);

    [RelayCommand]
    private void Start()
        => signalDataProvider.StartReceiving();

    [RelayCommand]
    private void Stop()
        => signalDataProvider.StopReceiving();
}