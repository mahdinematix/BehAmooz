namespace _01_Framework.Infrastructure
{
    public class DayViewModel : AutoId
    {
        public string Name { get; set; }

        public DayViewModel(string name)
        {
            Name = name;
        }
    }
}
