using System;
using System.Windows.Forms;

namespace UEK_VisualizeScheduleV2
{
    public partial class FormInfo : Form
    {
        #region Instances

        /// <summary>
        /// Holds main instance of <c>FormInfo</c> class.
        /// </summary>
        private static FormInfo instance = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Auto generated.
        /// Initialized GUI components.
        /// </summary>
        public FormInfo()
        {
            InitializeComponent();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Get main instance of <c>FormInfo</c>.
        /// If no instance exsists one is created.
        /// The instance is shown and focussed on.
        /// </summary>
        /// <returns>Main instance of <c>FormInfo</c>.</returns>
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

        #region Methods

        /// <summary>
        /// Resizes the width of the form according to the length of the given string.
        /// </summary>
        /// <param name="input">String to resize form width.</param>
        public void AdjustForm(string input)
        {

            string[] inputLines = input.Split(new char[] { '\n' });
            string linesLongest = "";
            foreach (string line in inputLines)
            {
                if (line.Length > linesLongest.Length)
                {
                    linesLongest = line;
                }
            }

            this.label_test.Text = linesLongest;
            this.Refresh();

            //Width = String width + Padding + Form Border
            this.Width = this.label_test.Width + 36 + 20;
            this.richTextBox_info.Width = this.label_test.Width + 20;

            this.richTextBox_info.Text = input;
        }

        #region Event Handlers

        /// <summary>
        /// Event Handler of <c>button_close</c> for the <c>Click</c> event.
        /// Disposes of current form instance.
        /// </summary>
        /// <param name="sender">Event Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void button_close_Click(object sender, EventArgs e)
        {
            FormInfo.Instance().Hide();
            FormInfo.Instance().Dispose();
            FormInfo.instance = null;
        }

        #endregion

        #endregion

        #endregion

    }
}
