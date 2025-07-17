namespace _01_Framework.Infrastructure
{
    public class UniversityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UniversityTypeId { get; set; }
        public UniversityTypeViewModel Type { get; set; }

        public UniversityViewModel(int id, string name, int universityTypeId)
        {
            Id = id;
            Name = name;
            UniversityTypeId = universityTypeId;
        }
    }
}
