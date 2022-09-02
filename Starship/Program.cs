using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starship
{
	class Program
	{
		static void Main(string[] args)
		{
			List<SpaceObject> universe;

			// "-generate" triggers universe generation and saving to file only
			if (args.Length == 0 || "-generate".Equals(args[0]))
			{
				int numberOfObjects = 15000;
				universe = SpaceObjectManipulator.GenerateUniverse(numberOfObjects);
				//RunTests(universe);
				
				if (!(args.Length == 0) && "-generate".Equals(args[0]) && args.Length > 1)	// If filepath to universe file specified
				{
					SpaceObjectManipulator.SaveUniverseToFile(universe, args[1]);
				}
				else
				{
					SpaceObjectManipulator.SaveUniverseToFile(universe);
				}
				Console.WriteLine("Universe generated and saved to file.");
			}

			// "-plan" triggers loading universe from file, creating flight plan and saving flight plan to file only
			if (args.Length == 0 || "-plan".Equals(args[0]))
			{
				// Load universe from file
				if (!(args.Length == 0) && "-plan".Equals(args[0]) && args.Length > 1)	// If filepath to universe file specified
				{
					universe = SpaceObjectManipulator.LoadUniverseFromFile(args[1]);
				}
				else
				{
					universe = SpaceObjectManipulator.LoadUniverseFromFile();
				}


				// Calculate flight plan
				double maxTime = 2 * 24 * 60 * 60;
				double totalTimeTaken = 0;
				int totalSurfaceAreaColonized = 0;
				HabitablePlanet currentPlanet = (HabitablePlanet) SpaceObjectManipulator.HomePlanet;
				HabitablePlanet nextPlanet;
				List<HabitablePlanet> colonizedPlanets = new List<HabitablePlanet>();

				Console.WriteLine("Calculating Flight Plan.");
				while (totalTimeTaken < maxTime)
				{
					universe.CalculateDistancesFromObject(currentPlanet);
					nextPlanet = SpaceColonizationPathCalculator.FindQuickestColonizablePlanet(universe);

					if (!(nextPlanet is null) && totalTimeTaken + nextPlanet.TravelAndColonizationTime < maxTime)
					{
						currentPlanet = nextPlanet;
						currentPlanet.IsColonised = true;
						colonizedPlanets.Add((HabitablePlanet) currentPlanet.Clone());	// Clone the object to prevent time/distance values being overwritten when recalculating
						totalTimeTaken += currentPlanet.TravelAndColonizationTime.GetValueOrDefault();
						totalSurfaceAreaColonized += currentPlanet.SurfaceAreaNeededToColonize();
						Console.WriteLine($"Simulated colonising of planet {currentPlanet.Id}");
					}
					else
					{
						Console.WriteLine((nextPlanet is null) ? "No more planets to colonize." : $"Will not colonize next planet {nextPlanet.Id} in time.");
						break;
					}
				}
				Console.WriteLine();


				// Output report
				TimeSpan totalTime = new TimeSpan((long) Math.Round(totalTimeTaken * 10000000));
				StringBuilder output = new StringBuilder();
				output.AppendLine("Flight Plan: ");
				output.AppendLine($"Total Time: {totalTime:hh\\hmm\\mss\\.fff\\s}");
				output.AppendLine($"Total Surface Area to be Colonized: {totalSurfaceAreaColonized} SqM");	// Assumption: only physically colonised portion of surface area is counted.
				output.AppendLine($"Total Planets to be Colonized: {colonizedPlanets.Count} planets");
				output.AppendLine();
				output.AppendLine(totalTimeTaken == 0 ? "No habitable planet is close or small enough to colonize within 24 hours." : SpaceColonizationPathCalculator.SerializeFlightPlan(colonizedPlanets));

				if (!(args.Length == 0) && "-plan".Equals(args[0]) && args.Length > 2)	// If filepath to flight plan file specified
				{
					SpaceColonizationPathCalculator.SaveReportToFile(output.ToString(), args[2]);
				}
				else
				{
					SpaceColonizationPathCalculator.SaveReportToFile(output.ToString());
				}

				Console.WriteLine(output);
				Console.WriteLine();
				Console.WriteLine("Flight Plan written to file.");
			}

			Console.WriteLine();
			Console.WriteLine("Press <Enter> to exit.");
			Console.ReadLine();
		}

		static void RunTests(List<SpaceObject> universe)
		{
			// Test Deserialization of single object
/*
			string serializedObject = 
@"{
  ""$type"": ""Starship.HabitablePlanet, Starship"",
  ""Id"": ""c4bb247d-ac43-4382-9bb4-b5363c1f56ec"",
  ""X"": 743485694,
  ""Y"": 596686023,
  ""Z"": 975518672,
  ""SurfaceArea"": 18529116,
  ""ColonizationTime"": 398376.037
}
";
			SpaceObject spaceObject = SpaceObjectManipulator.DeserializeObject(serializedObject);
*/

			// Test Serialization/Deserialization of entire universe
/*
			string serializedUniverse = SpaceObjectManipulator.SerializeUniverse(universe);
			Console.WriteLine(serializedUniverse);
			List<SpaceObject> newUniverse = SpaceObjectManipulator.DeserializeUniverse(serializedUniverse);
*/

			// Test Serialization/Deserialization of entire universe using files
/*
			SpaceObjectManipulator.SaveUniverseToFile(universe);
			List<SpaceObject> newUniverse = SpaceObjectManipulator.LoadUniverseFromFile();
*/
		}
	}
}
