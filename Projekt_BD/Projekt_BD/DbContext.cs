using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projekt_BD
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext():base("Przychodnia")
        {
            
        }

        public DbSet<Pacjent> Pacjentci { get; set; } 

    }
}

