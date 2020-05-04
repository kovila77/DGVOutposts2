namespace DGVOutposts
{
    partial class formDGVOutposts
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpOutposts = new System.Windows.Forms.TabPage();
            this.dgvOutposts = new System.Windows.Forms.DataGridView();
            this.tpMissions = new System.Windows.Forms.TabPage();
            this.dgvMissions = new System.Windows.Forms.DataGridView();
            this.contextMenuStripNULL = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setNULLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезагрузитьДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new DGVOutposts.CalendarColumn();
            this.calendarColumn2 = new DGVOutposts.CalendarColumn();
            this.calendarColumn3 = new DGVOutposts.CalendarColumn();
            this.tabControl1.SuspendLayout();
            this.tpOutposts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutposts)).BeginInit();
            this.tpMissions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMissions)).BeginInit();
            this.contextMenuStripNULL.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpOutposts);
            this.tabControl1.Controls.Add(this.tpMissions);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 426);
            this.tabControl1.TabIndex = 1;
            // 
            // tpOutposts
            // 
            this.tpOutposts.Controls.Add(this.dgvOutposts);
            this.tpOutposts.Location = new System.Drawing.Point(4, 22);
            this.tpOutposts.Name = "tpOutposts";
            this.tpOutposts.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutposts.Size = new System.Drawing.Size(792, 400);
            this.tpOutposts.TabIndex = 0;
            this.tpOutposts.Text = "Форпосты";
            this.tpOutposts.UseVisualStyleBackColor = true;
            // 
            // dgvOutposts
            // 
            this.dgvOutposts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOutposts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.NullValue = "DBNull.Value";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOutposts.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOutposts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutposts.Location = new System.Drawing.Point(3, 3);
            this.dgvOutposts.Name = "dgvOutposts";
            this.dgvOutposts.Size = new System.Drawing.Size(786, 394);
            this.dgvOutposts.TabIndex = 0;
            this.dgvOutposts.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvOutposts_CellBeginEdit);
            this.dgvOutposts.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutposts_CellEndEdit);
            this.dgvOutposts.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutposts_CellLeave);
            this.dgvOutposts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutposts_CellValueChanged);
            this.dgvOutposts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvOutposts_DataError);
            this.dgvOutposts.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvOutposts_UserDeletingRow);
            this.dgvOutposts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvOutposts_KeyDown);
            this.dgvOutposts.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvOutposts_PreviewKeyDown);
            // 
            // tpMissions
            // 
            this.tpMissions.Controls.Add(this.dgvMissions);
            this.tpMissions.Location = new System.Drawing.Point(4, 22);
            this.tpMissions.Name = "tpMissions";
            this.tpMissions.Padding = new System.Windows.Forms.Padding(3);
            this.tpMissions.Size = new System.Drawing.Size(792, 400);
            this.tpMissions.TabIndex = 1;
            this.tpMissions.Text = "Миссии";
            this.tpMissions.UseVisualStyleBackColor = true;
            // 
            // dgvMissions
            // 
            this.dgvMissions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMissions.Location = new System.Drawing.Point(3, 3);
            this.dgvMissions.Name = "dgvMissions";
            this.dgvMissions.Size = new System.Drawing.Size(786, 394);
            this.dgvMissions.TabIndex = 0;
            this.dgvMissions.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMissions_CellMouseEnter);
            this.dgvMissions.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMissions_RowValidated);
            this.dgvMissions.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvMissions_RowValidating);
            this.dgvMissions.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvMissions_UserDeletingRow);
            // 
            // contextMenuStripNULL
            // 
            this.contextMenuStripNULL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setNULLToolStripMenuItem});
            this.contextMenuStripNULL.Name = "contextMenuStrip1";
            this.contextMenuStripNULL.Size = new System.Drawing.Size(123, 26);
            // 
            // setNULLToolStripMenuItem
            // 
            this.setNULLToolStripMenuItem.Name = "setNULLToolStripMenuItem";
            this.setNULLToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.setNULLToolStripMenuItem.Text = "Set NULL";
            this.setNULLToolStripMenuItem.Click += new System.EventHandler(this.setNULLToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.правкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.перезагрузитьДанныеToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // перезагрузитьДанныеToolStripMenuItem
            // 
            this.перезагрузитьДанныеToolStripMenuItem.Name = "перезагрузитьДанныеToolStripMenuItem";
            this.перезагрузитьДанныеToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.перезагрузитьДанныеToolStripMenuItem.Text = "Перезагрузить данные";
            this.перезагрузитьДанныеToolStripMenuItem.Click += new System.EventHandler(this.перезагрузитьДанныеToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Название";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 149;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Ценность";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 148;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "x";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 149;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "y";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 148;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "z";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 149;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "id";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Описание";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 186;
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.HeaderText = "Дата начала миссии";
            this.calendarColumn1.Name = "calendarColumn1";
            this.calendarColumn1.Width = 186;
            // 
            // calendarColumn2
            // 
            this.calendarColumn2.HeaderText = "Плановое окончание";
            this.calendarColumn2.Name = "calendarColumn2";
            this.calendarColumn2.Width = 185;
            // 
            // calendarColumn3
            // 
            this.calendarColumn3.HeaderText = "Действительное окончание";
            this.calendarColumn3.Name = "calendarColumn3";
            this.calendarColumn3.Width = 186;
            // 
            // formDGVOutposts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "formDGVOutposts";
            this.Text = "Миссии форпостов";
            this.tabControl1.ResumeLayout(false);
            this.tpOutposts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutposts)).EndInit();
            this.tpMissions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMissions)).EndInit();
            this.contextMenuStripNULL.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpOutposts;
        private System.Windows.Forms.TabPage tpMissions;
        private System.Windows.Forms.DataGridView dgvOutposts;
        private System.Windows.Forms.DataGridView dgvMissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private CalendarColumn calendarColumn1;
        private CalendarColumn calendarColumn2;
        private CalendarColumn calendarColumn3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNULL;
        private System.Windows.Forms.ToolStripMenuItem setNULLToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перезагрузитьДанныеToolStripMenuItem;
    }
}

