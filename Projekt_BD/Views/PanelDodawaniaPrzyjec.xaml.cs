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
using Projekt_BD.Models;

namespace Projekt_BD.Views {
    /// <summary>
    /// Interaction logic for PanelDodawaniaPrzyjec.xaml
    /// </summary>
    public partial class PanelDodawaniaPrzyjec : UserControl {
        private List<String> godziny;

        public PanelDodawaniaPrzyjec() {
            godziny = new List<string>();

            for (int i = 8; i <= 20; i++) {
                if (i < 10) {
                    godziny.Add("0" + i + ":00");
                    godziny.Add("0" + i + ":15");
                    godziny.Add("0" + i + ":30");
                    godziny.Add("0" + i + ":45");
                }
                godziny.Add(i + ":00");
                godziny.Add(i + ":15");
                godziny.Add(i + ":30");
                godziny.Add(i + ":45");
            }

            InitializeComponent();

            rozpoczecieBox.ItemsSource = godziny;
            zakoczenieBox.ItemsSource = godziny;

            gridRefresh();

        }

        private void gridRefresh() {
            using (DbContext db = new DbContext()) {
                var przyjecia = from DniPrzyjec in db.DniPrzyjec select DniPrzyjec;
                przyjeciaDataGrid.ItemsSource = przyjecia.ToList();
                Kalendarz.DisplayDateStart = DateTime.Today;
            }
        }

        private void bDodajGodziny_Click(object sender, RoutedEventArgs e) {
            using (DbContext db = new DbContext()) {
                var lekarz = db.Lekarze.First();

                DniPrzyjec dniPrzyjec = new DniPrzyjec();
                dniPrzyjec.Lekarz = lekarz;
                if (Kalendarz.SelectedDate.HasValue) {
                    dniPrzyjec.CzasDataRozpoczecia = Kalendarz.SelectedDate.Value;
                    //dniPrzyjec.CzasDataRozpoczecia.Tim = 
                }
                //dniPrzyjec.CzasZakonczenia.Time

                TimeSpan czasOd;
                TimeSpan czasDo;

                TimeSpan.TryParseExact(rozpoczecieBox.SelectedValue.ToString(), @"hh\:mm", null, out czasOd);
                TimeSpan.TryParseExact(zakoczenieBox.SelectedValue.ToString(), @"hh\:mm", null, out czasDo);

                if (czasDo > czasOd) {
                    db.DniPrzyjec.Add(new Models.DniPrzyjec {
                        CzasDataRozpoczecia = Kalendarz.SelectedDate.Value + czasOd,
                        CzasZakonczenia = Kalendarz.SelectedDate.Value + czasDo,
                        Lekarz = lekarz
                    });
                }
                else {
                    lInfo.Content = "Czas rozpoczęcia\n musi być mniejszy\n od czasu zakończenia";
                }

                db.SaveChanges();
            }

            bDodajGodziny.Content = "Pomyślnie dodano wizytę";
            gridRefresh();
        }
    }
}
