using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    // Tabela Fornecedores
    [Table("FORNECEDOR")]
    public class Fornecedor
    {
        [Key]
        public int FornecedorId { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [Required, MaxLength(100)]
        public string Contato { get; set; }

        [Required, MaxLength(20)]
        [Phone]
        public string Telefone { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Endereco { get; set; }

        [Required, MaxLength(20)]
        public string Cnpj { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
