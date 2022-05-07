namespace AcuRiteSniffer
{
	partial class MQTT_Test
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
			this.txtOut = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtOut
			// 
			this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOut.BackColor = System.Drawing.SystemColors.Window;
			this.txtOut.Location = new System.Drawing.Point(12, 12);
			this.txtOut.Multiline = true;
			this.txtOut.Name = "txtOut";
			this.txtOut.ReadOnly = true;
			this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOut.Size = new System.Drawing.Size(776, 426);
			this.txtOut.TabIndex = 0;
			// 
			// MQTT_Test
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.txtOut);
			this.Name = "MQTT_Test";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MQTT Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MQTT_Test_FormClosing);
			this.Load += new System.EventHandler(this.MQTT_Test_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtOut;
	}
}