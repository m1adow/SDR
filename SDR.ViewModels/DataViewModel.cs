using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SDR.Models;
using SDR.Models.Interfaces;
using System.Collections.ObjectModel;

namespace SDR.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly ISignalDataProvider signalDataProvider;

    public DataViewModel(ISignalDataProvider signalDataProvider)
    {
        signalDataProvider.SignalReceived += OnSignalDataProviderSignalReceived;
        this.signalDataProvider = signalDataProvider;
    }

    public ObservableCollection<Signal> Signals { get; } = [];

    private void OnSignalDataProviderSignalReceived(Signal signal)
        => Signals.Add(signal);

    [RelayCommand]
    private void Start()
        => signalDataProvider.StartReceiving();

    [RelayCommand]
    private void Stop()
        => signalDataProvider.StopReceiving();
}