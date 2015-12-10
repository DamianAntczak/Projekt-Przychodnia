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
using System.ComponentModel;
using System.Windows.Threading;

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
        readonly BackgroundWorker worker = new BackgroundWorker();
        public Login()
        {
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            InitializeComponent();
            sha = new SHA1Managed();
            adminPass = sha.ComputeHash(Encoding.ASCII.GetBytes("admin"));
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                Close();
            }
            else
            {
                InvalidLoginData.Content = "Nieprawidlowe dane logowania";
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var passHash = sha.ComputeHash(Encoding.ASCII.GetBytes(passwordBox.Password));
            string password = "";
            string login = "";
            passwordBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => password = passwordBox.Password));
            loginBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => login = loginBox.Text));
            sprawdzHaslo = SprawdzHaslo(password, login);
            e.Result = sprawdzHaslo;
        }

        private void bLogowanie_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
        public bool SprawdzHaslo(string haslo, string login)
        {
            using (var context = new DbContext())
            {
                var query = context.Uzytkownicy.Where(s => s.Haslo == haslo);

                var uzytkownik = query.FirstOrDefault();

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
