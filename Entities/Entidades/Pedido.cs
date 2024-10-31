using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    // Tabela Pedido
    [Table("PEDIDO")]
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public DateTime DataPedido { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPedido { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
        public virtual ICollection<HistoricoAcao> HistoricoAcoes { get; set; } = new List<HistoricoAcao>();
    }
}
