using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_BD.Models
{
    public class Wizyta
    {
        [Key]
        public Guid IdWizyty { get; set; }
        public DateTime Data { get; set; }
        //ma być typu DateTime, gdzie będziemy określać ile ma trwać wizyta, np. 1 h 20 min czy Int32
        //do określenia ilości minut -> 80 min?
       // public Int32 CzasWizyty { get; set; }
        //[ForeignKey("IdLekarza")]
       // public Lekarz Lekarz { get; set; }
        //[ForeignKey("IdPacjenta")]
        public Guid PacjentId { get; set; }
        public virtual Pacjent Pacjenci { get; set; }
        //Pacjent u danego specjalisty może mieć zdjagnozowaną jedną chorobę
        //Jeżeli uwzględnimy więcej chorób można stworzyć nową klasę Diagnoza, która będzie miała
        //Kolekcję wykrytych chorób w danej wizycie
        //[ForeignKey("IdChoroby")]
        //public Choroba Choroba { get; set; }
        //Wystawiona będzie jedna recepta na daną wizytę
        //public  Recepta Recepta { get; set; }
    }
}
