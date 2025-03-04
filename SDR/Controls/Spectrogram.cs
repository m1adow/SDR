using DependencyPropertyGenerator;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Numerics;
using SDR.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI;

namespace SDR.Controls;

[DependencyProperty<UniqueReplacementNotifyCollection<float, float>>("Data")]
[DependencyProperty<double>("XMin")]
[DependencyProperty<double>("XMax")]
[DependencyProperty<string>("XLabel")]
[DependencyProperty<double>("YMin")]
[DependencyProperty<double>("YMax")]
[DependencyProperty<string>("YLabel")]
public sealed partial class Spectrogram : Control
{
    private const string PlotCanvasName = "PlotControl";

    private CanvasControl plotControl;

    public Spectrogram()
    {
        this.DefaultStyleKey = typeof(Spectrogram);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        plotControl = GetTemplateChild(PlotCanvasName) as CanvasControl;
        plotControl.Draw += OnPlotControlDraw;
        this.Unloaded += OnSpectrogramUnloaded;
    }

    private void OnSpectrogramUnloaded(object sender, RoutedEventArgs e)
    {
        plotControl.RemoveFromVisualTree();
        plotControl = null;
        Data.Updated -= OnDataUpdated;
    }

    partial void OnDataChanged(UniqueReplacementNotifyCollection<float, float>? oldValue, UniqueReplacementNotifyCollection<float, float>? newValue)
    {
        if (oldValue != null)
        {
            oldValue.Updated -= OnDataUpdated;
        }

        if (newValue != null)
        {
            newValue.Updated += OnDataUpdated;
        }
    }

    private void OnDataUpdated()
        => plotControl.Invalidate();

    private void OnPlotControlDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        for (int i = 0; i < Data.Count - 1; i++)
        {
            var color = Colors.White;
            var currentCoordinates = Data[i];
            var current = new Vector2(currentCoordinates.Key, currentCoordinates.Value);
            var nextCoordinates = Data[i + 1];
            var next = new Vector2(nextCoordinates.Key, nextCoordinates.Value);

            var currentInterpolated = new Vector2(GraphicsTools.Interpolate(current.X, XMin, XMax, sender.Size.Width),
                (float)sender.Size.Height - GraphicsTools.Interpolate(current.Y, YMin, YMax, sender.Size.Height));
            var nextInterpolated = new Vector2(GraphicsTools.Interpolate(next.X, XMin, XMax, sender.Size.Width),
                (float)sender.Size.Height - GraphicsTools.Interpolate(next.Y, YMin, YMax, sender.Size.Height));
            args.DrawingSession.DrawLine(currentInterpolated, nextInterpolated, color);
        }
    }
}