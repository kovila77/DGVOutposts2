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
        private DataGridViewComboBoxColumnOutpost _comboBoxColumnOutpost = new DataGridViewComboBoxColumnOutpost();
        private int currentTab = 0;

        private delegate void InitializeDGV();

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
            tabControl1.TabPages[0].Tag = new InitializeDGV(InitializeDGVOutposts);
            tabControl1.TabPages[1].Tag = new InitializeDGV(InitializeDGVMissions);

        }

        //private void OffColumnSortingDGV(DataGridView dgv) { foreach (DataGridViewColumn column in dgv.Columns) column.SortMode = DataGridViewColumnSortMode.NotSortable; }


        private void ReloadDGVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //switch (tabControl1.SelectedIndex)
            //{
            //    case 0:
            //        InitializeDGVOutposts();
            //        break;
            //    case 1:
            //        InitializeDGVMissions();
            //        break;
            //    default:
            //        break;
            //}
            ((InitializeDGV)tabControl1.TabPages[currentTab].Tag).Invoke();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //switch (currentTab)
            //{
            //    case 0:
            //        dgvOutposts.CancelEdit();
            //        dgvOutposts_RowValidating(dgvOutposts, new DataGridViewCellCancelEventArgs(dgvOutposts.CurrentCell.ColumnIndex, dgvOutposts.CurrentCell.RowIndex));
            //        break;
            //    case 1:
            //        dgvMissions.CancelEdit();
            //        dgvMissions_RowValidating(dgvMissions, new DataGridViewCellCancelEventArgs(dgvMissions.CurrentCell.ColumnIndex, dgvMissions.CurrentCell.RowIndex));
            //        break;
            //    default:
            //        break;
            //}
            currentTab = tabControl1.SelectedIndex;
        }

        //private bool IsEntireRowEmpty(DataGridViewRow row, int columnIndexNotBeLook)
        //{
        //    foreach (DataGridViewCell cell in row.Cells)
        //        if (cell.FormattedValue.ToString().RmvExtrSpaces() != "" && cell.OwningColumn.Index != columnIndexNotBeLook)
        //            return false;
        //    return true;
        //}

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; } catch { return; }
            if (dgv.Rows[e.RowIndex].IsNewRow || !dgv[e.ColumnIndex, e.RowIndex].IsInEditMode) return;

            var cell = dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = e.FormattedValue.ToString().RmvExtrSpaces();
            int t;

            if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
                && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32)
                && !int.TryParse(cellFormatedValue, out t))
            {
                if (cellFormatedValue == "" || MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    dgv.CancelEdit();
                else
                    e.Cancel = true;
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
