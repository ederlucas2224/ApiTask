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

			// Mapear la entidad TaskItem a la tabla Task
			modelBuilder.Entity<TaskItem>(entity =>
			{
				// Especificar el nombre de la tabla en la base de datos
				entity.ToTable("Task");

				entity.HasKey(t => t.Id);

				entity.Property(t => t.Id)
					.ValueGeneratedOnAdd() // IDENTITY(1,1)
					.IsRequired();

				entity.Property(t => t.Title)
					.IsRequired()
					.HasMaxLength(255);

				entity.Property(t => t.Description)
					.HasColumnType("NVARCHAR(MAX)") // Especificar el tipo exacto
					.IsRequired(false); // NULL

				entity.Property(t => t.Status)
					.IsRequired()
					.HasConversion<string>()
					.HasMaxLength(20);

				entity.Property(t => t.CreationDate)
					.IsRequired()
					.HasDefaultValueSql("GETDATE()"); // DEFAULT GETDATE()

				// Configurar el check constraint para Status (opcional pero recomendado)
				entity.HasCheckConstraint("CK_Task_Status", "Status IN ('Pending', 'InProgress', 'Completed')");

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