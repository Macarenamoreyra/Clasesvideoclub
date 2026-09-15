using Clasesvideoclub.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Clasesvideoclub.Data;

namespace Clasesvideoclub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AplicationDbContext _context;

        public GenericRepository()
        {
            _context = new AplicationDbContext();
        }

        public void Agregar(T entidad)
        {
            _context.Set<T>().Add(entidad);
            _context.SaveChanges();
        }

        public List<T> ObtenerTodos()
        {
            return _context.Set<T>().ToList();
        }

        public void Modificar(T entidad)
        {
            _context.Set<T>().Update(entidad);
            _context.SaveChanges();
        }
    }
}