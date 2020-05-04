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
