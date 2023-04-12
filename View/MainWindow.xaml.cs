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
using View.ViewModel;

namespace View
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

        /*private void ButtonVisib()
        {
            btnPlay.Visibility = Visibility.Collapsed;
        }*/

        //navigate to PlayerMode page
        

        //navigate to ViewerMode page
        public void ViewerMode_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ViewerMode();

        }

        //navigate to AdminLobby page
        private void AdmMode_Click(object sender, RoutedEventArgs e)
        {
            //  this.Content = new AdminLobby();
        }

        //navigate to diary
        private void Diary_Click(object sender, RoutedEventArgs e)
        {
            //  this.Content = (new Diary1());
        }

        
    }
}
