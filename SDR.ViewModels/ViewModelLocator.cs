using CommunityToolkit.Mvvm.DependencyInjection;

namespace SDR.ViewModels;

public class ViewModelLocator
{
    public static DataViewModel DataViewModel => Ioc.Default.GetRequiredService<DataViewModel>();
}