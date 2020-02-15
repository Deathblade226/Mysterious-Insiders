using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Mysterious_Insiders.Models;

namespace Mysterious_Insiders.Services
{
	public class SheetService
	{
		private readonly IMongoCollection<DummySheet> sheets;

		public SheetService(ISheetDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			sheets = database.GetCollection<DummySheet>(settings.SheetCollectionName);
		}

		public List<DummySheet> Get() { return sheets.Find(sheet => true).ToList(); }

		public DummySheet Get(string id) { return sheets.Find<DummySheet>(sheet => sheet.DatabaseId == id).FirstOrDefault(); }

		public DummySheet Create(DummySheet sheet)
		{
			sheets.InsertOne(sheet);
			return sheet;
		}

		public void Update(string id, DummySheet sheet)
		{
			sheets.ReplaceOne(s => s.DatabaseId == id, sheet);
		}

		public void Remove(DummySheet sheet)
		{
			sheets.DeleteOne(s => s.DatabaseId == sheet.DatabaseId);
		}

		public void Remove(string id)
		{
			sheets.DeleteOne(sheet => sheet.DatabaseId == id);
		}

	}
}
