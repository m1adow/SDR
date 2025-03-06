using DependencyPropertyGenerator;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Numerics;
using SDR.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace SDR.Controls;

[DependencyProperty<UniqueReplacementNotifyCollection<Point>>("Data")]
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
        Data.Updated -= OnDataUpdated;
        plotControl.RemoveFromVisualTree();
        plotControl = null;
    }

    partial void OnDataChanged(UniqueReplacementNotifyCollection<Point>? oldValue, UniqueReplacementNotifyCollection<Point?> newValue)
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
            var currentCoordinates = Data[i];
            var current = new Vector2(currentCoordinates.X, currentCoordinates.Y);
            var nextCoordinates = Data[i + 1];
            var next = new Vector2(nextCoordinates.X, nextCoordinates.Y);

            var currentInterpolated = new Vector2(GraphicsTools.Interpolate(current.X, XMin, XMax, sender.Size.Width),
                (float)sender.Size.Height - GraphicsTools.Interpolate(current.Y, YMin, YMax, sender.Size.Height));
            var nextInterpolated = new Vector2(GraphicsTools.Interpolate(next.X, XMin, XMax, sender.Size.Width),
                (float)sender.Size.Height - GraphicsTools.Interpolate(next.Y, YMin, YMax, sender.Size.Height));
            args.DrawingSession.DrawLine(currentInterpolated, nextInterpolated, (Foreground as SolidColorBrush)?.Color ?? Colors.White);
        }
    }
}