using System;
using System.Collections.Generic;
using System.Text;

namespace Clasesvideoclub.Models
{
   
        public class DetalleAlquiler
        {
            public int Id { get; set; }

            public int AlquilerId { get; set; }

            public Alquiler Alquiler { get; set; }

            public int PeliculaId { get; set; }

            public Pelicula Pelicula { get; set; }

            public decimal PrecioPorDia { get; set; }

            public int CantidadDias { get; set; }

            public decimal Subtotal { get; set; }
        }
    
}
