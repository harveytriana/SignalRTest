namespace FormsAppTest
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTime = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTime
            // 
            this.labelTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTime.Location = new System.Drawing.Point(28, 22);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(200, 40);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "00:00:00.0000";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValue
            // 
            this.labelValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelValue.Location = new System.Drawing.Point(28, 73);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(200, 40);
            this.labelValue.TabIndex = 0;
            this.labelValue.Text = "00:00:00.0000";
            this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 142);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelTime);
            this.Name = "MainForm";
            this.Text = "Forms App Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelValue;
    }
}

