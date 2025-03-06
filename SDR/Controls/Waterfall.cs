using DependencyPropertyGenerator;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SDR.Models;
using System;
using System.Collections.Generic;
using System.Timers;
using Windows.UI;

namespace SDR.Controls;

[DependencyProperty<UniqueReplacementNotifyCollection<Point>>("Data")]
[DependencyProperty<double>("XMin")]
[DependencyProperty<double>("XMax")]
[DependencyProperty<string>("XLabel")]
[DependencyProperty<double>("Time")]
[DependencyProperty<double>("UpdateInterval")]
[DependencyProperty<string>("YLabel")]
[DependencyProperty<double>("ValueMin")]
[DependencyProperty<double>("ValueMax")]
[DependencyProperty<bool>("IsDisplaying")]
public sealed partial class Waterfall : Control
{
    private const string PlotCanvasName = "PlotControl";

    private CanvasControl plotControl;

    private Timer updateTimer;
    private float currentTimeline;
    private float maxTime;

    private readonly List<Color> zones = [];

    private readonly (float position, Color color)[] gradientStops =
    [
        (0f, Color.FromArgb(255, 0, 0, 255)),
        (0.25f, Color.FromArgb(255, 0, 255, 255)),
        (0.5f, Color.FromArgb(255, 0, 255, 0)),
        (0.75f, Color.FromArgb(255, 255, 255, 0)),
        (1f, Color.FromArgb(255, 255, 0, 0))
    ];

    public Waterfall()
    {
        this.DefaultStyleKey = typeof(Waterfall);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        plotControl = GetTemplateChild(PlotCanvasName) as CanvasControl;
        plotControl.Draw += OnPlotControlDraw;
        this.Unloaded += OnWaterfallUnloaded;
        updateTimer = new Timer(TimeSpan.FromSeconds(UpdateInterval));
        updateTimer.Elapsed += OnUpdateTimerElapsed;
    }

    private void OnUpdateTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (currentTimeline >= maxTime)
        {
            currentTimeline = 0;
        }
        else
        {
            currentTimeline += (float)updateTimer.Interval / 1000;
        }

        plotControl.Invalidate();
    }

    partial void OnTimeChanged(double newValue)
        => maxTime = (float)newValue;

    partial void OnIsDisplayingChanged(bool newValue)
    {
        if (newValue)
        {
            updateTimer.Start();
        }
        else
        {
            updateTimer.Stop();
        }
    }

    private void OnWaterfallUnloaded(object sender, RoutedEventArgs e)
    {
        updateTimer.Elapsed -= OnUpdateTimerElapsed;
        updateTimer.Stop();
        plotControl.RemoveFromVisualTree();
        plotControl = null;
    }

    //Disappointed with performance
    private void OnPlotControlDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        var drawingSession = args.DrawingSession;

        //So I'm not that happy with using aliasing, but I'm not an expert in graphics for now. So I will use it here (。﹏。)
        args.DrawingSession.Antialiasing = CanvasAntialiasing.Aliased;

        var data = Data;
        foreach (var item in data)
        {           
            var color = InterpolateColor(GraphicsTools.Interpolate(item.Y, ValueMin, ValueMax, 1));
            zones.Add(color);
        }

        if (zones.Count > maxTime * (float)updateTimer.Interval / 10 * data.Count)
        {
            zones.RemoveRange(0, data.Count);
        }

        var pointWidth = (float)(sender.Size.Width / data.Count);
        var pointHeight = (float)(sender.Size.Height / maxTime / updateTimer.Interval * 10);
        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];

            var colorIndexOffset = zones.Count - data.Count + i;
            var x = GraphicsTools.Interpolate(item.X, XMin, XMax, sender.Size.Width);
            float drawnCurrentTimeline = 0;

            while (colorIndexOffset >= 0)
            {
                var y = GraphicsTools.Interpolate(drawnCurrentTimeline, 0, maxTime, sender.Size.Height);
                var color = zones[colorIndexOffset];
                colorIndexOffset -= data.Count;
                drawingSession.FillRectangle(x, y, pointWidth, pointHeight, color);
                drawnCurrentTimeline += (float)updateTimer.Interval / 1000;
            }
        }
    }

    private Color InterpolateColor(float value)
    {
        for (int i = 0; i < gradientStops.Length - 1; i++)
        {
            var (startPos, startColor) = gradientStops[i];
            var (endPos, endColor) = gradientStops[i + 1];

            if (value >= startPos && value <= endPos)
            {
                float amt = (value - startPos) / (endPos - startPos);
                return GraphicsTools.LerpColor(startColor, endColor, amt);
            }
        }

        return value <= 0 ? gradientStops[0].color : gradientStops[^1].color;
    }
}