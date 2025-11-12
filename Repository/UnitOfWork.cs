using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	/// <summary>
	/// Implementación del patrón Unit of Work para gestionar transacciones y repositorios
	/// de manera coordinada sobre el mismo contexto de base de datos
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private ITaskRepository _tasks;

		/// <summary>
		/// Inicializa una nueva instancia del UnitOfWork con el contexto de base de datos
		/// </summary>
		/// <param name="context">Contexto de Entity Framework para acceso a datos</param>
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Repositorio para operaciones específicas de entidades TaskItem
		/// Implementa el patrón Lazy Initialization para crear la instancia solo cuando se necesita
		/// </summary>
		public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);

		/// <summary>
		/// Guarda todos los cambios pendientes en el contexto de base de datos de manera asíncrona
		/// </summary>
		/// <returns>Número de registros afectados en la base de datos</returns>
		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Inicia una nueva transacción de base de datos de manera asíncrona
		/// Todos los cambios posteriores se agruparán en esta transacción
		/// </summary>
		public async Task BeginTransactionAsync()
		{
			await _context.Database.BeginTransactionAsync();
		}

		/// <summary>
		/// Confirma la transacción actual de manera asíncrona, aplicando todos los cambios a la base de datos
		/// </summary>
		public async Task CommitTransactionAsync()
		{
			await _context.Database.CommitTransactionAsync();
		}

		/// <summary>
		/// Revierte la transacción actual de manera asíncrona, descartando todos los cambios no confirmados
		/// </summary>
		public async Task RollbackTransactionAsync()
		{
			await _context.Database.RollbackTransactionAsync();
		}

		/// <summary>
		/// Libera los recursos del contexto de base de datos siguiendo el patrón IDisposable
		/// </summary>
		public void Dispose()
		{
			_context?.Dispose();
		}
	}
}