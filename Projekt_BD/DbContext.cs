using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;


namespace Projekt_BD {
    public class DbContext : System.Data.Entity.DbContext {
        //public DbContext() : base("name=DbContext") { }
        //public DbContext() : base("PrzychodniaDB") { }
        public DbContext() : base("BazaDanychPrzychodnia") { }

        public DbSet<Pacjent> Pacjentci { get; set; }
        public DbSet<Lek> Leki { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Recepta> Recepty { get; set; }
        public DbSet<Lekarz> Lekarze { get; set; }
        public DbSet<HistoriaChoroby> HistoriaChoroby { get; set; }
        public DbSet<Wizyta> Wizyty { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<SpisChorb> SpisChorob { get; set; }
        public DbSet<SpisLekow> SpisLekow { get; set; }

        public void OnModeOnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Lekarz>()
                .HasRequired<Specjalizacja>(s => s.Specjalizacja)
                .WithMany(s => s.Lekarze)
                .HasForeignKey(s => s.IdLekarza);



            modelBuilder.Entity<HistoriaChoroby>()
                .HasMany(w => w.Wizyty)
                .WithRequired(w => w.HistoriaChoroby)
                .HasForeignKey(w => w.IdWizyty);
            
            modelBuilder.Entity<Pacjent>()
                .HasMany(c => c.HistoriaChoroby)
                .WithRequired(p => p.Pacjent)
                .HasForeignKey(c => c.Pesel);

            modelBuilder.Entity<HistoriaChoroby>()
                .HasRequired(c => c.ChorobaZeSpisu)
                .WithMany(c => c.Choroba)
                .HasForeignKey(c => new { c.Pesel,c.IdLekarza});

            modelBuilder.Entity<Lek>()
                .HasRequired(l => l.SpisLekow)
                .WithMany(l => l.Leki)
                .HasForeignKey(l => l.IdLeku);

            modelBuilder.Entity<Lek>()
                .HasRequired(l => l.Recepta)
                .WithMany(l => l.Leki)
                .HasForeignKey(l => l.IdLeku);
        }
    }
}

