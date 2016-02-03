using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Projekt_BD.Annotations;

namespace Projekt_BD.Models
{
    [Table("Wizyty")]
    public class Wizyta : INotifyPropertyChanged
    {
        public Wizyta()
        {
            Recepty = new List<Recepta>();
        }

        [Key]
        public Guid IdWizyty { get; set; }
        public DateTime Data { get; set; }
        //ma być typu DateTime, gdzie będziemy określać ile ma trwać wizyta, np. 1 h 20 min czy Int32
        //do określenia ilości minut -> 80 min?
        public TimeSpan CzasWizyty { get; set; }
        public string Opis { get; set; }
        public bool CzyOdbyta { get; set; }
        
        public ICollection<Recepta> Recepty { get; set; }
        public virtual HistoriaChoroby HistoriaChoroby { get; set; }

        //Pacjent u danego specjalisty może mieć zdjagnozowaną jedną chorobę
        //Jeżeli uwzględnimy więcej chorób można stworzyć nową klasę Diagnoza, która będzie miała
        //Kolekcję wykrytych chorób w danej wizycie
        //[ForeignKey("IdChoroby")]
        //public Choroba Choroba { get; set; }
        //Wystawiona będzie jedna recepta na daną wizytę
        //public  Recepta Recepta { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
