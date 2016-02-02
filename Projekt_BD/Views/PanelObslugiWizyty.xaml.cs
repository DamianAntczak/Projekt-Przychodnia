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
using System.Data;

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
        private SpisLekow Wybrany_Lek;
        private List<Lek> ListaLekow;
        private Recepta NowaRecepta;

        public PanelObslugiWizyty()
        {
            InitializeComponent();

            ListaLekow = new List<Lek>();
            using (DbContext db = new DbContext())
            {
                var lekarz = db.Lekarze.First();
                var idLekarza = lekarz.IdLekarza;

                var pacjent = from p in db.Pacjentci
                    join h in db.HistoriaChoroby on p.Pesel equals h.Pesel
                    where h.Lekarz.IdLekarza == idLekarza
                    select p;
                this.pacjent = (Pacjent)pacjent.FirstOrDefault();

                var leks = from leki in db.SpisLekow select new { leki.NazwaLeku};

                
                dataGrid_WyborLeku.ItemsSource = leks.ToList();

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

        private void dataGrid_WyborLeku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dataGrid_WyborLeku.SelectedItem;
            var typ = row.GetType();
            using (DbContext db = new DbContext())
            {
                if( typ.GetProperty("NazwaLeku").GetValue(row) != null)
                {
                    string nazwaLek = typ.GetProperty("NazwaLeku").GetValue(row).ToString();
                    var result = (from SpisLekow in db.SpisLekow
                                  where SpisLekow.NazwaLeku == nazwaLek
                                  select SpisLekow).First();
                    Wybrany_Lek = (SpisLekow)result; //
                    label5_WybranyLek.Content = nazwaLek;
                }
            }
        }

        private void DodajLekButtton_Click(object sender, RoutedEventArgs e)
        {
            if(Wybrany_Lek != null && comboBox_Dawka.Text != "" && comboBox_Przyjmowanie.Text != "")
            {
                using (DbContext db = new DbContext())
                {
                    Lek nowyLek = new Lek { IdLeku = Guid.NewGuid(), Dawka = comboBox_Dawka.Text, Przyjmowanie = comboBox_Przyjmowanie.Text, StopienRefundacji = 0, SpisLekow = Wybrany_Lek, Recepta = NowaRecepta };
                    //db.Set<Lek>().AddOrUpdate(nowyLek);
                    ListaLekow.Add(nowyLek);
                    //dataGridDodaneLeki.ItemsSource = ListaLekow.Select(o => o.SpisLekow).Select( o => o.NazwaLeku);
                    dataGridDodaneLeki.ItemsSource = ListaLekow.Select(o => o.SpisLekow);
                    dataGridDodaneLeki.Items.Refresh();
                }
                    
            }
        }

        private void ZatwierdzRecepteButton_Click(object sender, RoutedEventArgs e)
        {
            if(ListaLekow.Count > 0)
            {
                NowaRecepta = new Recepta { IdRecepty = Guid.NewGuid(), Leki = ListaLekow, CzasWystawienia = DateTime.Now, Wizyta = Wybrana_Wizyta };
                //NowaRecepta.Leki = ListaLekow;
                using (DbContext db = new DbContext())
                {
                    if(Wybrana_Wizyta != null) { 
                        db.Set<Lek>().AddRange(ListaLekow);
                        db.Set<Recepta>().Add(NowaRecepta);
                        db.Set<Wizyta>().AddOrUpdate(Wybrana_Wizyta);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
