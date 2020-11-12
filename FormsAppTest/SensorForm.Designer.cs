namespace FormsAppTest
{
    partial class SensorForm
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
            this.labelTime.BackColor = System.Drawing.Color.Salmon;
            this.labelTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTime.ForeColor = System.Drawing.Color.LavenderBlush;
            this.labelTime.Location = new System.Drawing.Point(28, 22);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(200, 40);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Sensor Time";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValue
            // 
            this.labelValue.BackColor = System.Drawing.Color.DarkSalmon;
            this.labelValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelValue.ForeColor = System.Drawing.Color.LavenderBlush;
            this.labelValue.Location = new System.Drawing.Point(28, 64);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(200, 40);
            this.labelValue.TabIndex = 0;
            this.labelValue.Text = "Sensor Value";
            this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 133);
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

