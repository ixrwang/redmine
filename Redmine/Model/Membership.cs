using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redmine.Model
{
    public class Membership
    {
        public string id { get; set; }
        public Item project { get; set; }
        public Item role { get; set; }
        public string project_id { get { return project.id;} }
        public string project_name { get { return project.name;} }
        public string project_role { get { return string.Format("{0}  - {1}", project_name, role_name); } }
        public string role_id { get { return role.id; } }
        public string role_name { get { return role.name; } }
        public string role_project { get { return string.Format("{0}  - {1}", role_name, project_name); } }
    }
}
