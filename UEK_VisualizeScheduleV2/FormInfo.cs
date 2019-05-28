using System;
using System.Windows.Forms;

namespace UEK_VisualizeScheduleV2
{
    public partial class FormInfo : Form
    {
        private static FormInfo instance = null;

        public FormInfo()
        {
            InitializeComponent();
        }

        public void AdjustForm(string input)
        {

            string[] inputLines = input.Split(new char[] { '\n' });
            string linesLongest = "";
            foreach(string line in inputLines)
            {
                if(line.Length > linesLongest.Length)
                {
                    linesLongest = line;
                }
            }

            this.label_test.Text = linesLongest;
            this.Refresh();

            this.Width = this.label_test.Width + 36 + 20;
            this.richTextBox_info.Width = this.label_test.Width+20;

            this.richTextBox_info.Text = input;
        }

        public static FormInfo Instance()
        {
            if (FormInfo.instance == null)
            {
                FormInfo.instance = new FormInfo();
            }
            FormInfo.instance.Show();
            FormInfo.instance.Focus();
            return FormInfo.instance;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            FormInfo.Instance().Hide();
            FormInfo.Instance().Dispose();
            FormInfo.instance = null;
        }
    }
}
