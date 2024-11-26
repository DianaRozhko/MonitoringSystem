using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Scientist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "Scientist";
        public string Username { get; set; }
        public string Password { get; set; }

        private bool _isLoggedIn = false;
        public ICollection<Report> Reports { get; set; }

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
