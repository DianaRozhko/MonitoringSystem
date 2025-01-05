using System.Collections.Generic;

namespace BLL.DTO
{
    public class ScientistDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        // Звіти, пов'язані з ученим
        public ICollection<int> ReportIds { get; set; } = new List<int>();
    }
}
