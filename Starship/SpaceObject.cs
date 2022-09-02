using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Starship
{
	abstract class SpaceObject
	{
		[JsonProperty(Order = -2)]
		public Guid Id {get; set;}
		[JsonProperty(Order = -2)]
		public int X {get; set;}
		[JsonProperty(Order = -2)]
		public int Y {get; set;}
		[JsonProperty(Order = -2)]
		public int Z {get; set;}
		[JsonProperty(Order = -2)]
		public double? DistanceFromPreviousObject {get; set;}
		[JsonProperty(Order = -2)]
		public double? TravelTime {get; set;}

		public SpaceObject()
		{

		}

		public SpaceObject(Random random)
		{
			Id = Guid.NewGuid();		// Every entity needs a unique Id! :)
			X = random.Next(1000000000);
			Y = random.Next(1000000000);
			Z = random.Next(1000000000);
		}

	}
}
