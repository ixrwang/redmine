using Redmine.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Redmine
{
    public partial class Login : Form
    {
        private string key_file = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\redmine\\key.config";

        private bool update_key = false;

        public Login(bool update_key) 
        {
            this.update_key = update_key;
            InitializeComponent();
        }

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            FileInfo info = new FileInfo(key_file);
            if (info.Exists)
            {
                tb_key.Text = File.ReadAllText(key_file, System.Text.Encoding.UTF8);

                if (!update_key && tb_key.Text != null && !"".Equals(tb_key.Text.Trim()))
                {
                    bt_login_Click(sender, e);
                }
            }
        }

        private void bt_login_Click(object sender, EventArgs e)
        {
            try
            {
                bt_login.Enabled = false;
                Main main = new Main();
                main.service = new RedmineService(tb_key.Text);
                this.Hide();
                FileInfo info = new FileInfo(key_file);
                if (!info.Directory.Exists)
                {
                    info.Directory.Create();
                }
                File.WriteAllText(key_file, tb_key.Text, System.Text.Encoding.UTF8);
                main.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("KEY错误，登录失败！" + ex.Message);
            }
            bt_login.Enabled = true;
        }
    }
}
