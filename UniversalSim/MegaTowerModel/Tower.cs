using System;
using System.Collections.Generic;

namespace S3.UniversalSim.Model
{
	/// <summary>
	/// Main class that contains, which contain Buildings on each floor and manages
	/// everything.
	///
	/// Possible actions a player can take:
	/// - Build a floor
	/// - Build a building inside a ready floor
	/// </summary>
	public class Tower
	{
		/// <summary>
		/// Stores all buildings in format num of florr --> List buildings
		/// </summary>
		public Dictionary<int, List<Building>> Floors { get; }
		/// <summary>
		/// Construction costs for each floor number - later
		/// will have to be read from settings etc
		/// </summary>
		public Dictionary<int, List<Resource>> ConstructionCost { get; }
		/// <summary>
		/// Current resources that the tower has, either bought or produced
		/// </summary>
		public Dictionary<Resource.ResTypes, List<Resource>> CurrentResources { get; }

		public Tower()
		{
			Floors = new Dictionary<int, List<Building>>();
			CurrentResources = new Dictionary<Resource.ResTypes, List<Resource>>();

			ConstructionCost = new Dictionary<int, List<Resource>>();
			// initializing construction costs for each floor explicitly
			ConstructionCost[0] = new List<Resource>(new Resource[] {
				new Resource(Resource.ResTypes.TIME, 5),
                new Resource(Resource.ResTypes.CONCRETE, 100),
                new Resource(Resource.ResTypes.GLASS, 50)
            });
		}
	}
}

