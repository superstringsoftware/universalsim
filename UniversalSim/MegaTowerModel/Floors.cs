using System;
using System.Collections.Generic;



namespace S3.UniversalSim.Model
{
    /*
    public class Floor
    {
        public int Num { get; set; }
        public List<Building> Buildings { get; }

        public Floor()
        {
            Buildings = new List<Building>();
        }
    }*/

    /// <summary>
    /// Base building class
    /// </summary>
    public class Building
    {
        public string Name { get; set; }

        
        /// <summary>
        /// Resources that need to be spent each turn for this building
        /// to function
        /// </summary>
        public List<Resource> OPEX { get; set; }
        /// <summary>
        /// Resources that are spent each turn per SIM in this building
        /// (either living or working, but not visiting)
        /// </summary>
        public List<Resource> OPEXSim { get; set; }
        /// <summary>
        /// Resources that need to be spent once to build this type of building
        /// </summary>
        public List<Resource> CAPEX { get; set; }

        public Building(string name, List<Resource> opex,
            List<Resource> opexsim, List<Resource> capex)
        {
            Name = name;
            OPEX = opex;
            OPEXSim = opexsim;
            CAPEX = capex;
        }

        /// <summary>
        /// Calculates resource consumption each turn. Virtual since it depends
        /// on # of sims and other potential conditions.
        /// </summary>
        /// <returns>Simply base OPEX</returns>
        public virtual List<Resource> CalculateConsumption()
        {
            return OPEX;
        }

        /// <summary>
        /// This calculates whatever the building produces, both useful and not
        /// (such as garbage)
        /// </summary>
        /// <returns>Base class returns nothing</returns>
        public virtual List<Resource>? CalculateProduction()
        {
            return null;
        }
    }

    /// <summary>
    /// Building where Sims can live
    /// </summary>
    public class BuildingHousing : Building
    {
        /// <summary>
        /// current sims living in it
        /// </summary>
        public List<Sim> Sims { get; }

        /// <summary>
        /// square footage (in meters)
        /// </summary>
        public float Area { get; set; }
        /// <summary>
        /// some sort of setting the overall quality
        /// </summary>
        public int Level { get; set; }

        public BuildingHousing(string name, List<Resource> opex,
            List<Resource> opexsim, List<Resource> capex,
            float area, int level) : base(name, opex, opexsim, capex)
        {
            Sims = new List<Sim>();
            Area = area;
            Level = level;
        }

        /// <summary>
        /// Calculates resource consumption in one turn by adding
        /// base consumption and per sim
        /// </summary>
        /// <returns>Consumption Resource vector</returns>
        public override List<Resource> CalculateConsumption()
        {
            List<Resource> res = Resource.ScalarMulList(OPEX, 1);
            foreach (var s in Sims)
            {
                float fix = 200 - s.Education;
                if (fix < 10) fix = 10;
                // consumption per sim depends on education level (logarithmically)
                res = Resource.AddLists(res, Resource.ScalarMulList(OPEXSim, MathF.Log10(fix)));
            }
            return res;
        }

        /// <summary>
        /// Make a Sim inhabit this house
        /// </summary>
        /// <param name="s"></param>
        public void AddSim(Sim s)
        {
            Sims.Add(s);
            s.Home = this;
        }

        public void RemoveSim(Sim s)
        {

        }
    }

    /// <summary>
    /// Building with Jobs
    /// </summary>
    public class BuildingJobs : Building
    {
        /// <summary>
        /// Dictionary that maps education level to number of jobs and salary
        /// </summary>
        public Dictionary<Sim.EducationLevel, (int, float)> Jobs { get; }
        /// <summary>
        /// Employed sims in job places by edu level
        /// Sims can be overqualified, but can't be underqualified
        /// </summary>
        public Dictionary<Sim.EducationLevel, List<Sim>> Employees { get; }

        public BuildingJobs(string name, List<Resource> opex,
            List<Resource> opexsim, List<Resource> capex) : base(name, opex, opexsim, capex)
        {
            Jobs = new Dictionary<Sim.EducationLevel, (int, float)>(5);
            Jobs[Sim.EducationLevel.None] = (0, 0);
            Jobs[Sim.EducationLevel.School] = (0, 0);
            Jobs[Sim.EducationLevel.College] = (0, 0);
            Jobs[Sim.EducationLevel.Master] = (0, 0);
            Jobs[Sim.EducationLevel.PhD] = (0, 0);

            Employees = new Dictionary<Sim.EducationLevel, List<Sim>>(5);
            Employees[Sim.EducationLevel.None] = new List<Sim>();
            Employees[Sim.EducationLevel.School] = new List<Sim>();
            Employees[Sim.EducationLevel.College] = new List<Sim>();
            Employees[Sim.EducationLevel.Master] = new List<Sim>();
            Employees[Sim.EducationLevel.PhD] = new List<Sim>();
        }

        /// <summary>
        /// how many taken jobs of a given level there are
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public int TakenJobs(Sim.EducationLevel lvl)
        {
            return Employees[lvl].Count();
        }

        /// <summary>
        /// how many free jovs of a given level there are
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public int FreeJobs(Sim.EducationLevel lvl)
        {
            return (Jobs[lvl].Item1 - TakenJobs(lvl));
        }

    }

    /// <summary>
    /// Type of building that consumes some resources and produces other
    /// </summary>
    public class BuildingMfg : BuildingJobs
    {
        /// <summary>
        /// what kind of thing we are producing
        /// </summary>
        public Resource Production { get; set; }

        public BuildingMfg(string name, List<Resource> opex,
            List<Resource> opexsim, List<Resource> capex) : base(name, opex, opexsim, capex)
        {
            
        }
    }
}

