using Clasesvideoclub.Models;
using Microsoft.EntityFrameworkCore;
using Clasesvideoclub.Models;
using Microsoft.EntityFrameworkCore;
using System;
namespace Clasesvideoclub.Data
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<AlquilerDetalle> AlquilerDetalles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\databases\\Videoclub.db");
        }
    }
}