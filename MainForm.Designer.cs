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
            if (disposing && (components is not null))
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
            this.openFileDialogSelectConfig = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxAutoStartParsingSequence = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonImport = new System.Windows.Forms.Button();
            this.openFileDialogSelectCSVImport = new System.Windows.Forms.OpenFileDialog();
            this.textBoxIDCodes = new System.Windows.Forms.TextBox();
            this.buttonBrowseIDCodes = new System.Windows.Forms.Button();
            this.labelIDCodes = new System.Windows.Forms.Label();
            this.labelIDCodeNotes = new System.Windows.Forms.Label();
            this.openFileDialogSelectIDCodes = new System.Windows.Forms.OpenFileDialog();
            this.labelComPorts = new System.Windows.Forms.Label();
            this.buttonBrowseComPorts = new System.Windows.Forms.Button();
            this.textBoxComPorts = new System.Windows.Forms.TextBox();
            this.labelComPortNotes = new System.Windows.Forms.Label();
            this.openFileDialogSelectComPorts = new System.Windows.Forms.OpenFileDialog();
            this.labelFeedback = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonChangeMode = new System.Windows.Forms.Button();
            this.radioButtonControlling = new System.Windows.Forms.RadioButton();
            this.radioButtonListening = new System.Windows.Forms.RadioButton();
            this.groupBoxMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStartComPort
            // 
            this.labelStartComPort.AutoSize = true;
            this.labelStartComPort.Location = new System.Drawing.Point(14, 19);
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
            this.labelEndComPort.Location = new System.Drawing.Point(209, 18);
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
            this.buttonScan.Location = new System.Drawing.Point(719, 10);
            this.buttonScan.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(110, 33);
            this.buttonScan.TabIndex = 21;
            this.buttonScan.Text = "Sca&n";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(738, 569);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(91, 33);
            this.buttonCancel.TabIndex = 28;
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
            this.groupBoxMessages.Location = new System.Drawing.Point(11, 203);
            this.groupBoxMessages.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxMessages.Name = "groupBoxMessages";
            this.groupBoxMessages.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.groupBoxMessages.Size = new System.Drawing.Size(818, 337);
            this.groupBoxMessages.TabIndex = 29;
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
            this.textBoxMessageOutput.Size = new System.Drawing.Size(814, 319);
            this.textBoxMessageOutput.TabIndex = 0;
            this.textBoxMessageOutput.TabStop = false;
            // 
            // linkLabelClearConsole
            // 
            this.linkLabelClearConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelClearConsole.AutoSize = true;
            this.linkLabelClearConsole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkLabelClearConsole.Location = new System.Drawing.Point(736, 195);
            this.linkLabelClearConsole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelClearConsole.Name = "linkLabelClearConsole";
            this.linkLabelClearConsole.Size = new System.Drawing.Size(93, 19);
            this.linkLabelClearConsole.TabIndex = 27;
            this.linkLabelClearConsole.TabStop = true;
            this.linkLabelClearConsole.Text = "Clear Console";
            this.linkLabelClearConsole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabelClearConsole.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClearConsole_LinkClicked);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(14, 570);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(723, 32);
            this.progressBar.TabIndex = 31;
            // 
            // checkBoxRescan
            // 
            this.checkBoxRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRescan.AutoSize = true;
            this.checkBoxRescan.Location = new System.Drawing.Point(402, 93);
            this.checkBoxRescan.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.checkBoxRescan.Name = "checkBoxRescan";
            this.checkBoxRescan.Size = new System.Drawing.Size(309, 21);
            this.checkBoxRescan.TabIndex = 13;
            this.checkBoxRescan.Text = "&Rescan Existing Devices (for config updates)";
            this.checkBoxRescan.UseVisualStyleBackColor = true;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(719, 45);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(110, 33);
            this.buttonSettings.TabIndex = 22;
            this.buttonSettings.Text = "Se&ttings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // textBoxEndIDCode
            // 
            this.textBoxEndIDCode.Location = new System.Drawing.Point(334, 91);
            this.textBoxEndIDCode.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxEndIDCode.Name = "textBoxEndIDCode";
            this.textBoxEndIDCode.Size = new System.Drawing.Size(47, 23);
            this.textBoxEndIDCode.TabIndex = 12;
            this.textBoxEndIDCode.Text = "65535";
            this.textBoxEndIDCode.Click += new System.EventHandler(this.SelectAll);
            this.textBoxEndIDCode.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxEndIDCode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxEndIDCode_Validating);
            this.textBoxEndIDCode.Validated += new System.EventHandler(this.textBoxEndIDCode_Validated);
            // 
            // labelEndIDCode
            // 
            this.labelEndIDCode.AutoSize = true;
            this.labelEndIDCode.Location = new System.Drawing.Point(220, 95);
            this.labelEndIDCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEndIDCode.Name = "labelEndIDCode";
            this.labelEndIDCode.Size = new System.Drawing.Size(110, 17);
            this.labelEndIDCode.TabIndex = 11;
            this.labelEndIDCode.Text = "En&ding ID Code:";
            // 
            // textBoxStartIDCode
            // 
            this.textBoxStartIDCode.Location = new System.Drawing.Point(144, 91);
            this.textBoxStartIDCode.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxStartIDCode.Name = "textBoxStartIDCode";
            this.textBoxStartIDCode.Size = new System.Drawing.Size(47, 23);
            this.textBoxStartIDCode.TabIndex = 10;
            this.textBoxStartIDCode.Text = "1";
            this.textBoxStartIDCode.Click += new System.EventHandler(this.SelectAll);
            this.textBoxStartIDCode.Enter += new System.EventHandler(this.SelectAll);
            this.textBoxStartIDCode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStartIDCode_Validating);
            this.textBoxStartIDCode.Validated += new System.EventHandler(this.textBoxStartIDCode_Validated);
            // 
            // labelStartIDCode
            // 
            this.labelStartIDCode.AutoSize = true;
            this.labelStartIDCode.Location = new System.Drawing.Point(25, 95);
            this.labelStartIDCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartIDCode.Name = "labelStartIDCode";
            this.labelStartIDCode.Size = new System.Drawing.Size(115, 17);
            this.labelStartIDCode.TabIndex = 9;
            this.labelStartIDCode.Text = "S&tarting ID Code:";
            // 
            // labelSourceConfig
            // 
            this.labelSourceConfig.AutoSize = true;
            this.labelSourceConfig.Location = new System.Drawing.Point(39, 176);
            this.labelSourceConfig.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceConfig.Name = "labelSourceConfig";
            this.labelSourceConfig.Size = new System.Drawing.Size(101, 17);
            this.labelSourceConfig.TabIndex = 18;
            this.labelSourceConfig.Text = "So&urce Config:";
            // 
            // textBoxSourceConfig
            // 
            this.textBoxSourceConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceConfig.Location = new System.Drawing.Point(144, 172);
            this.textBoxSourceConfig.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxSourceConfig.Name = "textBoxSourceConfig";
            this.textBoxSourceConfig.Size = new System.Drawing.Size(527, 23);
            this.textBoxSourceConfig.TabIndex = 19;
            this.textBoxSourceConfig.Text = "SIEGate.exe.config";
            this.textBoxSourceConfig.Click += new System.EventHandler(this.SelectAll);
            this.textBoxSourceConfig.Enter += new System.EventHandler(this.SelectAll);
            // 
            // buttonBrowseConfig
            // 
            this.buttonBrowseConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseConfig.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBrowseConfig.Location = new System.Drawing.Point(670, 171);
            this.buttonBrowseConfig.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBrowseConfig.Name = "buttonBrowseConfig";
            this.buttonBrowseConfig.Size = new System.Drawing.Size(38, 25);
            this.buttonBrowseConfig.TabIndex = 20;
            this.buttonBrowseConfig.Text = "...";
            this.buttonBrowseConfig.UseVisualStyleBackColor = true;
            this.buttonBrowseConfig.Click += new System.EventHandler(this.buttonBrowseConfig_Click);
            // 
            // openFileDialogSelectConfig
            // 
            this.openFileDialogSelectConfig.DefaultExt = "exe.config";
            this.openFileDialogSelectConfig.Filter = "Confg Files|*.exe.config|All Files|*.*";
            this.openFileDialogSelectConfig.InitialDirectory = "C:\\Program Files\\";
            this.openFileDialogSelectConfig.RestoreDirectory = true;
            this.openFileDialogSelectConfig.Title = "Select Source TSF Host Application Configuration File";
            // 
            // checkBoxAutoStartParsingSequence
            // 
            this.checkBoxAutoStartParsingSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(719, 80);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(110, 33);
            this.buttonImport.TabIndex = 23;
            this.buttonImport.Text = "&Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // openFileDialogSelectCSVImport
            // 
            this.openFileDialogSelectCSVImport.DefaultExt = "csv";
            this.openFileDialogSelectCSVImport.Filter = "CSV Files|*.csv|All Files|*.*";
            this.openFileDialogSelectCSVImport.RestoreDirectory = true;
            this.openFileDialogSelectCSVImport.Title = "Select CSV File with Serial Port Number to ID Code Mapping";
            // 
            // textBoxIDCodes
            // 
            this.textBoxIDCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIDCodes.Location = new System.Drawing.Point(144, 118);
            this.textBoxIDCodes.Name = "textBoxIDCodes";
            this.textBoxIDCodes.Size = new System.Drawing.Size(527, 23);
            this.textBoxIDCodes.TabIndex = 15;
            this.textBoxIDCodes.TextChanged += new System.EventHandler(this.textBoxIDCodeValues_TextChanged);
            this.textBoxIDCodes.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxIDCodeValues_Validating);
            this.textBoxIDCodes.Validated += new System.EventHandler(this.textBoxIDCodeValues_Validated);
            // 
            // buttonBrowseIDCodes
            // 
            this.buttonBrowseIDCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseIDCodes.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBrowseIDCodes.Location = new System.Drawing.Point(670, 117);
            this.buttonBrowseIDCodes.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBrowseIDCodes.Name = "buttonBrowseIDCodes";
            this.buttonBrowseIDCodes.Size = new System.Drawing.Size(38, 25);
            this.buttonBrowseIDCodes.TabIndex = 16;
            this.buttonBrowseIDCodes.Text = "...";
            this.buttonBrowseIDCodes.UseVisualStyleBackColor = true;
            this.buttonBrowseIDCodes.Click += new System.EventHandler(this.buttonBrowseIDCodes_Click);
            // 
            // labelIDCodes
            // 
            this.labelIDCodes.AutoSize = true;
            this.labelIDCodes.Location = new System.Drawing.Point(31, 123);
            this.labelIDCodes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIDCodes.Name = "labelIDCodes";
            this.labelIDCodes.Size = new System.Drawing.Size(109, 17);
            this.labelIDCodes.TabIndex = 14;
            this.labelIDCodes.Text = "ID Code &Values:";
            // 
            // labelIDCodeNotes
            // 
            this.labelIDCodeNotes.AutoSize = true;
            this.labelIDCodeNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIDCodeNotes.ForeColor = System.Drawing.SystemColors.InfoText;
            this.labelIDCodeNotes.Location = new System.Drawing.Point(144, 140);
            this.labelIDCodeNotes.Name = "labelIDCodeNotes";
            this.labelIDCodeNotes.Size = new System.Drawing.Size(507, 15);
            this.labelIDCodeNotes.TabIndex = 17;
            this.labelIDCodeNotes.Text = "Comma separated list of ID codes. Overrides starting / ending ID code range when " +
    "specified.";
            // 
            // openFileDialogSelectIDCodes
            // 
            this.openFileDialogSelectIDCodes.DefaultExt = "txt";
            this.openFileDialogSelectIDCodes.Filter = "Text Files|*.txt|CSV Files|*.csv|All Files|*.*";
            this.openFileDialogSelectIDCodes.RestoreDirectory = true;
            this.openFileDialogSelectIDCodes.Title = "Select Text/CSV File Containing List of ID Codes";
            // 
            // labelComPorts
            // 
            this.labelComPorts.AutoSize = true;
            this.labelComPorts.Location = new System.Drawing.Point(20, 47);
            this.labelComPorts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelComPorts.Name = "labelComPorts";
            this.labelComPorts.Size = new System.Drawing.Size(120, 17);
            this.labelComPorts.TabIndex = 5;
            this.labelComPorts.Text = "COM &Port Values:";
            // 
            // buttonBrowseComPorts
            // 
            this.buttonBrowseComPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseComPorts.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBrowseComPorts.Location = new System.Drawing.Point(670, 41);
            this.buttonBrowseComPorts.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBrowseComPorts.Name = "buttonBrowseComPorts";
            this.buttonBrowseComPorts.Size = new System.Drawing.Size(38, 25);
            this.buttonBrowseComPorts.TabIndex = 7;
            this.buttonBrowseComPorts.Text = "...";
            this.buttonBrowseComPorts.UseVisualStyleBackColor = true;
            this.buttonBrowseComPorts.Click += new System.EventHandler(this.buttonBrowseComPorts_Click);
            // 
            // textBoxComPorts
            // 
            this.textBoxComPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComPorts.Location = new System.Drawing.Point(144, 42);
            this.textBoxComPorts.Name = "textBoxComPorts";
            this.textBoxComPorts.Size = new System.Drawing.Size(527, 23);
            this.textBoxComPorts.TabIndex = 6;
            this.textBoxComPorts.TextChanged += new System.EventHandler(this.textBoxComPorts_TextChanged);
            this.textBoxComPorts.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxComPorts_Validating);
            this.textBoxComPorts.Validated += new System.EventHandler(this.textBoxComPorts_Validated);
            // 
            // labelComPortNotes
            // 
            this.labelComPortNotes.AutoSize = true;
            this.labelComPortNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelComPortNotes.ForeColor = System.Drawing.SystemColors.InfoText;
            this.labelComPortNotes.Location = new System.Drawing.Point(144, 64);
            this.labelComPortNotes.Name = "labelComPortNotes";
            this.labelComPortNotes.Size = new System.Drawing.Size(527, 15);
            this.labelComPortNotes.TabIndex = 8;
            this.labelComPortNotes.Text = "Comma separated list of COM ports. Overrides starting / ending COM port range whe" +
    "n specified.";
            // 
            // openFileDialogSelectComPorts
            // 
            this.openFileDialogSelectComPorts.DefaultExt = "txt";
            this.openFileDialogSelectComPorts.Filter = "Text Files|*.txt|CSV Files|*.csv|All Files|*.*";
            this.openFileDialogSelectComPorts.RestoreDirectory = true;
            this.openFileDialogSelectComPorts.Title = "Select Text/CSV File Containing List of COM Ports";
            // 
            // labelFeedback
            // 
            this.labelFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFeedback.BackColor = System.Drawing.Color.Black;
            this.labelFeedback.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFeedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFeedback.ForeColor = System.Drawing.Color.Yellow;
            this.labelFeedback.Location = new System.Drawing.Point(14, 542);
            this.labelFeedback.Name = "labelFeedback";
            this.labelFeedback.Size = new System.Drawing.Size(813, 25);
            this.labelFeedback.TabIndex = 30;
            this.labelFeedback.Tag = "Estimated Time Remaining: {0} -- Total Discovered Devices: {1:N0}";
            this.labelFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.BackColor = System.Drawing.SystemColors.Control;
            this.labelVersion.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelVersion.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelVersion.Location = new System.Drawing.Point(-1, 602);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelVersion.Size = new System.Drawing.Size(100, 15);
            this.labelVersion.TabIndex = 32;
            this.labelVersion.Text = "Version: x.x.x";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonChangeMode
            // 
            this.buttonChangeMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeMode.Location = new System.Drawing.Point(719, 115);
            this.buttonChangeMode.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonChangeMode.Name = "buttonChangeMode";
            this.buttonChangeMode.Size = new System.Drawing.Size(110, 33);
            this.buttonChangeMode.TabIndex = 24;
            this.buttonChangeMode.Text = "Change M&ode";
            this.buttonChangeMode.UseVisualStyleBackColor = true;
            this.buttonChangeMode.Click += new System.EventHandler(this.buttonChangeMode_Click);
            // 
            // radioButtonControlling
            // 
            this.radioButtonControlling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonControlling.AutoSize = true;
            this.radioButtonControlling.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonControlling.Location = new System.Drawing.Point(723, 149);
            this.radioButtonControlling.Name = "radioButtonControlling";
            this.radioButtonControlling.Size = new System.Drawing.Size(112, 17);
            this.radioButtonControlling.TabIndex = 25;
            this.radioButtonControlling.TabStop = true;
            this.radioButtonControlling.Text = "Controllin&g (Active)";
            this.radioButtonControlling.UseVisualStyleBackColor = true;
            this.radioButtonControlling.CheckedChanged += new System.EventHandler(this.radioButtonControlling_CheckedChanged);
            // 
            // radioButtonListening
            // 
            this.radioButtonListening.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonListening.AutoSize = true;
            this.radioButtonListening.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonListening.Location = new System.Drawing.Point(723, 166);
            this.radioButtonListening.Name = "radioButtonListening";
            this.radioButtonListening.Size = new System.Drawing.Size(113, 17);
            this.radioButtonListening.TabIndex = 26;
            this.radioButtonListening.TabStop = true;
            this.radioButtonListening.Text = "&Listening (Passive)";
            this.radioButtonListening.UseVisualStyleBackColor = true;
            this.radioButtonListening.CheckedChanged += new System.EventHandler(this.radioButtonListening_CheckedChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonScan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(840, 616);
            this.Controls.Add(this.radioButtonListening);
            this.Controls.Add(this.radioButtonControlling);
            this.Controls.Add(this.buttonChangeMode);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelFeedback);
            this.Controls.Add(this.labelComPorts);
            this.Controls.Add(this.buttonBrowseComPorts);
            this.Controls.Add(this.textBoxComPorts);
            this.Controls.Add(this.labelComPortNotes);
            this.Controls.Add(this.labelIDCodes);
            this.Controls.Add(this.buttonBrowseIDCodes);
            this.Controls.Add(this.textBoxIDCodes);
            this.Controls.Add(this.buttonImport);
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
            this.Controls.Add(this.labelIDCodeNotes);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MinimumSize = new System.Drawing.Size(855, 655);
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
        private System.Windows.Forms.OpenFileDialog openFileDialogSelectConfig;
        private System.Windows.Forms.LinkLabel linkLabelClearConsole;
        private System.Windows.Forms.CheckBox checkBoxAutoStartParsingSequence;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.OpenFileDialog openFileDialogSelectCSVImport;
        private System.Windows.Forms.Label labelIDCodes;
        private System.Windows.Forms.Button buttonBrowseIDCodes;
        private System.Windows.Forms.TextBox textBoxIDCodes;
        private System.Windows.Forms.Label labelIDCodeNotes;
        private System.Windows.Forms.OpenFileDialog openFileDialogSelectIDCodes;
        private System.Windows.Forms.Label labelComPorts;
        private System.Windows.Forms.Button buttonBrowseComPorts;
        private System.Windows.Forms.TextBox textBoxComPorts;
        private System.Windows.Forms.Label labelComPortNotes;
        private System.Windows.Forms.OpenFileDialog openFileDialogSelectComPorts;
        private System.Windows.Forms.Label labelFeedback;
        internal System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonChangeMode;
        private System.Windows.Forms.RadioButton radioButtonListening;
        private System.Windows.Forms.RadioButton radioButtonControlling;
    }
}

