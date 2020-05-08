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
            this.Name = MyHelper.strOutpostId;
            this.HeaderText = "Форпост";
            this.DisplayMember = MyHelper.strOutpostName;
            this.ValueMember = MyHelper.strOutpostId;
            this.DataPropertyName = MyHelper.strOutpostId;
            this.FlatStyle = FlatStyle.Flat;
            InitializeDataTableOutpost();
        }

        public void InitializeDataTableOutpost()
        {
            _dtOutposts = new DataTable();
            _dtOutposts.Columns.Add(MyHelper.strOutpostId, typeof(int));
            _dtOutposts.Columns.Add(MyHelper.strOutpostName, typeof(string));
            this.DataSource = _dtOutposts;
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
            DataRow forChange = _dtOutposts.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strOutpostId) == outpost_id);
            if (forChange != null)
            {
                forChange[MyHelper.strOutpostName] = outpost_name + " — "
                                            + outpost_coordinate_x.ToString() + ";"
                                            + outpost_coordinate_y.ToString() + ";"
                                            + outpost_coordinate_z.ToString();
            }
        }

        public void Remove(int outpost_id)
        {
            DataRow forDel = _dtOutposts.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strOutpostId) == outpost_id);
            if (forDel != null)
            {
                _dtOutposts.Rows.Remove(forDel);
            }
        }

        //public void Clear()
        //{
        //    _dtOutposts.Clear();
        //}
    }
}
