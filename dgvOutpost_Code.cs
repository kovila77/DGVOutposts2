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
        private void InitializeDGVOutposts()
        {
            _comboBoxColumnOutpost.InitializeDataTableOutpost();
            dgvOutposts.DefaultCellStyle.NullValue = null;


            dgvOutposts.Rows.Clear();
            dgvOutposts.Columns.Clear();
            dgvOutposts.Columns.Add(strOutpostName, "Название");
            dgvOutposts.Columns.Add(strOutpostEconomicValue, "Экономическая ценность");
            dgvOutposts.Columns.Add(strOutpostCoordinateX, "Координата x");
            dgvOutposts.Columns.Add(strOutpostCoordinateY, "Координата y");
            dgvOutposts.Columns.Add(strOutpostCoordinateZ, "Координата z");
            dgvOutposts.Columns[dgvOutposts.Columns.Add("outpost_id", "id")].Visible = false;

            dgvOutposts.Columns[strOutpostName].ValueType = typeof(string);
            dgvOutposts.Columns[strOutpostEconomicValue].ValueType = typeof(int);
            dgvOutposts.Columns[strOutpostCoordinateX].ValueType = typeof(int);
            dgvOutposts.Columns[strOutpostCoordinateY].ValueType = typeof(int);
            dgvOutposts.Columns[strOutpostCoordinateZ].ValueType = typeof(int);

            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT outpost_id,
                                            outpost_name,
                                            outpost_economic_value,
                                            outpost_coordinate_x,
                                            outpost_coordinate_y,
                                            outpost_coordinate_z
                                    FROM outposts order by outpost_name; "
                };
                var reader = sCommand.ExecuteReader();
                while (reader.Read())
                {
                    _comboBoxColumnOutpost.Add((int)reader["outpost_id"],
                                               (string)reader["outpost_name"],
                                               (int)reader["outpost_coordinate_x"],
                                               (int)reader["outpost_coordinate_y"],
                                               (int)reader["outpost_coordinate_z"]);
                    dgvOutposts.Rows.Add((string)reader["outpost_name"],
                                         (int)reader["outpost_economic_value"],
                                         (int)reader["outpost_coordinate_x"],
                                         (int)reader["outpost_coordinate_y"],
                                         (int)reader["outpost_coordinate_z"],
                                         (int)reader["outpost_id"]);
                }
            }

            //if (dgvMissions.Columns.Count > 1)
            //{
            //    List<object> newDataOutposts = _dtOutposts.AsEnumerable().Select(x => x["id"]).ToList();
            //    for (int i = 0; i < dgvMissions.Rows.Count; i++)
            //    {
            //        if (dgvMissions["outpost_id", i].Value != null)
            //        {
            //            //if (!(newDataOutposts    .Contains(dgvMissions["outpost_id", i].Value)))
            //            if (!(newDataOutposts.Contains(dgvMissions["outpost_id", i].Value)))
            //            {
            //                dgvMissions["outpost_id", i].Value = DBNull.Value;
            //                dgvMissions["outpost_id", i].ErrorText = "Пустое значение!";
            //            }
            //        }
            //    }
            //}
        }

        private void dgvOutposts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            oldCellValue = dgvOutposts[e.ColumnIndex, e.RowIndex].Value;
        }

        private void dgvOutposts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOutposts.Rows[e.RowIndex].IsNewRow
                || !dgvOutposts.Columns[e.ColumnIndex].Visible) return;

            // Проверка на пустоту и попытка распаристь значение ячейки
            var row = dgvOutposts.Rows[e.RowIndex];
            var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //if (dgvOutposts.Columns[e.ColumnIndex].CellType == typeof(DataGridViewComboBoxCell))
            //{
            //    return;
            //}
            int t;
            if (cell.FormattedValue.ToString().RmvSpaces() == "")
            {
                //dgvOutposts.CancelEdit();
                cell.Value = oldCellValue;
                if (IsEntireRowEmpty(row))
                    dgvOutposts.Rows.Remove(row);
                else
                    cell.ErrorText = strEmptyCell;
                return;
            }
            else if (dgvOutposts.Columns[e.ColumnIndex].ValueType == typeof(Int32) && !int.TryParse(cell.FormattedValue.ToString().Trim(), out t))
            {
                //dgvOutposts.CancelEdit();
                //cell.Value = oldCellValue==null?DBNull.Value:oldCellValue;
                cell.Value = oldCellValue;
                //if (IsEmptyRow(row))
                //    dgvOutposts.Rows.Remove(row);
                //else
                cell.ErrorText = strOnlyIntCell;
                return;
            }
            else
            {
                cell.ErrorText = "";
                //if (!dgvOutposts.IsCurrentRowDirty || dgvOutposts.Rows[e.RowIndex].IsNewRow) return;
            }


            // Проверка можно ли фиксировать строку
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[strOutpostName],
                                                   row.Cells[strOutpostEconomicValue],
                                                   row.Cells[strOutpostCoordinateX],
                                                   row.Cells[strOutpostCoordinateY],
                                                   row.Cells[strOutpostCoordinateZ],
                                                 };
            foreach (var cellMaybeError in cellsWithPotentialErrors)
            {
                if (string.IsNullOrEmpty(cellMaybeError.FormattedValue.ToString().Trim()))
                {
                    cellMaybeError.ErrorText = strEmptyCell;
                    row.ErrorText = strBadRow;
                }
                else
                {
                    cellMaybeError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellMaybeError => cellMaybeError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return;

            // Проверка уникальности строк
            foreach (DataGridViewRow curRow in dgvOutposts.Rows)
            {
                if (!curRow.IsNewRow
                    && curRow.Index != row.Index
                    && curRow.Cells[strOutpostName].Value.ToString().Trim() == row.Cells[strOutpostName].Value.ToString().Trim()
                    && (int)curRow.Cells[strOutpostCoordinateX].Value == (int)row.Cells[strOutpostCoordinateX].Value
                    && (int)curRow.Cells[strOutpostCoordinateY].Value == (int)row.Cells[strOutpostCoordinateY].Value
                    && (int)curRow.Cells[strOutpostCoordinateZ].Value == (int)row.Cells[strOutpostCoordinateZ].Value)
                {
                    //dgvOutposts.CancelEdit();
                    MessageBox.Show($"Форпост {curRow.Cells[strOutpostName].Value.ToString().Trim()} с координатами {(int)curRow.Cells[strOutpostCoordinateX].Value};{(int)curRow.Cells[strOutpostCoordinateY].Value};{(int)curRow.Cells[strOutpostCoordinateZ].Value} уже существует!");
                    row.Cells[e.ColumnIndex].Value = oldCellValue;
                    if (oldCellValue == null || string.IsNullOrEmpty(oldCellValue.ToString().Trim()))
                    {
                        cell.ErrorText = strEmptyCell;
                        row.ErrorText = strBadRow;
                    }
                    //row.ErrorText = strExistingOutpostRow;
                    return;
                }
            }

            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn
                };
                sCommand.Parameters.AddWithValue("outpost_name", row.Cells[strOutpostName].Value.ToString());
                sCommand.Parameters.AddWithValue("outpost_economic_value", (int)row.Cells[strOutpostEconomicValue].Value);
                sCommand.Parameters.AddWithValue("outpost_coordinate_x", (int)row.Cells[strOutpostCoordinateX].Value);
                sCommand.Parameters.AddWithValue("outpost_coordinate_y", (int)row.Cells[strOutpostCoordinateY].Value);
                sCommand.Parameters.AddWithValue("outpost_coordinate_z", (int)row.Cells[strOutpostCoordinateZ].Value);
                if (row.Cells[strOutpostId].FormattedValue.ToString().Trim().Length > 0)
                {
                    sCommand.CommandText = @"
                            UPDATE outposts
                            SET outpost_name           = @outpost_name,
                                outpost_economic_value = @outpost_economic_value,
                                outpost_coordinate_x   = @outpost_coordinate_x,
                                outpost_coordinate_y   = @outpost_coordinate_y,
                                outpost_coordinate_z   = @outpost_coordinate_z
                            WHERE outpost_id = @outpost_id;";
                    sCommand.Parameters.AddWithValue("outpost_id", (int)row.Cells[strOutpostId].Value);
                    sCommand.ExecuteNonQuery();
                    _comboBoxColumnOutpost.Change((int)row.Cells[strOutpostId].Value,
                                                row.Cells[strOutpostName].Value.ToString(),
                                                (int)row.Cells[strOutpostCoordinateX].Value,
                                                (int)row.Cells[strOutpostCoordinateY].Value,
                                                (int)row.Cells[strOutpostCoordinateZ].Value);
                }
                else
                {
                    sCommand.CommandText = @"
                            INSERT INTO outposts (outpost_name,
                                                  outpost_economic_value,
                                                  outpost_coordinate_x,
                                                  outpost_coordinate_y,
                                                  outpost_coordinate_z)
                            VALUES (@outpost_name,
                                    @outpost_economic_value,
                                    @outpost_coordinate_x,
                                    @outpost_coordinate_y,
                                    @outpost_coordinate_z)
                            RETURNING outpost_id;";
                    row.Cells["outpost_id"].Value = sCommand.ExecuteScalar();
                    _comboBoxColumnOutpost.Add((int)row.Cells[strOutpostId].Value,
                                                row.Cells[strOutpostName].Value.ToString(),
                                                (int)row.Cells[strOutpostCoordinateX].Value,
                                                (int)row.Cells[strOutpostCoordinateY].Value,
                                                (int)row.Cells[strOutpostCoordinateZ].Value);
                }
            }
        }


        private void dgvOutposts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (!dgvOutposts.IsCurrentRowDirty || dgvOutposts.Rows[e.RowIndex].IsNewRow) return;

            //var row = dgvOutposts.Rows[e.RowIndex];
            //var cellsWithPotentialErrors = new List<DataGridViewCell> {
            //                                       row.Cells[strOutpostName],
            //                                       row.Cells[strOutpostEconomicValue],
            //                                       row.Cells[strOutpostCoordinateX],
            //                                       row.Cells[strOutpostCoordinateY],
            //                                       row.Cells[strOutpostCoordinateZ],
            //                                     };
            //foreach (var cell in cellsWithPotentialErrors)
            //{
            //    if (string.IsNullOrEmpty(cell.FormattedValue.ToString().Trim()))
            //    {
            //        cell.ErrorText = strEmptyCell;
            //        row.ErrorText = strBadRow;
            //    }
            //    else
            //    {
            //        cell.ErrorText = "";
            //    }
            //}
            //if (cellsWithPotentialErrors.FirstOrDefault(cell => cell.ErrorText.Length > 0) == null)
            //    row.ErrorText = "";
            //else
            //    return;

            //foreach (DataGridViewRow curRow in dgvOutposts.Rows)
            //{
            //    if (!curRow.IsNewRow
            //        && curRow.Index != row.Index
            //        && curRow.Cells[strOutpostName].Value.ToString() == row.Cells[strOutpostName].Value.ToString()
            //        && (int)curRow.Cells[strOutpostCoordinateX].Value == (int)row.Cells[strOutpostCoordinateX].Value
            //        && (int)curRow.Cells[strOutpostCoordinateY].Value == (int)row.Cells[strOutpostCoordinateY].Value
            //        && (int)curRow.Cells[strOutpostCoordinateZ].Value == (int)row.Cells[strOutpostCoordinateZ].Value)
            //    {
            //        //dgvOutposts.CancelEdit();
            //        row.Cells[e.ColumnIndex].Value = oldCellValue;
            //        row.ErrorText = strExistingOutpostRow;
            //        return;
            //    }
            //}

            //using (var sConn = new NpgsqlConnection(sConnStr))
            //{
            //    sConn.Open();
            //    var sCommand = new NpgsqlCommand
            //    {
            //        Connection = sConn
            //    };
            //    sCommand.Parameters.AddWithValue("outpost_name", row.Cells[strOutpostName].Value.ToString());
            //    sCommand.Parameters.AddWithValue("outpost_economic_value", (int)row.Cells[strOutpostEconomicValue].Value);
            //    sCommand.Parameters.AddWithValue("outpost_coordinate_x", (int)row.Cells[strOutpostCoordinateX].Value);
            //    sCommand.Parameters.AddWithValue("outpost_coordinate_y", (int)row.Cells[strOutpostCoordinateY].Value);
            //    sCommand.Parameters.AddWithValue("outpost_coordinate_z", (int)row.Cells[strOutpostCoordinateZ].Value);
            //    if (row.Cells[strOutpostId].FormattedValue.ToString().Trim().Length > 0)
            //    {
            //        sCommand.CommandText = @"
            //                UPDATE outposts
            //                SET outpost_name           = @outpost_name,
            //                    outpost_economic_value = @outpost_economic_value,
            //                    outpost_coordinate_x   = @outpost_coordinate_x,
            //                    outpost_coordinate_y   = @outpost_coordinate_y,
            //                    outpost_coordinate_z   = @outpost_coordinate_z
            //                WHERE outpost_id = @outpost_id;";
            //        sCommand.Parameters.AddWithValue("outpost_id", (int)row.Cells[strOutpostId].Value);
            //        sCommand.ExecuteNonQuery();
            //        _comboBoxColumnOutpost.Change((int)row.Cells[strOutpostId].Value,
            //                                    row.Cells[strOutpostName].Value.ToString(),
            //                                    (int)row.Cells[strOutpostCoordinateX].Value,
            //                                    (int)row.Cells[strOutpostCoordinateY].Value,
            //                                    (int)row.Cells[strOutpostCoordinateZ].Value);
            //    }
            //    else
            //    {
            //        sCommand.CommandText = @"
            //                INSERT INTO outposts (outpost_name,
            //                                      outpost_economic_value,
            //                                      outpost_coordinate_x,
            //                                      outpost_coordinate_y,
            //                                      outpost_coordinate_z)
            //                VALUES (@outpost_name,
            //                        @outpost_economic_value,
            //                        @outpost_coordinate_x,
            //                        @outpost_coordinate_y,
            //                        @outpost_coordinate_z)
            //                RETURNING outpost_id;";
            //        row.Cells["outpost_id"].Value = sCommand.ExecuteScalar();
            //        _comboBoxColumnOutpost.Add((int)row.Cells[strOutpostId].Value,
            //                                    row.Cells[strOutpostName].Value.ToString(),
            //                                    (int)row.Cells[strOutpostCoordinateX].Value,
            //                                    (int)row.Cells[strOutpostCoordinateY].Value,
            //                                    (int)row.Cells[strOutpostCoordinateZ].Value);
            //    }
            //}
        }

        private void dgvOutposts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context.ToString().Contains(DataGridViewDataErrorContexts.Parsing.ToString()))
            {
                //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
                //cell.Value = dgvOutposts[e.ColumnIndex, e.RowIndex].FormattedValue;
                dgvOutposts.CancelEdit();
                e.Cancel = false;
                return;
            }
        }

        private void dgvOutposts_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!(e.Row.Cells[strOutpostId].Value is null) /*&& e.row.cells["id"].value != dbnull.value*/)
            {
                var outpost_id = (int)e.Row.Cells[strOutpostId].Value;
                //DataGridViewCell cell;
                //for (int i = 0; i < dgvmissions.rows.count - 1; i++)
                //{
                //    cell = dgvmissions.rows[i].cells["outpost_id"];
                //    if (cell.Value != null && cell.value != dbnull.value && (int)cell.value == id)
                //    {
                //        MessageBox.Show("данный форпост связан с миссией! измените или удалите миссию!");
                //        e.Cancel = true;
                //        return;
                //    }
                //}
                using (var sconn = new NpgsqlConnection(sConnStr))
                {
                    sconn.Open();
                    var scommand = new NpgsqlCommand
                    {
                        Connection = sconn,
                        CommandText = "delete from outposts where outpost_id = @outpost_id"
                    };
                    scommand.Parameters.AddWithValue("outpost_id", outpost_id);
                    scommand.ExecuteNonQuery();
                    _comboBoxColumnOutpost.Remove(outpost_id);
                }
            }
        }
    }
}
