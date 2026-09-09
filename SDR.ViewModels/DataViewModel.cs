using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using SDR.Models;
using SDR.Models.Interfaces;
using SDR.Models.Settings;

namespace SDR.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly ISignalDataProvider signalDataProvider;

    [ObservableProperty]
    private bool isDisplaying;

    public DataViewModel(ISignalDataProvider signalDataProvider, IOptions<SignalSettings> signalSettings)
    {
        signalDataProvider.SignalReceived += OnSignalDataProviderSignalReceived;
        this.signalDataProvider = signalDataProvider;
        this.signalSettings = signalSettings.Value;
        Signals = new UniqueReplacementNotifyCollection<Point>(signalSettings.Value.Count);
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