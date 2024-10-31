using Domain.Interfaces.IHistoricoAcao;
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
    public class ProdutoController : ControllerBase
    {
        private readonly InterfaceProduto _interfaceProduto;
        private readonly InterfaceHistoricoAcao _interfaceHistoricoAcao;

        public ProdutoController(InterfaceProduto interfaceProduto, InterfaceHistoricoAcao interfaceHistoricoAcao)
        {
            _interfaceProduto = interfaceProduto; 
            _interfaceHistoricoAcao = interfaceHistoricoAcao;
        }

        /// <summary>
        /// Adiciona um novo produto.
        /// </summary>
        /// <param name="produtoDto">Objeto do produto a ser adicionado.</param>
        /// <returns>Confirmação de adição do produto.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Produto adicionado com sucesso.", typeof(ProdutoDTO))]
        [SwaggerResponse(400, "Produto inválido.")]
        [SwaggerResponse(500, "Erro ao adicionar o produto.")]
        public async Task<IActionResult> Add([FromBody] ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Produto inválido.");

            try
            {
                var produto = new Produto
                {
                    FornecedorId = produtoDto.FornecedorId,
                    Nome = produtoDto.Nome,
                    Descricao = produtoDto.Descricao,
                    Preco = produtoDto.Preco,
                    EstoqueAtual = produtoDto.EstoqueAtual,
                    EstoqueMinimo = produtoDto.EstoqueMinimo,
                    Ativo = produtoDto.Ativo
                };

                await _interfaceProduto.Add(produto);
                produtoDto.ProdutoId = produto.ProdutoId; // Atualiza o ID gerado 

                // Registra a ação de adição no histórico
                var historicoAcao = new HistoricoAcao
                {
                    ProdutoId = produto.ProdutoId,
                    Acao = "Adição de Produto",
                    DataHora = DateTime.Now,
                    UsuarioId = null, // Ajuste conforme necessário para identificar o usuário
                    PedidoId = null
                };

                await _interfaceHistoricoAcao.Add(historicoAcao); // Assegure-se de ter injetado o repositório de histórico
                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="produtoDto">Objeto do produto a ser atualizado.</param>
        /// <returns>Confirmação de atualização do produto.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Produto atualizado com sucesso.")]
        [SwaggerResponse(400, "Produto inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o produto.")]
        public async Task<IActionResult> Update([FromBody] ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Produto inválido.");

            try
            {
                var produto = new Produto
                {
                    ProdutoId = produtoDto.ProdutoId,
                    FornecedorId = produtoDto.FornecedorId,
                    Nome = produtoDto.Nome,
                    Descricao = produtoDto.Descricao,
                    Preco = produtoDto.Preco,
                    EstoqueAtual = produtoDto.EstoqueAtual,
                    EstoqueMinimo = produtoDto.EstoqueMinimo,
                    Ativo = produtoDto.Ativo
                };

                await _interfaceProduto.Update(produto);
                return Ok("Produto atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto a ser deletado.</param>
        /// <returns>Confirmação de remoção do produto.</returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerResponse(200, "Produto removido com sucesso.")]
        [SwaggerResponse(404, "Produto com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao remover o produto.")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var produto = await _interfaceProduto.GetEntityByID(id);
                if (produto == null)
                    return NotFound("Produto não encontrado.");

                await _interfaceProduto.Delete(produto);
                return Ok("Produto removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um produto específico pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Detalhes do produto.</returns>
        [HttpGet("GetByID/{id}")]
        [SwaggerResponse(200, "Produto encontrado.", typeof(ProdutoDTO))]
        [SwaggerResponse(404, "Produto com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar produto.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var produto = await _interfaceProduto.GetEntityByID(id);
                if (produto == null)
                    return NotFound("Produto não encontrado.");

                var produtoDto = new ProdutoDTO
                {
                    ProdutoId = produto.ProdutoId,
                    FornecedorId = produto.FornecedorId,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    EstoqueAtual = produto.EstoqueAtual,
                    EstoqueMinimo = produto.EstoqueMinimo,
                    Ativo = produto.Ativo
                };

                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os produtos.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        [HttpGet("List")]
        [SwaggerResponse(200, "Produtos listados com sucesso.", typeof(List<ProdutoDTO>))]
        [SwaggerResponse(500, "Erro ao listar produtos.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var produtos = await _interfaceProduto.List();
                var produtosDto = new List<ProdutoDTO>();

                foreach (var produto in produtos)
                {
                    produtosDto.Add(new ProdutoDTO
                    {
                        ProdutoId = produto.ProdutoId,
                        FornecedorId = produto.FornecedorId,
                        Nome = produto.Nome,
                        Descricao = produto.Descricao,
                        Preco = produto.Preco,
                        EstoqueAtual = produto.EstoqueAtual,
                        EstoqueMinimo = produto.EstoqueMinimo,
                        Ativo = produto.Ativo
                    });
                }

                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os produtos: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// DTO para representar dados do produto de forma simplificada.
    /// </summary>
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public int FornecedorId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int EstoqueAtual { get; set; }
        public int EstoqueMinimo { get; set; }
        public bool Ativo { get; set; }
    }
}
