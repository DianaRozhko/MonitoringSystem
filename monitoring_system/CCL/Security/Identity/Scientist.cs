namespace CCL.Security.Identity
{
    public class Scientist : User
    {
        public Scientist(int userId, string name)
            : base(userId, name, nameof(Scientist))
        {
        }

        
    }
}
