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

		public void CreateDnDSheet(string user, string name) { 
		ModularSheet sampleSheet = new ModularSheet();

		sampleSheet.UserOwner = user;
		sampleSheet.Name = name;

		ModuleData module = new ModuleData(ModuleData.moduleType.NONE);
		//Armor
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Armor";
		module.X = 250;
		module.Y = 400;
		module.Width = 70;
		module.Height = 70;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/armor-class.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "ArmorData";
		module.X = 250;
		module.Y = 423;
		module.Width = 50;
		module.Height = 30;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		//Speed
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Speed";
		module.X = 335;
		module.Y = 400;
		module.Width = 75;
		module.Height = 75;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/initiative.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "SpeedData";
		module.X = 336;
		module.Y = 428;
		module.Width = 50;
		module.Height = 30;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "init";
		module.X = 430;
		module.Y = 400;
		module.Width = 75;
		module.Height = 75;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/initiative.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "initData";
		module.X = 430;
		module.Y = 428;
		module.Width = 75;
		module.Height = 30;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Proficiency";
		module.X = 20;
		module.Y = 695;
		module.Width = 200;
		module.Height = 65;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/proficiency-bonus.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "ProficiencyText";
		module.X = 44;
		module.Y = 690;
		module.Width = 200;
		module.Height = 65;
		module.Data = "PROFICIENCY BONUS";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "ProficiencyData";
		module.X = 20;
		module.Y = 720;
		module.Width = 55;
		module.Height = 35;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Perception";
		module.X = 20;
		module.Y = 795;
		module.Width = 300;
		module.Height = 70;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/perception.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "PerceptionText";
		module.X = 50;
		module.Y = 798;
		module.Width = 290;
		module.Height = 70;
		module.Data = "PASSIVE WISDOM (PERCEPTION)";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "PerceptionData";
		module.X = 20;
		module.Y = 822;
		module.Width = 70;
		module.Height = 70;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.CHECK);
		module.Id = "Inspiration";
		module.X = 20;
		module.Y = 600;
		module.Width = 200;
		module.Height = 65;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/inspiration.svg";
		module.SerializedLogic = ModuleData.SerializeLogicCHECK();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "InspirationText";
		module.X = 20;
		module.Y = 624;
		module.Width = 200;
		module.Height = 65;
		module.Data = "INSPIRATION";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicCHECK();
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat1";
		module.X = 20;
		module.Y = 150;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat1Data";
		module.X = 20;
		module.Y = 210;
		module.Width = 100;
		module.Height = 50;
		module.Data = "10";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat1Mod";
		module.X = 20;
		module.Y = 270;
		module.Width = 100;
		module.Height = 50;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
						
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat2";
		module.X = 130;
		module.Y = 150;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat2Data";
		module.X = 130;
		module.Y = 210;
		module.Width = 100;
		module.Height = 50;
		module.Data = "10";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat2Mod";
		module.X = 130;
		module.Y = 270;
		module.Width = 100;
		module.Height = 50;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat3";
		module.X = 20;
		module.Y = 284;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat3Data";
		module.X = 20;
		module.Y = 344;
		module.Width = 100;
		module.Height = 50;
		module.Data = "10";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat3Mod";
		module.X = 20;
		module.Y = 404;
		module.Width = 100;
		module.Height = 50;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat4";
		module.X = 130;
		module.Y = 284;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat4Data";
		module.X = 130;
		module.Y = 344;
		module.Width = 100;
		module.Height = 50;
		module.Data = "10";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat4Mod";
		module.X = 130;
		module.Y = 404;
		module.Width = 100;
		module.Height = 50;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat5";
		module.X = 20;
		module.Y = 418;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat5Data";
		module.X = 20;
		module.Y = 478;
		module.Width = 100;
		module.Height = 50;
		module.TextColor = Color.Black;
		module.Data = "10";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat5Mod";
		module.X = 20;
		module.Y = 538;
		module.Width = 100;
		module.Height = 50;
		module.TextColor = Color.Black;
		module.Data = "0";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "Stat6";
		module.X = 130;
		module.Y = 418;
		module.Width = 100;
		module.Height = 175;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/stats.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
		
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat6Data";
		module.X = 130;
		module.Y = 478;
		module.Width = 100;
		module.Height = 50;
		module.Data = "10";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "Stat6Mod";
		module.X = 130;
		module.Y = 538;
		module.Width = 100;
		module.Height = 50;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "health";
		module.X = 260;
		module.Y = 165;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/health.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "healthData";
		module.X = 270;
		module.Y = 185;
		module.Width = 230;
		module.Height = 85;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "temphealth";
		module.X = 260;
		module.Y = 280;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/Temphealth.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NUMERIC);
		module.Id = "temphealthData";
		module.X = 270;
		module.Y = 300;
		module.Width = 230;
		module.Height = 85;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "hiddie";
		module.X = 260;
		module.Y = 480;
		module.Width = 120;
		module.Height = 90;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/HitDie.svg";
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "hiddieData";
		module.X = 260;
		module.Y = 490;
		module.Width = 120;
		module.Height = 30;
		module.Data = "0";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicNUMERIC(ModuleNumeric.KindOfNumber.INTEGER);
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "deathsave";
		module.X = 390;
		module.Y = 480;
		module.Width = 120;
		module.Height = 90;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/DeathSave.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "saves";
		module.X = 260;
		module.Y = 560;
		module.Width = 256;
		module.Height = 250;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/SavingThrows.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "skills";
		module.X = 820;
		module.Y = 179;
		module.Width = 350;
		module.Height = 687;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/skills.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
		
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "traits";
		module.X = 1500;
		module.Y = 20;
		module.Width = 400;
		module.Height = 850;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/features&traits.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "traitsData";
		module.X = 1515;
		module.Y = 35;
		module.Width = 370;
		module.Height = 812;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "languages";
		module.X = 1170;
		module.Y = 250;
		module.Width = 300;
		module.Height = 300;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/languages.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "languagesData";
		module.X = 1177;
		module.Y = 282;
		module.Width = 287;
		module.Height = 220;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "equipment";
		module.X = 530;
		module.Y = 565;
		module.Width = 300;
		module.Height = 300;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/equipment.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "equipmentData";
		module.X = 620;
		module.Y = 574;
		module.Width = 200;
		module.Height = 270;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "actions";
		module.X = 1170;
		module.Y = 530;
		module.Width = 300;
		module.Height = 350;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/attacks.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "actionsData";
		module.X = 1185;
		module.Y = 548;
		module.Width = 270;
		module.Height = 305;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);

		//Character Name	
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "characterName";
		module.X = 20;
		module.Y = 57;
		module.Width = 300;
		module.Height = 100;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/charactername.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "characterNameData";
		module.X = 40;
		module.Y = 90;
		module.Width = 255;
		module.Height = 20;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
		//Character Info	
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "characterInfo";
		module.X = 297;
		module.Y = 0;
		module.Width = 700;
		module.Height = 150;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/characterinfo.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		///
		///
		///
		///
		///
		///

		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "personalityTraits";
		module.X = 540;
		module.Y = 165;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/personality-traits.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "personalityTraitsData";
		module.X = 550;
		module.Y = 185;
		module.Width = 230;
		module.Height = 75;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "ideals";
		module.X = 540;
		module.Y = 260;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/ideals.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "idealsData";
		module.X = 550;
		module.Y = 287;
		module.Width = 230;
		module.Height = 73;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "bonds";
		module.X = 540;
		module.Y = 345;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/bonds.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "bondsData";
		module.X = 550;
		module.Y = 372;
		module.Width = 230;
		module.Height = 73;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);
			
		module = new ModuleData(ModuleData.moduleType.NONE);
		module.Id = "flaws";
		module.X = 540;
		module.Y = 443;
		module.Width = 250;
		module.Height = 125;
		module.TextColor = Color.Black;
		module.BgImageUrl = "../Img/flaws.svg";
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);

		module = new ModuleData(ModuleData.moduleType.TEXT);
		module.Id = "flawsData";
		module.X = 550;
		module.Y = 470;
		module.Width = 230;
		module.Height = 75;
		module.Data = "Placeholder";
		module.TextColor = Color.Black;
		module.SerializedLogic = ModuleData.SerializeLogicTEXT();
		sampleSheet.AddModuleData(module);

		Create(sampleSheet);
		}

	}
}
