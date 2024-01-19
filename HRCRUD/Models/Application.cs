using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public ApplicationStatus Status { get; set; }
        public Candidate Candidate { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}
