namespace _01_Framework.Infrastructure
{
    public class UniversityViewModel : AutoId
    {
        public string Name { get; set; }
        public int UniversityTypeId { get; set; }
        public UniversityTypeViewModel Type { get; set; }

        public UniversityViewModel(string name, int universityTypeId)
        {
            Name = name;
            UniversityTypeId = universityTypeId;
        }
    }
}
