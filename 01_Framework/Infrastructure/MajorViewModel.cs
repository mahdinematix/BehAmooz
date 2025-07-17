namespace _01_Framework.Infrastructure
{
    public class MajorViewModel
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public MajorViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
