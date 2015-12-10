using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;


namespace Projekt_BD
{
    public class DbContext : System.Data.Entity.DbContext
    {
        //public DbContext() : base("name=DbContext") {}
        public DbContext() : base("PrzychodniaDB") { }

        public DbSet<Pacjent> Pacjentci { get; set; }
        public DbSet<Lek> Leki { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Recepta> Recepty { get; set; }
        public DbSet<Lekarz> Lekarze { get; set; } 
        public DbSet<Choroba> Choroby { get; set; }
        public DbSet<Wizyta> Wizyty { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }

        public void OnModeOnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lekarz>()
                .HasRequired<Specjalizacja>(s => s.Specjalizacja)
                .WithMany(s => s.Lekarze)
                .HasForeignKey(s => s.IdLekarza);

            modelBuilder.Entity<Pacjent>()
                .HasMany<Wizyta>(w => w.Wizyty)
                .WithRequired(p => p.Pacjenci)
                .HasForeignKey(p => p.IdWizyty);

            modelBuilder.Entity<Recepta>()
                .HasMany<Lek>(l => l.Leki)
                .WithRequired(r => r.Recepta)
                .HasForeignKey(r => r.IdLeku);

            modelBuilder.Entity<Wizyta>()
                .HasMany<Recepta>(r => r.Recepty)
                .WithRequired(w => w.Wizyta)
                .HasForeignKey(w => w.IdRecepty);
        }
    }
}

