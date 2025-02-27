using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.Foundation;
using DependencyPropertyGenerator;
using System;

namespace SDR.Controls;

[DependencyProperty<ObservableCollection<Point>>("Data")]
[DependencyProperty<Tuple<float, float>>("X")]
[DependencyProperty<Tuple<float, float>>("Y")]
public sealed partial class Spectrogram : Control
{
    private const string PlotCanvasName = "PlotCanvas";

    private Canvas plot;

    public Spectrogram()
    {
        this.DefaultStyleKey = typeof(Spectrogram);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        plot = GetTemplateChild(PlotCanvasName) as Canvas;
    }

    partial void OnDataChanged(ObservableCollection<Point>? newValue)
    {
    }
}