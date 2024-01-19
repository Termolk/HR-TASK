using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class Vacancy
    {
        public int VacancyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public VacancyStatus Status { get; set; }
        public int HRId { get; set; }
        public Department Department { get; set; }
        public HR HR { get; set; }
        public ICollection<Application> Applications { get; set; }

        public Vacancy()
        {
            Applications = new List<Application>();
        }
    }
}
