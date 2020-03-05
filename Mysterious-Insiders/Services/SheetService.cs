using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Mysterious_Insiders.Models;

namespace Mysterious_Insiders.Services
{
	public class SheetService
	{
		private readonly IMongoCollection<ModularSheet> sheets;

		public SheetService(ISheetDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			sheets = database.GetCollection<ModularSheet>(settings.SheetCollectionName);

			/* This was used to create the example ModularSheet in the database. If you need to re-create it, uncomment this and run.
			ModularSheet sampleSheet = new ModularSheet();
			ModuleData module = new ModuleData(ModuleData.moduleType.NONE);
			module.Id = "charnameLabel";
			module.X = 5;
			module.Y = 5;
			module.Width = 200;
			module.Height = 50;
			module.TextColor = Color.Blue;
			module.SerializedLogic = "Character Name:";
			sampleSheet.AddModuleData(module);
			module = new ModuleData(ModuleData.moduleType.TEXT);
			module.Id = "charname";
			module.X = 210;
			module.Y = 5;
			module.Width = 500;
			module.Height = 50;
			module.SerializedLogic = ModuleData.SerializeLogicTEXT(50);
			sampleSheet.AddModuleData(module);
			module = new ModuleData(ModuleData.moduleType.NONE);
			module.Id = "storyLabel";
			module.X = 5;
			module.Y = 60;
			module.Width = 150;
			module.Height = 30;
			module.SerializedLogic = "Backstory";
			sampleSheet.AddModuleData(module);
			module = new ModuleData(ModuleData.moduleType.TEXT);
			module.Id = "story";
			module.X = 5;
			module.Y = 100;
			module.Width = 500;
			module.Height = 500;
			module.SerializedLogic = ModuleData.SerializeLogicTEXT(int.MaxValue, 10);
			sampleSheet.AddModuleData(module);
			Create(sampleSheet);
			*/
		}

		public List<ModularSheet> Get() { return sheets.Find(sheet => true).ToList(); }

		public ModularSheet Get(string id) { return sheets.Find<ModularSheet>(sheet => sheet.DatabaseId == id).FirstOrDefault(); }

		public ModularSheet Create(ModularSheet sheet)
		{
			sheets.InsertOne(sheet);
			return sheet;
		}

		public void Update(string id, ModularSheet sheet)
		{
			sheet.DatabaseId = id;
			sheets.ReplaceOne(s => s.DatabaseId == id, sheet);
		}

		public void Remove(ModularSheet sheet)
		{
			sheets.DeleteOne(s => s.DatabaseId == sheet.DatabaseId);
		}

		public void Remove(string id)
		{
			sheets.DeleteOne(sheet => sheet.DatabaseId == id);
		}

		public List<ModularSheet> FilterByUser(string username)
		{
			List<ModularSheet> allSheets = Get();
			allSheets = allSheets.Where(s => s.UserOwner == username).ToList();

			return allSheets;
		}

		public List<ModularSheet> FilterBySheetName(string searchString)
		{
			List<ModularSheet> allSheets = Get().Where(s => s.Name.Contains(searchString)).ToList();

			return allSheets;
		}

		public void CreateDnDSheet() { 
		ModularSheet sampleSheet = new ModularSheet();
		sampleSheet.AddImageUrl("~/Img/armor-class.svg");
		sampleSheet.AddImageUrl("~/Img/initiative.svg");
		sampleSheet.AddImageUrl("~/Img/inspiration.svg");
		sampleSheet.AddImageUrl("~/Img/proficiency-bonus.svg");
		sampleSheet.AddImageUrl("~/Img/speed.svg");
		sampleSheet.AddImageUrl("~/Img/stats.svg");
		ModuleData module = new ModuleData(ModuleData.moduleType.NONE);
		//Armor
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Armor";
		module.X = 500;
		module.Y = 100;
		module.Width = 50;
		module.Height = 50;
		module.BgImageIndex = 0;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
		//Speed
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Speed";
		module.X = 555;
		module.Y = 100;
		module.Width = 50;
		module.Height = 50;
		module.BgImageIndex = 4;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.CHECK);
		module.Id = "Inspiration";
		module.X = 150;
		module.Y = 5;
		module.Width = 150;
		module.Height = 30;
		module.SerializedLogic = ModuleData.SerializeLogicCHECK();
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat1";
		module.X = 20;
		module.Y = 50;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat2";
		module.X = 20;
		module.Y = 120;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat3";
		module.X = 20;
		module.Y = 190;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat4";
		module.X = 20;
		module.Y = 260;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat5";
		module.X = 20;
		module.Y = 330;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat6";
		module.X = 20;
		module.Y = 400;
		module.Width = 50;
		module.Height = 70;
		module.BgImageIndex = 5;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		Update("5e572d68083f8b4924a2411f", sampleSheet);

		//sampleSheet.AddModuleData(module);
		//Create(sampleSheet);
		}

	}
}
