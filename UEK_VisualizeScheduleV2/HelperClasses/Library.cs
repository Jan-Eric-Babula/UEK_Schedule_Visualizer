using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace UEK_VisualizeScheduleV2
{
    /// <summary>
    /// Holds constants, instances and helper functions.
    /// </summary>
    class Library
    {
        #region Constants
        
        /// <summary>
        /// Path and name of program config file.
        /// </summary>
        public static readonly string CONFIG_PATH = "uekvconfig.json";

        /// <summary>
        /// Path and name of program data file.
        /// </summary>
        public static readonly string DATA_PATH = "data.json";

        /// <summary>
        /// URL to request data from webserver.
        /// </summary>
        public static readonly string DATA_URL = "https://e-uczelnia.ue.katowice.pl/wsrest/rest/phz/harmonogram/zajecia?_dc=1552301604786&idGrupa=41200&idNauczyciel=0&idJednostkaPanelJednostka=0&dataOd=&dataDo=&widok=STUDENT&authUzytkownikId=0&page=1&start=0&limit=1000";

        #endregion

        #region Instances

        /// <summary>
        /// A list to hold all instances of <c>CourseMain</c>
        /// </summary>
        public static List<CourseMain> ALL_COURSE_MAIN = new List<CourseMain>();

        /// <summary>
        /// A list to hold all instances of <c>CourseEvent</c>
        /// </summary>
        public static List<CourseEvent> ALL_COURSE_EVENT = new List<CourseEvent>();

        /// <summary>
        /// A list to hold all instances of <c>LectureType</c>
        /// </summary>
        public static List<LectureType> ALL_LECTURE_TYPE = new List<LectureType>();

        /// <summary>
        /// A dictionary to hold the abbreviations for all insatnces of <c>CourseMain</c>
        /// </summary>
        public static Dictionary<CourseMain, string> COURSE_MAIN_ABBRV = new Dictionary<CourseMain, string>();

        /// <summary>
        /// A dictionary to hold all instances of <c>CourseMain</c> in relation to their subject names.
        /// </summary>
        public static Dictionary<string, CourseMain> COURSE_MAIN_SUBJ = new Dictionary<string, CourseMain>();

        /// <summary>
        /// Currently selected and active year in the calendar view.
        /// </summary>
        public static int SELECTED_YEAR;

        /// <summary>
        /// Currently selected and active month in the calendar view.
        /// </summary>
        public static int SELECTED_MONTH;

        /// <summary>
        /// Lower boundary for month and year selection. 
        /// </summary>
        public static DateTime SELECTED_MIN;

        /// <summary>
        /// Upper boundary for month and year selection.
        /// </summary>
        public static DateTime SELECTED_MAX;

        /// <summary>
        /// A dictionary holding all instances of <c>RichTextBox</c> in relation to their column and row as displayed in the calendar view.
        /// </summary>
        public static Dictionary<int, Dictionary<int, RichTextBox>> ALL_CALENDAR_PANE = new Dictionary<int, Dictionary<int, RichTextBox>>();

        /// <summary>
        /// A summary of the subjects selected and their abbreviations.
        /// </summary>
        public static string ABBRV_INFORMATION;

        #endregion

        #region Functions

        /// <summary>
        /// Format a DateTime.
        /// </summary>
        /// <param name="dt">DateTime to format.</param>
        /// <returns>'yyyy-MM-dd'</returns>
        public static string FormatDTDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Format a DateTime.
        /// </summary>
        /// <param name="dt">DateTime to format.</param>
        /// <returns>'HH:mm'</returns>
        public static string FormatDTTime(DateTime dt)
        {
            return dt.ToString("HH:mm");
        }

        /// <summary>
        /// Takes a <c>CourseJSON</c> instance and serializes it.
        /// Necessary instances of <c>LectureType</c>, <c>CourseMain</c> and <c>CourseEvent</c> are created.
        /// Duplicates of exsisting instances are ignored.
        /// All new instances are added to their respective list holding all instances.
        /// </summary>
        /// <param name="c">An instance of <c>CourseJSON</c> to deserialize.</param>
        public static void SerializeCourseData(CourseJSON c)
        {
            //Serialize LectureType
            LectureType lt = new LectureType(Library.SplitRawCourse(c.GetSubject())[1]);
            //Check for duplicate, add if new
            if (!ALL_LECTURE_TYPE.Contains(lt))
            {
                ALL_LECTURE_TYPE.Add(lt);
            }
            //Retrieve fitting LectureType instance
            lt = ALL_LECTURE_TYPE[ALL_LECTURE_TYPE.IndexOf(lt)];

            //Serialize CourseMain
            CourseMain cm = new CourseMain(Library.SplitRawCourse(c.GetSubject())[0]);
            //Check for duplicate, add if new
            if (!ALL_COURSE_MAIN.Contains(cm))
            {
                ALL_COURSE_MAIN.Add(cm);
                COURSE_MAIN_SUBJ.Add(cm.Subject, cm);
            }
            //Retrieve fitting CourseMain instance
            cm = ALL_COURSE_MAIN[ALL_COURSE_MAIN.IndexOf(cm)];

            //Serialize CourseEvent
            CourseEvent ce = new CourseEvent(c.GetDate(), c.GetStart(), c.GetEnd(), c.GetRoom(), c.GetBuilding(), c.GetTeacher(), cm, lt);
            //Check for duplicate, add if new
            if (!ALL_COURSE_EVENT.Contains(ce))
            {
                ALL_COURSE_EVENT.Add(ce);
            }
        }

        /// <summary>
        /// Creates a hash from a given string.
        /// </summary>
        /// <param name="inputString">String to hash.</param>
        /// <returns>Hash of string in bytes.</returns>
        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Creates a hash from a given string.
        /// </summary>
        /// <param name="inputString">String ti hash.</param>
        /// <returns>Hash of string as string.</returns>
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Splits the given raw course name to seperate the subject and type of the course.
        /// </summary>
        /// <param name="fullCourseName">Raw course name.</param>
        /// <returns>Seperate and trimmed parts of the raw name.</returns>
        public static string[] SplitRawCourse(string fullCourseName) { 
            string[] a = fullCourseName.Split(new char[] { '-' });
            string ret_a, ret_b;
            ret_b = a[a.Length - 1];

            if (a.Length > 2)
            {
                ret_a = "";
                for (int i = 0; i < a.Length - 1; i++)
                {
                    ret_a += $"{(i == 0 ? "" : "-")}{a[i]}";
                }
            }
            else
            {
                ret_a = a[0];
            }
            return new string[] { ret_a, ret_b };
        }

        /// <summary>
        /// Generates a not yet used abbreviation of a given subject.
        /// </summary>
        /// <param name="cm">Subject to create abbreviation for.</param>
        /// <returns>If an abbreviation could be added, failes if subject already has abbreviation.</returns>
        public static bool GenerateCourseMainAbbrv(CourseMain cm)
        {
            if (!Library.COURSE_MAIN_ABBRV.Keys.Contains(cm))
            {
                string[] cmWords = cm.Subject.Split(new char[] { ' ' });
                string sol;
                for (int i = -1; i < cmWords.Length; i++)
                {
                    sol = "";
                    for (int j = 0; j < cmWords.Length; j++)
                    {
                        sol += cmWords[j][0];
                        if (i >= j)
                        {
                            sol += cmWords[j][1];
                        }
                    }
                    if (!Library.COURSE_MAIN_ABBRV.Values.Contains(sol))
                    {
                        COURSE_MAIN_ABBRV.Add(cm, sol);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Converts <c>DayOfWeek</c> instance to number representing the position of the day in a week.
        /// </summary>
        /// <param name="day"><c>DayOfWeek</c> instance</param>
        /// <returns>Position of day in a week, starts with Monday as 0.</returns>
        public static int CDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                case DayOfWeek.Sunday: return 6;
                default: return 0;
            }
        }

        #endregion
    }
}
