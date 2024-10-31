using Domain.Interfaces.ICliente;
using Domain.Interfaces.IHistoricoAcao;
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
    public class ClienteController : ControllerBase
    {
        private readonly InterfaceCliente _interfaceCliente; 

        public ClienteController(InterfaceCliente interfaceCliente )
        {
            _interfaceCliente = interfaceCliente; 
    }

        /// <summary>
        /// Adiciona um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Objeto do cliente a ser adicionado.</param>
        /// <returns>Confirmação de adição do cliente.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Cliente adicionado com sucesso.", typeof(ClienteDTO))]
        [SwaggerResponse(400, "Cliente inválido.")]
        [SwaggerResponse(500, "Erro ao adicionar o cliente.")]
        public async Task<IActionResult> Add([FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
                return BadRequest("Cliente inválido.");

            try
            {
                var cliente = new Cliente
                {
                    Nome = clienteDto.Nome,
                    Email = clienteDto.Email,
                    Telefone = clienteDto.Telefone,
                    Endereco = clienteDto.Endereco
                };

                await _interfaceCliente.Add(cliente);
                clienteDto.ClienteId = cliente.ClienteId; // Atualiza o ID gerado

               

                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="clienteDto">Objeto do cliente a ser atualizado.</param>
        /// <returns>Confirmação de atualização do cliente.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Cliente atualizado com sucesso.")]
        [SwaggerResponse(400, "Cliente inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o cliente.")]
        public async Task<IActionResult> Update([FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
                return BadRequest("Cliente inválido.");

            try
            {
                var cliente = new Cliente
                {
                    ClienteId = clienteDto.ClienteId,
                    Nome = clienteDto.Nome,
                    Email = clienteDto.Email,
                    Telefone = clienteDto.Telefone,
                    Endereco = clienteDto.Endereco
                };

                await _interfaceCliente.Update(cliente);
                return Ok("Cliente atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser deletado.</param>
        /// <returns>Confirmação de remoção do cliente.</returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerResponse(200, "Cliente removido com sucesso.")]
        [SwaggerResponse(404, "Cliente com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao remover o cliente.")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _interfaceCliente.GetEntityByID(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                await _interfaceCliente.Delete(cliente);
                return Ok("Cliente removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover o cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Detalhes do cliente.</returns>
        [HttpGet("GetByID/{id}")]
        [SwaggerResponse(200, "Cliente encontrado.", typeof(ClienteDTO))]
        [SwaggerResponse(404, "Cliente com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar cliente.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var cliente = await _interfaceCliente.GetEntityByID(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                var clienteDto = new ClienteDTO
                {
                    ClienteId = cliente.ClienteId,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Telefone = cliente.Telefone,
                    Endereco = cliente.Endereco
                };

                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os clientes.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        [HttpGet("List")]
        [SwaggerResponse(200, "Clientes listados com sucesso.", typeof(List<ClienteDTO>))]
        [SwaggerResponse(500, "Erro ao listar clientes.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var clientes = await _interfaceCliente.List();
                var clientesDto = new List<ClienteDTO>();

                foreach (var cliente in clientes)
                {
                    clientesDto.Add(new ClienteDTO
                    {
                        ClienteId = cliente.ClienteId,
                        Nome = cliente.Nome,
                        Email = cliente.Email,
                        Telefone = cliente.Telefone,
                        Endereco = cliente.Endereco
                    });
                }

                return Ok(clientesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os clientes: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// DTO para representar dados do cliente de forma simplificada.
    /// </summary>
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
    }
}
