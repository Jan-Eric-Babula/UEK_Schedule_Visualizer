using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UEK_Schedule_Visualizer_ClassLibrary.Forms
{
    class Helper
    {

        public static DateTime SELECTED_MIN;
        public static DateTime SELECTED_MAX;

        public static int SELECTED_YEAR = DateTime.Now.Year;
        public static int SELECTED_MONTH = DateTime.Now.Month;

        public static Dictionary<int, Dictionary<int, RichTextBox>> CALENDAR_PANES;
        public static string ABBREVIATION_INFORMATION;

    }
}
