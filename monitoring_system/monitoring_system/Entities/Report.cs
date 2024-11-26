using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories.Impl;

namespace DAL.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }

        public int ScientistId { get; set; }
        public Scientist Scientist { get; set; }

        public int DataId { get; set; }
        public Data Data { get; set; }
    }
}
