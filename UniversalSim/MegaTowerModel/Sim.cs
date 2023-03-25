namespace S3.UniversalSim.Model;

public class Sim
{
    public enum EducationLevel
    {
        None,
        School, 
        College, 
        Master,
        PhD
    }

    public float Age { get; set; }
    public float Health { get; set; }
    public float IQ { get; set; }
    public float Education { get; set; }
    public float Happiness { get; set; }
    public float Salary { get; set; }
    public float Savings { get; set; }

    public EducationLevel EduLevel
    {
        get
        {
            if (Education < 20) return EducationLevel.None;
            if (Education < 50) return EducationLevel.School;
            if (Education < 100) return EducationLevel.College;
            if (Education < 200) return EducationLevel.Master;
            return EducationLevel.PhD;
        }
    }

    public string Name { get; set; }

    private BuildingHousing? _home;
    /// <summary>
    /// Home building for the Sim. Processing logic here instead of a building.
    /// </summary>
    public BuildingHousing? Home {
        get { return _home; }
        set
        {
            if (value != null)
            {
                // setting a home to a new home
                if (_home != null)
                {
                    // current home is not null, so we need to remove sim from there
                    _home.Sims.Remove(this);
                }
                _home = value;
                _home.Sims.Add(this);
            } else
            {
                // new home is null, so simply need to remove from the current home
                if (_home != null)
                {
                    _home.Sims.Remove(this);
                }
                _home = value;
            }
        }
    }
    /// <summary>
    /// Work building for the Sim
    /// </summary>
    public BuildingJobs? Work { get; set; }

    public Sim(string name, float age, float health, float iq, float education, float happiness, float salary, float savings)
    {
        Name = name;
        Age = age;
        Health = health;
        IQ = iq;
        Education = education;
        Happiness = happiness;
        Salary = salary;
        Savings = savings;
    }
}
