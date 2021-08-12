using Sinema.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Sinema.ViewModel
{
    public class ListViewSum
    {
        public static readonly DependencyProperty SumProperty = DependencyProperty.RegisterAttached("Sum", typeof(string), typeof(ListViewSum), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty Urunler = DependencyProperty.RegisterAttached("Urunler", typeof(ObservableCollection<Urun>), typeof(ListViewSum), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Changed));

        public static string GetSum(DependencyObject obj)
        {
            return (string)obj.GetValue(SumProperty);
        }

        public static ObservableCollection<Urun> GetUrunler(DependencyObject obj)
        {
            return (ObservableCollection<Urun>)obj.GetValue(Urunler);
        }

        public static void SetSum(DependencyObject obj, string value)
        {
            obj.SetValue(SumProperty, value);
        }

        public static void SetUrunler(DependencyObject obj, ObservableCollection<Urun> value)
        {
            obj.SetValue(Urunler, value);
        }

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && d is DependencyObject listView && e.NewValue is not null)
            {
                var toplam = (e.NewValue as ObservableCollection<Urun>).Sum(z => z.ToplamFiyat);
                SetSum(listView, toplam.ToString());
            }
        }
    }
}