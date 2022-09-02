using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Starship
{
	static class SpaceColonizationPathCalculator
	{
		const string defaultFlightPlanFilePath = @"..\..\FlightPlan.txt";   // Created in project root

		#region Calculation methods
		public static void CalculateDistancesFromObject(this List<SpaceObject> universe, SpaceObject currentObject)
		{	
			// Calculate distances to current planet (seen as the "previous object" from the next objects)
			universe.ForEach(x => x.DistanceFromPreviousObject = CalculateDistanceBetweenObjects(currentObject, x));
		}
		
		public static double CalculateDistanceBetweenObjects(SpaceObject fromObject, SpaceObject toObject) 
		{	
			// Uses Pythagoras formula to calculate straight-line discance
			return Math.Round(Math.Sqrt(Math.Pow(toObject.X - fromObject.X, 2) + Math.Pow(toObject.Y - fromObject.Y, 2) + Math.Pow(toObject.Z - fromObject.Z, 2)), 3);
		}

		public static HabitablePlanet FindQuickestColonizablePlanet(List<SpaceObject> universe)
		{
			// Order by nearest to current planet i.e. "previous object"
			List<SpaceObject> orderedUniverseByDistance = universe.OrderBy(x => x.DistanceFromPreviousObject).ToList();
			int travelTime = 0;
			
			// Add travel time according to convension: nearest object is 10min away. subsequent objects are +10min.
			orderedUniverseByDistance.ForEach(x => 
			{
				travelTime += 600; 
				x.TravelTime = travelTime;
				if (x is HabitablePlanet habitablePlanet)
				{
					habitablePlanet.TravelAndColonizationTime = Math.Round(travelTime + habitablePlanet.ColonizationTime, 3);
				}
			});

			// Reorder by the sum of travel and colonization time to determine the quickest colonisable planet (colonization time in most cases far exceed travel time).
			List<HabitablePlanet> orderedColonizableUniverse = 
				orderedUniverseByDistance
					.Where(x => (x is HabitablePlanet hp) && (!hp.IsColonised.GetValueOrDefault()))
					.Cast<HabitablePlanet>()
					.OrderBy(x => x.TravelAndColonizationTime).ToList();
			
			return orderedColonizableUniverse.FirstOrDefault();
		}

		#endregion

		public static string SerializeFlightPlan(List<HabitablePlanet> colonizedPlanets)
		{
			return JsonConvert.SerializeObject(colonizedPlanets, Formatting.Indented, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects, NullValueHandling = NullValueHandling.Ignore});
		}

		public static void SaveReportToFile(string fileContents, string filePath = defaultFlightPlanFilePath)
		{
			File.WriteAllText(filePath, fileContents);
		}

	}
}
