using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<HR> HRs { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }

        public Department()
        {
            HRs = new List<HR>();
            Vacancies = new List<Vacancy>();
        }
    }
}
