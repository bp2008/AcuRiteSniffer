namespace AcuRiteSniffer
{
	partial class EditSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSettings));
			this.label1 = new System.Windows.Forms.Label();
			this.txtSmartHubAddress = new System.Windows.Forms.TextBox();
			this.nudPort = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbInterface = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnTextFileDefinitions = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(262, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "SmartHUB Address:";
			// 
			// txtSmartHubAddress
			// 
			this.txtSmartHubAddress.Location = new System.Drawing.Point(280, 38);
			this.txtSmartHubAddress.Name = "txtSmartHubAddress";
			this.txtSmartHubAddress.Size = new System.Drawing.Size(135, 20);
			this.txtSmartHubAddress.TabIndex = 2;
			this.txtSmartHubAddress.TextChanged += new System.EventHandler(this.txtSmartHubAddress_TextChanged);
			// 
			// nudPort
			// 
			this.nudPort.Location = new System.Drawing.Point(327, 12);
			this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudPort.Name = "nudPort";
			this.nudPort.Size = new System.Drawing.Size(88, 20);
			this.nudPort.TabIndex = 1;
			this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(309, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "This service\'s embedded web server listens on port:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(403, 105);
			this.label4.TabIndex = 7;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// cbInterface
			// 
			this.cbInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbInterface.FormattingEnabled = true;
			this.cbInterface.Location = new System.Drawing.Point(12, 193);
			this.cbInterface.Name = "cbInterface";
			this.cbInterface.Size = new System.Drawing.Size(403, 21);
			this.cbInterface.TabIndex = 3;
			this.cbInterface.SelectedIndexChanged += new System.EventHandler(this.cbInterface_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 175);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(310, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "The packet capturing engine will listen on this network interface:";
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.SystemColors.Info;
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(12, 297);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(403, 40);
			this.label7.TabIndex = 11;
			this.label7.Text = "Settings are automatically saved.  Restart the service to make them take effect!\r" +
    "\n";
			// 
			// btnTextFileDefinitions
			// 
			this.btnTextFileDefinitions.Location = new System.Drawing.Point(122, 233);
			this.btnTextFileDefinitions.Name = "btnTextFileDefinitions";
			this.btnTextFileDefinitions.Size = new System.Drawing.Size(179, 42);
			this.btnTextFileDefinitions.TabIndex = 12;
			this.btnTextFileDefinitions.Text = "Edit text file definitions";
			this.btnTextFileDefinitions.UseVisualStyleBackColor = true;
			this.btnTextFileDefinitions.Click += new System.EventHandler(this.btnTextFileDefinitions_Click);
			// 
			// EditSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 347);
			this.Controls.Add(this.btnTextFileDefinitions);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbInterface);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtSmartHubAddress);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "EditSettings";
			this.Text = "Restart service after changing settings";
			this.Load += new System.EventHandler(this.EditSettings_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSmartHubAddress;
		private System.Windows.Forms.NumericUpDown nudPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbInterface;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnTextFileDefinitions;
	}
}