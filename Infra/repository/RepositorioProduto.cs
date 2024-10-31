using Domain.Interfaces.IProduto;
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
    public class RepositorioProduto : RepositorioGeneric<Produto>, InterfaceProduto
    {
        private readonly DbContextOptions<ContextBase> _optionsBuilder;
        private readonly ContextBase _context;

        public RepositorioProduto(ContextBase context) : base(context)
        {
            _optionsBuilder = new DbContextOptions<ContextBase>();
            _context = context;
        }

        public async Task<bool> VerificarEstoqueDisponivel(int produtoId, int quantidadeSolicitada)
        {
            var produto = await _context.Produtos
                .Where(p => p.ProdutoId == produtoId)
                .Select(p => p.EstoqueAtual)
                .FirstOrDefaultAsync();

            // Verifica se o estoque é suficiente para a quantidade solicitada
            return produto >= quantidadeSolicitada;
        }
    }
}
