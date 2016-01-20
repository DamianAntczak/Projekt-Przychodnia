using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projekt_BD.Models {
    public class SpisChorb {
        public SpisChorb() {
            Choroba = new List<HistoriaChoroby>();
        }
        [Key]
        public string NazwaChoroby { get; set; }
        public string NazwaPolskaChoroby { get; set; }
        public string Opis { get; set; }
        public string Objawy { get; set; }
        public string SposobyLeczenia { get; set; }
        public ICollection<HistoriaChoroby> Choroba { get; set; }
    }
}
