﻿using System;
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
        public String Nazwa { get; set; }
        public Int32 StopienRefundacji { get; set; }
        public String Producent { get; set; }
        public virtual Recepta Recepta { get; set; }
    }
}
