using Clasesvideoclub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clasesvideoclub.Models
{
    public class Alquiler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SocioId { get; set; }
        public Socio Socio { get; set; }

        public DateTime FechaAlquiler { get; set; } = DateTime.Now;
        public int DiasAlquilado { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Devuelto { get; set; } = false;

        public List<AlquilerDetalle> Detalles { get; set; } = new List<AlquilerDetalle>();
    }

    public class AlquilerDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AlquilerId { get; set; }
        public Alquiler Alquiler { get; set; }

        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}