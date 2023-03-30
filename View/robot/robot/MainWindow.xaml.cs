using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //navigate to PlayerMode page
        private void PLayerMode_Click(object sender, RoutedEventArgs e)
        {
           // frame.NavigationService.Navigate(new PlayerLobby());
            frame.NavigationService.Navigate(new PlayerMode());
        }

        //navigate to ViewerMode page
        private void ViewerMode_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new ViewerMode());
        }

        //navigate to AdminLobby page
        private void AdmMode_Click(object sender, RoutedEventArgs e)
        {
          //  frame.NavigationService.Navigate(new AdminLobby());
        }

        //navigate to diary
        private void Diary_Click(object sender, RoutedEventArgs e)
        {
          //  frame.NavigationService.Navigate(new Diary1());
        }
    }
}
