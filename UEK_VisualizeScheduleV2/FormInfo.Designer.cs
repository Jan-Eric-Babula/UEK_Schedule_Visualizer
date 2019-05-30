namespace UEK_Schedule_Visualizer
{
    partial class FormInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_test = new System.Windows.Forms.Label();
            this.richTextBox_info = new System.Windows.Forms.RichTextBox();
            this.button_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_test
            // 
            this.label_test.AutoSize = true;
            this.label_test.Enabled = false;
            this.label_test.Location = new System.Drawing.Point(12, 9);
            this.label_test.Name = "label_test";
            this.label_test.Size = new System.Drawing.Size(100, 13);
            this.label_test.TabIndex = 0;
            this.label_test.Text = "<Width Test Label>";
            this.label_test.Visible = false;
            // 
            // richTextBox_info
            // 
            this.richTextBox_info.Location = new System.Drawing.Point(15, 9);
            this.richTextBox_info.Name = "richTextBox_info";
            this.richTextBox_info.ReadOnly = true;
            this.richTextBox_info.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_info.Size = new System.Drawing.Size(97, 103);
            this.richTextBox_info.TabIndex = 1;
            this.richTextBox_info.Text = "<Date>\n\n<1>\n<2>\n<3>\n<4>";
            // 
            // button_close
            // 
            this.button_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_close.Location = new System.Drawing.Point(15, 118);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 2;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // FormInfo
            // 
            this.AcceptButton = this.button_close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_close;
            this.ClientSize = new System.Drawing.Size(120, 155);
            this.ControlBox = false;
            this.Controls.Add(this.richTextBox_info);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.label_test);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInfo";
            this.ShowIcon = false;
            this.Text = "Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_test;
        private System.Windows.Forms.RichTextBox richTextBox_info;
        private System.Windows.Forms.Button button_close;
    }
}