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
using System.Xml;
using System.Data;
using System.IO;

namespace RustAutoLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerManagement servermanagement;

        public MainWindow()
        {
            InitializeComponent();
            servermanagement = new ServerManagement();
            bindServerDropDown();
        }

        private void bindServerDropDown(String selected = null)
        {
            XmlNode selectednode = (XmlNode) serverdropdown.SelectedItem;

            serverdropdown.ItemsSource = servermanagement.getServerList();
            if(serverdropdown.Items.Count > 0)
            {
                if (serverdropdown.Items.Contains(selectednode) && selectednode != null)
                {
                    serverdropdown.SelectedItem = selectednode;
                }
                else
                {
                    serverdropdown.SelectedIndex = 0;
                }
            }
        }
        private void closelabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minimizelabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void connectbutton_Click(object sender, RoutedEventArgs e)
        {
            XmlNode server = (XmlNode) serverdropdown.SelectedItem;
            RustConnector rust = new RustConnector(server.Attributes["server"].Value.ToString(), server.Attributes["port"].Value.ToString());
            try
            {
                rust.connect(true);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addserverbutton_Click(object sender, RoutedEventArgs e)
        {
            addserverwindow();
        }

        private void addserverwindow(String name = null, String host = null, String port = null)
        {
            addserver addserver = new addserver(name, host, port);
            if (addserver.ShowDialog().Value)
            {
                try
                {
                    servermanagement.addServer(addserver.name, addserver.host, addserver.port);
                    bindServerDropDown();
                }
                catch (DuplicateNameException)
                {
                    MessageBox.Show(String.Format("A server with name {0} already exist.  Please choose a different name.", addserver.name));
                    addserverwindow(addserver.name, addserver.host, addserver.port);
                }
            }
        }
        private void deleteserverbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String servername = ((XmlElement)serverdropdown.SelectedItem).InnerText.ToString();
                try
                {
                    servermanagement.removeServer(servername);
                    MessageBox.Show(String.Format("The server {0} has been deleted.", servername));
                    bindServerDropDown();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("The server has already been deleted.");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a server from the dropdown list.");
            }
            
        }

        private void launchtomenubutton_Click(object sender, RoutedEventArgs e)
        {
            RustConnector rust = new RustConnector();
            try
            {
                rust.connect(false);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
