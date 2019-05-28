using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace UEK_VisualizeScheduleV2
{
    class Library
    {
        #region Constants
        public static readonly string CONFIG_PATH = "uekvconfig.json";
        public static readonly string DATA_PATH = "data.json";
        public static readonly string DATA_URL = "https://e-uczelnia.ue.katowice.pl/wsrest/rest/phz/harmonogram/zajecia?_dc=1552301604786&idGrupa=41200&idNauczyciel=0&idJednostkaPanelJednostka=0&dataOd=&dataDo=&widok=STUDENT&authUzytkownikId=0&page=1&start=0&limit=1000";
        #endregion

        #region Instances
        public static List<CourseMain> ALL_COURSE_MAIN = new List<CourseMain>();
        public static List<CourseEvent> ALL_COURSE_EVENT = new List<CourseEvent>();
        public static List<LectureType> ALL_LECTURE_TYPE = new List<LectureType>();
        public static Dictionary<CourseMain, string> COURSE_MAIN_ABBRV = new Dictionary<CourseMain, string>();
        public static Dictionary<string, CourseMain> COURSE_MAIN_SUBJ = new Dictionary<string, CourseMain>();
        public static int SELECTED_YEAR;
        public static int SELECTED_MONTH;
        public static DateTime SELECTED_MIN;
        public static DateTime SELECTED_MAX;
        public static Dictionary<int, Dictionary<int, RichTextBox>> ALL_CALENDAR_PANE = new Dictionary<int, Dictionary<int, RichTextBox>>();
        public static string ABBRV_INFORMATION;
        #endregion

        #region Helpers
        public static string FormatDTDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string FormatDTTime(DateTime dt)
        {
            return dt.ToString("HH:mm");
        }

        public static void SerializeCourseData(CourseJSON c)
        {
            LectureType lt = new LectureType(Library.SplitRawCourse(c.GetSubject())[1]);
            if (!ALL_LECTURE_TYPE.Contains(lt))
            {
                ALL_LECTURE_TYPE.Add(lt);
            }
            lt = ALL_LECTURE_TYPE[ALL_LECTURE_TYPE.IndexOf(lt)];

            CourseMain cm = new CourseMain(Library.SplitRawCourse(c.GetSubject())[0]);
            if (!ALL_COURSE_MAIN.Contains(cm))
            {
                ALL_COURSE_MAIN.Add(cm);
                COURSE_MAIN_SUBJ.Add(cm.Subject, cm);
            }
            cm = ALL_COURSE_MAIN[ALL_COURSE_MAIN.IndexOf(cm)];

            CourseEvent ce = new CourseEvent(c.GetDate(), c.GetStart(), c.GetEnd(), c.GetRoom(), c.GetBuilding(), c.GetTeacher(), cm, lt);
            if (!ALL_COURSE_EVENT.Contains(ce))
            {
                ALL_COURSE_EVENT.Add(ce);
            }
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

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
