using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    [Table("HISTORICOACAO")]
    public class HistoricoAcao
    {
        [Key]
        public int HistoricoId { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required, MaxLength(100)]
        public string Acao { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }
    }

}
