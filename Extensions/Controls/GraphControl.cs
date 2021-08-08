﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Extensions
{
    public class GraphControl : FrameworkElement
    {
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register("Series", typeof(ObservableCollection<Chart>), typeof(GraphControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public GraphControl()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Series = new ObservableCollection<Chart>
                {
                    new Chart() { ChartBrush = Brushes.Red, ChartValue = 10, Description = "Data1" },
                    new Chart() { ChartBrush = Brushes.Green, ChartValue = 20, Description = "Data2" },
                    new Chart() { ChartBrush = Brushes.Orange, ChartValue = 15, Description = "Data3" }
                };
            }
        }

        public ObservableCollection<Chart> Series
        {
            get => (ObservableCollection<Chart>)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Series is not null)
            {
                double max = Series.Max(z => z.ChartValue);
                Pen pen = null;
                DrawingGroup graph = null;

                for (int i = 0; i < Series.Count; i++)
                {
                    Chart item = Series[i];
                    pen = new Pen(item.ChartBrush, ActualWidth / Series.Count);
                    pen.Freeze();

                    graph = new()
                    {
                        Transform = new RotateTransform
                        {
                            Angle = 180,
                            CenterX = (ActualWidth - (pen.Thickness / 2)) / 2,
                            CenterY = ActualHeight / 2
                        }
                    };
                    using (DrawingContext dcgraph = graph.Open())
                    {
                        dcgraph.DrawLine(pen, new Point(i * ActualWidth / Series.Count, 0.0), new Point(i * ActualWidth / Series.Count, Series[i].ChartValue / max * ActualHeight));
                        drawingContext.DrawDrawing(graph);
                    }

                    drawingContext.DrawText(new FormattedText(item.Description, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 12, item.ChartBrush), new Point(0, i * 16));
                    graph.Freeze();
                }
            }
        }

        public class Chart : InpcBase
        {
            private Brush chartBrush = Brushes.Gray;

            private double chartValue;

            private string description;

            public Brush ChartBrush
            {
                get => chartBrush;

                set
                {
                    if (chartBrush != value)
                    {
                        chartBrush = value;
                        OnPropertyChanged(nameof(ChartBrush));
                    }
                }
            }

            public double ChartValue
            {
                get => chartValue;

                set
                {
                    if (chartValue != value)
                    {
                        chartValue = value;
                        OnPropertyChanged(nameof(ChartValue));
                    }
                }
            }

            public string Description
            {
                get => description;

                set
                {
                    if (description != value)
                    {
                        description = value;
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }
        }
    }
}