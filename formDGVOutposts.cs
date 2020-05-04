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
        //private DataGridViewCellEventArgs mouseLocation;
        //private DataTable _dtOutposts = null;
        private readonly string strEmptyCell = "Вы ничего не ввели!";
        private readonly string strOnlyIntCell = "Вы ввели некорректное число!";
        private readonly string strUncorrectValueCell = "Некорректное значение!";
        private readonly string strBadRow = "Плохая строка!";
        private readonly string strExistingOutpostRow = "Форпост уже существует!";
        private readonly string strOutpostId = "outpost_id";
        private readonly string strOutpostName = "outpost_name";
        private readonly string strOutpostEconomicValue = "outpost_economic_value";
        private readonly string strOutpostCoordinateX = "outpost_coordinate_x";
        private readonly string strOutpostCoordinateY = "outpost_coordinate_y";
        private readonly string strOutpostCoordinateZ = "outpost_coordinate_z";
        private bool rowCanBeCommitedDGVOutpost;

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

        private void InitializeDGVMissions()
        {
            dgvMissions.Rows.Clear();
            dgvMissions.Columns.Clear();

            dgvMissions.Columns.Add("mission_description", "Описание миссии");
            dgvMissions.Columns.Add("mission_id", "id");
            dgvMissions.Columns.Add(_comboBoxColumnOutpost);
            dgvMissions.Columns.Add(new CalendarColumn { Name = "date_begin", HeaderText = "Дата начала" });
            dgvMissions.Columns.Add(new CalendarColumn { Name = "date_plan_end", HeaderText = "Планируемое завершение" });
            dgvMissions.Columns.Add(new CalendarColumn { Name = "date_actual_end", HeaderText = "Реальное завершение", ContextMenuStrip = contextMenuStripNULL });

            dgvMissions.Columns["mission_id"].Visible = false;
            dgvMissions.Columns["outpost_id"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns["date_begin"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns["date_plan_end"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns["date_actual_end"].SortMode = DataGridViewColumnSortMode.Automatic;

            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT mission_id,
                                           outpost_id,
                                           mission_description,
                                           date_begin,
                                           date_plan_end,
                                           date_actual_end
                                    FROM outpost_missions order by outpost_id; "
                };
                var reader = sCommand.ExecuteReader();
                while (reader.Read())
                {
                    dgvMissions.Rows.Add((string)reader["mission_description"],
                                         (int)reader["mission_id"],
                                         (int)reader["outpost_id"],
                                         reader["date_begin"],
                                         reader["date_plan_end"],
                                         reader["date_actual_end"]);
                }
            }
        }


        private void dgvMissions_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (!dgvMissions.IsCurrentRowDirty)
            //{
            //    return;
            //}
            //var row = dgvMissions.Rows[e.RowIndex];
            //row.Tag = false;
            //bool willCommit = true;
            ////row.ErrorText = "error";
            //var cellsWithPotentialErrors = new[] { row.Cells["outpost_id"], row.Cells["description"], row.Cells["date_begin"], row.Cells["date_plan_end"] };
            //foreach (var cell in cellsWithPotentialErrors)
            //{
            //    if (cell.Value is DBNull
            //            || string.IsNullOrWhiteSpace((string)cell.FormattedValue))
            //    {
            //        if (cell.OwningColumn.Name == "outpost_id" && cell.Value is DBNull)
            //        {
            //            cell.ErrorText = "Данный форпост не сохранён в базе данных!";
            //        }
            //        else
            //        {
            //            cell.ErrorText = "Пустое значение!";
            //        }
            //        willCommit = false;
            //        row.ErrorText = $"Проверьте данные";
            //    }
            //    else
            //    {
            //        cell.ErrorText = "";
            //    }
            //}
            //if (willCommit)
            //{
            //    row.ErrorText = ""; row.Tag = willCommit;
            //    return;
            //}
        }
         
        private void dgvMissions_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            //if (!(dgvMissions.Rows[e.RowIndex].Tag is bool && (bool)dgvMissions.Rows[e.RowIndex].Tag) || dgvMissions.CurrentRow.ErrorText != "")
            //{
            //    return;
            //}
            //var row = dgvMissions.Rows[e.RowIndex];

            //using (var sConn = new NpgsqlConnection(sConnStr))
            //{
            //    sConn.Open();
            //    var sCommand = new NpgsqlCommand
            //    {
            //        Connection = sConn
            //    };
            //    sCommand.Parameters.AddWithValue("description", row.Cells["description"].Value);
            //    sCommand.Parameters.AddWithValue("outpost_id", row.Cells["outpost_id"].Value);
            //    sCommand.Parameters.AddWithValue("date_begin", row.Cells["date_begin"].Value);
            //    sCommand.Parameters.AddWithValue("date_plan_end", row.Cells["date_plan_end"].Value);
            //    sCommand.Parameters.AddWithValue("date_actual_end", row.Cells["date_actual_end"].Value is null ? DBNull.Value : row.Cells["date_actual_end"].Value);

            //    if (row.Cells["id"].Value is int)
            //    {
            //        sCommand.CommandText = @"
            //            UPDATE outpost_missions
            //            SET outpost_id          = @outpost_id,
            //                mission_description = @description,
            //                date_begin          = @date_begin,
            //                date_plan_end       = @date_plan_end,
            //                date_actual_end     = @date_actual_end
            //            WHERE mission_id        = @id;";
            //        sCommand.Parameters.AddWithValue("id", (int)row.Cells["id"].Value);
            //        var res = sCommand.ExecuteScalar();
            //    }
            //    else
            //    {
            //        sCommand.CommandText = @"
            //            INSERT INTO outpost_missions (outpost_id,
            //                                          mission_description,
            //                                          date_begin,
            //                                          date_plan_end,
            //                                          date_actual_end)
            //            VALUES (@outpost_id,
            //                    @description,
            //                    @date_begin,
            //                    @date_plan_end,
            //                    @date_actual_end)
            //            RETURNING mission_id;";
            //        row.Cells["id"].Value = sCommand.ExecuteScalar();
            //    }
            //}
            //row.Tag = false;
        }

        private void dgvMissions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if (e.ColumnIndex == dgvMissions.Columns["outpost_id"].Index)
            //{
            //    if (e.Context.ToString().Contains(DataGridViewDataErrorContexts.Display.ToString()))
            //    {
            //        var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //        cell.Value = null;
            //    }
            //}
        }

        private void setNULLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dgvMissions[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Value = DBNull.Value;
            // dgvMissions.Rows[mouseLocation.RowIndex].Tag = true;
            // dgvMissions_RowValidated(null, new DataGridViewCellEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }

        private void dgvMissions_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //mouseLocation = e;
        }

        private void перезагрузитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
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


        private void dgvMissions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //if (!(e.Row.Cells["id"].Value == null) && !(e.Row.Cells["id"].Value == DBNull.Value))
            //{
            //    var id = (int)e.Row.Cells["id"].Value;
            //    using (var sConn = new NpgsqlConnection(sConnStr))
            //    {
            //        sConn.Open();
            //        var sCommand = new NpgsqlCommand
            //        {
            //            Connection = sConn,
            //            CommandText = @"DELETE
            //                            FROM outpost_missions
            //                            WHERE mission_id = @id;"
            //        };
            //        sCommand.Parameters.AddWithValue("id", id);
            //        sCommand.ExecuteNonQuery();
            //    }
            //}
        }

        private void dgvOutposts_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvOutposts_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());
        }

        private void dgvOutposts_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());

        }

    }
}
