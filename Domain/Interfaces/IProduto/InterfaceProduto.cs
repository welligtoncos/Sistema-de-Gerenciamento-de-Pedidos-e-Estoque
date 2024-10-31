using dominio.Interfaces.Generics;
using Entities.Entidades;
using System.Threading.Tasks;

namespace Domain.Interfaces.IProduto
{
    public interface InterfaceProduto : InterfaceGeneric<Produto>
    {
        Task<bool> VerificarEstoqueDisponivel(int produtoId, int quantidadeSolicitada);
    }
}
