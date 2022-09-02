using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Starship
{
	class HabitablePlanet : Planet
	{
		public int SurfaceArea {get; set;}
		public double ColonizationTime {get; set;}
		public double? TravelAndColonizationTime {get; set;}
		[JsonIgnore]
		public bool? IsColonised {get; set;}

		public HabitablePlanet()
		{

		}

		public HabitablePlanet(Random random) : base(random)
		{
			SurfaceArea = random.Next(1000000, 4000000);//100000000 //4000000 // Smaller max surface area used to improve colonization rate.
			ColonizationTime = Math.Round(SurfaceAreaNeededToColonize() * 0.043, 3);
		}

		public int SurfaceAreaNeededToColonize() => (SurfaceArea / 2) + 1;	// 50% area + 1 SqMeter to colonize
		
	}
}
