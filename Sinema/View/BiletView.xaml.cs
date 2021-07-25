using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Sinema.View
{
    /// <summary>
    /// Interaction logic for BiletView.xaml
    /// </summary>
    public partial class BiletView : UserControl
    {
        public BiletView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(FdBilet, "Bilet Yazdır.");
            }
        }
    }
}
