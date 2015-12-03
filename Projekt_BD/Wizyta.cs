using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD
{
    public class Wizyta
    {
        [Key]
        public Guid IdWizyty { get; set; }
        public DateTime Data { get; set; }
        //ma być typu DateTime, gdzie będziemy określać ile ma trwać wizyta, np. 1 h 20 min czy Int32
        //do określenia ilości minut -> 80 min?
        public Int32 CzasWizyty { get; set; }
        public Lekarz lekarz { get; set; }
        public Pacjent pacjent { get; set; }
        //Pacjent u danego specjalisty może mieć zdjagnozowaną jedną chorobę
        //Jeżeli uwzględnimy więcej chorób można stworzyć nową klasę Diagnoza, która będzie miała
        //Kolekcję wykrytych chorób w danej wizycie
        public Choroba choroba { get; set; }
        //Wystawiona będzie jedna recepta na daną wizytę
        public Recepta Recepta { get; set; }
    }
}
