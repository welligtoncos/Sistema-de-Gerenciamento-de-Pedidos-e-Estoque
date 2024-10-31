using Domain.Interfaces.IHistoricoAcao;
using Domain.Interfaces.IPedido;
using Domain.Interfaces.IProduto;
using Entities.Entidades;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly InterfacePedido _interfacePedido;
        private readonly InterfaceHistoricoAcao _interfaceHistoricoAcao;
        private readonly InterfaceProduto _interfaceProduto;

        public PedidoController(InterfacePedido interfacePedido, InterfaceHistoricoAcao interfaceHistoricoAcao, InterfaceProduto interfaceProduto)
        {

            _interfacePedido = interfacePedido;
            _interfaceHistoricoAcao = interfaceHistoricoAcao;
            _interfaceProduto = interfaceProduto;
        }
        /// <summary>
        /// Adiciona um novo pedido e registra a ação no histórico.
        /// </summary>
        /// <param name="pedidoDto">Objeto do pedido a ser adicionado.</param>
        /// <returns>Confirmação de adição do pedido.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Pedido adicionado com sucesso.", typeof(PedidoDTO))]
        [SwaggerResponse(400, "Pedido inválido ou estoque insuficiente.")]
        [SwaggerResponse(500, "Erro ao adicionar o pedido.")]
        public async Task<IActionResult> Add([FromBody] PedidoDTO pedidoDto)
        {
            if (pedidoDto == null)
                return BadRequest("Pedido inválido.");

            try
            {
                // Verifica o estoque para cada produto no pedido
                foreach (var item in pedidoDto.PedidoProdutos)
                {
                    var estoqueSuficiente = await _interfaceProduto.VerificarEstoqueDisponivel(item.ProdutoId, item.Quantidade);
                    if (!estoqueSuficiente)
                    {
                        return BadRequest($"Estoque insuficiente para o produto com ID {item.ProdutoId}.");
                    }
                }

                // Calcula o total do pedido com base nos itens
                pedidoDto.TotalPedido = pedidoDto.PedidoProdutos.Sum(i => i.Quantidade * i.PrecoUnitario);

                // Mapeia os dados do DTO para a entidade Pedido
                var pedido = new Pedido
                {
                    ClienteId = pedidoDto.ClienteId,
                    DataPedido = pedidoDto.DataPedido,
                    TotalPedido = pedidoDto.TotalPedido,
                    Status = pedidoDto.Status,
                    PedidoProdutos = pedidoDto.PedidoProdutos.Select(i => new PedidoProduto
                    {
                        ProdutoId = i.ProdutoId,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario,
                        TotalItem = i.TotalItem
                    }).ToList()
                };

                // Adiciona o pedido ao banco de dados
                await _interfacePedido.Add(pedido);
                pedidoDto.PedidoId = pedido.PedidoId; // Atualiza o ID gerado

                // Registra a ação de adição no histórico
                var historicoAcao = new HistoricoAcao
                {
                    PedidoId = pedido.PedidoId,
                    Acao = "Criação de Pedido",
                    DataHora = DateTime.Now,
                    UsuarioId = null, // Substitua pelo ID do usuário atual, se disponível
                    ProdutoId = null  // Pode ser nulo, pois se refere ao pedido como um todo
                };

                await _interfaceHistoricoAcao.Add(historicoAcao);

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o pedido: {ex.Message}");
            }
        }



        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        /// <param name="pedidoDto">Objeto do pedido a ser atualizado.</param>
        /// <returns>Confirmação de atualização do pedido.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Pedido atualizado com sucesso.")]
        [SwaggerResponse(400, "Pedido inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o pedido.")]
        public async Task<IActionResult> Update([FromBody] PedidoDTO pedidoDto)
        {
            if (pedidoDto == null)
                return BadRequest("Pedido inválido.");

            try
            {
                var pedido = new Pedido
                {
                    PedidoId = pedidoDto.PedidoId,
                    ClienteId = pedidoDto.ClienteId,
                    DataPedido = pedidoDto.DataPedido,
                    TotalPedido = pedidoDto.TotalPedido,
                    Status = pedidoDto.Status
                };

                await _interfacePedido.Update(pedido);
                return Ok("Pedido atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido a ser deletado.</param>
        /// <returns>Confirmação de remoção do pedido.</returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerResponse(200, "Pedido removido com sucesso.")]
        [SwaggerResponse(404, "Pedido com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao remover o pedido.")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var pedido = await _interfacePedido.GetEntityByID(id);
                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                await _interfacePedido.Delete(pedido);
                return Ok("Pedido removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover o pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um pedido específico pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Detalhes do pedido.</returns>
        [HttpGet("GetByID/{id}")]
        [SwaggerResponse(200, "Pedido encontrado.", typeof(PedidoDTO))]
        [SwaggerResponse(404, "Pedido com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar pedido.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var pedido = await _interfacePedido.GetEntityByID(id);
                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                var pedidoDto = new PedidoDTO
                {
                    PedidoId = pedido.PedidoId,
                    ClienteId = pedido.ClienteId,
                    DataPedido = pedido.DataPedido,
                    TotalPedido = pedido.TotalPedido,
                    Status = pedido.Status
                };

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os pedidos.
        /// </summary>
        /// <returns>Lista de pedidos.</returns>
        [HttpGet("List")]
        [SwaggerResponse(200, "Pedidos listados com sucesso.", typeof(List<PedidoDTO>))]
        [SwaggerResponse(500, "Erro ao listar pedidos.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var pedidos = await _interfacePedido.List();
                var pedidosDto = new List<PedidoDTO>();

                foreach (var pedido in pedidos)
                {
                    pedidosDto.Add(new PedidoDTO
                    {
                        PedidoId = pedido.PedidoId,
                        ClienteId = pedido.ClienteId,
                        DataPedido = pedido.DataPedido,
                        TotalPedido = pedido.TotalPedido,
                        Status = pedido.Status
                    });
                }

                return Ok(pedidosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os pedidos: {ex.Message}");
            }
        }
    }

    public class PedidoDTO
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public string Status { get; set; }

        // Lista de produtos incluídos no pedido, correspondendo a PedidoProdutos na entidade Pedido
        public List<PedidoProdutoDTO> PedidoProdutos { get; set; } = new List<PedidoProdutoDTO>();
    }

    public class PedidoProdutoDTO
    {
        public int ProdutoId { get; set; }           // ID do produto no pedido
        public int Quantidade { get; set; }          // Quantidade solicitada do produto
        public decimal PrecoUnitario { get; set; }   // Preço unitário do produto
        public decimal TotalItem { get; set; }       // Total para este item (Quantidade * PrecoUnitario)
    }


}
