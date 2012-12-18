using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redmine.Model
{
    public class User
    {
        public string id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mail { get; set; }
        public string created_on { get; set; }
        public string last_login_on { get; set; }
        public string name { get { return string.Format("{0} {1}", lastname, firstname); } }
    }
}
