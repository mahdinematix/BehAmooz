namespace _01_Framework.Infrastructure
{
    public class DayViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DayViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
