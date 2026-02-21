using _01_Framework.Infrastructure;

namespace StudyManagement.Application.Contracts.University
{
    public class DefineUniversity
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public List<UniversityTypeViewModel> UniTypes { get; set; }
    }
}
