namespace Redmine
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mst = new System.Windows.Forms.MenuStrip();
            this.mst_time = new System.Windows.Forms.ToolStripMenuItem();
            this.mst_issues = new System.Windows.Forms.ToolStripMenuItem();
            this.mst_key = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_context = new System.Windows.Forms.Panel();
            this.mst.SuspendLayout();
            this.SuspendLayout();
            // 
            // mst
            // 
            this.mst.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mst_time,
            this.mst_issues,
            this.mst_key});
            this.mst.Location = new System.Drawing.Point(0, 0);
            this.mst.Name = "mst";
            this.mst.Size = new System.Drawing.Size(642, 25);
            this.mst.TabIndex = 0;
            this.mst.Text = "menuStrip1";
            // 
            // mst_time
            // 
            this.mst_time.Name = "mst_time";
            this.mst_time.Size = new System.Drawing.Size(68, 21);
            this.mst_time.Text = "登记工时";
            this.mst_time.Click += new System.EventHandler(this.mst_time_Click);
            // 
            // mst_issues
            // 
            this.mst_issues.Name = "mst_issues";
            this.mst_issues.Size = new System.Drawing.Size(68, 21);
            this.mst_issues.Text = "我的任务";
            this.mst_issues.Click += new System.EventHandler(this.mst_issues_Click);
            // 
            // mst_key
            // 
            this.mst_key.Name = "mst_key";
            this.mst_key.Size = new System.Drawing.Size(66, 21);
            this.mst_key.Text = "更换KEY";
            this.mst_key.Click += new System.EventHandler(this.mst_key_Click);
            // 
            // panel_context
            // 
            this.panel_context.Location = new System.Drawing.Point(12, 28);
            this.panel_context.Name = "panel_context";
            this.panel_context.Size = new System.Drawing.Size(618, 353);
            this.panel_context.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(642, 393);
            this.Controls.Add(this.panel_context);
            this.Controls.Add(this.mst);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mst;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.mst.ResumeLayout(false);
            this.mst.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mst;
        private System.Windows.Forms.ToolStripMenuItem mst_time;
        private System.Windows.Forms.Panel panel_context;
        private System.Windows.Forms.ToolStripMenuItem mst_issues;
        private System.Windows.Forms.ToolStripMenuItem mst_key;




    }
}

