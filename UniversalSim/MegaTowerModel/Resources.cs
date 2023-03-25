using System;
using System.Collections.Generic;

namespace S3.UniversalSim.Model 
{
	public class Resource
	{
		public enum ResTypes
		{
			TIME = 0, // Special resources
            ENERGY, 
			WATER,

			COAL = 10, // "mined" resources start from 10
			IRON,
			GAS,
			SILICON, // sand basically, but is needed for glass also

			CONCRETE = 100, // manufactured resources start from 100
			STEEL,
			GLASS,

			BREAD = 1000, // consumer goods start from 1000
			COMPUTERS
		}

		public ResTypes ResType { get; }
		public float Quantity { get; set; }

		public Resource(ResTypes tp, float q)
		{
			ResType = tp;
			Quantity = q;
		}

		/// <summary>
		/// Adds two lists of resources. INEFFICIENT
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static List<Resource> AddLists(List<Resource> a, List<Resource> b)
		{
			List<Resource> res = new List<Resource>();
			foreach (var r in a)
			{
				Resource? r1 = b.Find(x => x.ResType == r.ResType);
				float q = r.Quantity;
				if (r1 != null)
				{
					q += r1.Quantity;
				}
				res.Add(new Resource(r.ResType, q));
			}
			foreach (var r in b)
			{
                Resource? r1 = a.Find(x => x.ResType == r.ResType);
				if (r1 == null)
				{
                    res.Add(new Resource(r.ResType, r.Quantity));
                }
            }
			return res;
		}

		public static List<Resource> ScalarMulList(List<Resource> a, float x)
		{
			List<Resource> res = new List<Resource>();
			foreach (var r in a)
			{
				res.Add(new Resource(r.ResType, r.Quantity * x));
			}
			return res;
		}
	}
}

