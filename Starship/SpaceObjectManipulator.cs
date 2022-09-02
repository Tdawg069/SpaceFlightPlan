using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Starship
{
	static class SpaceObjectManipulator
	{
		const string defaultUniverseFilePath = @"..\..\Universe.txt";	// Created in project root
		static readonly SpaceObject _homePlanet = new HabitablePlanet()	// Initialising home planet
		{
			Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
			X = 123123991, 
			Y = 98098111, 
			Z = 456456999,
			SurfaceArea = 510000000,
			IsColonised = true,
			TravelTime = 0,
			ColonizationTime = 0,
			DistanceFromPreviousObject = 0,
			TravelAndColonizationTime = 0
		};
		public static SpaceObject HomePlanet {get {return _homePlanet;} }

		#region Generation methods
		public static SpaceObject GenerateObject(Random random)	// %chance -> SpaceMonster:50%, HabitablePlanet:25%, InhabitablePlanet:25%
		{
			return (random.Next(2) == 0) ? new SpaceMonster(random) : ((random.Next(2) == 0) ? (SpaceObject)new InhabitablePlanet(random) : new HabitablePlanet(random));
		}

		public static List<SpaceObject> GenerateUniverse(int numberOfObjects = 20)
		{
			Random random = new Random();	//Random object is created once and passed into each created SpaceObject to generate co-ordinates. Eliminates risk of multiple Random objects created with same seed.

			List<SpaceObject> universe = new List<SpaceObject>(numberOfObjects);
			for (int i = 0; i < numberOfObjects; i++)
			{
				universe.Add(GenerateObject(random));
			}

			return universe;
		}

		#endregion

		#region Serialization methods
		public static string SerializeObject(SpaceObject spaceObject)
		{
			return JsonConvert.SerializeObject(spaceObject, Formatting.Indented, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects});
		}

		public static SpaceObject DeserializeObject(string serializedSpaceObject)
		{
			return JsonConvert.DeserializeObject<SpaceObject>(serializedSpaceObject, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects});
		}

		public static string SerializeUniverse(List<SpaceObject> universe)
		{
			return JsonConvert.SerializeObject(universe, Formatting.Indented, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects, NullValueHandling = NullValueHandling.Ignore});
		}

		public static List<SpaceObject> DeserializeUniverse(string serializedUniverse)
		{
			return JsonConvert.DeserializeObject<List<SpaceObject>>(serializedUniverse, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects, NullValueHandling = NullValueHandling.Ignore});
		}

		public static SpaceObject Clone(this SpaceObject spaceObject)
		{
			return DeserializeObject(SerializeObject(spaceObject));
		}

		#endregion

		#region File Storage methods
		public static void SaveUniverseToFile(List<SpaceObject> universe, string filePath = defaultUniverseFilePath)
		{
			string fileContents = SerializeUniverse(universe);
			File.WriteAllText(filePath, fileContents);
		}

		public static List<SpaceObject> LoadUniverseFromFile(string filePath = defaultUniverseFilePath)
		{
			string fileContents = File.ReadAllText(filePath);
			List<SpaceObject> output = DeserializeUniverse(fileContents);
			return output;
		}

		#endregion
	}
}
