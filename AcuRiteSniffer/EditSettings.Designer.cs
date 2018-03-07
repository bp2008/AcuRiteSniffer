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
			this.components = new System.ComponentModel.Container();
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
			this.cbEasyParse = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.txtServiceName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtAcuriteAccessList = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.nudHttpsPort = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHttpsPort)).BeginInit();
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
			this.label7.Location = new System.Drawing.Point(15, 380);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(403, 40);
			this.label7.TabIndex = 11;
			this.label7.Text = "Settings are automatically saved.  Restart the service to make them take effect!\r" +
    "\n";
			// 
			// btnTextFileDefinitions
			// 
			this.btnTextFileDefinitions.Location = new System.Drawing.Point(131, 333);
			this.btnTextFileDefinitions.Name = "btnTextFileDefinitions";
			this.btnTextFileDefinitions.Size = new System.Drawing.Size(179, 42);
			this.btnTextFileDefinitions.TabIndex = 12;
			this.btnTextFileDefinitions.Text = "Edit text file definitions";
			this.btnTextFileDefinitions.UseVisualStyleBackColor = true;
			this.btnTextFileDefinitions.Click += new System.EventHandler(this.btnTextFileDefinitions_Click);
			// 
			// cbEasyParse
			// 
			this.cbEasyParse.AutoSize = true;
			this.cbEasyParse.Location = new System.Drawing.Point(12, 237);
			this.cbEasyParse.Name = "cbEasyParse";
			this.cbEasyParse.Size = new System.Drawing.Size(181, 17);
			this.cbEasyParse.TabIndex = 13;
			this.cbEasyParse.Text = "Use easy packet parsing method";
			this.toolTip1.SetToolTip(this.cbEasyParse, "Some SmartHUBs appear to send extremely messy TCP packets.\r\n\r\nIf you end up with " +
        "high CPU usage from this program, or partial/incorrect sensor data, try disablin" +
        "g this.");
			this.cbEasyParse.UseVisualStyleBackColor = true;
			this.cbEasyParse.CheckedChanged += new System.EventHandler(this.cbEasyParse_CheckedChanged);
			// 
			// txtServiceName
			// 
			this.txtServiceName.Location = new System.Drawing.Point(188, 263);
			this.txtServiceName.Name = "txtServiceName";
			this.txtServiceName.Size = new System.Drawing.Size(227, 20);
			this.txtServiceName.TabIndex = 15;
			this.toolTip1.SetToolTip(this.txtServiceName, "The service name should be changed if running multiple instances of the service.");
			this.txtServiceName.TextChanged += new System.EventHandler(this.txtServiceName_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 266);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(170, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Service Name:";
			this.toolTip1.SetToolTip(this.label3, "The service name should be changed if running multiple instances of the service.");
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 286);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(403, 23);
			this.label6.TabIndex = 16;
			this.label6.Text = "Important: Uninstall the service before changing the name here!";
			// 
			// txtAcuriteAccessList
			// 
			this.txtAcuriteAccessList.Location = new System.Drawing.Point(165, 423);
			this.txtAcuriteAccessList.Name = "txtAcuriteAccessList";
			this.txtAcuriteAccessList.Size = new System.Drawing.Size(253, 20);
			this.txtAcuriteAccessList.TabIndex = 18;
			this.toolTip1.SetToolTip(this.txtAcuriteAccessList, "A semicolon-separated list of IP addresses of AcuRite Access devices which are se" +
        "nding their data to this server.");
			this.txtAcuriteAccessList.TextChanged += new System.EventHandler(this.txtAcuriteAccessList_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(15, 426);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(144, 32);
			this.label8.TabIndex = 17;
			this.label8.Text = "AcuRite Access Addresses:\r\n(semicolon separated)\r\n";
			this.toolTip1.SetToolTip(this.label8, "A semicolon-separated list of IP addresses of AcuRite Access devices which are se" +
        "nding their data to this server.");
			// 
			// nudHttpsPort
			// 
			this.nudHttpsPort.Location = new System.Drawing.Point(328, 456);
			this.nudHttpsPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudHttpsPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.nudHttpsPort.Name = "nudHttpsPort";
			this.nudHttpsPort.Size = new System.Drawing.Size(88, 20);
			this.nudHttpsPort.TabIndex = 19;
			this.toolTip1.SetToolTip(this.nudHttpsPort, "If -1, the server will not listen for https connections.  Should be set to 443 if" +
        " using AcuRite Access proxying.");
			this.nudHttpsPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.nudHttpsPort.ValueChanged += new System.EventHandler(this.nudHttpsPort_ValueChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(13, 458);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(309, 13);
			this.label9.TabIndex = 20;
			this.label9.Text = "Listen for https connections on port:";
			this.toolTip1.SetToolTip(this.label9, "If -1, the server will not listen for https connections.  Should be set to 443 if" +
        " using AcuRite Access proxying.");
			// 
			// EditSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 483);
			this.Controls.Add(this.nudHttpsPort);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtAcuriteAccessList);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtServiceName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbEasyParse);
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
			((System.ComponentModel.ISupportInitialize)(this.nudHttpsPort)).EndInit();
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
		private System.Windows.Forms.CheckBox cbEasyParse;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox txtServiceName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtAcuriteAccessList;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudHttpsPort;
		private System.Windows.Forms.Label label9;
	}
}