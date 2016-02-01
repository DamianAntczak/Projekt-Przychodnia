using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        private Guid IDwizyty;
        private HistoriaChoroby historia;
        private Pacjent pacjent;
        private Wizyta Wybrana_Wizyta; //wybrana wizyta

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
                this.pacjent = (Pacjent)pacjent.FirstOrDefault();

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
                
                var wizyty = from w in db.Wizyty
                    where w.HistoriaChoroby.Pacjent.Pesel == pacjentPesel.Pesel
                    select w;

                

                wyborWizytyBox.ItemsSource = wizyty.ToList();
                wyborWizytyBox.DisplayMemberPath = "Data";
                wyborWizytyBox.SelectedValuePath = "IdWizyty";

                var historia = from h in db.HistoriaChoroby
                    where h.Pacjent.Pesel == pacjentPesel.Pesel
                    select h as HistoriaChoroby;

                opisText.Text = historia.First().OpisChoroby;
                this.historia = historia.First();

                
            }

        }

        private void opisText_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void wyborWizytyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedWizyta = (Wizyta)wyborWizytyBox.SelectedItem;

            Wybrana_Wizyta = selectedWizyta;

            using (DbContext db = new DbContext())
            {
                var wizyta = (from w in db.Wizyty
                    where w.IdWizyty == selectedWizyta.IdWizyty
                    select w).First();

                IDwizyty = wizyta.IdWizyty;
                tOpisWizyty.Text = wizyta.Opis;
            }
        }

        private void opisText_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (DbContext db = new DbContext())
            {
                var queryHistoria = from HistoriaChoroby in db.HistoriaChoroby
                    where HistoriaChoroby.Pacjent == pacjent
                    select HistoriaChoroby;

                var historia = queryHistoria.FirstOrDefault();
                historia.OpisChoroby = opisText.Text;

                db.SaveChanges();
            }

        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            using (DbContext db = new DbContext())
            {
                var historia = (from HistoriaChoroby in db.HistoriaChoroby
                                    where HistoriaChoroby.Pacjent.Pesel == pacjent.Pesel
                                    select HistoriaChoroby).First();

                historia.OpisChoroby = opisText.Text;

                var wiz = (from w in db.Wizyty
                    where w.IdWizyty == IDwizyty
                    select w).First();

                wiz.Opis = tOpisWizyty.Text;

                db.SaveChanges();
            }
        }
    }
}
