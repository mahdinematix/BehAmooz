namespace _01_Framework.Infrastructure
{
    public class MajorViewModel : AutoId
    {
        public string Name { get; set; }


        public MajorViewModel(string name)
        {
            Name = name;
        }
    }

}
