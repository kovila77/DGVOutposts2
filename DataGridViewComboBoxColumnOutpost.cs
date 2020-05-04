using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DGVOutposts
{
    class DataGridViewComboBoxColumnOutpost : DataGridViewComboBoxColumn
    {
        private DataTable _dtOutposts = null;

        public DataGridViewComboBoxColumnOutpost() : base()
        {
            this.Name = "outpost_id";
            this.HeaderText = "Форпост";
            this.DisplayMember = "outpost_name";
            this.ValueMember = "outpost_id";
            this.DataPropertyName = "outpost_id";
            this.FlatStyle = FlatStyle.Flat;
            InitializeDataTableOutpost();
        }

        public void InitializeDataTableOutpost()
        {
            //if (_dtOutposts == null)
            //{
            _dtOutposts = new DataTable();
            _dtOutposts.Columns.Add("outpost_id", typeof(int));
            _dtOutposts.Columns.Add("outpost_name", typeof(string));
            //dtOutposts.Columns.Add("economic_value", typeof(int));
            //_dtOutposts.Columns.Add("outpost_coordinate_x", typeof(int));
            //_dtOutposts.Columns.Add("outpost_coordinate_y", typeof(int));
            //_dtOutposts.Columns.Add("outpost_coordinate_z", typeof(int));
            this.DataSource = _dtOutposts;
            //}
            //else
            //{
            //    //_dtOutposts.Dispose();
            //    _dtOutposts = null;
            //    InitializeDataTableOutpost();
            //}
        }

        public void Add(int outpost_id, string outpost_name, int outpost_coordinate_x, int outpost_coordinate_y, int outpost_coordinate_z)
        {
            //_dtOutposts.Rows.Add(outpost_id, outpost_name, outpost_coordinate_x, outpost_coordinate_y, outpost_coordinate_z);
            _dtOutposts.Rows.Add(outpost_id, outpost_name + " — "
                                             + outpost_coordinate_x.ToString() + ";"
                                             + outpost_coordinate_y.ToString() + ";"
                                             + outpost_coordinate_z.ToString());
        }

        public void Change(int outpost_id, string outpost_name, int outpost_coordinate_x, int outpost_coordinate_y, int outpost_coordinate_z)
        {
            DataRow forChange = _dtOutposts.AsEnumerable().SingleOrDefault(row => row.Field<int>("outpost_id") == outpost_id);
            if (forChange != null)
            {
                forChange["outpost_name"] = outpost_name + " — "
                                            + outpost_coordinate_x.ToString() + ";"
                                            + outpost_coordinate_y.ToString() + ";"
                                            + outpost_coordinate_z.ToString();
            }
        }

        public void Remove(int outpost_id)
        {
            DataRow forDel = _dtOutposts.AsEnumerable().SingleOrDefault(row => row.Field<int>("outpost_id") == outpost_id);
            if (forDel != null)
            {
                _dtOutposts.Rows.Remove(forDel);
            }
        }
    }
}
