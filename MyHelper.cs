using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DGVOutposts
{
    public static class MyHelper
    {
        public static readonly string strEmptyCell = "Вы ввели пустое значение!";
        public static readonly string strUncorrectIntValueCell = "Вы ввели некорректное число!";
        public static readonly string strExistingMision = "Для данного форпоста существует точная копия ведённой миссии!";
        //public static readonly string strUncorrectValueCell = "Некорректное значение!";
        public static readonly string strBadRow = "Плохая строка!";
        //public static readonly string strExistingOutpostRow = "Форпост уже существует!";
        public static readonly string strOutpostId = "outpost_id";
        public static readonly string strOutpostName = "outpost_name";
        public static readonly string strOutpostEconomicValue = "outpost_economic_value";
        public static readonly string strOutpostCoordinateX = "outpost_coordinate_x";
        public static readonly string strOutpostCoordinateY = "outpost_coordinate_y";
        public static readonly string strOutpostCoordinateZ = "outpost_coordinate_z";

        public static readonly string strMissionId = "mission_id";
        public static readonly string strMissionDescription = "mission_description";
        public static readonly string strDateBegin = "date_begin";
        public static readonly string strDatePlanEnd = "date_plan_end";
        public static readonly string strDateActualEnd = "date_actual_end";


        public static readonly string strUniqueOutpostConstraintName = "unique_outpost";

        public static string RmvExtrSpaces(this String str)
        {
            if (str == null) return str;
            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }

        public static bool IsEntireRowEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
                if (cell.FormattedValue.ToString().RmvExtrSpaces() != "")
                    return false;
            return true;
        }


    }
}
