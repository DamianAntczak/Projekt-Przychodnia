﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
