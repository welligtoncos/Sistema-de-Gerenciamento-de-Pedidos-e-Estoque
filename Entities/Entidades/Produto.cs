using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    // Tabela Produto
    [Table("PRODUTO")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        public int FornecedorId { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(500)]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Required]
        public int EstoqueAtual { get; set; }

        [Required]
        public int EstoqueMinimo { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor Fornecedor { get; set; }

        public virtual ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
        public virtual ICollection<HistoricoAcao> HistoricoAcoes { get; set; } = new List<HistoricoAcao>();
    }
}
