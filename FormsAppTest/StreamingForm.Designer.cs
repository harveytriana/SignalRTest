
namespace FormsAppTest
{
    partial class StreamingForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelPrompt = new System.Windows.Forms.Label();
            this.buttonServerToClient = new System.Windows.Forms.Button();
            this.buttonClientToServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTitle.Location = new System.Drawing.Point(6, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(396, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "SignalR Streaming";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(125, 125);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(160, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            // 
            // labelPrompt
            // 
            this.labelPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPrompt.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelPrompt.Location = new System.Drawing.Point(6, 62);
            this.labelPrompt.Name = "labelPrompt";
            this.labelPrompt.Size = new System.Drawing.Size(396, 50);
            this.labelPrompt.TabIndex = 2;
            this.labelPrompt.Text = "SignalR Streaming";
            this.labelPrompt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonServerToClient
            // 
            this.buttonServerToClient.Location = new System.Drawing.Point(125, 154);
            this.buttonServerToClient.Name = "buttonServerToClient";
            this.buttonServerToClient.Size = new System.Drawing.Size(160, 23);
            this.buttonServerToClient.TabIndex = 3;
            this.buttonServerToClient.Text = "Server » Client";
            this.buttonServerToClient.UseVisualStyleBackColor = true;
            // 
            // buttonClientToServer
            // 
            this.buttonClientToServer.Location = new System.Drawing.Point(125, 183);
            this.buttonClientToServer.Name = "buttonClientToServer";
            this.buttonClientToServer.Size = new System.Drawing.Size(160, 23);
            this.buttonClientToServer.TabIndex = 4;
            this.buttonClientToServer.Text = "Client » Server";
            this.buttonClientToServer.UseVisualStyleBackColor = true;
            // 
            // StreamingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 236);
            this.Controls.Add(this.buttonClientToServer);
            this.Controls.Add(this.buttonServerToClient);
            this.Controls.Add(this.labelPrompt);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.labelTitle);
            this.Name = "StreamingForm";
            this.Text = "StreamingForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelPrompt;
        private System.Windows.Forms.Button buttonServerToClient;
        private System.Windows.Forms.Button buttonClientToServer;
    }
}