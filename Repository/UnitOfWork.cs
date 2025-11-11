using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private ITaskRepository _tasks;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async Task BeginTransactionAsync()
		{
			await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await _context.Database.CommitTransactionAsync();
		}

		public async Task RollbackTransactionAsync()
		{
			await _context.Database.RollbackTransactionAsync();
		}

		public void Dispose()
		{
			_context?.Dispose();
		}
	}
}
