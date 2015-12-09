using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Projekt_BD.Models;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private SHA1 sha;
        private byte[] adminPass;
        private bool sprawdzHaslo = false;

        public Login()
        {
            InitializeComponent();
            sha = new SHA1Managed();
            adminPass = sha.ComputeHash(Encoding.ASCII.GetBytes("admin"));
        }

        private void bLogowanie_Click(object sender, RoutedEventArgs e)
        {
            var passHash = sha.ComputeHash(Encoding.ASCII.GetBytes(passwordBox.Password));
            sprawdzHaslo = SprawdzHaslo(passwordBox.Password, loginBox.Text);
            if (sprawdzHaslo)
            {
                Close();
            }
            else
            {
                InvalidLoginData.Content = "Nieprawidlowe dane logowania";
            }

        }
        public bool SprawdzHaslo(String haslo, String login)
        {
            using (var context = new DbContext())
            {
                var query = context.Uzytkownicy.Where(s => s.Haslo == haslo);

                var uzytkownik = query.FirstOrDefault<Uzytkownik>();

                if (uzytkownik == null)
                {
                    return false;
                }
                else if (uzytkownik.Login == login)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            if (sprawdzHaslo)
                this.Close();
            else
                Application.Current.Shutdown();
        }
    }
}
