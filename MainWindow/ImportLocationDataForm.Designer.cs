﻿namespace SAOT
{
    partial class ImportLocationDataForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonImportLocs = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioImportLocsReplace = new System.Windows.Forms.RadioButton();
            this.radioImportLocsAppend = new System.Windows.Forms.RadioButton();
            this.comboImportLocsTarget = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboCreateLocs = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panelLevel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.textLevelStart = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textLevelEnd = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.radioLevelNumbers = new System.Windows.Forms.RadioButton();
            this.radioLevelLetters = new System.Windows.Forms.RadioButton();
            this.buttonCreateLocationSeq = new System.Windows.Forms.Button();
            this.panelBin = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.textBinStart = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBinEnd = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.radioBinNumbers = new System.Windows.Forms.RadioButton();
            this.radioBinLetters = new System.Windows.Forms.RadioButton();
            this.panelRack = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textRackStart = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textRackEnd = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioRackNumbers = new System.Windows.Forms.RadioButton();
            this.radioRackLetters = new System.Windows.Forms.RadioButton();
            this.panelAisle = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.textAisleStart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioAisleNumbers = new System.Windows.Forms.RadioButton();
            this.radioAisleLetters = new System.Windows.Forms.RadioButton();
            this.textAisleEnd = new System.Windows.Forms.TextBox();
            this.buttonDeleteAllLocs = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelLevel.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panelBin.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panelRack.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelAisle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonImportLocs);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.comboImportLocsTarget);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 286);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import Location Contents";
            // 
            // buttonImportLocs
            // 
            this.buttonImportLocs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImportLocs.Location = new System.Drawing.Point(156, 103);
            this.buttonImportLocs.Name = "buttonImportLocs";
            this.buttonImportLocs.Size = new System.Drawing.Size(170, 23);
            this.buttonImportLocs.TabIndex = 13;
            this.buttonImportLocs.Text = "Import Location Contents";
            this.buttonImportLocs.UseVisualStyleBackColor = true;
            this.buttonImportLocs.Click += new System.EventHandler(this.buttonImportLocs_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioImportLocsReplace);
            this.groupBox2.Controls.Add(this.radioImportLocsAppend);
            this.groupBox2.Location = new System.Drawing.Point(9, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(134, 70);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Contents Options";
            // 
            // radioImportLocsReplace
            // 
            this.radioImportLocsReplace.AutoSize = true;
            this.radioImportLocsReplace.Checked = true;
            this.radioImportLocsReplace.Location = new System.Drawing.Point(8, 42);
            this.radioImportLocsReplace.Name = "radioImportLocsReplace";
            this.radioImportLocsReplace.Size = new System.Drawing.Size(65, 17);
            this.radioImportLocsReplace.TabIndex = 12;
            this.radioImportLocsReplace.TabStop = true;
            this.radioImportLocsReplace.Text = "Replace";
            this.radioImportLocsReplace.UseVisualStyleBackColor = true;
            // 
            // radioImportLocsAppend
            // 
            this.radioImportLocsAppend.AutoSize = true;
            this.radioImportLocsAppend.Enabled = false;
            this.radioImportLocsAppend.Location = new System.Drawing.Point(8, 19);
            this.radioImportLocsAppend.Name = "radioImportLocsAppend";
            this.radioImportLocsAppend.Size = new System.Drawing.Size(62, 17);
            this.radioImportLocsAppend.TabIndex = 11;
            this.radioImportLocsAppend.Text = "Append";
            this.radioImportLocsAppend.UseVisualStyleBackColor = true;
            // 
            // comboImportLocsTarget
            // 
            this.comboImportLocsTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboImportLocsTarget.FormattingEnabled = true;
            this.comboImportLocsTarget.Location = new System.Drawing.Point(108, 19);
            this.comboImportLocsTarget.Name = "comboImportLocsTarget";
            this.comboImportLocsTarget.Size = new System.Drawing.Size(350, 21);
            this.comboImportLocsTarget.TabIndex = 10;
            this.comboImportLocsTarget.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Warehouse";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel4);
            this.groupBox3.Controls.Add(this.comboCreateLocs);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.buttonDeleteAllLocs);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(468, 268);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Create Locations";
            // 
            // comboCreateLocs
            // 
            this.comboCreateLocs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCreateLocs.FormattingEnabled = true;
            this.comboCreateLocs.Location = new System.Drawing.Point(108, 19);
            this.comboCreateLocs.Name = "comboCreateLocs";
            this.comboCreateLocs.Size = new System.Drawing.Size(354, 21);
            this.comboCreateLocs.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Target Warehouse";
            // 
            // panelLevel
            // 
            this.panelLevel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelLevel.Controls.Add(this.label8);
            this.panelLevel.Controls.Add(this.textLevelStart);
            this.panelLevel.Controls.Add(this.label9);
            this.panelLevel.Controls.Add(this.textLevelEnd);
            this.panelLevel.Controls.Add(this.panel8);
            this.panelLevel.Location = new System.Drawing.Point(3, 71);
            this.panelLevel.Name = "panelLevel";
            this.panelLevel.Size = new System.Drawing.Size(452, 28);
            this.panelLevel.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Level Start";
            // 
            // textLevelStart
            // 
            this.textLevelStart.Location = new System.Drawing.Point(69, 5);
            this.textLevelStart.Name = "textLevelStart";
            this.textLevelStart.Size = new System.Drawing.Size(50, 20);
            this.textLevelStart.TabIndex = 0;
            this.textLevelStart.Text = "A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(155, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Level End";
            // 
            // textLevelEnd
            // 
            this.textLevelEnd.Location = new System.Drawing.Point(216, 5);
            this.textLevelEnd.Name = "textLevelEnd";
            this.textLevelEnd.Size = new System.Drawing.Size(50, 20);
            this.textLevelEnd.TabIndex = 21;
            this.textLevelEnd.Text = "F";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.radioLevelNumbers);
            this.panel8.Controls.Add(this.radioLevelLetters);
            this.panel8.Location = new System.Drawing.Point(317, 5);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(132, 20);
            this.panel8.TabIndex = 23;
            // 
            // radioLevelNumbers
            // 
            this.radioLevelNumbers.AutoSize = true;
            this.radioLevelNumbers.Location = new System.Drawing.Point(59, 1);
            this.radioLevelNumbers.Name = "radioLevelNumbers";
            this.radioLevelNumbers.Size = new System.Drawing.Size(67, 17);
            this.radioLevelNumbers.TabIndex = 25;
            this.radioLevelNumbers.Text = "Numbers";
            this.radioLevelNumbers.UseVisualStyleBackColor = true;
            // 
            // radioLevelLetters
            // 
            this.radioLevelLetters.AutoSize = true;
            this.radioLevelLetters.Checked = true;
            this.radioLevelLetters.Location = new System.Drawing.Point(3, 1);
            this.radioLevelLetters.Name = "radioLevelLetters";
            this.radioLevelLetters.Size = new System.Drawing.Size(57, 17);
            this.radioLevelLetters.TabIndex = 24;
            this.radioLevelLetters.TabStop = true;
            this.radioLevelLetters.Text = "Letters";
            this.radioLevelLetters.UseVisualStyleBackColor = true;
            // 
            // buttonCreateLocationSeq
            // 
            this.buttonCreateLocationSeq.Location = new System.Drawing.Point(278, 147);
            this.buttonCreateLocationSeq.Name = "buttonCreateLocationSeq";
            this.buttonCreateLocationSeq.Size = new System.Drawing.Size(182, 23);
            this.buttonCreateLocationSeq.TabIndex = 36;
            this.buttonCreateLocationSeq.Text = "Create Location Sequence";
            this.buttonCreateLocationSeq.UseVisualStyleBackColor = true;
            this.buttonCreateLocationSeq.Click += new System.EventHandler(this.buttonCreateLocationSeq_Click);
            // 
            // panelBin
            // 
            this.panelBin.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelBin.Controls.Add(this.label6);
            this.panelBin.Controls.Add(this.textBinStart);
            this.panelBin.Controls.Add(this.label7);
            this.panelBin.Controls.Add(this.textBinEnd);
            this.panelBin.Controls.Add(this.panel6);
            this.panelBin.Location = new System.Drawing.Point(3, 105);
            this.panelBin.Name = "panelBin";
            this.panelBin.Size = new System.Drawing.Size(452, 28);
            this.panelBin.TabIndex = 35;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Bin Start";
            // 
            // textBinStart
            // 
            this.textBinStart.Location = new System.Drawing.Point(69, 5);
            this.textBinStart.Name = "textBinStart";
            this.textBinStart.Size = new System.Drawing.Size(50, 20);
            this.textBinStart.TabIndex = 0;
            this.textBinStart.Text = "01";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(166, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Bin End";
            // 
            // textBinEnd
            // 
            this.textBinEnd.Location = new System.Drawing.Point(216, 5);
            this.textBinEnd.Name = "textBinEnd";
            this.textBinEnd.Size = new System.Drawing.Size(50, 20);
            this.textBinEnd.TabIndex = 21;
            this.textBinEnd.Text = "08";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.radioBinNumbers);
            this.panel6.Controls.Add(this.radioBinLetters);
            this.panel6.Location = new System.Drawing.Point(317, 5);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(132, 20);
            this.panel6.TabIndex = 23;
            // 
            // radioBinNumbers
            // 
            this.radioBinNumbers.AutoSize = true;
            this.radioBinNumbers.Checked = true;
            this.radioBinNumbers.Location = new System.Drawing.Point(59, 1);
            this.radioBinNumbers.Name = "radioBinNumbers";
            this.radioBinNumbers.Size = new System.Drawing.Size(67, 17);
            this.radioBinNumbers.TabIndex = 25;
            this.radioBinNumbers.TabStop = true;
            this.radioBinNumbers.Text = "Numbers";
            this.radioBinNumbers.UseVisualStyleBackColor = true;
            // 
            // radioBinLetters
            // 
            this.radioBinLetters.AutoSize = true;
            this.radioBinLetters.Location = new System.Drawing.Point(3, 1);
            this.radioBinLetters.Name = "radioBinLetters";
            this.radioBinLetters.Size = new System.Drawing.Size(57, 17);
            this.radioBinLetters.TabIndex = 24;
            this.radioBinLetters.Text = "Letters";
            this.radioBinLetters.UseVisualStyleBackColor = true;
            // 
            // panelRack
            // 
            this.panelRack.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelRack.Controls.Add(this.label4);
            this.panelRack.Controls.Add(this.textRackStart);
            this.panelRack.Controls.Add(this.label5);
            this.panelRack.Controls.Add(this.textRackEnd);
            this.panelRack.Controls.Add(this.panel3);
            this.panelRack.Location = new System.Drawing.Point(3, 37);
            this.panelRack.Name = "panelRack";
            this.panelRack.Size = new System.Drawing.Size(452, 28);
            this.panelRack.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Rack Start";
            // 
            // textRackStart
            // 
            this.textRackStart.Location = new System.Drawing.Point(69, 5);
            this.textRackStart.Name = "textRackStart";
            this.textRackStart.Size = new System.Drawing.Size(50, 20);
            this.textRackStart.TabIndex = 0;
            this.textRackStart.Text = "01";
            this.textRackStart.TextChanged += new System.EventHandler(this.textRackStart_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Rack End";
            // 
            // textRackEnd
            // 
            this.textRackEnd.Location = new System.Drawing.Point(216, 5);
            this.textRackEnd.Name = "textRackEnd";
            this.textRackEnd.Size = new System.Drawing.Size(50, 20);
            this.textRackEnd.TabIndex = 21;
            this.textRackEnd.Text = "08";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioRackNumbers);
            this.panel3.Controls.Add(this.radioRackLetters);
            this.panel3.Location = new System.Drawing.Point(317, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(132, 20);
            this.panel3.TabIndex = 23;
            // 
            // radioRackNumbers
            // 
            this.radioRackNumbers.AutoSize = true;
            this.radioRackNumbers.Checked = true;
            this.radioRackNumbers.Location = new System.Drawing.Point(59, 1);
            this.radioRackNumbers.Name = "radioRackNumbers";
            this.radioRackNumbers.Size = new System.Drawing.Size(67, 17);
            this.radioRackNumbers.TabIndex = 25;
            this.radioRackNumbers.TabStop = true;
            this.radioRackNumbers.Text = "Numbers";
            this.radioRackNumbers.UseVisualStyleBackColor = true;
            // 
            // radioRackLetters
            // 
            this.radioRackLetters.AutoSize = true;
            this.radioRackLetters.Location = new System.Drawing.Point(3, 1);
            this.radioRackLetters.Name = "radioRackLetters";
            this.radioRackLetters.Size = new System.Drawing.Size(57, 17);
            this.radioRackLetters.TabIndex = 24;
            this.radioRackLetters.Text = "Letters";
            this.radioRackLetters.UseVisualStyleBackColor = true;
            // 
            // panelAisle
            // 
            this.panelAisle.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelAisle.Controls.Add(this.label2);
            this.panelAisle.Controls.Add(this.textAisleStart);
            this.panelAisle.Controls.Add(this.label3);
            this.panelAisle.Controls.Add(this.panel1);
            this.panelAisle.Controls.Add(this.textAisleEnd);
            this.panelAisle.Location = new System.Drawing.Point(3, 3);
            this.panelAisle.Name = "panelAisle";
            this.panelAisle.Size = new System.Drawing.Size(452, 28);
            this.panelAisle.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Aisle Start";
            // 
            // textAisleStart
            // 
            this.textAisleStart.Location = new System.Drawing.Point(69, 5);
            this.textAisleStart.Name = "textAisleStart";
            this.textAisleStart.Size = new System.Drawing.Size(50, 20);
            this.textAisleStart.TabIndex = 0;
            this.textAisleStart.Text = "HW";
            this.textAisleStart.TextChanged += new System.EventHandler(this.textAisleStart_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Aisle End";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioAisleNumbers);
            this.panel1.Controls.Add(this.radioAisleLetters);
            this.panel1.Location = new System.Drawing.Point(317, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 20);
            this.panel1.TabIndex = 23;
            // 
            // radioAisleNumbers
            // 
            this.radioAisleNumbers.AutoSize = true;
            this.radioAisleNumbers.Location = new System.Drawing.Point(59, 1);
            this.radioAisleNumbers.Name = "radioAisleNumbers";
            this.radioAisleNumbers.Size = new System.Drawing.Size(67, 17);
            this.radioAisleNumbers.TabIndex = 25;
            this.radioAisleNumbers.Text = "Numbers";
            this.radioAisleNumbers.UseVisualStyleBackColor = true;
            // 
            // radioAisleLetters
            // 
            this.radioAisleLetters.AutoSize = true;
            this.radioAisleLetters.Checked = true;
            this.radioAisleLetters.Location = new System.Drawing.Point(3, 1);
            this.radioAisleLetters.Name = "radioAisleLetters";
            this.radioAisleLetters.Size = new System.Drawing.Size(57, 17);
            this.radioAisleLetters.TabIndex = 24;
            this.radioAisleLetters.TabStop = true;
            this.radioAisleLetters.Text = "Letters";
            this.radioAisleLetters.UseVisualStyleBackColor = true;
            // 
            // textAisleEnd
            // 
            this.textAisleEnd.Location = new System.Drawing.Point(216, 5);
            this.textAisleEnd.Name = "textAisleEnd";
            this.textAisleEnd.Size = new System.Drawing.Size(50, 20);
            this.textAisleEnd.TabIndex = 21;
            this.textAisleEnd.Text = "HW";
            this.textAisleEnd.TextChanged += new System.EventHandler(this.textAisleEnd_TextChanged);
            // 
            // buttonDeleteAllLocs
            // 
            this.buttonDeleteAllLocs.Location = new System.Drawing.Point(280, 46);
            this.buttonDeleteAllLocs.Name = "buttonDeleteAllLocs";
            this.buttonDeleteAllLocs.Size = new System.Drawing.Size(182, 23);
            this.buttonDeleteAllLocs.TabIndex = 9;
            this.buttonDeleteAllLocs.Text = "Delete All Locations";
            this.buttonDeleteAllLocs.UseVisualStyleBackColor = true;
            this.buttonDeleteAllLocs.Click += new System.EventHandler(this.buttonDeleteAllLocations);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Location = new System.Drawing.Point(486, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(624, 428);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Currently Existing Locations";
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.Control;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.errorProvider1.SetIconAlignment(this.listView1, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.listView1.LabelWrap = false;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(119, 93);
            this.listView1.TabIndex = 0;
            this.listView1.TileSize = new System.Drawing.Size(64, 24);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Location = new System.Drawing.Point(6, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(618, 409);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.buttonCreateLocationSeq);
            this.panel4.Controls.Add(this.panelAisle);
            this.panel4.Controls.Add(this.panelRack);
            this.panel4.Controls.Add(this.panelLevel);
            this.panel4.Controls.Add(this.panelBin);
            this.panel4.Location = new System.Drawing.Point(2, 87);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(460, 174);
            this.panel4.TabIndex = 10;
            // 
            // ImportLocationDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 452);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "ImportLocationDataForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Manage Location Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelLevel.ResumeLayout(false);
            this.panelLevel.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panelBin.ResumeLayout(false);
            this.panelBin.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panelRack.ResumeLayout(false);
            this.panelRack.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelAisle.ResumeLayout(false);
            this.panelAisle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboImportLocsTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonImportLocs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioImportLocsReplace;
        private System.Windows.Forms.RadioButton radioImportLocsAppend;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioAisleNumbers;
        private System.Windows.Forms.RadioButton radioAisleLetters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textAisleEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textAisleStart;
        private System.Windows.Forms.Button buttonDeleteAllLocs;
        private System.Windows.Forms.Panel panelAisle;
        private System.Windows.Forms.Button buttonCreateLocationSeq;
        private System.Windows.Forms.Panel panelBin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBinStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBinEnd;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton radioBinNumbers;
        private System.Windows.Forms.RadioButton radioBinLetters;
        private System.Windows.Forms.Panel panelRack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textRackStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textRackEnd;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioRackNumbers;
        private System.Windows.Forms.RadioButton radioRackLetters;
        private System.Windows.Forms.Panel panelLevel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textLevelStart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textLevelEnd;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RadioButton radioLevelNumbers;
        private System.Windows.Forms.RadioButton radioLevelLetters;
        private System.Windows.Forms.ComboBox comboCreateLocs;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
    }
}