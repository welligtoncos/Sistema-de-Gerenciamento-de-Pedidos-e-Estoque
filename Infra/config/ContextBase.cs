using Entities.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Entities.Context
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }

        // DbSets para cada entidade
        public DbSet<PedidoProduto> PedidoProdutos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<HistoricoAcao> HistoricoAcoes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ObterStringConexao());
            }
        }

        private string ObterStringConexao()
        {
            return "Data Source=DESKTOP-BBMM82V;Initial Catalog=sistema-estoque-fiotec;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento entre PedidoProduto e Pedido (muitos para um)
            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada PedidoProduto está associado a um Pedido específico,
            // enquanto um Pedido pode ter vários itens de PedidoProduto.
            // O DeleteBehavior.Restrict impede que a exclusão de um Pedido
            // cause a exclusão dos itens associados.

            // Relacionamento entre PedidoProduto e Produto (muitos para um)
            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada PedidoProduto está vinculado a um Produto específico,
            // enquanto um Produto pode estar presente em vários itens de PedidoProduto.
            // A exclusão de um Produto não afetará os registros de PedidoProduto.

            // Relacionamento entre Pedido e Cliente (muitos para um)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada Pedido está vinculado a um Cliente específico,
            // enquanto um Cliente pode ter vários Pedidos.
            // O DeleteBehavior.Restrict evita que a exclusão de um Cliente
            // exclua todos os Pedidos associados a ele.

            // Relacionamento entre HistoricoAcao e Pedido (muitos para um)
            modelBuilder.Entity<HistoricoAcao>()
                .HasOne(ha => ha.Pedido)
                .WithMany(p => p.HistoricoAcoes)
                .HasForeignKey(ha => ha.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada ação no Histórico está associada a um Pedido específico,
            // enquanto um Pedido pode ter várias ações registradas no Histórico.
            // Esse relacionamento não permite a exclusão em cascata de ações históricas
            // ao excluir um Pedido.

            // Relacionamento entre HistoricoAcao e Usuario (muitos para um)
            modelBuilder.Entity<HistoricoAcao>()
                .HasOne(ha => ha.Usuario)
                .WithMany(u => u.HistoricoAcoes)
                .HasForeignKey(ha => ha.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada ação no Histórico é realizada por um Usuário específico,
            // e um Usuário pode ter várias ações no Histórico.
            // A exclusão de um Usuário não remove os registros de ação associados.

            // Relacionamento entre HistoricoAcao e Produto (muitos para um)
            modelBuilder.Entity<HistoricoAcao>()
                .HasOne(ha => ha.Produto)
                .WithMany(p => p.HistoricoAcoes)
                .HasForeignKey(ha => ha.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada ação no Histórico envolve um Produto específico,
            // e um Produto pode ter várias ações no Histórico.
            // Esse relacionamento impede que a exclusão de um Produto afete as ações
            // históricas registradas.

            // Relacionamento entre Produto e Fornecedor (muitos para um)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cada Produto está associado a um Fornecedor específico,
            // e um Fornecedor pode fornecer vários Produtos.
            // A exclusão de um Fornecedor não causará a exclusão dos Produtos associados.
        }
    }
}
