using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DGVOutposts
{

    public partial class formDGVOutposts : Form
    {
        //private DataTable dtOutposts;
        //private DataTable _dtOutposts = null;


        //private bool rowCanBeCommitedDGVOutpost;

        //DataGridViewRow RowForDelete = null;
        private object oldCellValue;
        private DataGridViewComboBoxColumnOutpost _comboBoxColumnOutpost = new DataGridViewComboBoxColumnOutpost();

        private readonly string sConnStr = new NpgsqlConnectionStringBuilder
        {
            Host = Database.Default.Host,
            Port = Database.Default.Port,
            Database = Database.Default.Name,
            Username = Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME"),
            Password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD"),
        }.ConnectionString;

        public formDGVOutposts()
        {
            InitializeComponent();
            InitializeDGVOutposts();
            InitializeDGVMissions();
            //OffColumnSortingDGV(dgvOutposts);
            //OffColumnSortingDGV(dgvMissions);
            tabControl1.TabPages[0].Tag = dgvOutposts;
            tabControl1.TabPages[1].Tag = dgvMissions;
        }

        //private void OffColumnSortingDGV(DataGridView dgv) { foreach (DataGridViewColumn column in dgv.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable; }


        private void ReloadDGVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    InitializeDGVOutposts();
                    break;
                case 1:
                    InitializeDGVMissions();
                    break;
                default:
                    break;
            }
        }


        private void dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; }
            catch { return; }

            oldCellValue = dgv[e.ColumnIndex, e.RowIndex].Value;
        }

        private void dgv_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; }
            catch { return; }

            var cell = dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = e.Value.ToString().RmvExtrSpaces();
            int t;

            if (cellFormatedValue == "")
            {
                dgv.CancelEdit();
                //e.ParsingApplied = false;
                MessageBox.Show(MyHelper.strEmptyCell);
                return;
            }
            else if (dgvMissions.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
                && dgvMissions.Columns[e.ColumnIndex].ValueType == typeof(Int32)
                && !int.TryParse(cellFormatedValue, out t))
            {
                dgv.CancelEdit();
                MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{e.Value}\"");
                //e.ParsingApplied = false;
                return;
            }
            else
            {
                cell.ErrorText = "";
            }
        }

        private void dgv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ReloadDGVToolStripMenuItem_Click(null, null);
            }
        }
    }
}
