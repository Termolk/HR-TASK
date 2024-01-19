using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Skills { get; set; }
        public CandidateStatus Status { get; set; }

        public ICollection<TestTask> TestTasks { get; set; }
        public ICollection<Application> Applications { get; set; }

        public Candidate()
        {
            TestTasks = new List<TestTask>();
            Applications = new List<Application>();
        }
    }
}
