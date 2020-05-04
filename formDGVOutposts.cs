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
        private readonly string strEmptyCell = "Вы ничего не ввели!";
        private readonly string strOnlyIntCell = "Вы ввели некорректное число!";
        //private readonly string strUncorrectValueCell = "Некорректное значение!";
        private readonly string strBadRow = "Плохая строка!";
        //private readonly string strExistingOutpostRow = "Форпост уже существует!";
        private readonly string strOutpostId = "outpost_id";
        private readonly string strOutpostName = "outpost_name";
        private readonly string strOutpostEconomicValue = "outpost_economic_value";
        private readonly string strOutpostCoordinateX = "outpost_coordinate_x";
        private readonly string strOutpostCoordinateY = "outpost_coordinate_y";
        private readonly string strOutpostCoordinateZ = "outpost_coordinate_z";

        private readonly string strMissionId = "mission_id";
        private readonly string strMissionDescription = "mission_description";
        private readonly string strDateBegin = "date_begin";
        private readonly string strDatePlanEnd = "date_plan_end";
        private readonly string strDateActualEnd = "date_actual_end";
        
        //private bool rowCanBeCommitedDGVOutpost;

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
    }
}
