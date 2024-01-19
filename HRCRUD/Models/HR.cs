using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class HR
    {
        public int HRId { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int ClosedVacanciesCount { get; set; }
        public List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();

        public void UpdateClosedVacanciesCount()
        {
            ClosedVacanciesCount = Vacancies.Count(v => v.Status == VacancyStatus.Closed);
        }
    }
}
