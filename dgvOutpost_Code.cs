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

            dgvOutposts.CancelEdit();
            dgvOutposts.Rows.Clear();
            dgvOutposts.Columns.Clear();
            dgvOutposts.DefaultCellStyle.NullValue = null;

            dgvOutposts.Columns.Add(MyHelper.strOutpostName, "Название");
            dgvOutposts.Columns.Add(MyHelper.strOutpostEconomicValue, "Экономическая ценность");
            dgvOutposts.Columns.Add(MyHelper.strOutpostCoordinateX, "Координата x");
            dgvOutposts.Columns.Add(MyHelper.strOutpostCoordinateY, "Координата y");
            dgvOutposts.Columns.Add(MyHelper.strOutpostCoordinateZ, "Координата z");
            dgvOutposts.Columns.Add(MyHelper.strOutpostId, "id");

            dgvOutposts.Columns[MyHelper.strOutpostName].ValueType = typeof(string);
            dgvOutposts.Columns[MyHelper.strOutpostEconomicValue].ValueType = typeof(int);
            dgvOutposts.Columns[MyHelper.strOutpostCoordinateX].ValueType = typeof(int);
            dgvOutposts.Columns[MyHelper.strOutpostCoordinateY].ValueType = typeof(int);
            dgvOutposts.Columns[MyHelper.strOutpostCoordinateZ].ValueType = typeof(int);

            dgvOutposts.Columns[MyHelper.strOutpostId].Visible = false;

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
        }

        private void dgvOutposts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOutposts.Rows[e.RowIndex].IsNewRow
                || !dgvOutposts.Columns[e.ColumnIndex].Visible) return;

            //var row = dgvOutposts.Rows[e.RowIndex];
            var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            if (cell.ValueType == typeof(String))
            {
                cell.Value = cellFormatedValue;
            }
        }

        private void dgvOutposts_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvOutposts.Rows[e.RowIndex];
            if (row.IsNewRow || !dgvOutposts.IsCurrentRowDirty) return;
            //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            // Проверка можно ли фиксировать строку
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strOutpostName],
                                                   row.Cells[MyHelper.strOutpostEconomicValue],
                                                   row.Cells[MyHelper.strOutpostCoordinateX],
                                                   row.Cells[MyHelper.strOutpostCoordinateY],
                                                   row.Cells[MyHelper.strOutpostCoordinateZ],
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

            try
            {
                using (var sConn = new NpgsqlConnection(sConnStr))
                {
                    sConn.Open();
                    //var outpost_name = row.Cells[MyHelper.strOutpostName].Value.ToString().RmvExtrSpaces();
                    //var outpost_economic_value = (int)row.Cells[MyHelper.strOutpostEconomicValue].Value;
                    //var outpost_coordinate_x = (int)row.Cells[MyHelper.strOutpostCoordinateX].Value;
                    //var outpost_coordinate_y = (int)row.Cells[MyHelper.strOutpostCoordinateY].Value;
                    //var outpost_coordinate_z = (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value;

                    // Проверка на уникальность без учёта регистра
                    var sCommand = new NpgsqlCommand
                    {
                        Connection = sConn,
                        CommandText = @"SELECT 1
                                        FROM outposts
                                        WHERE lower(outpost_name) = lower(@outpost_name)
                                          AND outpost_coordinate_x = @outpost_coordinate_x
                                          AND outpost_coordinate_y = @outpost_coordinate_y
                                          AND outpost_coordinate_z = @outpost_coordinate_z
                                          AND outpost_id IS DISTINCT FROM @outpost_id;"
                    };
                    sCommand.Parameters.AddWithValue("outpost_id", row.Cells[MyHelper.strOutpostId].Value == null ? DBNull.Value : row.Cells[MyHelper.strOutpostId].Value);
                    sCommand.Parameters.AddWithValue("outpost_name", row.Cells[MyHelper.strOutpostName].Value.ToString().RmvExtrSpaces());
                    sCommand.Parameters.AddWithValue("outpost_economic_value", (int)row.Cells[MyHelper.strOutpostEconomicValue].Value);
                    sCommand.Parameters.AddWithValue("outpost_coordinate_x", (int)row.Cells[MyHelper.strOutpostCoordinateX].Value);
                    sCommand.Parameters.AddWithValue("outpost_coordinate_y", (int)row.Cells[MyHelper.strOutpostCoordinateY].Value);
                    sCommand.Parameters.AddWithValue("outpost_coordinate_z", (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value);
                    if (sCommand.ExecuteScalar() != null)
                    {
                        string eo = $"Форпост {row.Cells[MyHelper.strOutpostName].Value.ToString().RmvExtrSpaces()} с координатами {(int)row.Cells[MyHelper.strOutpostCoordinateX].Value};{(int)row.Cells[MyHelper.strOutpostCoordinateY].Value};{(int)row.Cells[MyHelper.strOutpostCoordinateZ].Value} уже существует!";
                        MessageBox.Show(eo);
                        row.ErrorText = MyHelper.strBadRow + " " + eo;
                        return;
                    }

                    if (row.Cells[MyHelper.strOutpostId].FormattedValue.ToString().Trim().Length > 0)
                    {
                        sCommand.CommandText = @"
                            UPDATE outposts
                            SET outpost_name           = @outpost_name,
                                outpost_economic_value = @outpost_economic_value,
                                outpost_coordinate_x   = @outpost_coordinate_x,
                                outpost_coordinate_y   = @outpost_coordinate_y,
                                outpost_coordinate_z   = @outpost_coordinate_z
                            WHERE outpost_id = @outpost_id;";
                        sCommand.ExecuteNonQuery();
                        _comboBoxColumnOutpost.Change((int)row.Cells[MyHelper.strOutpostId].Value,
                                                    row.Cells[MyHelper.strOutpostName].Value.ToString(),
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateX].Value,
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateY].Value,
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value);
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

                        row.Cells[MyHelper.strOutpostId].Value = sCommand.ExecuteScalar();

                        _comboBoxColumnOutpost.Add((int)row.Cells[MyHelper.strOutpostId].Value,
                                                    row.Cells[MyHelper.strOutpostName].Value.ToString(),
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateX].Value,
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateY].Value,
                                                    (int)row.Cells[MyHelper.strOutpostCoordinateZ].Value);

                    }
                }
            }
            catch (Exception err2)
            {
                MessageBox.Show(err2.Message);
            }
        }

        private void dgvOutposts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context.ToString().Contains(DataGridViewDataErrorContexts.Parsing.ToString()))
            {
                //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
                //cell.Value = dgvOutposts[e.ColumnIndex, e.RowIndex].FormattedValue;
                //dgvOutposts.CancelEdit();
                e.Cancel = false;
                return;
            }
        }

        private void dgvOutposts_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strOutpostId].Value != null)
            {
                try
                {
                    var outpost_id = (int)e.Row.Cells[MyHelper.strOutpostId].Value;
                    if (dgvMissions.Rows.Cast<DataGridViewRow>().Select(row => row.Cells[MyHelper.strOutpostId].Value)
                                                                .FirstOrDefault(id => id != null && id != DBNull.Value && (int)id == outpost_id) != null)
                    {
                        MessageBox.Show(MyHelper.strOutpostIsUsing);
                        e.Cancel = true;
                        return;
                    }
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
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
    }
}
