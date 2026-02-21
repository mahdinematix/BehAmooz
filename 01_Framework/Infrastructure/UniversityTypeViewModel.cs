using System.Security.AccessControl;

namespace _01_Framework.Infrastructure
{
    public class UniversityTypeViewModel
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public UniversityTypeViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
