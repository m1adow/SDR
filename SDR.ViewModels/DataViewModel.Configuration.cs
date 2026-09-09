using SDR.Models.Settings;

namespace SDR.ViewModels;

public partial class DataViewModel
{
    private readonly SignalSettings signalSettings;

    public SignalSettings SignalSettings => signalSettings;
}