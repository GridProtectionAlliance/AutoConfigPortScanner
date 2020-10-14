namespace AutoConfigPortScanner
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
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelStartComPort = new System.Windows.Forms.Label();
            this.textBoxStartComPort = new System.Windows.Forms.TextBox();
            this.labelEndComPort = new System.Windows.Forms.Label();
            this.textBoxEndComPort = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxMessages = new System.Windows.Forms.GroupBox();
            this.textBoxMessageOutput = new System.Windows.Forms.TextBox();
            this.linkLabelClearConsole = new System.Windows.Forms.LinkLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.checkBoxRescan = new System.Windows.Forms.CheckBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.textBoxEndIDCode = new System.Windows.Forms.TextBox();
            this.labelEndIDCode = new System.Windows.Forms.Label();
            this.textBoxStartIDCode = new System.Windows.Forms.TextBox();
            this.labelStartIDCode = new System.Windows.Forms.Label();
            this.labelSourceConfig = new System.Windows.Forms.Label();
            this.textBoxSourceConfig = new System.Windows.Forms.TextBox();
            this.buttonBrowseConfig = new System.Windows.Forms.Button();
            this.selectConfigFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxAutoStartParsingSequence = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStartComPort
            // 
            this.labelStartComPort.AutoSize = true;
            this.labelStartComPort.Location = new System.Drawing.Point(11, 18);
            this.labelStartComPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartComPort.Name = "labelStartComPort";
            this.labelStartComPort.Size = new System.Drawing.Size(126, 17);
            this.labelStartComPort.TabIndex = 0;
            this.labelStartComPort.Text = "&Starting COM Port:";
            // 
            // textBoxStartComPort
            // 
            this.textBoxStartComPort.Location = new System.Drawing.Point(144, 15);
            this.textBoxStartComPort.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxStartComPort.Name = "textBoxStartComPort";
            this.textBoxStartComPort.Size = new System.Drawing.Size(47, 23);
            this.textBoxStartComPort.TabIndex = 1;
            this.textBoxStartComPort.Text = "1";
            this.textBoxStartComPort.Click += new System.EventHandler(this.SelectAll);
            this.textBoxStartComPort.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxStartComPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStartComPort_Validating);
            this.textBoxStartComPort.Validated += new System.EventHandler(this.textBoxStartComPort_Validated);
            // 
            // labelEndComPort
            // 
            this.labelEndComPort.AutoSize = true;
            this.labelEndComPort.Location = new System.Drawing.Point(206, 18);
            this.labelEndComPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEndComPort.Name = "labelEndComPort";
            this.labelEndComPort.Size = new System.Drawing.Size(121, 17);
            this.labelEndComPort.TabIndex = 2;
            this.labelEndComPort.Text = "&Ending COM Port:";
            // 
            // textBoxEndComPort
            // 
            this.textBoxEndComPort.Location = new System.Drawing.Point(334, 15);
            this.textBoxEndComPort.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxEndComPort.Name = "textBoxEndComPort";
            this.textBoxEndComPort.Size = new System.Drawing.Size(47, 23);
            this.textBoxEndComPort.TabIndex = 3;
            this.textBoxEndComPort.Text = "100";
            this.textBoxEndComPort.Click += new System.EventHandler(this.SelectAll);
            this.textBoxEndComPort.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxEndComPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxEndComPort_Validating);
            this.textBoxEndComPort.Validated += new System.EventHandler(this.textBoxEndComPort_Validated);
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(738, 10);
            this.buttonScan.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(91, 33);
            this.buttonScan.TabIndex = 13;
            this.buttonScan.Text = "Sca&n";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(738, 475);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(91, 33);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxMessages
            // 
            this.groupBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMessages.Controls.Add(this.textBoxMessageOutput);
            this.groupBoxMessages.Location = new System.Drawing.Point(11, 122);
            this.groupBoxMessages.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxMessages.Name = "groupBoxMessages";
            this.groupBoxMessages.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.groupBoxMessages.Size = new System.Drawing.Size(818, 353);
            this.groupBoxMessages.TabIndex = 17;
            this.groupBoxMessages.TabStop = false;
            this.groupBoxMessages.Text = "&Messages:";
            // 
            // textBoxMessageOutput
            // 
            this.textBoxMessageOutput.BackColor = System.Drawing.Color.Black;
            this.textBoxMessageOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessageOutput.Font = new System.Drawing.Font("Consolas", 10F);
            this.textBoxMessageOutput.ForeColor = System.Drawing.Color.White;
            this.textBoxMessageOutput.Location = new System.Drawing.Point(2, 17);
            this.textBoxMessageOutput.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMessageOutput.Multiline = true;
            this.textBoxMessageOutput.Name = "textBoxMessageOutput";
            this.textBoxMessageOutput.ReadOnly = true;
            this.textBoxMessageOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessageOutput.Size = new System.Drawing.Size(814, 335);
            this.textBoxMessageOutput.TabIndex = 0;
            this.textBoxMessageOutput.TabStop = false;
            // 
            // linkLabelClearConsole
            // 
            this.linkLabelClearConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelClearConsole.AutoSize = true;
            this.linkLabelClearConsole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkLabelClearConsole.Location = new System.Drawing.Point(736, 118);
            this.linkLabelClearConsole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelClearConsole.Name = "linkLabelClearConsole";
            this.linkLabelClearConsole.Size = new System.Drawing.Size(93, 19);
            this.linkLabelClearConsole.TabIndex = 15;
            this.linkLabelClearConsole.TabStop = true;
            this.linkLabelClearConsole.Text = "C&lear Console";
            this.linkLabelClearConsole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabelClearConsole.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClearConsole_LinkClicked);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(14, 476);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(723, 32);
            this.progressBar.TabIndex = 18;
            // 
            // checkBoxRescan
            // 
            this.checkBoxRescan.AutoSize = true;
            this.checkBoxRescan.Location = new System.Drawing.Point(402, 52);
            this.checkBoxRescan.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.checkBoxRescan.Name = "checkBoxRescan";
            this.checkBoxRescan.Size = new System.Drawing.Size(309, 21);
            this.checkBoxRescan.TabIndex = 9;
            this.checkBoxRescan.Text = "&Rescan Existing Devices (for config updates)";
            this.checkBoxRescan.UseVisualStyleBackColor = true;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(738, 45);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(91, 33);
            this.buttonSettings.TabIndex = 14;
            this.buttonSettings.Text = "Se&ttings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // textBoxEndIDCode
            // 
            this.textBoxEndIDCode.Location = new System.Drawing.Point(334, 50);
            this.textBoxEndIDCode.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxEndIDCode.Name = "textBoxEndIDCode";
            this.textBoxEndIDCode.Size = new System.Drawing.Size(47, 23);
            this.textBoxEndIDCode.TabIndex = 8;
            this.textBoxEndIDCode.Text = "65535";
            this.textBoxEndIDCode.Click += new System.EventHandler(this.SelectAll);
            this.textBoxEndIDCode.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxEndIDCode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxEndIDCode_Validating);
            this.textBoxEndIDCode.Validated += new System.EventHandler(this.textBoxEndIDCode_Validated);
            // 
            // labelEndIDCode
            // 
            this.labelEndIDCode.AutoSize = true;
            this.labelEndIDCode.Location = new System.Drawing.Point(217, 53);
            this.labelEndIDCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEndIDCode.Name = "labelEndIDCode";
            this.labelEndIDCode.Size = new System.Drawing.Size(110, 17);
            this.labelEndIDCode.TabIndex = 7;
            this.labelEndIDCode.Text = "En&ding ID Code:";
            // 
            // textBoxStartIDCode
            // 
            this.textBoxStartIDCode.Location = new System.Drawing.Point(144, 50);
            this.textBoxStartIDCode.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxStartIDCode.Name = "textBoxStartIDCode";
            this.textBoxStartIDCode.Size = new System.Drawing.Size(47, 23);
            this.textBoxStartIDCode.TabIndex = 6;
            this.textBoxStartIDCode.Text = "1";
            this.textBoxStartIDCode.Click += new System.EventHandler(this.SelectAll);
            this.textBoxStartIDCode.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxStartIDCode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStartIDCode_Validating);
            this.textBoxStartIDCode.Validated += new System.EventHandler(this.textBoxStartIDCode_Validated);
            // 
            // labelStartIDCode
            // 
            this.labelStartIDCode.AutoSize = true;
            this.labelStartIDCode.Location = new System.Drawing.Point(22, 53);
            this.labelStartIDCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartIDCode.Name = "labelStartIDCode";
            this.labelStartIDCode.Size = new System.Drawing.Size(115, 17);
            this.labelStartIDCode.TabIndex = 5;
            this.labelStartIDCode.Text = "S&tarting ID Code:";
            // 
            // labelSourceConfig
            // 
            this.labelSourceConfig.AutoSize = true;
            this.labelSourceConfig.Location = new System.Drawing.Point(36, 89);
            this.labelSourceConfig.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceConfig.Name = "labelSourceConfig";
            this.labelSourceConfig.Size = new System.Drawing.Size(101, 17);
            this.labelSourceConfig.TabIndex = 10;
            this.labelSourceConfig.Text = "Source Confi&g:";
            // 
            // textBoxSourceConfig
            // 
            this.textBoxSourceConfig.Location = new System.Drawing.Point(144, 86);
            this.textBoxSourceConfig.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxSourceConfig.Name = "textBoxSourceConfig";
            this.textBoxSourceConfig.Size = new System.Drawing.Size(527, 23);
            this.textBoxSourceConfig.TabIndex = 11;
            this.textBoxSourceConfig.Text = "SIEGate.exe.config";
            this.textBoxSourceConfig.Click += new System.EventHandler(this.SelectAll);
            this.textBoxSourceConfig.Enter += new System.EventHandler(this.SelectAll);
            // 
            // buttonBrowseConfig
            // 
            this.buttonBrowseConfig.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBrowseConfig.Location = new System.Drawing.Point(670, 85);
            this.buttonBrowseConfig.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBrowseConfig.Name = "buttonBrowseConfig";
            this.buttonBrowseConfig.Size = new System.Drawing.Size(38, 25);
            this.buttonBrowseConfig.TabIndex = 12;
            this.buttonBrowseConfig.Text = "...";
            this.buttonBrowseConfig.UseVisualStyleBackColor = true;
            this.buttonBrowseConfig.Click += new System.EventHandler(this.buttonBrowseConfig_Click);
            // 
            // selectConfigFileDialog
            // 
            this.selectConfigFileDialog.DefaultExt = "exe.config";
            this.selectConfigFileDialog.Filter = "Confg Files|*.config|All Files|*.*";
            this.selectConfigFileDialog.InitialDirectory = "C:\\Program Files\\";
            this.selectConfigFileDialog.RestoreDirectory = true;
            this.selectConfigFileDialog.Title = "Select Source TSF Host Application Configuration File";
            // 
            // checkBoxAutoStartParsingSequence
            // 
            this.checkBoxAutoStartParsingSequence.AutoSize = true;
            this.checkBoxAutoStartParsingSequence.Location = new System.Drawing.Point(402, 18);
            this.checkBoxAutoStartParsingSequence.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.checkBoxAutoStartParsingSequence.Name = "checkBoxAutoStartParsingSequence";
            this.checkBoxAutoStartParsingSequence.Size = new System.Drawing.Size(261, 21);
            this.checkBoxAutoStartParsingSequence.TabIndex = 4;
            this.checkBoxAutoStartParsingSequence.Text = "&Auto-start parsing sequence for scan";
            this.checkBoxAutoStartParsingSequence.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonScan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(840, 522);
            this.Controls.Add(this.checkBoxAutoStartParsingSequence);
            this.Controls.Add(this.linkLabelClearConsole);
            this.Controls.Add(this.buttonBrowseConfig);
            this.Controls.Add(this.textBoxSourceConfig);
            this.Controls.Add(this.labelSourceConfig);
            this.Controls.Add(this.labelStartIDCode);
            this.Controls.Add(this.textBoxStartIDCode);
            this.Controls.Add(this.labelEndIDCode);
            this.Controls.Add(this.textBoxEndIDCode);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.checkBoxRescan);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBoxMessages);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.textBoxEndComPort);
            this.Controls.Add(this.labelEndComPort);
            this.Controls.Add(this.textBoxStartComPort);
            this.Controls.Add(this.labelStartComPort);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MinimumSize = new System.Drawing.Size(856, 561);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IEEE C37.118 Serial Port Scanner and Configuration Loader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxMessages.ResumeLayout(false);
            this.groupBoxMessages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStartComPort;
        private System.Windows.Forms.TextBox textBoxStartComPort;
        private System.Windows.Forms.Label labelEndComPort;
        private System.Windows.Forms.TextBox textBoxEndComPort;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxMessages;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox textBoxMessageOutput;
        private System.Windows.Forms.CheckBox checkBoxRescan;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.TextBox textBoxEndIDCode;
        private System.Windows.Forms.Label labelEndIDCode;
        private System.Windows.Forms.TextBox textBoxStartIDCode;
        private System.Windows.Forms.Label labelStartIDCode;
        private System.Windows.Forms.Label labelSourceConfig;
        private System.Windows.Forms.TextBox textBoxSourceConfig;
        private System.Windows.Forms.Button buttonBrowseConfig;
        private System.Windows.Forms.OpenFileDialog selectConfigFileDialog;
        private System.Windows.Forms.LinkLabel linkLabelClearConsole;
        private System.Windows.Forms.CheckBox checkBoxAutoStartParsingSequence;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}

