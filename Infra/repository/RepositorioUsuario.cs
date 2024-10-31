using Domain.Interfaces.IProduto;
using Domain.Interfaces.IUsuario;
using Entities.Context;
using Entities.Entidades;
using Infra.repository.generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.repository
{
    public class RepositorioUsuario : RepositorioGeneric<Usuario>, InterfaceUsuario
    {
        private readonly DbContextOptions<ContextBase> _optionsBuilder;
        private readonly ContextBase _context;

        public RepositorioUsuario(ContextBase context) : base(context)
        {
            _optionsBuilder = new DbContextOptions<ContextBase>();
            _context = context;
        }
    }
}
