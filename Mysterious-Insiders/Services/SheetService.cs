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

		ModuleData module = new ModuleData(ModuleData.moduleType.NONE);
		//Armor
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Armor";
		module.X = 250;
		module.Y = 520;
		module.Width = 70;
		module.Height = 70;
		module.BGImage = "../Img/armor-class.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
		//Speed
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Speed";
		module.X = 335;
		module.Y = 520;
		module.Width = 75;
		module.Height = 75;
		module.BGImage = "../Img/initiative.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
		//Speed
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "init";
		module.X = 430;
		module.Y = 520;
		module.Width = 75;
		module.Height = 75;
		module.BGImage = "../Img/initiative.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Proficiency";
		module.X = 20;
		module.Y = 815;
		module.Width = 200;
		module.Height = 65;
		module.BGImage = "../Img/proficiency-bonus.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Perception";
		module.X = 20;
		module.Y = 915;
		module.Width = 300;
		module.Height = 70;
		module.BGImage = "../Img/perception.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.CHECK);
		module.Id = "Inspiration";
		module.X = 20;
		module.Y = 720;
		module.Width = 200;
		module.Height = 65;
		module.BGImage = "../Img/inspiration.svg";
		module.SerializedLogic = ModuleData.SerializeLogicCHECK();
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat1";
		module.X = 20;
		module.Y = 270;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
						
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat2";
		module.X = 130;
		module.Y = 270;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat3";
		module.X = 20;
		module.Y = 404;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat4";
		module.X = 130;
		module.Y = 404;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat5";
		module.X = 20;
		module.Y = 538;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat6";
		module.X = 130;
		module.Y = 538;
		module.Width = 100;
		module.Height = 175;
		module.BGImage = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "health";
		module.X = 260;
		module.Y = 285;
		module.Width = 250;
		module.Height = 125;
		module.BGImage = "../Img/health.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "temphealth";
		module.X = 260;
		module.Y = 400;
		module.Width = 250;
		module.Height = 125;
		module.BGImage = "../Img/Temphealth.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "hiddie";
		module.X = 260;
		module.Y = 600;
		module.Width = 120;
		module.Height = 90;
		module.BGImage = "../Img/HitDie.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "deathsave";
		module.X = 390;
		module.Y = 600;
		module.Width = 120;
		module.Height = 90;
		module.BGImage = "../Img/DeathSave.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "saves";
		module.X = 260;
		module.Y = 680;
		module.Width = 260;
		module.Height = 250;
		module.BGImage = "../Img/SavingThrows.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);

		//Character Name	
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "characterName";
		module.X = 20;
		module.Y = 157;
		module.Width = 300;
		module.Height = 100;
		module.BGImage = "../Img/charactername.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
		//Character Info	
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "characterInfo";
		module.X = 297;
		module.Y = 100;
		module.Width = 700;
		module.Height = 150;
		module.BGImage = "../Img/characterinfo.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);

		Update("5e572d68083f8b4924a2411f", sampleSheet);

		//sampleSheet.AddModuleData(module);
		//Create(sampleSheet);
		}

	}
}
