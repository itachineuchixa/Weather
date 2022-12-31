using System.Windows;
using System.Windows.Input;

namespace Weather
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Login());
            manager.MainFrame = MainFrame;
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Login());
            manager.MainFrame = MainFrame;

        }

        private void MainFrame_ContentRendered(object sender, System.EventArgs e)
        {

        }
    }
}