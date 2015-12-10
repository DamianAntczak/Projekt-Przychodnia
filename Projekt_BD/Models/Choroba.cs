using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;

namespace Projekt_BD
{
    public class Choroba
    {
        [Key]
        public Guid IdChoroby { get; set; }
        public String Nazwa { get; set; }
        public String Opis { get; set; }
        public String Objawy { get; set; }
        public String SposobyLeczenia { get; set; }
        public virtual Guid IdPacjenta { get; set; }
        public virtual Pacjent Pacjent { get; set; }
    }
}
