using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DGVOutposts
{
    public partial class formDGVOutposts : Form
    {
        private DataGridViewCellEventArgs mouseLocation;

        private void InitializeDGVMissions()
        {
            dgvMissions.Rows.Clear();
            dgvMissions.Columns.Clear();
            dgvMissions.DefaultCellStyle.NullValue = null;

            dgvMissions.Columns.Add(MyHelper.strMissionDescription, "Описание миссии");
            dgvMissions.Columns.Add(MyHelper.strMissionId, "id");
            dgvMissions.Columns.Add(_comboBoxColumnOutpost);
            dgvMissions.Columns.Add(new CalendarColumn { Name = MyHelper.strDateBegin, HeaderText = "Дата начала" });
            dgvMissions.Columns.Add(new CalendarColumn { Name = MyHelper.strDatePlanEnd, HeaderText = "Планируемое завершение" });
            dgvMissions.Columns.Add(new CalendarColumn { Name = MyHelper.strDateActualEnd, HeaderText = "Реальное завершение", ContextMenuStrip = contextMenuStripNULL });

            dgvMissions.Columns[MyHelper.strMissionDescription].ValueType = typeof(string);
            dgvMissions.Columns[MyHelper.strOutpostId].ValueType = typeof(int);
            //dgvMissions.Columns[MyHelper.strDateBegin].ValueType = typeof(DateTime);
            //dgvMissions.Columns[MyHelper.strDatePlanEnd].ValueType = typeof(DateTime);
            //dgvMissions.Columns[MyHelper.strDateActualEnd].ValueType = typeof(DateTime);
            dgvMissions.Columns[MyHelper.strMissionId].ValueType = typeof(int);


            dgvMissions.Columns[MyHelper.strMissionId].Visible = false;

            dgvMissions.Columns[MyHelper.strOutpostId].SortMode = DataGridViewColumnSortMode.Programmatic;

            dgvMissions.Columns[MyHelper.strDateBegin].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgvMissions.Columns[MyHelper.strDatePlanEnd].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgvMissions.Columns[MyHelper.strDateActualEnd].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgvMissions.ColumnHeaderMouseClick -= DgvMissions_ColumnHeaderMouseClick;
            dgvMissions.ColumnHeaderMouseClick += DgvMissions_ColumnHeaderMouseClick;

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
                using (var reader = sCommand.ExecuteReader())
                {
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
        }

        private void DgvMissions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strDateBegin
                || dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strDatePlanEnd
                || dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strDateActualEnd
                || dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strOutpostId)
            {
                if (dgvMissions.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    if (dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strOutpostId)
                        dgvMissions.Sort(new RowComparerForComboboxString(SortOrder.Descending, e.ColumnIndex));
                    else
                        dgvMissions.Sort(new RowComparerForDate(SortOrder.Descending, e.ColumnIndex));
                    dgvMissions.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    if (dgvMissions.Columns[e.ColumnIndex].Name == MyHelper.strOutpostId)
                        dgvMissions.Sort(new RowComparerForComboboxString(SortOrder.Ascending, e.ColumnIndex));
                    else
                        dgvMissions.Sort(new RowComparerForDate(SortOrder.Ascending, e.ColumnIndex));
                    dgvMissions.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            foreach (DataGridViewColumn column in dgvMissions.Columns)
            {
                if (column.Index == e.ColumnIndex) continue;
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private class RowComparerForDate : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            private int columnIndex;

            public RowComparerForDate(SortOrder sortOrder, int columnIndex)
            {
                this.columnIndex = columnIndex;
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow row1 = (DataGridViewRow)x;
                DataGridViewRow row2 = (DataGridViewRow)y;

                int CompareResult = 0;
                if (row1.Cells[columnIndex].Value != DBNull.Value && row1.Cells[columnIndex].Value != null && row2.Cells[columnIndex].Value != DBNull.Value && row2.Cells[columnIndex].Value != null)
                {
                    CompareResult = DateTime.Compare((DateTime)row1.Cells[columnIndex].Value, (DateTime)row2.Cells[columnIndex].Value);
                }
                else if (row1.Cells[columnIndex].Value != DBNull.Value && row1.Cells[columnIndex].Value != null)
                {
                    CompareResult = 1;
                }
                else if (row2.Cells[columnIndex].Value != DBNull.Value && row2.Cells[columnIndex].Value != null)
                {
                    CompareResult = -1;
                }

                return CompareResult * sortOrderModifier;
            }
        }

        private class RowComparerForComboboxString : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            private int columnIndex;

            public RowComparerForComboboxString(SortOrder sortOrder, int columnIndex)
            {
                this.columnIndex = columnIndex;
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow row1 = (DataGridViewRow)x;
                DataGridViewRow row2 = (DataGridViewRow)y;

                int CompareResult = System.String.Compare(
                    row1.Cells[columnIndex].FormattedValue.ToString(),
                    row2.Cells[columnIndex].FormattedValue.ToString());

                return CompareResult * sortOrderModifier;
            }
        }

        private void dgvMissions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
            var row = dgvMissions.Rows[e.RowIndex];
            var valueBegin = dgvMissions[MyHelper.strDateBegin, e.RowIndex].Value;
            var valuePlanEnd = dgvMissions[MyHelper.strDatePlanEnd, e.RowIndex].Value;
            var valueActual = dgvMissions[MyHelper.strDateActualEnd, e.RowIndex].Value;
            if (cell.OwningColumn.Name == MyHelper.strDateBegin)
            {
                if (valuePlanEnd != null && valuePlanEnd != DBNull.Value && valueActual != null && valueActual != DBNull.Value)
                    ((CalendarCell)cell).MyMaxDate = ((DateTime)valuePlanEnd).Date > ((DateTime)valueActual).Date
                        ? ((DateTime)valueActual).Date : ((DateTime)valuePlanEnd).Date;

                else if (valuePlanEnd != null && valuePlanEnd != DBNull.Value)
                    ((CalendarCell)cell).MyMaxDate = ((DateTime)valuePlanEnd).Date;

                else if (valueActual != null && valueActual != DBNull.Value)
                    ((CalendarCell)cell).MyMaxDate = ((DateTime)valueActual).Date;
            }
            else if (valueBegin != null && valueBegin != DBNull.Value && (cell.OwningColumn.Name == MyHelper.strDatePlanEnd || cell.OwningColumn.Name == MyHelper.strDateActualEnd))
            {
                ((CalendarCell)cell).MyMinDate = ((DateTime)valueBegin).Date;
            }
        }

        private void dgvMissions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMissions.Rows[e.RowIndex].IsNewRow
               || !dgvMissions.Columns[e.ColumnIndex].Visible) return;
            var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();
            if (cell.ValueType == typeof(String))
            {
                cell.Value = cellFormatedValue;
            }
        }

        private void dgvMissions_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvMissions.Rows[e.RowIndex];
            if ((row.IsNewRow || !dgvMissions.IsCurrentRowDirty) && sender != contextMenuStripNULL)
                return;

            // Проверка можно ли фиксировать строку
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strMissionDescription],
                                                   row.Cells[MyHelper.strOutpostId],
                                                   row.Cells[MyHelper.strDateBegin],
                                                   row.Cells[MyHelper.strDatePlanEnd],
                                                 };
            foreach (var cellWithPotentialError in cellsWithPotentialErrors)
            {
                if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
                {
                    cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                    row.ErrorText = MyHelper.strBadRow;
                }
                else
                {
                    cellWithPotentialError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellWithPotentialError => cellWithPotentialError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return;

            ////Проверка соответсвия ячейки
            //var valueBegin = ((DateTime)dgvMissions[MyHelper.strDateBegin, e.RowIndex].Value).Date;
            //var valuePlanEnd = ((DateTime)dgvMissions[MyHelper.strDatePlanEnd, e.RowIndex].Value).Date;
            //var valueActual = dgvMissions[MyHelper.strDateActualEnd, e.RowIndex].Value;
            //if (valueActual != null && valueBegin > ((DateTime)valueActual).Date
            //    || valueBegin > valuePlanEnd)
            //{
            //    MessageBox.Show("Дата окончания не может быть меньше даты начала!");
            //    row.ErrorText = MyHelper.strBadRow + " Дата окончания не может быть меньше даты начала!";
            //    return;
            //}
            //MessageBox.Show("Фиксация...");
            try
            {
                using (var sConn = new NpgsqlConnection(sConnStr))
                {
                    sConn.Open();
                    var sCommand = new NpgsqlCommand
                    {
                        Connection = sConn,
                        CommandText = @"SELECT 1
                                        FROM outpost_missions
                                        WHERE outpost_id = @outpost_id
                                          AND mission_description = @mission_description
                                          AND date_begin = @date_begin
                                          AND date_plan_end = @date_plan_end
                                          AND date_actual_end IS NOT DISTINCT FROM @date_actual_end
                                          AND mission_id IS DISTINCT FROM @mission_id;"
                    };
                    sCommand.Parameters.AddWithValue("mission_id", row.Cells[MyHelper.strMissionId].Value == null ? DBNull.Value : row.Cells[MyHelper.strMissionId].Value);
                    sCommand.Parameters.AddWithValue("mission_description", row.Cells[MyHelper.strMissionDescription].Value);
                    sCommand.Parameters.AddWithValue("outpost_id", row.Cells[MyHelper.strOutpostId].Value);
                    sCommand.Parameters.AddWithValue("date_begin", row.Cells[MyHelper.strDateBegin].Value);
                    sCommand.Parameters.AddWithValue("date_plan_end", row.Cells[MyHelper.strDatePlanEnd].Value);
                    sCommand.Parameters.AddWithValue("date_actual_end", row.Cells[MyHelper.strDateActualEnd].Value == null ? DBNull.Value : row.Cells[MyHelper.strDateActualEnd].Value);
                    if (sCommand.ExecuteScalar() != null)
                    {
                        MessageBox.Show(MyHelper.strExistingMision);
                        row.ErrorText = MyHelper.strBadRow + " " + MyHelper.strExistingMision;
                        return;
                    }

                    if (row.Cells[MyHelper.strMissionId].Value != null)
                    {
                        sCommand.CommandText = @"
                        UPDATE outpost_missions
                        SET outpost_id          = @outpost_id,
                            mission_description = @mission_description,
                            date_begin          = @date_begin,
                            date_plan_end       = @date_plan_end,
                            date_actual_end     = @date_actual_end
                        WHERE mission_id        = @mission_id;";
                        sCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        sCommand.CommandText = @"
                        INSERT INTO outpost_missions (outpost_id,
                                                      mission_description,
                                                      date_begin,
                                                      date_plan_end,
                                                      date_actual_end)
                        VALUES (@outpost_id,
                                @mission_description,
                                @date_begin,
                                @date_plan_end,
                                @date_actual_end)
                        RETURNING mission_id;";
                        row.Cells[MyHelper.strMissionId].Value = sCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //private void dgvMissions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        //{
        //    if (e.Context.ToString().Contains(DataGridViewDataErrorContexts.Display.ToString()))
        //    {
        //        var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
        //        cell.Value = null;
        //    }
        //}

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dgvMissions.ClearSelection();
            dgvMissions[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Selected = true;
        }

        private void setNULLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvMissions[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Value = DBNull.Value;
            dgvMissions_RowValidating(contextMenuStripNULL, new DataGridViewCellCancelEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }

        private void dgvMissions_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void dgvMissions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strMissionId].Value != null)
            {
                try
                {
                    var mission_id = (int)e.Row.Cells[MyHelper.strMissionId].Value;
                    using (var sConn = new NpgsqlConnection(sConnStr))
                    {
                        sConn.Open();
                        var sCommand = new NpgsqlCommand
                        {
                            Connection = sConn,
                            CommandText = @"DELETE
                                        FROM outpost_missions
                                        WHERE mission_id = @mission_id;"
                        };
                        sCommand.Parameters.AddWithValue("mission_id", mission_id);
                        sCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
    }
}
