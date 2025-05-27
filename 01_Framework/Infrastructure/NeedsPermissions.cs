namespace _01_Framework.Infrastructure
{
    public class NeedsPermissions : Attribute
    {
        public int Permission { get; set; }

        public NeedsPermissions(int permission)
        {
            Permission = permission;
        }
    }
}
