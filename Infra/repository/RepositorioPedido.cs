using Domain.Interfaces.ICliente;
using Domain.Interfaces.IPedido;
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
    public class RepositorioPedido : RepositorioGeneric<Pedido>, InterfacePedido
    {

        private readonly DbContextOptions<ContextBase> _optionsBuilder;
        private readonly ContextBase _context;

        public RepositorioPedido(ContextBase context) : base(context)
        {
            _optionsBuilder = new DbContextOptions<ContextBase>();
            _context = context;
        }

        
    }
}
