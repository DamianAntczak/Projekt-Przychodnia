//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekt_BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Specjalizacjas
    {
        public Specjalizacjas()
        {
            this.Lekarzs = new HashSet<Lekarzs>();
        }
    
        public System.Guid IdSpecjalizacji { get; set; }
        public string Nazwa { get; set; }
    
        public virtual ICollection<Lekarzs> Lekarzs { get; set; }
    }
}