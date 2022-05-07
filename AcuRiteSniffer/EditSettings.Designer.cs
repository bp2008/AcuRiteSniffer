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
			this.nudPort = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnTextFileDefinitions = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.txtServiceName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAcuriteAccessList = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.nudHttpsPort = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnMqttTest = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.txtMqttHost = new System.Windows.Forms.TextBox();
			this.nudMqttPort = new System.Windows.Forms.NumericUpDown();
			this.label12 = new System.Windows.Forms.Label();
			this.txtMqttUser = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.txtMqttPass = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHttpsPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMqttPort)).BeginInit();
			this.SuspendLayout();
			// 
			// nudPort
			// 
			this.nudPort.Location = new System.Drawing.Point(204, 101);
			this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.nudPort.Name = "nudPort";
			this.nudPort.Size = new System.Drawing.Size(73, 20);
			this.nudPort.TabIndex = 10;
			this.toolTip1.SetToolTip(this.nudPort, "If -1, the server will not listen for http connections.  Should use HTTPS if not " +
        "using HTTP.");
			this.nudPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 103);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(186, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Embedded web server HTTP port:";
			this.toolTip1.SetToolTip(this.label2, "If -1, the server will not listen for http connections.  Should use HTTPS if not " +
        "using HTTP.");
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.SystemColors.Info;
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(12, 9);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(403, 40);
			this.label7.TabIndex = 11;
			this.label7.Text = "Settings are automatically saved.  Restart the service to make them take effect!\r" +
    "\n";
			// 
			// btnTextFileDefinitions
			// 
			this.btnTextFileDefinitions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnTextFileDefinitions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnTextFileDefinitions.Location = new System.Drawing.Point(314, 101);
			this.btnTextFileDefinitions.Name = "btnTextFileDefinitions";
			this.btnTextFileDefinitions.Size = new System.Drawing.Size(101, 46);
			this.btnTextFileDefinitions.TabIndex = 50;
			this.btnTextFileDefinitions.Text = "Edit text file definitions";
			this.btnTextFileDefinitions.UseVisualStyleBackColor = false;
			this.btnTextFileDefinitions.Click += new System.EventHandler(this.btnTextFileDefinitions_Click);
			// 
			// txtServiceName
			// 
			this.txtServiceName.Location = new System.Drawing.Point(100, 52);
			this.txtServiceName.Name = "txtServiceName";
			this.txtServiceName.Size = new System.Drawing.Size(315, 20);
			this.txtServiceName.TabIndex = 1;
			this.toolTip1.SetToolTip(this.txtServiceName, "The service name should be changed if running multiple instances of the service.");
			this.txtServiceName.TextChanged += new System.EventHandler(this.txtServiceName_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(170, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Service Name:";
			this.toolTip1.SetToolTip(this.label3, "The service name should be changed if running multiple instances of the service.");
			// 
			// txtAcuriteAccessList
			// 
			this.txtAcuriteAccessList.Location = new System.Drawing.Point(162, 186);
			this.txtAcuriteAccessList.Name = "txtAcuriteAccessList";
			this.txtAcuriteAccessList.Size = new System.Drawing.Size(253, 20);
			this.txtAcuriteAccessList.TabIndex = 30;
			this.toolTip1.SetToolTip(this.txtAcuriteAccessList, "A semicolon-separated list of IP addresses of AcuRite Access devices which are se" +
        "nding their data to this server.");
			this.txtAcuriteAccessList.TextChanged += new System.EventHandler(this.txtAcuriteAccessList_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(12, 189);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(144, 32);
			this.label8.TabIndex = 17;
			this.label8.Text = "AcuRite Access Addresses:\r\n(semicolon separated)\r\n";
			this.toolTip1.SetToolTip(this.label8, "A semicolon-separated list of IP addresses of AcuRite Access devices which are se" +
        "nding their data to this server.");
			// 
			// nudHttpsPort
			// 
			this.nudHttpsPort.Location = new System.Drawing.Point(204, 127);
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
			this.nudHttpsPort.Size = new System.Drawing.Size(73, 20);
			this.nudHttpsPort.TabIndex = 20;
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
			this.label9.Location = new System.Drawing.Point(12, 129);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(186, 13);
			this.label9.TabIndex = 20;
			this.label9.Text = "Embedded web server HTTPS port:";
			this.toolTip1.SetToolTip(this.label9, "If -1, the server will not listen for https connections.  Should be set to 443 if" +
        " using AcuRite Access proxying.");
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(9, 257);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(406, 32);
			this.label5.TabIndex = 23;
			this.label5.Text = "This service can subscribe to an MQTT topic to read packets from rtl_433 software" +
    ", and make the data accessible through the embedded web server.";
			this.toolTip1.SetToolTip(this.label5, "A semicolon-separated list of IP addresses of AcuRite Access devices which are se" +
        "nding their data to this server.");
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(403, 23);
			this.label6.TabIndex = 16;
			this.label6.Text = "Important: Uninstall the service before changing the name here!";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 160);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(403, 23);
			this.label1.TabIndex = 21;
			this.label1.Text = "-- AcuRite Access Configuration --";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12, 231);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(403, 23);
			this.label4.TabIndex = 22;
			this.label4.Text = "-- MQTT Client Configuration --";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnMqttTest
			// 
			this.btnMqttTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnMqttTest.Location = new System.Drawing.Point(141, 391);
			this.btnMqttTest.Name = "btnMqttTest";
			this.btnMqttTest.Size = new System.Drawing.Size(136, 31);
			this.btnMqttTest.TabIndex = 40;
			this.btnMqttTest.Text = "Test MQTT";
			this.btnMqttTest.UseVisualStyleBackColor = false;
			this.btnMqttTest.Click += new System.EventHandler(this.btnMqttTest_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(13, 291);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(151, 13);
			this.label11.TabIndex = 51;
			this.label11.Text = "MQTT Broker Hostname or IP:";
			// 
			// txtMqttHost
			// 
			this.txtMqttHost.Location = new System.Drawing.Point(170, 288);
			this.txtMqttHost.Name = "txtMqttHost";
			this.txtMqttHost.Size = new System.Drawing.Size(245, 20);
			this.txtMqttHost.TabIndex = 40;
			this.txtMqttHost.TextChanged += new System.EventHandler(this.txtMqttHost_TextChanged);
			// 
			// nudMqttPort
			// 
			this.nudMqttPort.Location = new System.Drawing.Point(170, 314);
			this.nudMqttPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudMqttPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudMqttPort.Name = "nudMqttPort";
			this.nudMqttPort.Size = new System.Drawing.Size(88, 20);
			this.nudMqttPort.TabIndex = 53;
			this.nudMqttPort.Value = new decimal(new int[] {
            1883,
            0,
            0,
            0});
			this.nudMqttPort.ValueChanged += new System.EventHandler(this.nudMqttPort_ValueChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(12, 316);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(121, 13);
			this.label12.TabIndex = 52;
			this.label12.Text = "MQTT Broker TCP Port:";
			// 
			// txtMqttUser
			// 
			this.txtMqttUser.Location = new System.Drawing.Point(170, 339);
			this.txtMqttUser.Name = "txtMqttUser";
			this.txtMqttUser.Size = new System.Drawing.Size(245, 20);
			this.txtMqttUser.TabIndex = 54;
			this.txtMqttUser.TextChanged += new System.EventHandler(this.txtMqttUser_TextChanged);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(13, 342);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(97, 13);
			this.label13.TabIndex = 55;
			this.label13.Text = "MQTT User Name:";
			// 
			// txtMqttPassword
			// 
			this.txtMqttPass.Location = new System.Drawing.Point(170, 365);
			this.txtMqttPass.Name = "txtMqttPassword";
			this.txtMqttPass.PasswordChar = '*';
			this.txtMqttPass.Size = new System.Drawing.Size(245, 20);
			this.txtMqttPass.TabIndex = 56;
			this.txtMqttPass.TextChanged += new System.EventHandler(this.txtMqttPassword_TextChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(13, 368);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(90, 13);
			this.label10.TabIndex = 57;
			this.label10.Text = "MQTT Password:";
			// 
			// EditSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 435);
			this.Controls.Add(this.txtMqttPass);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtMqttUser);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.nudMqttPort);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.txtMqttHost);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.btnMqttTest);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudHttpsPort);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtAcuriteAccessList);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtServiceName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnTextFileDefinitions);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.nudPort);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "EditSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Restart service after changing settings";
			this.Load += new System.EventHandler(this.EditSettings_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHttpsPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMqttPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.NumericUpDown nudPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnTextFileDefinitions;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox txtServiceName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtAcuriteAccessList;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudHttpsPort;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnMqttTest;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtMqttHost;
		private System.Windows.Forms.NumericUpDown nudMqttPort;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtMqttUser;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox txtMqttPass;
		private System.Windows.Forms.Label label10;
	}
}