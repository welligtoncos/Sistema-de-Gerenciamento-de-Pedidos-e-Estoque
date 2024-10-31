using Domain.Interfaces.IFornecedor;
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
    public class FornecedorController : ControllerBase
    {
        private readonly InterfaceFornecedor _interfaceFornecedor;

        public FornecedorController(InterfaceFornecedor interfaceFornecedor)
        {
            _interfaceFornecedor = interfaceFornecedor;
        }

        /// <summary>
        /// Adiciona um novo fornecedor.
        /// </summary>
        /// <param name="fornecedorDto">Objeto do fornecedor a ser adicionado.</param>
        /// <returns>Confirmação de adição do fornecedor.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Fornecedor adicionado com sucesso.", typeof(FornecedorDTO))]
        [SwaggerResponse(400, "Fornecedor inválido.")]
        [SwaggerResponse(500, "Erro ao adicionar o fornecedor.")]
        public async Task<IActionResult> Add([FromBody] FornecedorDTO fornecedorDto)
        {
            if (fornecedorDto == null)
                return BadRequest("Fornecedor inválido.");

            try
            {
                var fornecedor = new Fornecedor
                {
                    Nome = fornecedorDto.Nome,
                    Contato = fornecedorDto.Contato,
                    Telefone = fornecedorDto.Telefone,
                    Email = fornecedorDto.Email,
                    Endereco = fornecedorDto.Endereco,
                    Cnpj = fornecedorDto.Cnpj
                };

                await _interfaceFornecedor.Add(fornecedor);
                fornecedorDto.FornecedorId = fornecedor.FornecedorId; // Atualiza o ID gerado

                return Ok(fornecedorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o fornecedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um fornecedor existente.
        /// </summary>
        /// <param name="fornecedorDto">Objeto do fornecedor a ser atualizado.</param>
        /// <returns>Confirmação de atualização do fornecedor.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Fornecedor atualizado com sucesso.")]
        [SwaggerResponse(400, "Fornecedor inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o fornecedor.")]
        public async Task<IActionResult> Update([FromBody] FornecedorDTO fornecedorDto)
        {
            if (fornecedorDto == null)
                return BadRequest("Fornecedor inválido.");

            try
            {
                var fornecedor = new Fornecedor
                {
                    FornecedorId = fornecedorDto.FornecedorId,
                    Nome = fornecedorDto.Nome,
                    Contato = fornecedorDto.Contato,
                    Telefone = fornecedorDto.Telefone,
                    Email = fornecedorDto.Email,
                    Endereco = fornecedorDto.Endereco,
                    Cnpj = fornecedorDto.Cnpj
                };

                await _interfaceFornecedor.Update(fornecedor);
                return Ok("Fornecedor atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o fornecedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um fornecedor pelo ID.
        /// </summary>
        /// <param name="id">ID do fornecedor a ser deletado.</param>
        /// <returns>Confirmação de remoção do fornecedor.</returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerResponse(200, "Fornecedor removido com sucesso.")]
        [SwaggerResponse(404, "Fornecedor com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao remover o fornecedor.")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var fornecedor = await _interfaceFornecedor.GetEntityByID(id);
                if (fornecedor == null)
                    return NotFound("Fornecedor não encontrado.");

                await _interfaceFornecedor.Delete(fornecedor);
                return Ok("Fornecedor removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover o fornecedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um fornecedor específico pelo ID.
        /// </summary>
        /// <param name="id">ID do fornecedor.</param>
        /// <returns>Detalhes do fornecedor.</returns>
        [HttpGet("GetByID/{id}")]
        [SwaggerResponse(200, "Fornecedor encontrado.", typeof(FornecedorDTO))]
        [SwaggerResponse(404, "Fornecedor com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar fornecedor.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var fornecedor = await _interfaceFornecedor.GetEntityByID(id);
                if (fornecedor == null)
                    return NotFound("Fornecedor não encontrado.");

                var fornecedorDto = new FornecedorDTO
                {
                    FornecedorId = fornecedor.FornecedorId,
                    Nome = fornecedor.Nome,
                    Contato = fornecedor.Contato,
                    Telefone = fornecedor.Telefone,
                    Email = fornecedor.Email,
                    Endereco = fornecedor.Endereco,
                    Cnpj = fornecedor.Cnpj
                };

                return Ok(fornecedorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o fornecedor: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os fornecedores.
        /// </summary>
        /// <returns>Lista de fornecedores.</returns>
        [HttpGet("List")]
        [SwaggerResponse(200, "Fornecedores listados com sucesso.", typeof(List<FornecedorDTO>))]
        [SwaggerResponse(500, "Erro ao listar fornecedores.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var fornecedores = await _interfaceFornecedor.List();
                var fornecedoresDto = new List<FornecedorDTO>();

                foreach (var fornecedor in fornecedores)
                {
                    fornecedoresDto.Add(new FornecedorDTO
                    {
                        FornecedorId = fornecedor.FornecedorId,
                        Nome = fornecedor.Nome,
                        Contato = fornecedor.Contato,
                        Telefone = fornecedor.Telefone,
                        Email = fornecedor.Email,
                        Endereco = fornecedor.Endereco,
                        Cnpj = fornecedor.Cnpj
                    });
                }

                return Ok(fornecedoresDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os fornecedores: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// DTO para representar dados do fornecedor de forma simplificada.
    /// </summary>
    public class FornecedorDTO
    {
        public int FornecedorId { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Cnpj { get; set; }
    }
}
