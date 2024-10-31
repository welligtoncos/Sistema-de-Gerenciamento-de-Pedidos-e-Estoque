using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio.Interfaces.Generics
{
    public interface InterfaceGeneric<T> where T : class
    {
        Task Add(T Objeto);
        Task Update(T Objeto);
        Task Delete(T Objeto);
        Task<T> GetEntityByID(int id);
        Task<List<T>> List();
    }
}
