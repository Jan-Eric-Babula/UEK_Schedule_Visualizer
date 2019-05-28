using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UEK_VisualizeScheduleV2
{
    public partial class FormInfo : Form
    {
        private static FormInfo instance = null;

        public FormInfo()
        {
            InitializeComponent();
            this.label_test.Visible = false;
        }

        public void AdjustForm(string inp)
        {

            string[] lines = inp.Split(new char[] { '\n' });
            string max = "";
            foreach(string s in lines)
            {
                if(s.Length > max.Length)
                {
                    max = s;
                }
            }

            this.label_test.Text = max;
            this.Refresh();

            this.Width = this.label_test.Width + 36 + 20;
            this.richTextBox_info.Width = this.label_test.Width+20;

            this.richTextBox_info.Text = inp;
        }

        public static FormInfo GetInstance()
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
            FormInfo.GetInstance().Hide();
            FormInfo.GetInstance().Dispose();
            FormInfo.instance = null;
        }
    }
}
