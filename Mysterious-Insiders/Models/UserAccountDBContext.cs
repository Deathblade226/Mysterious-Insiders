using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mysterious_Insiders.Models
{
	public class UserAccountDBContext : DbContext
	{
		public DbSet<UserAccount> UserAccounts { get; set; }
		public UserAccountDBContext(DbContextOptions<UserAccountDBContext> options) : base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();
		}
	}

	
}
