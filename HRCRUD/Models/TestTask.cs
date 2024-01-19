using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Models
{
    public class TestTask
    {
        public int TestTaskId { get; set; }
        public string Description { get; set; }
        public TaskComplexity Complexity { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
