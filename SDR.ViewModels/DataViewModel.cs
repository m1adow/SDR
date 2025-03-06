using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SDR.Models;
using SDR.Models.Interfaces;

namespace SDR.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly ISignalDataProvider signalDataProvider;

    [ObservableProperty]
    private bool isDisplaying;

    public DataViewModel(ISignalDataProvider signalDataProvider, int maxSignalsCount)
    {
        signalDataProvider.SignalReceived += OnSignalDataProviderSignalReceived;
        this.signalDataProvider = signalDataProvider;
        Signals = new UniqueReplacementNotifyCollection<Point>(maxSignalsCount);
    }

    public UniqueReplacementNotifyCollection<Point> Signals { get; }

    private void OnSignalDataProviderSignalReceived(Signal signal)
        => Signals.Add(new Point(signal.Frequency, signal.Strength));

    [RelayCommand]
    private void Start()
    {
        signalDataProvider.StartReceiving();
        IsDisplaying = true;
    }

    [RelayCommand]
    private void Stop()
    {
        signalDataProvider.StopReceiving();
        IsDisplaying = false;
    }
}