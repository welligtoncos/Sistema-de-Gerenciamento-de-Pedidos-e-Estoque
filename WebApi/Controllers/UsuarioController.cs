using Domain.Interfaces.IUsuario;
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
    public class UsuarioController : ControllerBase
    {
        private readonly InterfaceUsuario _interfaceUsuario;

        public UsuarioController(InterfaceUsuario interfaceUsuario)
        {
            _interfaceUsuario = interfaceUsuario;
        }

        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        /// <param name="usuarioDto">Objeto do usuário a ser adicionado.</param>
        /// <returns>Confirmação de adição do usuário.</returns>
        [HttpPost("Add")]
        [SwaggerResponse(200, "Usuário adicionado com sucesso.", typeof(UsuarioDTO))]
        [SwaggerResponse(400, "Usuário inválido.")]
        [SwaggerResponse(500, "Erro ao adicionar o usuário.")]
        public async Task<IActionResult> Add([FromBody] UsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
                return BadRequest("Usuário inválido.");

            try
            {
                // Mapeia os dados do DTO para a entidade Usuario
                var usuario = new Usuario
                {
                    Nome = usuarioDto.Nome,
                    Email = usuarioDto.Email,
                    Senha = usuarioDto.Senha
                };

                await _interfaceUsuario.Add(usuario);

                // Atualiza o DTO com o ID gerado
                usuarioDto.UsuarioId = usuario.UsuarioId;

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar o usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="usuario">Objeto do usuário a ser atualizado.</param>
        /// <returns>Confirmação de atualização do usuário.</returns>
        [HttpPut("Update")]
        [SwaggerResponse(200, "Usuário atualizado com sucesso.")]
        [SwaggerResponse(400, "Usuário inválido.")]
        [SwaggerResponse(500, "Erro ao atualizar o usuário.")]
        public async Task<IActionResult> Update([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Usuário inválido.");

            try
            {
                await _interfaceUsuario.Update(usuario);
                return Ok("Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <returns>Confirmação de remoção do usuário.</returns>
        [HttpDelete("Delete/{id}")]
        [SwaggerResponse(200, "Usuário removido com sucesso.")]
        [SwaggerResponse(404, "Usuário com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao remover o usuário.")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _interfaceUsuario.GetEntityByID(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                await _interfaceUsuario.Delete(usuario);
                return Ok("Usuário removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover o usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Detalhes do usuário.</returns>
        [HttpGet("GetByID/{id}")]
        [SwaggerResponse(200, "Usuário encontrado.", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário com o ID fornecido não encontrado.")]
        [SwaggerResponse(500, "Erro ao buscar usuário.")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                var usuario = await _interfaceUsuario.GetEntityByID(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                // Mapeia a entidade para o DTO
                var usuarioDto = new UsuarioDTO
                {
                    UsuarioId = usuario.UsuarioId,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista todos os usuários.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        [HttpGet("List")]
        [SwaggerResponse(200, "Usuários listados com sucesso.", typeof(List<UsuarioDTO>))]
        [SwaggerResponse(500, "Erro ao listar usuários.")]
        public async Task<IActionResult> List()
        {
            try
            {
                var usuarios = await _interfaceUsuario.List();

                // Mapeia a lista de entidades para uma lista de DTOs
                var usuariosDto = new List<UsuarioDTO>();
                foreach (var usuario in usuarios)
                {
                    usuariosDto.Add(new UsuarioDTO
                    {
                        UsuarioId = usuario.UsuarioId,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Senha = usuario.Senha
                    });
                }

                return Ok(usuariosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os usuários: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// DTO para representar dados do usuário de forma simplificada.
    /// </summary>
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class UsuarioDTO2
    { 
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
