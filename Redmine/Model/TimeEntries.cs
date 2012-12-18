using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redmine.Model
{
    public class TimeEntries
    {
        public string issue_id { get; set; }
        public string project_id { get; set; }
        public string spent_on { get; set; }
        public string hours { get; set; }
        public string activity_id { get; set; }
        public string comments { get; set; }
    }
}
