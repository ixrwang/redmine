using Redmine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Redmine.BLL
{
    public class RedmineService
    {
        private static string domain = "http://redmine.palmyou.com/redmine";

        public string location { get { return domain; } }

        public string key { get; private set; }
        public User user { get; private set; }
        public IList<Membership> memberships = new List<Membership>();
        public IList<Item> activity = new List<Item>();

        public void load_config(Hashtable hb, string file)
        {
            FileInfo info = new FileInfo(file);
            if (info.Exists)
            {
                string[] lines = File.ReadAllLines(file, System.Text.Encoding.UTF8);
                foreach (var line in lines)
                {
                    string[] key_value = line.Split('=');
                    hb.Add(key_value[0], key_value[1]);
                }
            }
            
        }

        public void save_config(Hashtable hb, string file)
        {
            FileInfo info = new FileInfo(file);
            if (!info.Directory.Exists)
            {
                info.Directory.Create();
            }
            List<string> key_value = new List<string>();
            foreach (var key in hb.Keys)
            {
                if (hb[key] != null)
                {
                    key_value.Add(string.Format("{0}={1}", key, hb[key]));
                }
            }
            File.WriteAllLines(file, key_value.ToArray(), System.Text.Encoding.UTF8);
        }

        public RedmineService(string key)
        {
            this.key = key;
            init();
            activity.Add(new Item { name = "开发", id = "9" });
            activity.Add(new Item { name = "设计", id = "8" });
            activity.Add(new Item { name = "测试", id = "12" });
            activity.Add(new Item { name = "整理", id = "10" });
            activity.Add(new Item { name = "支持", id = "16" });
            activity.Add(new Item { name = "调研", id = "11" });
            activity.Add(new Item { name = "维护", id = "13" });
            activity.Add(new Item { name = "部署", id = "15" });
            activity.Add(new Item { name = "自学", id = "17" });
            activity.Add(new Item { name = "会议", id = "18" });
            activity.Add(new Item { name = "日常", id = "19" });
        }

        public void init()
        {
            WebClient client = newWebClient();
            client.QueryString.Add("include", "memberships");
            Stream stream = client.OpenRead(string.Format("{0}/users/current.xml", domain));
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            XmlNode node = xml.SelectSingleNode("user");
            user = new User();
            user.id = node.SelectSingleNode("id").InnerText;
            user.firstname = node.SelectSingleNode("firstname").InnerText;
            user.lastname = node.SelectSingleNode("lastname").InnerText;
            user.mail = node.SelectSingleNode("mail").InnerText;
            user.created_on = node.SelectSingleNode("created_on").InnerText;
            user.last_login_on = node.SelectSingleNode("last_login_on").InnerText;
            XmlNodeList nodeList = node.SelectNodes("memberships/membership");
            foreach (XmlNode m_node in nodeList)
            {
                Membership membership = new Membership();
                membership.id = m_node.SelectSingleNode("id").InnerText;
                XmlNode project = m_node.SelectSingleNode("project");
                membership.project = new Item { id = project.Attributes["id"].Value, name = project.Attributes["name"].Value };
                XmlNode role = m_node.SelectSingleNode("roles").SelectSingleNode("role");
                membership.role = new Item { id = role.Attributes["id"].Value, name = role.Attributes["name"].Value };
                memberships.Add(membership);
            }
        }

        public void update_issues_status(Issues issues, string status_id)
        {
            WebClient client = newWebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            NameValueCollection param = new NameValueCollection();
            param.Add("issue[status_id]", status_id);
            client.UploadValues(string.Format("{0}/issues/{1}.xml", domain,issues.id), "PUT", param);
        }

        public void timeEntries(TimeEntries timeEntries)
        {
            WebClient client = newWebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            NameValueCollection param = new NameValueCollection();
            param.Add("time_entry[issue_id]", timeEntries.issue_id);
            param.Add("time_entry[project_id]", timeEntries.project_id);
            param.Add("time_entry[spent_on]", timeEntries.spent_on);
            param.Add("time_entry[hours]", timeEntries.hours);
            param.Add("time_entry[activity_id]", timeEntries.activity_id);
            param.Add("time_entry[comments]", timeEntries.comments);
            client.UploadValues(string.Format("{0}/time_entries.xml", domain), param);
        }

        private WebClient newWebClient()
        {
            WebClient client = new WebClient();
            client.QueryString.Add("key", key);
            return client;
        }

        public IList<Issues> load_issues(NameValueCollection query_params)
        {
            IList<Issues> issues_list = new List<Issues>();
            WebClient client = newWebClient();
            client.QueryString.Add(query_params);
            Stream stream = client.OpenRead(string.Format("{0}/issues.xml", domain));
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            XmlNodeList node_list = xml.SelectNodes("issues/issue");

            foreach (XmlNode node in node_list)
            {
                Issues issues = new Issues();
                issues.id = node.SelectSingleNode("id").InnerText;
                XmlNode project = node.SelectSingleNode("project");
                issues.project = new Item { id = project.Attributes["id"].Value, name = project.Attributes["name"].Value };
                XmlNode tracker = node.SelectSingleNode("tracker");
                issues.tracker = new Item { id = tracker.Attributes["id"].Value, name = tracker.Attributes["name"].Value };
                XmlNode status = node.SelectSingleNode("status");
                issues.status = new Item { id = status.Attributes["id"].Value, name = status.Attributes["name"].Value };
                XmlNode priority = node.SelectSingleNode("priority");
                issues.priority = new Item { id = priority.Attributes["id"].Value, name = priority.Attributes["name"].Value };
                XmlNode author = node.SelectSingleNode("author");
                issues.author = new Item { id = author.Attributes["id"].Value, name = author.Attributes["name"].Value };
                XmlNode assigned_to = node.SelectSingleNode("assigned_to");
                if (assigned_to != null)
                issues.assigned_to = new Item { id = assigned_to.Attributes["id"].Value, name = assigned_to.Attributes["name"].Value };
                issues.subject = node.SelectSingleNode("subject").InnerText;
                issues.description = node.SelectSingleNode("description").InnerText;
                issues.start_date = node.SelectSingleNode("start_date").InnerText;
                issues.due_date = node.SelectSingleNode("due_date").InnerText;
                issues.done_ratio = node.SelectSingleNode("done_ratio").InnerText;
                issues.estimated_hours = node.SelectSingleNode("estimated_hours").InnerText;
                issues.created_on = node.SelectSingleNode("created_on").InnerText;
                issues.updated_on = node.SelectSingleNode("updated_on").InnerText;
                issues_list.Add(issues);
            }
            return issues_list;
        }
    }
}
