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
using System.Windows.Threading;
using Projekt_BD.Models;

namespace Projekt_BD.Views
{
    /// <summary>
    /// Interaction logic for PanelObslugiWizyty.xaml
    /// </summary>
    public partial class PanelObslugiWizyty : UserControl
    {
        public PanelObslugiWizyty()
        {
            InitializeComponent();

            using (DbContext db = new DbContext())
            {
                var lekarz = db.Lekarze.First();
                var idLekarza = lekarz.IdLekarza;

                var pacjent = from p in db.Pacjentci
                    join h in db.HistoriaChoroby on p.Pesel equals h.Pesel
                    where h.Lekarz.IdLekarza == idLekarza
                    select p;

                wyborPacjentaBox.ItemsSource = pacjent.ToList();
                wyborPacjentaBox.DisplayMemberPath = "Imie";
                wyborPacjentaBox.SelectedValuePath = "Pesel";


            }

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var pacjentPesel = (Models.Pacjent)wyborPacjentaBox.SelectedItem;

            using (DbContext db = new DbContext())
            {
                opisText.Text = pacjentPesel.Pesel.ToString();
                var wizyty = from w in db.Wizyty
                    where w.HistoriaChoroby.Pacjent.Pesel == pacjentPesel.Pesel
                    select w.Data + " " + w.CzasWizyty;

                wyborWizytyBox.ItemsSource = wizyty.ToList();
                wyborWizytyBox.DisplayMemberPath = "Data CzasWizyty";
            }

        }
    }
}
