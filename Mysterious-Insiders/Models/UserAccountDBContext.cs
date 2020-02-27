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

	public class UserAccount
	{


		public long UserAccountID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a username")]
		[MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
		public string UserName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "You must enter a password")]
		[MinLength(6, ErrorMessage = "Your password must be at least 6 characters")]
		public string Password { get; set; }

		public static bool operator ==(UserAccount ua1, UserAccount ua2)
		{
			return (ua1.UserName == ua2.UserName);
		}
		public static bool operator !=(UserAccount ua1, UserAccount ua2)
		{
			return !(ua1 == ua2);
		}

		public override bool Equals(object obj)
		{
			return this == (UserAccount)obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}


	}
}
