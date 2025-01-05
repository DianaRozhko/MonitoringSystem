using CCL.Security.Identity;

namespace CCL.Security
{
    
    // Статичний клас для зберігання та отримання поточного користувача.
    
    public static class SecurityContext
    {
        private static User _currentUser;
        public static void SetUser(User user)
        {
            _currentUser = user;
        }
        public static User GetUser()
        {
            return _currentUser;
        }

        public static bool IsAdmin()
        {
            return _currentUser is Admin;
        }

        public static bool IsScientist()
        {
            return _currentUser is Scientist;
        }
    }
}
