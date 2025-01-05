using System.Collections.Generic;

namespace BLL.DTO
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Password { get; set; } // Якщо потрібно

        // Дані, пов'язані з сенсором
        public ICollection<int> DataIds { get; set; } = new List<int>();
    }
}
