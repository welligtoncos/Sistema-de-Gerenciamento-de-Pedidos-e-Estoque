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

        public ProdutoController(InterfaceProduto interfaceProduto)
        {
            _interfaceProduto = interfaceProduto;
        }

        /// <summary>
        /// Adiciona um novo produto.
        /// </summary>
        /// <param name="produto">Objeto do produto a ser adicionado.</param>
        /// <returns>Confirmação de adição do produto.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Produto adicionado com sucesso.")]
        [SwaggerResponse(400, "Produto inválido.")]
        [SwaggerResponse(500, "Erro ao adicionar o produto.")]
        public async Task<IActionResult> Add([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest("Produto inválido.");

            try
            {
                await _interfaceProduto.Add(produto);
                return Ok("Produto adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="produto">Objeto do produto a ser atualizado.</param>
        /// <returns>Confirmação de atualização do produto.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Produto atualizado com sucesso.")]
        [SwaggerResponse(400, "Produto inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o produto.")]
        public async Task<IActionResult> Update([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest("Produto inválido.");

            try
            {
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
        [SwaggerResponse(200, "Produto encontrado.", typeof(Produto))]
        [SwaggerResponse(404, "Produto com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar produto.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var produto = await _interfaceProduto.GetEntityByID(id);
                if (produto == null)
                    return NotFound("Produto não encontrado.");

                return Ok(produto);
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
        [SwaggerResponse(200, "Produtos listados com sucesso.", typeof(List<Produto>))]
        [SwaggerResponse(500, "Erro ao listar produtos.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var produtos = await _interfaceProduto.List();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os produtos: {ex.Message}");
            }
        }
    }
}
