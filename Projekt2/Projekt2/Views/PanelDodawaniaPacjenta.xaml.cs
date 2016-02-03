using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for PanelDodawaniaPacjenta.xaml
    /// </summary>
    public partial class PanelDodawaniaPacjenta : UserControl {
        private DbContext dbContext;
        public PanelDodawaniaPacjenta() {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e) {
            if (TPesel.Text != string.Empty && sprawdzCzyJestNumerem(TPesel.Text) && tImie.Text != string.Empty && tNazwisko.Text != string.Empty && tUrodzenie.SelectedDate.HasValue) {
                using (dbContext = new DbContext()) {
                    var plec = (bool)Kobieta.IsChecked ? Models.Plec.Kobieta : Models.Plec.Mezczyzna;
                    dbContext.Pacjentci.Add(new Models.Pacjent { Pesel = TPesel.Text, Imie = tImie.Text, DataUrodzenie = tUrodzenie.SelectedDate.GetValueOrDefault(), Nazwisko = tNazwisko.Text, Mail = tMail.Text, MiejsceUrodzenia = tMiejsceUr.Text, NrTelefonu = tTel.Text, Plec= plec });
                    dbContext.SaveChanges();
                    MessageBox.Show("Pomyślnie dodano pacjenta!");
                }
            }
            else
                MessageBox.Show("Błąd przy wprowadzaniu danych");
        }
        private bool sprawdzCzyJestNumerem(string text) {
            Regex regex = new Regex("^[0-9]*$");
            return regex.IsMatch(text);
        }
    }
}
