namespace Redmine
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_key = new System.Windows.Forms.TextBox();
            this.lb_key = new System.Windows.Forms.Label();
            this.bt_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_key
            // 
            this.tb_key.Location = new System.Drawing.Point(53, 12);
            this.tb_key.Multiline = true;
            this.tb_key.Name = "tb_key";
            this.tb_key.Size = new System.Drawing.Size(255, 21);
            this.tb_key.TabIndex = 0;
            // 
            // lb_key
            // 
            this.lb_key.AutoSize = true;
            this.lb_key.Location = new System.Drawing.Point(12, 15);
            this.lb_key.Name = "lb_key";
            this.lb_key.Size = new System.Drawing.Size(35, 12);
            this.lb_key.TabIndex = 1;
            this.lb_key.Text = "KEY：";
            // 
            // bt_login
            // 
            this.bt_login.Location = new System.Drawing.Point(233, 39);
            this.bt_login.Name = "bt_login";
            this.bt_login.Size = new System.Drawing.Size(75, 23);
            this.bt_login.TabIndex = 2;
            this.bt_login.Text = "登录";
            this.bt_login.UseVisualStyleBackColor = true;
            this.bt_login.Click += new System.EventHandler(this.bt_login_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(320, 70);
            this.Controls.Add(this.bt_login);
            this.Controls.Add(this.lb_key);
            this.Controls.Add(this.tb_key);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_key;
        private System.Windows.Forms.Label lb_key;
        private System.Windows.Forms.Button bt_login;
    }
}