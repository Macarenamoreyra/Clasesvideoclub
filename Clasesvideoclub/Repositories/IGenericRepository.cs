using System.Collections.Generic;

namespace Clasesvideoclub.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Agregar(T entidad);
        List<T> ObtenerTodos();
        void Modificar(T entidad);
    }
}