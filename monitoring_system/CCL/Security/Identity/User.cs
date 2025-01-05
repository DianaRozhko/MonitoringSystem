namespace CCL.Security.Identity
{
    public abstract class User
    {
        
        protected User(int userId, string name, string userType)
        {
            UserId = userId;
            Name = name;
            UserType = userType;
        }

      
        public int UserId { get; }
        public string Name { get; }
        protected string UserType { get; }
    }
}
