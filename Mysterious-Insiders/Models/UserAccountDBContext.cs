using System;
using System.Collections.Generic;
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

		}
	}

	public class UserAccount
	{
		public long UserAccountID { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

	}
}
