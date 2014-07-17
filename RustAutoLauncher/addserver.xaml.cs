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
using System.Windows.Shapes;

namespace RustAutoLauncher
{
    /// <summary>
    /// Interaction logic for addserver.xaml
    /// </summary>
    public partial class addserver : Window
    {
        public String name;
        public String host;
        public String port;

        public addserver()
        {
            InitializeComponent();
        }

        public addserver(String name, String host, String port)
        {
            InitializeComponent();
            this.name = name;
            servernamebox.Text = name;
            this.host = host;
            hostbox.Text = host;
            this.port = port;
            portbox.Text = port;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void addserverbutton_Click(object sender, RoutedEventArgs e)
        {
            if (servernamebox.Text != "" && hostbox.Text != "" && portbox.Text != "")
            {
                name = servernamebox.Text;
                host = hostbox.Text;
                port = portbox.Text;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Servername, address and port all have to be filled in.  Please fill in all three boxes.");
            }
        }

        private void cancelbutton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
