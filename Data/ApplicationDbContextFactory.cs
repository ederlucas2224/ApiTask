using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaskAPIDb;Trusted_Connection=true;MultipleActiveResultSets=true");

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
