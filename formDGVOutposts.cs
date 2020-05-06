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
        //private bool needUniqueCheckAndCommitOutposts;
        private object oldCellValue = false;
        private DataGridViewComboBoxColumnOutpost _comboBoxColumnOutpost = new DataGridViewComboBoxColumnOutpost();

        //private delegate void RowCheck(DataGridViewCell cell, ref DataGridViewCellValidatingEventArgs e);

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
            //dgvOutposts.Tag = new RowCheck(dgvOutposts_RowEmtpyCellChecking);
            //dgvMissions.Tag = new RowCheck(dgvMission_RowEmtpyCellChecking);
            //dgvOutposts.Tag = new RowCheck(dgvOutposts_RowChecking);
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
            //if (sender == null) return;
            //DataGridView dgv = null;
            //try { dgv = (DataGridView)sender; }
            //catch { return; }

            //oldCellValue = dgv[e.ColumnIndex, e.RowIndex].Value;
        }

        private bool IsEntireRowEmpty(DataGridViewRow row, int columnIndexNotBeLook)
        {
            foreach (DataGridViewCell cell in row.Cells)
                if (cell.FormattedValue.ToString().RmvExtrSpaces() != "" && cell.OwningColumn.Index != columnIndexNotBeLook)
                    return false;
            return true;
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; }
            catch { return; }
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

        //private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        //{
        //    //var row = dgvOutposts.Rows[e.RowIndex];
        //    if (sender == null) return;
        //    DataGridView dgv = null;
        //    try { dgv = (DataGridView)sender; }
        //    catch { return; }
        //    if (dgv.Rows[e.RowIndex].IsNewRow || !dgv[e.ColumnIndex, e.RowIndex].IsInEditMode) return;


        //    var cell = dgv[e.ColumnIndex, e.RowIndex];
        //    var cellFormatedValue = e.FormattedValue.ToString().RmvExtrSpaces();
        //    int t;

        //    if (cellFormatedValue == "")
        //    {
        //        dgv.CancelEdit();
        //        return;
        //    }
        //    else if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
        //        && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32)
        //        && !int.TryParse(cellFormatedValue, out t))
        //    {
        //        if (MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            dgv.CancelEdit();
        //        }
        //        else e.Cancel = true;
        //        return;
        //    }
        //    else
        //    {
        //        cell.ErrorText = "";
        //        ((RowCheck)dgv.Tag).Invoke(cell, ref e);
        //    }
        //}

        private void dgv_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            //if (sender == null) return;
            //DataGridView dgv = null;
            //try { dgv = (DataGridView)sender; }
            //catch { return; }
            //var cell = dgv[e.ColumnIndex, e.RowIndex];
            //if (cell.ValueType == typeof(String)){
            //    e.Value = e.Value.ToString().RmvExtrSpaces();
            //}

            //var cell = dgv[e.ColumnIndex, e.RowIndex];
            //var cellFormatedValue = e.Value.ToString().RmvExtrSpaces();
            //int t;

            //if (cellFormatedValue == "")
            //{
            //    //dgv.CancelEdit();
            //    //e.Value = oldCellValue.ToString() ;
            //    //e.Value = oldCellValue;
            //    MessageBox.Show(MyHelper.strEmptyCell);
            //    e.ParsingApplied = false;
            //    //dgv.RefreshEdit();
            //    return;
            //}
            //else if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
            //    && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32)
            //    && !int.TryParse(cellFormatedValue, out t))
            //{
            //    //dgv.CancelEdit();
            //    //e.Value = null;
            //    MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{e.Value}\"");
            //    e.ParsingApplied = false;
            //    //dgv.RefreshEdit();
            //    return;
            //}
            //else
            //{
            //    cell.ErrorText = "";
            //    e.ParsingApplied = true;
            //}
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
