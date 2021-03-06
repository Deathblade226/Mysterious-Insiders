﻿using Mysterious_Insiders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Services
{
	public class UserAccountService
	{
		private readonly UserAccountDBContext _dbc;

		public UserAccountService(UserAccountDBContext dbContext)
		{
			this._dbc = dbContext;
		}

		public List<UserAccount> Get() { return _dbc.UserAccounts.Where(a => true).ToList(); }

		public UserAccount Get(long id) { return _dbc.UserAccounts.Where(a => a.UserAccountID == id).FirstOrDefault(); }
		public UserAccount Get(string username) { return _dbc.UserAccounts.Where(a => a.UserName == username).FirstOrDefault(); }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ua"></param>
		/// <returns>Whether the object was added to the database</returns>
		public bool Create(UserAccount ua)
		{
			if (!Contains(ua))
			{
				_dbc.UserAccounts.Add(ua);
				_dbc.SaveChanges();
				return true;
			}
			return false;
		}

		public void Update(long id, UserAccount uaIn)
		{
			UserAccount ua = _dbc.UserAccounts.Find(id);

			throw new NotImplementedException();
		}

		public void Remove(UserAccount ua)
		{
			_dbc.UserAccounts.Remove(ua);
		}

		public void Remove(long id)
		{
			UserAccount toRemove = _dbc.UserAccounts.Find(id);
			_dbc.UserAccounts.Remove(toRemove);
		}

		public void CreateDatabase()
		{
			_dbc.Database.EnsureCreated();
		}

		public bool Contains(UserAccount ua)
		{
			return _dbc.UserAccounts.ToList().Contains(ua);
		}

	}
}
