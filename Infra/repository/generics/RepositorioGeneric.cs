using dominio.Interfaces.Generics;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.repository.generics
{
    public class RepositorioGeneric<T> : InterfaceGeneric<T> where T : class
    {
        private readonly ContextBase _context;

        public RepositorioGeneric(ContextBase context)
        {
            _context = context;
        }

        public async Task Add(T objeto)
        {
            try
            {
                await _context.Set<T>().AddAsync(objeto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log e trate a exceção conforme necessário
                throw new Exception($"Erro ao adicionar a entidade: {ex.Message}", ex);
            }
        }

        public async Task Delete(T objeto)
        {
            try
            {
                _context.Set<T>().Remove(objeto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log e trate a exceção conforme necessário
                throw new Exception($"Erro ao remover a entidade: {ex.Message}", ex);
            }
        }

        public async Task<T> GetEntityByID(int id)
        {
            try
            {
                var result = await _context.Set<T>().FindAsync(id);
                if (result == null)
                    throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");

                return result;
            }
            catch (Exception ex)
            {
                // Log e trate a exceção conforme necessário
                throw new Exception($"Erro ao buscar a entidade: {ex.Message}", ex);
            }
        }

        public async Task<List<T>> List()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Log e trate a exceção conforme necessário
                throw new Exception($"Erro ao listar entidades: {ex.Message}", ex);
            }
        }

        public async Task Update(T objeto)
        {
            try
            {
                if (!_context.Set<T>().Local.Contains(objeto))
                    _context.Set<T>().Update(objeto);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log e trate a exceção conforme necessário
                throw new Exception($"Erro ao atualizar a entidade: {ex.Message}", ex);
            }
        }
    }
}
