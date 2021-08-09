using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Extensions
{
    public class GraphControl : Control
    {
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register("Series", typeof(ObservableCollection<Chart>), typeof(GraphControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        private const string PART_Lb = "PART_Lb";

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));
        }

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

        public ItemsControl GraphText { get; private set; }

        public ObservableCollection<Chart> Series
        {
            get => (ObservableCollection<Chart>)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GraphText = GetTemplateChild(PART_Lb) as ItemsControl;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Series?.Any() == true)
            {
                double max = Series.Max(z => z.ChartValue);
                Pen pen = null;
                DrawingGroup graph = null;

                for (int i = 1; i <= Series.Count; i++)
                {
                    Chart item = Series[i - 1];
                    pen = new Pen(item.ChartBrush, ActualWidth / Series.Count);
                    pen.Freeze();

                    graph = new();
                    using (DrawingContext dcgraph = graph.Open())
                    {
                        dcgraph.DrawLine(pen, new Point((pen.Thickness * i) - (pen.Thickness / 2), ActualHeight), new Point((pen.Thickness * i) - (pen.Thickness / 2), ActualHeight - (Series[i - 1].ChartValue / max * ActualHeight)));
                        drawingContext.DrawDrawing(graph);
                    }
                    graph.Freeze();
                }
                GraphText.ItemsSource = Series;
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