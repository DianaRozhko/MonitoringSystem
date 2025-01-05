using System.Collections.Generic;

namespace DAL.Entities
{
    public class Scientist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "Scientist";
        public string Username { get; set; }
        public string Password { get; set; }

        // Колекція звітів, пов'язаних з ученим
        public ICollection<Report> Reports { get; set; } = new List<Report>();

        // Логін/Логаут (якщо хочете використати)
        private bool _isLoggedIn = false;

        public bool Login(string username, string password)
        {
            if (Username == username && Password == password)
            {
                _isLoggedIn = true;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            _isLoggedIn = false;
        }

        public bool IsLoggedIn()
        {
            return _isLoggedIn;
        }
    }
}
