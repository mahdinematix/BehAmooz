namespace _01_Framework.Infrastructure
{
    public class AutoId
    {
        private static int _counter = 0;

        public int Id { get; private set; } 

        public AutoId()
        {
            _counter++; 
            Id = _counter; 
        }
    }
}
