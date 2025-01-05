using System;

namespace DAL.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }

        // Зв'язок з Scientist
        public int ScientistId { get; set; }
        public Scientist Scientist { get; set; }

        // Зв'язок з Data
        public int DataId { get; set; }
        public Data Data { get; set; }
    }
}
