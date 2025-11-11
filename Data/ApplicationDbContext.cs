using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Model.TaskStatus;

namespace Data
{
	public class ApplicationDbContext : DbContext
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<TaskItem> Tasks { get; set; }
		/// <summary>
		/// Metodo para crear el modelado
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<TaskItem>(entity =>
			{
				entity.HasKey(t => t.Id);

				entity.Property(t => t.Title)
					.IsRequired()
					.HasMaxLength(255);

				entity.Property(t => t.Description)
					.HasMaxLength(1000);

				entity.Property(t => t.Status)
					.IsRequired()
					.HasConversion<string>()
					.HasMaxLength(20);

				entity.Property(t => t.CreationDate)
					.IsRequired();

				// Seed data para pruebas
				entity.HasData(
					new TaskItem
					{
						Id = 1,
						Title = "Tarea de ejemplo 1",
						Description = "Esta es una tarea de ejemplo",
						Status = TaskStatus.Pending,
						CreationDate = DateTime.UtcNow.AddDays(-2)
					},
					new TaskItem
					{
						Id = 2,
						Title = "Tarea de ejemplo 2",
						Description = "Otra tarea de ejemplo",
						Status = TaskStatus.Completed,
						CreationDate = DateTime.UtcNow.AddDays(-1)
					}
				);
			});
		}
	}
}
