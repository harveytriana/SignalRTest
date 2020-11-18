
namespace FormsAppTest
{
    partial class MainForm
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
            if (disposing && (components != null)) {
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
            this.buttonSensor = new System.Windows.Forms.Button();
            this.buttonWeatherReport = new System.Windows.Forms.Button();
            this.buttonStreamingExample = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSensor
            // 
            this.buttonSensor.Location = new System.Drawing.Point(24, 37);
            this.buttonSensor.Name = "buttonSensor";
            this.buttonSensor.Size = new System.Drawing.Size(304, 23);
            this.buttonSensor.TabIndex = 0;
            this.buttonSensor.Text = "Sensor Client";
            this.buttonSensor.UseVisualStyleBackColor = true;
            // 
            // buttonWeatherReport
            // 
            this.buttonWeatherReport.Location = new System.Drawing.Point(24, 66);
            this.buttonWeatherReport.Name = "buttonWeatherReport";
            this.buttonWeatherReport.Size = new System.Drawing.Size(304, 23);
            this.buttonWeatherReport.TabIndex = 1;
            this.buttonWeatherReport.Text = "Weather Report Test  (Message Pack Protocol)";
            this.buttonWeatherReport.UseVisualStyleBackColor = true;
            // 
            // buttonStreamingExample
            // 
            this.buttonStreamingExample.Location = new System.Drawing.Point(24, 95);
            this.buttonStreamingExample.Name = "buttonStreamingExample";
            this.buttonStreamingExample.Size = new System.Drawing.Size(304, 23);
            this.buttonStreamingExample.TabIndex = 2;
            this.buttonStreamingExample.Text = "Streaming";
            this.buttonStreamingExample.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 160);
            this.Controls.Add(this.buttonStreamingExample);
            this.Controls.Add(this.buttonWeatherReport);
            this.Controls.Add(this.buttonSensor);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSensor;
        private System.Windows.Forms.Button buttonWeatherReport;
        private System.Windows.Forms.Button buttonStreamingExample;
    }
}