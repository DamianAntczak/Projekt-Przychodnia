using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD
{
    public class Specjalizacja
    {
        [Key]
        public Guid IdSpecjalizacji { get; set; }
        public String Nazwa { get; set; }
    }
}
