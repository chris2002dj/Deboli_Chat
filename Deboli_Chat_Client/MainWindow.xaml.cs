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
using Bassini_AsyncSocketLib;

namespace Deboli_Chat_Client
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AsyncSocketClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Username
        private void Txt_Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_Username.Text == "Nome utente")
            {
                Txt_Username.Text = "";
                Txt_Username.Foreground = Brushes.Black;
            }
        }
        private void Txt_Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_Username.Text == "")
            {
                Txt_Username.Text = "Nome utente";
                Txt_Username.Foreground = Brushes.Gray;
            }
        }

        // IP Address
        private void Txt_IpAddress_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_IpAddress.Text == "Indirizzo IP")
            {
                Txt_IpAddress.Text = "";
                Txt_IpAddress.Foreground = Brushes.Black;
            }
        }
        private void Txt_IpAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_IpAddress.Text == "")
            {
                Txt_IpAddress.Text = "Indirizzo IP";
                Txt_IpAddress.Foreground = Brushes.Gray;
            }
        }

        // Porta
        private void Txt_porta_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_porta.Text == "Numero porta")
            {
                Txt_porta.Text = "";
                Txt_porta.Foreground = Brushes.Black;
            }
        }
        private void Txt_porta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_porta.Text == "")
            {
                Txt_porta.Text = "Numero porta";
                Txt_porta.Foreground = Brushes.Gray;
            }
        }

        // CheckBox
        private void Chk_DefaultValue_Checked(object sender, RoutedEventArgs e)
        {
            Txt_IpAddress.Text = "127.0.0.1";
            Txt_porta.Text = "23000";
            Txt_IpAddress.Foreground = Brushes.Black;
            Txt_porta.Foreground = Brushes.Black;
        }
        private void Chk_DefaultValue_Unchecked(object sender, RoutedEventArgs e)
        {
            Txt_IpAddress.Text = "Indirizzo IP";
            Txt_porta.Text = "Numero porta";
            Txt_IpAddress.Foreground = Brushes.Gray;
            Txt_porta.Foreground = Brushes.Gray;
        }

        // Button Connetti
        private void Btn_Connetti_Click(object sender, RoutedEventArgs e)
        {
            // Controlli generali
            if (Txt_Username.Text == "Nome utente") {
                MessageBox.Show("Attenzione, devi inserire un nome utente", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                Txt_Username.Focus();
                return;
            }

            if (Txt_IpAddress.Text == "Indirizzo IP") {
                MessageBox.Show("Attenzione, devi inserire un indirizzo IP", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                Txt_IpAddress.Focus();
                return;
            }

            if (Txt_porta.Text == "Numero porta") {
                MessageBox.Show("Attenzione, devi inserire un numero di porta", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                Txt_porta.Focus();
                return;
            }

            // Creazione istanza
            client = new AsyncSocketClient();

            // Passaggio dei parametri ai metodi
            client.SetServerIPAddress(Txt_IpAddress.Text);
            client.SetServerPort(Txt_porta.Text);

            // Connessione al server
            client.ConnettiAlServer();
            client.Invia(Txt_Username.Text);

            Chat chat = new Chat();
            chat.Show();
            this.Hide();
        }
    }
}
