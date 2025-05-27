using System.Security.AccessControl;

namespace _01_Framework.Infrastructure
{
    public class UniversityTypeViewModel:AutoId
    {
        public string Name { get; set; }
        public List<UniversityViewModel> Universities { get; set; }

        public UniversityTypeViewModel(string name)
        {
            Name = name;
        }
    }
}
