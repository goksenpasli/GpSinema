using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Sinema.View
{
    /// <summary>
    /// Interaction logic for Hakkında.xaml
    /// </summary>
    public partial class Hakkında : Window
    {
        public Hakkında()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            Process.Start("https://github.com/goksenpasli/GpSinema");
        }
    }
}