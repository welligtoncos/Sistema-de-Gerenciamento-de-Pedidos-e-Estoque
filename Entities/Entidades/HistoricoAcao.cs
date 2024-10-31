using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entidades
{
    [Table("HISTORICOACAO")]
    public class HistoricoAcao
    {
        [Key]
        public int HistoricoId { get; set; }

        // Propriedades que agora aceitam valores nulos
        public int? PedidoId { get; set; }
        public int? UsuarioId { get; set; }
        public int? ProdutoId { get; set; }

        [Required, MaxLength(100)]
        public string Acao { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        // Relacionamentos com ForeignKey
        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }
    }
}
