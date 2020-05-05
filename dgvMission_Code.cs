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
            dgvMissions.Columns[MyHelper.strDateBegin].ValueType = typeof(DateTime);
            dgvMissions.Columns[MyHelper.strDatePlanEnd].ValueType = typeof(DateTime);
            dgvMissions.Columns[MyHelper.strDateActualEnd].ValueType = typeof(DateTime);
            dgvMissions.Columns[MyHelper.strMissionId].ValueType = typeof(int);


            dgvMissions.Columns[MyHelper.strMissionId].Visible = false;

            dgvMissions.Columns[MyHelper.strOutpostId].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns[MyHelper.strDateBegin].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns[MyHelper.strDatePlanEnd].SortMode = DataGridViewColumnSortMode.Automatic;
            dgvMissions.Columns[MyHelper.strDateActualEnd].SortMode = DataGridViewColumnSortMode.NotSortable;

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


        private void dgvMissions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            oldCellValue = dgvMissions[e.ColumnIndex, e.RowIndex].Value;
        }

        private void dgvMissions_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //if (dgvMissions.Columns[e.ColumnIndex].ValueType == typeof(String) && e.Value != null)
            //{
            //    var strValue = e.Value.ToString().RmvExtrSpaces();
            //    if (string.IsNullOrEmpty(strValue))
            //    {
            //        dgvMissions.CancelEdit();
            //    }
            //    else
            //    {
            //        e.Value = strValue;
            //        e.ParsingApplied = true;
            //    }
            //}
            var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = e.Value.ToString().RmvExtrSpaces();
            int t;
            if (cellFormatedValue == "" && cell.OwningColumn.Name != MyHelper.strDateActualEnd)
            {
                dgvMissions.CancelEdit();
                //e.ParsingApplied = false;
                return;
            }
            else if (dgvMissions.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
                && dgvMissions.Columns[e.ColumnIndex].ValueType == typeof(Int32)
                && !int.TryParse(cellFormatedValue, out t))
            {
                dgvMissions.CancelEdit();
                //cell.ErrorText = MyHelper.strUncorrectIntValueCell;
                MessageBox.Show(MyHelper.strUncorrectIntValueCell);
                //e.ParsingApplied = false;
                return;
            }
            else
            {
                cell.ErrorText = "";
            }
        }

        private void dgvMissions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMissions.Rows[e.RowIndex].IsNewRow
               || !dgvMissions.Columns[e.ColumnIndex].Visible) return;

            // Проверка на пустоту и попытка распаристь значение ячейки
            var row = dgvMissions.Rows[e.RowIndex];
            var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();
            //int t;
            //if (cellFormatedValue == "" && cell.OwningColumn.Name != MyHelper.strDateActualEnd)
            //{
            //    //dgvMissions.CancelEdit();
            //    if (MyHelper.IsEntireRowEmpty(row))
            //        dgvMissions.Rows.Remove(row);
            //    else
            //    {
            //        cell.ErrorText = MyHelper.strEmptyCell;
            //        cell.Value = oldCellValue;
            //    }
            //    return;
            //}
            ////else if (dgvMissions.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell))
            ////{
            ////    return;
            ////}
            ////else if (dgvMissions.Columns[e.ColumnIndex].ValueType == typeof(Int32) && !int.TryParse(cellFormatedValue, out t))
            ////{
            ////    //dgvMissions.CancelEdit();
            ////    //cell.Value = oldCellValue==null?DBNull.Value:oldCellValue;
            ////    cell.Value = oldCellValue;
            ////    //if (IsEmptyRow(row))
            ////    //    dgvMissions.Rows.Remove(row);
            ////    //else
            ////    cell.ErrorText = MyHelper.strOnlyIntCell;
            ////    return;
            ////}
            //else
            //{
            //    cell.ErrorText = "";
            //    //if (!dgvMissions.IsCurrentRowDirty || dgvMissions.Rows[e.RowIndex].IsNewRow) return;
            //}

            //Проверка соответсвия ячейки
            var valueBegin = dgvMissions[MyHelper.strDateBegin, e.RowIndex].Value;
            var valuePlanEnd = dgvMissions[MyHelper.strDatePlanEnd, e.RowIndex].Value;
            var valueActual = dgvMissions[MyHelper.strDateActualEnd, e.RowIndex].Value;
            if (cell.OwningColumn.Name == MyHelper.strDateBegin
                && ((valuePlanEnd != null) && (DateTime)cell.Value > (DateTime)valuePlanEnd
                    || valueActual != null && (DateTime)cell.Value > (DateTime)valueActual)
                ||
                (cell.OwningColumn.Name == MyHelper.strDatePlanEnd || cell.OwningColumn.Name == MyHelper.strDateActualEnd)
                    && valueBegin != null && (DateTime)cell.Value < (DateTime)valueBegin)
            {
                MessageBox.Show("Дата окончания не может быть меньше даты начала!");
                row.ErrorText = MyHelper.strBadRow;
                cell.Value = oldCellValue;
                return;
            }

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

            //// Проверка уникальности строк
            //foreach (DataGridViewRow curRow in dgvMissions.Rows)
            //{
            //    if (!curRow.IsNewRow
            //        && curRow.Index != row.Index
            //        && curRow.Cells[MyHelper.strOutpostName].FormattedValue.ToString().RmvExtrSpaces()
            //            == row.Cells[MyHelper.strOutpostName].FormattedValue.ToString().RmvExtrSpaces()
            //        && (int)curRow.Cells[MyHelper.strOutpostCoordinateX].Value
            //            == (int)row.Cells[MyHelper.strOutpostCoordinateX].Value
            //        && (int)curRow.Cells[MyHelper.strOutpostCoordinateY].Value
            //            == (int)row.Cells[MyHelper.strOutpostCoordinateY].Value
            //        && (int)curRow.Cells[MyHelper.strOutpostCoordinateZ].Value
            //            == (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value)
            //    {
            //        //dgvMissions.CancelEdit();
            //        MessageBox.Show($"Форпост {curRow.Cells[MyHelper.strOutpostName].Value.ToString().RmvExtrSpaces()} с координатами {(int)curRow.Cells[MyHelper.strOutpostCoordinateX].Value};{(int)curRow.Cells[MyHelper.strOutpostCoordinateY].Value};{(int)curRow.Cells[MyHelper.strOutpostCoordinateZ].Value} уже существует!");
            //        row.Cells[e.ColumnIndex].Value = oldCellValue;
            //        if (oldCellValue == null || oldCellValue.ToString().RmvExtrSpaces() == "")
            //        {
            //            cell.ErrorText = MyHelper.strEmptyCell;
            //            row.ErrorText = MyHelper.strBadRow;
            //        }
            //        //row.ErrorText = strExistingOutpostRow;
            //        return;
            //    }
            //}
            //MessageBox.Show("Фиксация");
            //return;

            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn
                };
                sCommand.Parameters.AddWithValue("mission_description", row.Cells[MyHelper.strMissionDescription].Value);
                sCommand.Parameters.AddWithValue("outpost_id", row.Cells[MyHelper.strOutpostId].Value);
                sCommand.Parameters.AddWithValue("date_begin", row.Cells[MyHelper.strDateBegin].Value);
                sCommand.Parameters.AddWithValue("date_plan_end", row.Cells[MyHelper.strDatePlanEnd].Value);
                sCommand.Parameters.AddWithValue("date_actual_end", row.Cells[MyHelper.strDateActualEnd].Value == null ? DBNull.Value : row.Cells[MyHelper.strDateActualEnd].Value);

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
                    sCommand.Parameters.AddWithValue("mission_id", (int)row.Cells[MyHelper.strMissionId].Value);
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

        private void dgvMissions_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!dgvMissions.IsCurrentRowDirty)
            {
                return;
            }
            var row = dgvMissions.Rows[e.RowIndex];
            row.Tag = false;
            bool willCommit = true;
            //row.ErrorText = "error";
            var cellsWithPotentialErrors = new[] { row.Cells["outpost_id"], row.Cells["description"], row.Cells["date_begin"], row.Cells["date_plan_end"] };
            foreach (var cell in cellsWithPotentialErrors)
            {
                if (cell.Value is DBNull
                        || string.IsNullOrWhiteSpace((string)cell.FormattedValue))
                {
                    if (cell.OwningColumn.Name == "outpost_id" && cell.Value is DBNull)
                    {
                        cell.ErrorText = "Данный форпост не сохранён в базе данных!";
                    }
                    else
                    {
                        cell.ErrorText = "Пустое значение!";
                    }
                    willCommit = false;
                    row.ErrorText = $"Проверьте данные";
                }
                else
                {
                    cell.ErrorText = "";
                }
            }
            if (willCommit)
            {
                row.ErrorText = ""; row.Tag = willCommit;
                return;
            }
        }

        private void dgvMissions_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!(dgvMissions.Rows[e.RowIndex].Tag is bool && (bool)dgvMissions.Rows[e.RowIndex].Tag) || dgvMissions.CurrentRow.ErrorText != "")
            {
                return;
            }
            var row = dgvMissions.Rows[e.RowIndex];

            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn
                };
                sCommand.Parameters.AddWithValue("description", row.Cells["description"].Value);
                sCommand.Parameters.AddWithValue("outpost_id", row.Cells["outpost_id"].Value);
                sCommand.Parameters.AddWithValue("date_begin", row.Cells["date_begin"].Value);
                sCommand.Parameters.AddWithValue("date_plan_end", row.Cells["date_plan_end"].Value);
                sCommand.Parameters.AddWithValue("date_actual_end", row.Cells["date_actual_end"].Value is null ? DBNull.Value : row.Cells["date_actual_end"].Value);

                if (row.Cells["id"].Value is int)
                {
                    sCommand.CommandText = @"
                        UPDATE outpost_missions
                        SET outpost_id          = @outpost_id,
                            mission_description = @description,
                            date_begin          = @date_begin,
                            date_plan_end       = @date_plan_end,
                            date_actual_end     = @date_actual_end
                        WHERE mission_id        = @id;";
                    sCommand.Parameters.AddWithValue("id", (int)row.Cells["id"].Value);
                    var res = sCommand.ExecuteScalar();
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
                                @description,
                                @date_begin,
                                @date_plan_end,
                                @date_actual_end)
                        RETURNING mission_id;";
                    row.Cells["id"].Value = sCommand.ExecuteScalar();
                }
            }
            row.Tag = false;
        }

        private void dgvMissions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if (e.ColumnIndex == dgvMissions.Columns["outpost_id"].Index)
            //{
            if (e.Context.ToString().Contains(DataGridViewDataErrorContexts.Display.ToString()))
            {
                var cell = dgvMissions[e.ColumnIndex, e.RowIndex];
                cell.Value = null;
            }
            //}
        }

        private void setNULLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvMissions[mouseLocation.ColumnIndex, mouseLocation.RowIndex].Value = null;
            dgvMissions_CellEndEdit(sender, new DataGridViewCellEventArgs(mouseLocation.ColumnIndex, mouseLocation.RowIndex));
        }

        private void dgvMissions_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void dgvMissions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strMissionId].Value != null)
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
        }
    }
}
