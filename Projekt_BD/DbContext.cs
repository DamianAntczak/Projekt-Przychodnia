using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;


namespace Projekt_BD {
    public class DbContext : System.Data.Entity.DbContext {
        public DbContext() : base("name=DbContext") { }
        //public DbContext() : base("PrzychodniaDB") { }

        public DbSet<Pacjent> Pacjentci { get; set; }
        public DbSet<Lek> Leki { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Recepta> Recepty { get; set; }
        public DbSet<Lekarz> Lekarze { get; set; }
        public DbSet<Choroba> Choroby { get; set; }
        public DbSet<Wizyta> Wizyty { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<SpisChorb> SpisChorob { get; set; }
        public DbSet<SpisLekow> SpisLekow { get; set; }

        public void OnModeOnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Lekarz>()
                .HasRequired<Specjalizacja>(s => s.Specjalizacja)
                .WithMany(s => s.Lekarze)
                .HasForeignKey(s => s.IdLekarza);

            modelBuilder.Entity<Pacjent>()
                .HasMany<Wizyta>(w => w.Wizyty)
                .WithRequired(p => p.Pacjent)
                .HasForeignKey(p => p.IdWizyty);

            modelBuilder.Entity<Wizyta>()
                .HasMany<Recepta>(r => r.Recepty)
                .WithRequired(w => w.Wizyta)
                .HasForeignKey(r => r.IdRecepty);

            modelBuilder.Entity<Wizyta>()
                .HasRequired(w => w.Lekarz)
                .WithMany(l => l.Wizyty)
                .HasForeignKey(l => l.IdWizyty);

            modelBuilder.Entity<Pacjent>()
                .HasMany(c => c.Choroby)
                .WithRequired(p => p.Pacjent)
                .HasForeignKey(c => c.IdPacjenta);

            modelBuilder.Entity<Choroba>()
                .HasRequired(c => c.ChorobaZeSpisu)
                .WithMany(c => c.Choroba)
                .HasForeignKey(c => c.IdChoroby);

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

