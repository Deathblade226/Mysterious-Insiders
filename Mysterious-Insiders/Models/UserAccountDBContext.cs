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

		}
	}

	public class UserAccount
	{

		//public UserAccount()
		//{
		//	UserAccountID = userIDIncrement++;
		//}
		//public UserAccount(string username, string password)
		//{
		//	UserName = username;
		//	Password = password;
		//	UserAccountID = userIDIncrement++;
		//}

		//[Key]
		public long UserAccountID { get; set; }
		//static long userIDIncrement = 0;
		public string UserName { get; set; }
		public string Password { get; set; }

	}
}
