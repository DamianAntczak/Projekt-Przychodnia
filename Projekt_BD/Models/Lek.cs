using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;

namespace Projekt_BD
{
    public class Lek
    {

        [Key]
        public Guid IdLeku { get; set; }
        public int StopienRefundacji { get; set; }
        public string Dawka { get; set; }
        public string Przyjmowanie { get; set; }
        public virtual Recepta Recepta { get; set; }
        public virtual SpisLekow SpisLekow { get; set; }
    }
}
