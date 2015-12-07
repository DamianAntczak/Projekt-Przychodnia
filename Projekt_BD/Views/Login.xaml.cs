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

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private SHA1 sha;
        private byte[] adminPass;

        public Login()
        {
            InitializeComponent();
            sha = new SHA1Managed();
            adminPass = sha.ComputeHash(Encoding.ASCII.GetBytes("admin"));
        }

        private void bLogowanie_Click(object sender, RoutedEventArgs e)
        {
            var passHash = sha.ComputeHash(Encoding.ASCII.GetBytes(passwordBox.Password));
            


            if (loginBox.Text == "admin" && passHash.SequenceEqual(passHash))
            {
                Close();
            }
            
        }


    }
}
