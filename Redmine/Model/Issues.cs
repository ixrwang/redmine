using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redmine.Model
{
    public class Issues
    {
        public string id{ get; set; }
        public Item project { get; set; }
        public Item tracker { get; set; }
        public Item status { get; set; }
        public Item priority { get; set; }
        public Item author { get; set; }
        public Item assigned_to { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public string due_date { get; set; }
        public string done_ratio { get; set; }
        public string estimated_hours { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }
    }
}
