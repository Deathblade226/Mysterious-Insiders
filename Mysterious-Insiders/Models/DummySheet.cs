using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace Mysterious_Insiders.Models
{
	public class DummySheet
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string DatabaseId { get; set; }

		[BsonElement("Name")]
		[JsonProperty("Name")]
		public string CharacterName { get; set; }

		public int Health { get; set; }
		public int Mana { get; set; }
	}
}
