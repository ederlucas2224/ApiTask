using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class ApplicationDbContextFactory
	{
		private readonly IConfiguration _configuration;

		public ApplicationDbContextFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public ApplicationDbContext CreateDbContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var connectionString = _configuration.GetConnectionString("DefaultConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
