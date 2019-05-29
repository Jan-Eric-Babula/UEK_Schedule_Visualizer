using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;

namespace UEK_VisualizeScheduleV2
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            Library.SELECTED_MONTH = DateTime.Now.Month;
            Library.SELECTED_YEAR = DateTime.Now.Year;

            this.LoadConfigJSON();
            this.SaveConfigJSON();
            
            InitializeComponent();
            
            RegisterCalendarPanes();
            ClearCalendarPanes();

            if (File.Exists(Library.DATA_PATH))
            {
                List<CourseJSON> lcjson = this.LoadRawCourseData();
                if (lcjson == null)
                {
                    this.UpdateData(false);
                }
                else
                {
                    this.SerializeRawData(lcjson);
                    this.GenerateCourseAbbreviations();
                    this.RefreshProfileList();
                    this.FindBoundries();
                    this.CheckBoundries();    
                }
                
            }
        }

        
        private void ClearCalendarPanes()
        {
            foreach(int r in Library.ALL_CALENDAR_PANE.Keys)
            {
                foreach(int c in Library.ALL_CALENDAR_PANE[r].Keys)
                {
                    Library.ALL_CALENDAR_PANE[r][c].Text = "";
                    Library.ALL_CALENDAR_PANE[r][c].BackColor = Control.DefaultBackColor;
                    Library.ALL_CALENDAR_PANE[r][c].Visible = true;
                    Library.ALL_CALENDAR_PANE[r][c].Enabled = true;
                    Library.ALL_CALENDAR_PANE[r][c].ReadOnly = true;
                }
            }
        }

        private void RefreshHeaderBox()
        {
            string content = $"Today: {Library.FormatDTDate(DateTime.Now)}\nSelected:{System.Globalization.CultureInfo.GetCultureInfo("en").DateTimeFormat.GetMonthName(Library.SELECTED_MONTH)} {Library.SELECTED_YEAR}";
            this.richTextBox_Header.Text = content;
            this.richTextBox_Header.SelectAll();
            this.richTextBox_Header.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void FindBoundries()
        {
            DateTime min, max;
            min = Library.ALL_COURSE_EVENT[0].Date; max = Library.ALL_COURSE_EVENT[0].Date;
            foreach(CourseEvent ce in Library.ALL_COURSE_EVENT)
            {
                if(ce.Date < min)
                {
                    min = ce.Date;
                }
                if(ce.Date > max)
                {
                    max = ce.Date;
                }
            }
            Library.SELECTED_MIN = min;
            Library.SELECTED_MAX = max;
        }

        private void CheckBoundries()
        {
            if((Library.SELECTED_YEAR < Library.SELECTED_MIN.Year) || ((Library.SELECTED_YEAR >= Library.SELECTED_MIN.Year) && (Library.SELECTED_MONTH < Library.SELECTED_MIN.Month)))
            {
                Library.SELECTED_MONTH = Library.SELECTED_MIN.Month;
                Library.SELECTED_YEAR = Library.SELECTED_MIN.Year;
            }
            if((Library.SELECTED_YEAR > Library.SELECTED_MAX.Year) || ( (Library.SELECTED_YEAR <= Library.SELECTED_MAX.Year) && (Library.SELECTED_MONTH > Library.SELECTED_MAX.Month)))
            {
                Library.SELECTED_MONTH = Library.SELECTED_MAX.Month;
                Library.SELECTED_YEAR = Library.SELECTED_MAX.Year;
            }
        }

        #region ElementEvents

        private void tabPage_Schedule_Enter(object sender, EventArgs e)
        {
            this.RefreshAbbreviationInfo();
            this.FindBoundries();
            this.CheckBoundries();
            this.RefreshCalendar();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.LoadConfigJSON();
            this.RefreshUpdateLU();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveConfigJSON();
        }

        private void button_abbrInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Library.ABBRV_INFORMATION);
        }

        private void button_leftleft_Click(object sender, EventArgs e)
        {
            Library.SELECTED_YEAR = 0;
            this.CheckBoundries();
            this.RefreshCalendar();
        }

        private void button_rightright_Click(object sender, EventArgs e)
        {
            Library.SELECTED_YEAR = 9999;
            this.CheckBoundries();
            this.RefreshCalendar();
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            Library.SELECTED_MONTH--;
            if(Library.SELECTED_MONTH < 1)
            {
                Library.SELECTED_MONTH = 12;
                Library.SELECTED_YEAR--;
            }
            this.CheckBoundries();
            this.RefreshCalendar();
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            Library.SELECTED_MONTH++;
            if(Library.SELECTED_MONTH > 12)
            {
                Library.SELECTED_MONTH = 1;
                Library.SELECTED_YEAR++;
            }
            this.CheckBoundries();
            this.RefreshCalendar();
        }

        private void calendar_double_click(object sender, MouseEventArgs e)
        {
            RichTextBox rtb = (RichTextBox)sender;

            rtb.Select(0, 2);
            DateTime dt = new DateTime(Library.SELECTED_YEAR, Library.SELECTED_MONTH, int.Parse(rtb.SelectedText));

            List<CourseEvent> cfd = GetCoursesForDay(dt);
            cfd.Sort();

            string info = $"{dt.ToString("yyyy-MM-dd")}\n";
            foreach(CourseEvent c in cfd)
            {
                info += $"\n{c.StartS} to {c.EndS}; at {c.Location}: {c.Course.Subject} ({c.Type.Name} by {c.Teacher})";
            }

            FormInfo.Instance().AdjustForm(info);

        }

        private void button_update_Click(object sender, EventArgs e)
        {
            this.UpdateData();
        }

        private void tabControlMain_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if(this.tabControlMain.SelectedTab.Name == "tabPage_Update")
            {
                if(this.GetConfigJSON().LastUpdatedRaw().Ticks == 0)
                {
                    
                    MessageBox.Show("Warning!\nThe data has to be updated at least once before the profile can be set or the calendar viewed!\n\nPlease refresh your data!");
                    e.Cancel = true;
                }
                else if (!File.Exists(Library.DATA_PATH))
                {
                    
                    MessageBox.Show("Warning!\nNo data file was found!\n\nPlease update your data now to use the tool!");
                    e.Cancel = true;
                }
                else
                {
                    this.LoadConfigCourseSelection();
                }
            }
        }

        #endregion

        #region Config

        private void LoadConfigJSON() {
            if (!File.Exists(Library.CONFIG_PATH))
            {
                this.SaveConfigJSON();
            }
            string confRaw = File.ReadAllText(Library.CONFIG_PATH, Encoding.UTF8);
            UEKVConfig c = JsonConvert.DeserializeObject<UEKVConfig>(confRaw);
            UEKVConfig.InitiateInstance(c);
        }

        private UEKVConfig GetConfigJSON() {
            return UEKVConfig.GetInstance();
        }

        private void SaveConfigJSON()
        {
            if (File.Exists(Library.CONFIG_PATH)) { File.Delete(Library.CONFIG_PATH); }
            string confRaw = JsonConvert.SerializeObject(this.GetConfigJSON(), Formatting.Indented);
            File.WriteAllText(Library.CONFIG_PATH, confRaw, Encoding.UTF8);
        }

        #endregion

        #region Update

        private void UpdateData(bool slow = true) {
            this.Enabled = false;

            try
            {
                this.progressBar_Update.Value = 0;
                this.progressBar_Update.PerformStep();

                this.label_UpdateStatus.Text = "Downloading data...";this.Refresh();
                this.DownloadRawData();
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);

                this.label_UpdateStatus.Text = "Loading data..."; this.Refresh();
                List<CourseJSON> lcjson = this.LoadRawCourseData();
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);

                this.label_UpdateStatus.Text = "Processing data (1)..."; this.Refresh();
                this.SerializeRawData(lcjson);
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);

                this.label_UpdateStatus.Text = "Processing data (2)..."; this.Refresh();
                this.GenerateCourseAbbreviations();
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);

                this.label_UpdateStatus.Text = "Visualizing data..."; this.Refresh();
                this.RefreshProfileList();
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);


                this.label_UpdateStatus.Text = "Finalizing..."; this.Refresh();
                this.GetConfigJSON().LastUpdated = Library.FormatDTDate(DateTime.Now);
                this.FindBoundries();
                this.CheckBoundries();
                this.progressBar_Update.PerformStep();
                Thread.Sleep(slow ? 1000 : 0);

                this.label_UpdateStatus.Text = "Finished Updating!"; this.Refresh();

            }
            catch(Exception e)
            {
                MessageBox.Show($"An error occured during the update!\n\nMessage: {e.Message}\n\nUpdate incomplete!");
                this.progressBar_Update.Value = 0;
            }
            finally
            {
                this.RefreshUpdateLU();
            }

            this.Enabled = true;
        }

        private void DownloadRawData() {
            File.Delete(Library.DATA_PATH);
            using(WebClient c = new WebClient())
            {
                c.DownloadFile(Library.DATA_URL, Library.DATA_PATH);
            }
        }

        private List<CourseJSON> LoadRawCourseData()
        {
            List<CourseJSON> ret = new List<CourseJSON>();
            try
            {
                string dataRawString = File.ReadAllText(Library.DATA_PATH, Encoding.UTF8);
                JObject dataRaw = JObject.Parse(dataRawString);
                IList<JToken> entries = dataRaw["result"].Children().ToList();
                CourseJSON c;
                foreach (JToken entry in entries)
                {
                    c = entry.ToObject<CourseJSON>();
                    ret.Add(c);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Warning!\nAn error occured during data retrieval!\n\nPlease try again later.\nIf it still does not work contact the Administrator!");
                ret = null;
            }
            return ret;
        }

        private void SerializeRawData(List<CourseJSON> courseJSONs)
        {
            foreach(CourseJSON cjson in courseJSONs)
            {
                Library.SerializeCourseData(cjson);
            }
        }

        private void GenerateCourseAbbreviations()
        {
            Library.COURSE_MAIN_ABBRV = new Dictionary<CourseMain, string>();
            foreach (CourseMain cm in Library.ALL_COURSE_MAIN)
            {
                Library.GenerateCourseMainAbbrv(cm);
            }
        }

        private void RefreshProfileList()
        {
            this.checkedListBox_Profile.Items.Clear();
            Library.ALL_COURSE_MAIN.Sort();
            foreach(CourseMain cm in Library.ALL_COURSE_MAIN)
            {
                string text = $"{cm.Subject} ({Library.COURSE_MAIN_ABBRV[cm]})";
                this.checkedListBox_Profile.Items.Add(text);
                if (this.GetConfigJSON().Subjects.Contains(cm.Subject))
                {
                    this.checkedListBox_Profile.SetItemChecked(this.checkedListBox_Profile.Items.IndexOf(text), true);
                }
            }
        }

        private void RefreshUpdateLU()
        {
            this.label_UpdateStatus.Text = "";
            if (this.GetConfigJSON().LastUpdatedRaw().Ticks == 0)
            {
                this.label_LastUpdated.Text = "Never";
            }
            else
            {
                this.label_LastUpdated.Text = this.GetConfigJSON().LastUpdated;
            }
        }

        #endregion

        #region Update Action
        #endregion

        #region Profile

        private void LoadConfigCourseSelection()
        {
            foreach(string s in UEKVConfig.GetInstance().Subjects)
            {
                string subjectText = $"{s} ({Library.COURSE_MAIN_ABBRV[Library.COURSE_MAIN_SUBJ[s]]})";
                int loc = this.checkedListBox_Profile.Items.IndexOf(subjectText);
                this.checkedListBox_Profile.SetItemChecked(loc, true);
            }
        }

        #endregion

        #region Profile Action

        private void checkedListBox_Profile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string subj = (((string)this.checkedListBox_Profile.Items[e.Index]).Split(new char[] {'(' }))[0].Trim();
            if (e.NewValue == CheckState.Checked)
            {
                if (!UEKVConfig.GetInstance().Subjects.Contains(subj))
                {
                    UEKVConfig.GetInstance().Subjects.Add(subj);
                }
            }
            else
            {
                if (UEKVConfig.GetInstance().Subjects.Contains(subj))
                {
                    UEKVConfig.GetInstance().Subjects.Remove(subj);
                }
            }
        }

        #endregion

        #region Calendar

        private void RegisterCalendarPanes()
        {
            Library.ALL_CALENDAR_PANE.Add(0, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[0].Add(0, this.richTextBox_0_0);
            Library.ALL_CALENDAR_PANE[0].Add(1, this.richTextBox_0_1);
            Library.ALL_CALENDAR_PANE[0].Add(2, this.richTextBox_0_2);
            Library.ALL_CALENDAR_PANE[0].Add(3, this.richTextBox_0_3);
            Library.ALL_CALENDAR_PANE[0].Add(4, this.richTextBox_0_4);
            Library.ALL_CALENDAR_PANE[0].Add(5, this.richTextBox_0_5);
            Library.ALL_CALENDAR_PANE[0].Add(6, this.richTextBox_0_6);

            Library.ALL_CALENDAR_PANE.Add(1, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[1].Add(0, this.richTextBox_1_0);
            Library.ALL_CALENDAR_PANE[1].Add(1, this.richTextBox_1_1);
            Library.ALL_CALENDAR_PANE[1].Add(2, this.richTextBox_1_2);
            Library.ALL_CALENDAR_PANE[1].Add(3, this.richTextBox_1_3);
            Library.ALL_CALENDAR_PANE[1].Add(4, this.richTextBox_1_4);
            Library.ALL_CALENDAR_PANE[1].Add(5, this.richTextBox_1_5);
            Library.ALL_CALENDAR_PANE[1].Add(6, this.richTextBox_1_6);

            Library.ALL_CALENDAR_PANE.Add(2, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[2].Add(0, this.richTextBox_2_0);
            Library.ALL_CALENDAR_PANE[2].Add(1, this.richTextBox_2_1);
            Library.ALL_CALENDAR_PANE[2].Add(2, this.richTextBox_2_2);
            Library.ALL_CALENDAR_PANE[2].Add(3, this.richTextBox_2_3);
            Library.ALL_CALENDAR_PANE[2].Add(4, this.richTextBox_2_4);
            Library.ALL_CALENDAR_PANE[2].Add(5, this.richTextBox_2_5);
            Library.ALL_CALENDAR_PANE[2].Add(6, this.richTextBox_2_6);

            Library.ALL_CALENDAR_PANE.Add(3, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[3].Add(0, this.richTextBox_3_0);
            Library.ALL_CALENDAR_PANE[3].Add(1, this.richTextBox_3_1);
            Library.ALL_CALENDAR_PANE[3].Add(2, this.richTextBox_3_2);
            Library.ALL_CALENDAR_PANE[3].Add(3, this.richTextBox_3_3);
            Library.ALL_CALENDAR_PANE[3].Add(4, this.richTextBox_3_4);
            Library.ALL_CALENDAR_PANE[3].Add(5, this.richTextBox_3_5);
            Library.ALL_CALENDAR_PANE[3].Add(6, this.richTextBox_3_6);

            Library.ALL_CALENDAR_PANE.Add(4, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[4].Add(0, this.richTextBox_4_0);
            Library.ALL_CALENDAR_PANE[4].Add(1, this.richTextBox_4_1);
            Library.ALL_CALENDAR_PANE[4].Add(2, this.richTextBox_4_2);
            Library.ALL_CALENDAR_PANE[4].Add(3, this.richTextBox_4_3);
            Library.ALL_CALENDAR_PANE[4].Add(4, this.richTextBox_4_4);
            Library.ALL_CALENDAR_PANE[4].Add(5, this.richTextBox_4_5);
            Library.ALL_CALENDAR_PANE[4].Add(6, this.richTextBox_4_6);

            Library.ALL_CALENDAR_PANE.Add(5, new Dictionary<int, RichTextBox>());
            Library.ALL_CALENDAR_PANE[5].Add(0, this.richTextBox_5_0);
            Library.ALL_CALENDAR_PANE[5].Add(1, this.richTextBox_5_1);
            Library.ALL_CALENDAR_PANE[5].Add(2, this.richTextBox_5_2);
            Library.ALL_CALENDAR_PANE[5].Add(3, this.richTextBox_5_3);
            Library.ALL_CALENDAR_PANE[5].Add(4, this.richTextBox_5_4);
            Library.ALL_CALENDAR_PANE[5].Add(5, this.richTextBox_5_5);
            Library.ALL_CALENDAR_PANE[5].Add(6, this.richTextBox_5_6);
        }

        private void RefreshCalendar()
        {
            this.ClearCalendarPanes();
            this.RefreshHeaderBox();

            DateTime act = new DateTime(Library.SELECTED_YEAR, Library.SELECTED_MONTH, 1);
            List<CourseEvent> coursesForDay = new List<CourseEvent>();

            int row = 0;
            int col = CDayOfWeek(act.DayOfWeek);
            bool conflict;

            for(int i = 0; i < col; i++)
            {
                Library.ALL_CALENDAR_PANE[0][i].Visible = false;
                Library.ALL_CALENDAR_PANE[0][i].Enabled = false;
            }

            while( (act.Year == Library.SELECTED_YEAR) && (act.Month == Library.SELECTED_MONTH))
            {
                if(col > 6)
                {
                    col = 0;
                    row++;
                }
                coursesForDay = this.GetCoursesForDay(act);
                coursesForDay.Sort();
                conflict = this.IsDailyConflict(coursesForDay);

                if (conflict)
                {
                    Library.ALL_CALENDAR_PANE[row][col].BackColor = Color.Red;
                }

                string cont = $"{act.ToString("dd")}";

                foreach(CourseEvent ce in coursesForDay)
                {
                    cont += $"\n{ce.StartS}-{ce.EndS} {Library.COURSE_MAIN_ABBRV[ce.Course]}";
                }
                Library.ALL_CALENDAR_PANE[row][col].Text = cont;

                if (act == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                {
                    Library.ALL_CALENDAR_PANE[row][col].Select(0, 2);
                    Library.ALL_CALENDAR_PANE[row][col].SelectionFont = new Font(Library.ALL_CALENDAR_PANE[row][col].Font, FontStyle.Bold);
                }

                col++;
                act = act + (new TimeSpan(1, 0, 0, 0));
            }

            int p = 5;
            int k = 6;
            do
            {

                Library.ALL_CALENDAR_PANE[p][k].Visible = false;
                Library.ALL_CALENDAR_PANE[p][k].Enabled = false;

                k--;
                if (k < 0)
                {
                    k = 6;
                    p--;
                }

            } while (Library.ALL_CALENDAR_PANE[p][k].Text == "");
        }

        private void RefreshAbbreviationInfo()
        {
            Library.ABBRV_INFORMATION = "Abbreviations:";
            List<string> _tmp = UEKVConfig.GetInstance().Subjects;
            _tmp.Sort();

            foreach(string s in _tmp)
            {
                Library.ABBRV_INFORMATION += $"\n{s} = {Library.COURSE_MAIN_ABBRV[Library.COURSE_MAIN_SUBJ[s]]}";
            }
            
        }

        private bool IsDailyConflict(List<CourseEvent> cList)
        {
            int confCount = 0;

            bool a;
            bool b;
            bool c;
            bool d;

            foreach (CourseEvent c1 in cList)
            {
                foreach (CourseEvent c2 in cList)
                {
                    if (c1 != c2)
                    {
                        a = c2.Start >= c1.Start && c2.Start <= c1.End;
                        b = c1.End >= c2.Start && c1.End <= c2.End;
                        c = c1.Start >= c2.Start && c1.Start <= c2.End;
                        d = c2.End >= c1.Start && c2.End <= c1.End;

                        if (a || b || c || d)
                        {
                            confCount++;
                            break;
                        }
                    }
                }
                if (confCount > 0) { break; }
            }

            return confCount > 0;
        }

          private List<CourseEvent> GetCoursesForDay(DateTime dt)
        {
            List<CourseEvent> ret = new List<CourseEvent>();
            DateTime comp = new DateTime(dt.Year, dt.Month, dt.Day);
            List<CourseMain> relevantCourses = new List<CourseMain>();
            foreach(string s in UEKVConfig.GetInstance().Subjects)
            {
                relevantCourses.Add(Library.COURSE_MAIN_SUBJ[s]);
            }

            foreach(CourseEvent ce in Library.ALL_COURSE_EVENT)
            {
                if(ce.Date == comp && relevantCourses.Contains(ce.Course))
                {
                    ret.Add(ce);
                }
            }
            return ret;
        }
          private int CDayOfWeek(DayOfWeek day)
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
                default: return 1;
            }
        }

        #endregion
        
    }
}
