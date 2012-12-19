using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redmine
{
    using System.Net;
    using System.IO;
    using System.Collections;
    using Redmine.BLL;
    using Redmine.Model;
    using System.Collections.Specialized;
    public partial class Main : Form
    {

        private string control_file = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\redmine\\main_control.config";

        public RedmineService service { get; set; }

        public Hashtable control_hb = new Hashtable();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0}  -  id：{1}", service.user.name, service.user.id);

            service.load_config(control_hb, control_file);

            panel_context.Layout += panel_context_Layout;
            mst_time_Click(null, null);
        }

        void panel_context_Layout(object sender, LayoutEventArgs e)
        {
            if (e.AffectedControl is ComboBox)
            {
                var cb = (ComboBox)e.AffectedControl;
                cb.SelectedValueChanged += control_ValueChanged;
                if (control_hb.ContainsKey(cb.Name)) cb.SelectedValue = control_hb[cb.Name];
            }
            else if (e.AffectedControl is MaskedTextBox)
            {
                var tb = (MaskedTextBox)e.AffectedControl;
                tb.TextChanged += control_ValueChanged;
                if (control_hb.ContainsKey(tb.Name)) tb.Text = (string)control_hb[tb.Name];
            }
            
        }

        public

        void control_ValueChanged(object sender, EventArgs e)
        {

            string name = null;
            object value = null;
            if (sender is ComboBox)
            {
                var cb = (ComboBox)sender;
                name = cb.Name;
                value = cb.SelectedValue;
            }
            else if (sender is MaskedTextBox)
            {
                var tb = (MaskedTextBox)sender;
                name = tb.Name;
                value = tb.Text;
            }
            if (control_hb.ContainsKey(name)) control_hb[name] = value;
            else control_hb.Add(name, value);

        }

        void btn_ok_Click(object sender, EventArgs e)
        {
            ComboBox cb_project = (ComboBox)panel_context.Controls["cb_project"];
            DateTimePicker dtp_time = (DateTimePicker)panel_context.Controls["dtp_time"];
            ComboBox cb_activity = (ComboBox)panel_context.Controls["cb_activity"];
            MaskedTextBox mtb_time = (MaskedTextBox)panel_context.Controls["mtb_time"];
            TextBox tb_context = (TextBox)panel_context.Controls["tb_context"];
            TimeEntries timeEntries = new TimeEntries();
            timeEntries.project_id = cb_project.SelectedValue.ToString();
            timeEntries.spent_on = dtp_time.Text;
            timeEntries.activity_id = cb_activity.SelectedValue.ToString();
            timeEntries.hours = mtb_time.Text;
            if (".".Equals(timeEntries.hours))
            {
                MessageBox.Show("请填写工时！");
                return;
            }
            else if (timeEntries.hours.EndsWith("."))
            {
                timeEntries.hours = timeEntries.hours.Substring(0, timeEntries.hours.Length - 1);
            }
            timeEntries.comments = tb_context.Text;
            try
            {
                service.timeEntries(timeEntries);
                MessageBox.Show("打卡成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mst_time_Click(object sender, EventArgs e)
        {
            panel_clear();

            ComboBox cb_project = new ComboBox();
            cb_project.Width = 250;
            cb_project.Name = "cb_project";
            cb_project.DataSource = (from m in service.memberships orderby m.role_name select m).ToList();
            cb_project.DisplayMember = "role_project";
            cb_project.ValueMember = "project_id";
            cb_project.DropDownStyle = ComboBoxStyle.DropDownList;
            add_control("项目：",cb_project, false);


            DateTimePicker dtp_time = new DateTimePicker();
            dtp_time.CustomFormat = "yyyy-MM-dd";
            dtp_time.Format = DateTimePickerFormat.Custom;
            dtp_time.Width = 250;
            dtp_time.Name = "dtp_time";
            add_control("时间：", dtp_time, false);

            //-------------------------------------------------------

            ComboBox cb_activity = new ComboBox();
            cb_activity.Width = 250;
            cb_activity.Name = "cb_activity";
            cb_activity.DataSource = service.activity;
            cb_activity.DisplayMember = "name";
            cb_activity.ValueMember = "id";
            cb_activity.DropDownStyle = ComboBoxStyle.DropDownList;
            add_control("活动：", cb_activity, false);

            MaskedTextBox mtb_time = new MaskedTextBox();
            mtb_time.Mask = "9.9";
            mtb_time.Width = 250;
            mtb_time.Name = "mtb_time";
            panel_context.Controls.Add(mtb_time);
            add_control("工时：", mtb_time, false);
            //-------------------------------------------------------

            TextBox tb_context = new TextBox();
            tb_context.Name = "tb_context";
            tb_context.ScrollBars = ScrollBars.Vertical;
            tb_context.Multiline = true;
            tb_context.Height = 230;
            tb_context.Width = 570;
            add_control("描述：", tb_context, true);
            //-------------------------------------------------------

            Button btn_ok = new Button();
            btn_ok.Text = "登记";
            btn_ok.Left = 615 - btn_ok.Width;
            btn_ok.Click += btn_ok_Click;
            add_control(btn_ok, false);
        }

        private void mst_issues_Click(object sender, EventArgs e)
        {

            panel_clear();

            ComboBox cb_project = new ComboBox();
            cb_project.Width = 250;
            cb_project.Name = "cb_project";
            IList<Membership> memberships = (from m in service.memberships orderby m.role_name select m).ToList();
            Membership all_membership = new Membership();
            all_membership.project = new Item { name = "全部项目"};
            all_membership.role = new Item { name = "ALL" };
            memberships.Insert(0, all_membership);
            cb_project.DataSource = memberships;
            cb_project.DisplayMember = "role_project";
            cb_project.ValueMember = "project_id";
            cb_project.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_project.SelectedValueChanged += load_issues;
            add_control("项目：", cb_project, false);

            ComboBox cb_type = new ComboBox();
            cb_type.Width = 250;
            cb_type.Name = "cb_type";
            cb_type.DataSource = new List<Item> { new Item { name = "指派给我", id = "1" },
            new Item { name = "我指派的", id = "2" },new Item { name = "全部", id = "3" }};
            cb_type.DisplayMember = "name";
            cb_type.ValueMember = "id";
            cb_type.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_type.SelectedValueChanged += load_issues;
            add_control("类型：", cb_type, false);

            //-------------------------------------------------------

            ComboBox cb_issues = new ComboBox();
            cb_issues.TabIndex = 0;
            cb_issues.Width = 571;
            cb_issues.Name = "cb_issues";
            cb_issues.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_issues.SelectedValueChanged += cb_issues_SelectedValueChanged;
            add_control("任务：", cb_issues, true);

            //-------------------------------------------------------
            ComboBox cb_status = new ComboBox();
            cb_status.Width = 250;
            cb_status.Name = "cb_status";
            cb_status.DataSource = new List<Item> { new Item { name = "全部"},
            new Item { name = "待分派", id = "1" },new Item { name = "已分派", id = "7" },
            new Item { name = "已接手", id = "8" },new Item { name = "已拒绝", id = "6" },
            new Item { name = "进行中", id = "2" },new Item { name = "已解决", id = "3" },
            new Item { name = "已关闭", id = "5" }};
            cb_status.DisplayMember = "name";
            cb_status.ValueMember = "id";
            cb_status.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_status.SelectedValueChanged += load_issues;
            add_control("状态：", cb_status, false);

            /**ComboBox cb_tracker = new ComboBox();
            cb_tracker.Width = 250;
            cb_tracker.Name = "cb_tracker";
            cb_tracker.DataSource = new List<Item> { new Item { name = "全部"},
            new Item { name = "错误", id = "1" },new Item { name = "功能", id = "2" },
            new Item { name = "支持", id = "3" },new Item { name = "维护", id = "5" }};
            cb_tracker.DisplayMember = "name";
            cb_tracker.ValueMember = "id";
            cb_tracker.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_tracker.SelectedValueChanged += load_issues;
            add_control("跟踪：", cb_tracker, false);*/

            add_item("项目：", "tb_project", true, true);
            ((TextBox)panel_context.Controls["tb_project"]).Width = 248;


            //---------------------------------------------------------------
            add_item("I  D：", "tb_id", true, false);
            add_item("作者：", "tb_author", true, false);
            add_item("指派：", "tb_assigned_to", true, true);
            //---------------------------------------------------------------
            add_item("跟踪：", "tb_tracker", true, false);
            add_item("状态：", "tb_status", true, false);
            add_item("优先：", "tb_priority", true, false);
            add_item("开始：", "tb_start_date", true, true);
            //---------------------------------------------------------------
            add_item("创建：", "tb_created_on", true, false);
            ((TextBox)panel_context.Controls["tb_created_on"]).Width = 248;
            add_item("更新：", "tb_updated_on", true, true);
            ((TextBox)panel_context.Controls["tb_updated_on"]).Width = 248;
            //---------------------------------------------------------------

            TextBox tb_description = new TextBox();
            tb_description.Name = "tb_description";
            tb_description.ScrollBars = ScrollBars.Vertical;
            tb_description.Multiline = true;
            tb_description.ReadOnly = true;
            tb_description.Height = 88;
            tb_description.Text = "...";
            tb_description.Width = 570;
            add_control("描述：", tb_description, true);
            //---------------------------------------------------------------
            Button btn_new = new Button();
            btn_new.Name = "btn_new";
            btn_new.Text = "新建问题";
            btn_new.Click += btn_new_Click;
            add_control("",btn_new, false);

            Button btn_solve = new Button();
            btn_solve.Name = "btn_solve";
            btn_solve.Text = "标记为解决";
            btn_solve.Click += btn_solve_Click;
            add_control(btn_solve, false);

            Button btn_close = new Button();
            btn_close.Name = "btn_close";
            btn_close.Text = "标记为关闭";
            btn_close.Click += btn_close_Click;
            add_control(btn_close, false);

            load_issues(sender, e);
        }

        void btn_new_Click(object sender, EventArgs e)
        {
            ComboBox cb_project = (ComboBox)panel_context.Controls["cb_project"];
            System.Diagnostics.Process.Start(string.Format("{0}/projects/{1}/issues/new", service.location, cb_project.SelectedValue.ToString()));
        }

        void btn_close_Click(object sender, EventArgs e)
        {
            ComboBox cb_issues = (ComboBox)panel_context.Controls["cb_issues"];
            if (cb_issues.SelectedValue != null)
            {
                Issues issues = (Issues)issues_tb[cb_issues.SelectedValue.ToString()];
                service.update_issues_status(issues, "5");
                load_issues(sender, e);
            }
            else
            {
                MessageBox.Show("请选择一个任务！");
            }
        }

        void btn_solve_Click(object sender, EventArgs e)
        {
            ComboBox cb_issues = (ComboBox)panel_context.Controls["cb_issues"];
            if (cb_issues.SelectedValue != null)
            {
                Issues issues = (Issues)issues_tb[cb_issues.SelectedValue.ToString()];
                service.update_issues_status(issues, "3");
                load_issues(sender, e);
            }
            else
            {
                MessageBox.Show("请选择一个任务！");
            }
        }
        public void add_item(string label, string name, bool ReadOnly,bool line)
        {
            TextBox tb = new TextBox();
            tb.Name = name;
            tb.ReadOnly = ReadOnly;
            tb.Width = 89;
            add_control(label, tb, false);
            if (line) idx = 4; else idx = 2;
        }
        public void add_control(string label, Control control, bool double_control)
        {
            Label lb = new Label();
            lb.Text = label;
            lb.Width = 45;
            add_control(lb, false);
            add_control(control, double_control);
        }
        public void panel_clear()
        {
            idx = 0;
            prev_control = null;
            panel_context.Controls.Clear();
        }
        private int idx = 0;
        private Control prev_control;
        public void add_control(Control control, bool double_control)
        {
            if (double_control) idx += 2;
            
            if (++idx > 1 && idx % 5 == 0)
            {
                idx = 1;
                control.Top = prev_control.Top + prev_control.Height + 20;
            }
            else if (prev_control != null)
            {
                control.Top = prev_control.Top + (prev_control is Label ? -5 : prev_control is Button ?  0 : 5);
                control.Left = prev_control.Left + prev_control.Width + (idx > 1 && idx % 3 == 0 ? 26 : 0);
            }
            else
            {
                control.Top = 10;
            }
            prev_control = control;
            panel_context.Controls.Add(control);
        }

        void cb_issues_SelectedValueChanged(object sender, EventArgs e)
        {
            if (panel_context.Controls.ContainsKey("btn_close"))
            {
                TextBox tb_description = (TextBox)panel_context.Controls["tb_description"];
                ComboBox cb_issues = (ComboBox)panel_context.Controls["cb_issues"];
                ((Button)panel_context.Controls["btn_solve"]).Enabled = false;
                ((Button)panel_context.Controls["btn_close"]).Enabled = false;
                if (cb_issues.SelectedValue != null && issues_tb.ContainsKey(cb_issues.SelectedValue.ToString()))
                {
                    Issues issues = (Issues)issues_tb[cb_issues.SelectedValue.ToString()];
                    ((TextBox)panel_context.Controls["tb_id"]).Text = issues.id;
                    ((TextBox)panel_context.Controls["tb_project"]).Text = issues.project.name;
                    ((TextBox)panel_context.Controls["tb_author"]).Text = issues.author.name;
                    ((TextBox)panel_context.Controls["tb_assigned_to"]).Text = issues.assigned_to.name;

                    ((TextBox)panel_context.Controls["tb_tracker"]).Text = issues.tracker.name;
                    ((TextBox)panel_context.Controls["tb_status"]).Text = issues.status.name;
                    ((TextBox)panel_context.Controls["tb_start_date"]).Text = issues.start_date;
                    ((TextBox)panel_context.Controls["tb_priority"]).Text = issues.priority.name;

                    ((TextBox)panel_context.Controls["tb_created_on"]).Text = issues.created_on.Substring(0, 19).Replace('T', ' ');
                    ((TextBox)panel_context.Controls["tb_updated_on"]).Text = issues.updated_on.Substring(0, 19).Replace('T', ' ');

                    tb_description.Text = issues.description;

                    if ((issues.status.id == "7" || issues.status.id == "8" || issues.status.id == "2") && issues.assigned_to.id == service.user.id)
                    {
                        ((Button)panel_context.Controls["btn_solve"]).Enabled = true;
                    }
                    if ((issues.status.id == "6" || issues.status.id == "3") && issues.author.id == service.user.id)
                    {
                        ((Button)panel_context.Controls["btn_close"]).Enabled = true;
                    }
                }
                else
                {

                    ((TextBox)panel_context.Controls["tb_id"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_project"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_author"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_assigned_to"]).Text = "";

                    ((TextBox)panel_context.Controls["tb_tracker"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_status"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_start_date"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_priority"]).Text = "";

                    ((TextBox)panel_context.Controls["tb_created_on"]).Text = "";
                    ((TextBox)panel_context.Controls["tb_updated_on"]).Text = "";

                    tb_description.Text = "";
                }
            }
        }
        private Hashtable issues_tb = new Hashtable();
        private void load_issues(object sender, EventArgs e)
        {
            if (panel_context.Controls.ContainsKey("btn_close"))
            {
                ComboBox cb_status = (ComboBox)panel_context.Controls["cb_status"];
                ComboBox cb_project = (ComboBox)panel_context.Controls["cb_project"];
                ComboBox cb_issues = (ComboBox)panel_context.Controls["cb_issues"];
                ComboBox cb_type = (ComboBox)panel_context.Controls["cb_type"];
                NameValueCollection query_params = new NameValueCollection { };
                if (cb_status.SelectedValue != null)
                {
                    query_params.Add("status_id", cb_status.SelectedValue.ToString());
                }
                if (cb_project.SelectedValue != null)
                {
                    query_params.Add("project_id", cb_project.SelectedValue.ToString());
                    ((Button)panel_context.Controls["btn_new"]).Enabled = true;
                }
                else
                {
                    ((Button)panel_context.Controls["btn_new"]).Enabled = false;
                }
                if ("1".Equals(cb_type.SelectedValue.ToString()))
                {
                    query_params.Add("assigned_to_id", service.user.id);
                }
                else if ("2".Equals(cb_type.SelectedValue.ToString()))
                {
                    query_params.Add("author_id", service.user.id);
                }
                IList<Issues> issues = service.load_issues(query_params);
                cb_issues.DataSource = issues;
                cb_issues.DisplayMember = "subject";
                cb_issues.ValueMember = "id";
                issues_tb.Clear();
                foreach (var item in issues)
                {
                    issues_tb.Add(item.id, item);
                }
                cb_issues_SelectedValueChanged(sender, e);
            }
        }

        private void mst_key_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login(true).ShowDialog();
            this.Close();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            service.save_config(control_hb, control_file);
        }

    }
}
