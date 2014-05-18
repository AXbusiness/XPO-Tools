namespace AXbusiness.XpoTools
{
    partial class axtForm_XpoFileViewer
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdImportXpo = new System.Windows.Forms.Button();
            this.lblApplicationObjects = new System.Windows.Forms.Label();
            this.tvApplicationObjects = new System.Windows.Forms.TreeView();
            this.rtfContent = new System.Windows.Forms.RichTextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.cmdFont = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayout_ApplicationObjects = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayout_Content = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayout_Main = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayout_Buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox_Project = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblXpoFilename = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblXpoFilelocation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayout_ApplicationObjects.SuspendLayout();
            this.tableLayout_Content.SuspendLayout();
            this.tableLayout_Main.SuspendLayout();
            this.flowLayout_Buttons.SuspendLayout();
            this.groupBox_Project.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdImportXpo
            // 
            this.cmdImportXpo.Location = new System.Drawing.Point(3, 3);
            this.cmdImportXpo.Name = "cmdImportXpo";
            this.cmdImportXpo.Size = new System.Drawing.Size(120, 23);
            this.cmdImportXpo.TabIndex = 2;
            this.cmdImportXpo.Text = "Import XPO";
            this.cmdImportXpo.UseVisualStyleBackColor = true;
            this.cmdImportXpo.Click += new System.EventHandler(this.cmdImportXpo_Click);
            // 
            // lblApplicationObjects
            // 
            this.lblApplicationObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationObjects.Location = new System.Drawing.Point(3, 0);
            this.lblApplicationObjects.Name = "lblApplicationObjects";
            this.lblApplicationObjects.Size = new System.Drawing.Size(194, 14);
            this.lblApplicationObjects.TabIndex = 2;
            this.lblApplicationObjects.Text = "Application objects";
            // 
            // tvApplicationObjects
            // 
            this.tvApplicationObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvApplicationObjects.Location = new System.Drawing.Point(3, 17);
            this.tvApplicationObjects.Name = "tvApplicationObjects";
            this.tvApplicationObjects.Size = new System.Drawing.Size(194, 458);
            this.tvApplicationObjects.TabIndex = 3;
            this.tvApplicationObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvApplicationObjects_AfterSelect);
            // 
            // rtfContent
            // 
            this.rtfContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfContent.Location = new System.Drawing.Point(3, 17);
            this.rtfContent.Name = "rtfContent";
            this.rtfContent.ReadOnly = true;
            this.rtfContent.Size = new System.Drawing.Size(639, 458);
            this.rtfContent.TabIndex = 5;
            this.rtfContent.Text = "";
            this.rtfContent.WordWrap = false;
            // 
            // lblContent
            // 
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Location = new System.Drawing.Point(3, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(639, 14);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "Content";
            // 
            // cmdFont
            // 
            this.cmdFont.Location = new System.Drawing.Point(3, 32);
            this.cmdFont.Name = "cmdFont";
            this.cmdFont.Size = new System.Drawing.Size(120, 23);
            this.cmdFont.TabIndex = 6;
            this.cmdFont.Text = "Font";
            this.cmdFont.UseVisualStyleBackColor = true;
            this.cmdFont.Click += new System.EventHandler(this.cmdFont_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(3, 79);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayout_ApplicationObjects);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayout_Content);
            this.splitContainer.Size = new System.Drawing.Size(848, 478);
            this.splitContainer.SplitterDistance = 200;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 9;
            // 
            // tableLayout_ApplicationObjects
            // 
            this.tableLayout_ApplicationObjects.ColumnCount = 1;
            this.tableLayout_ApplicationObjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_ApplicationObjects.Controls.Add(this.lblApplicationObjects, 0, 0);
            this.tableLayout_ApplicationObjects.Controls.Add(this.tvApplicationObjects, 0, 1);
            this.tableLayout_ApplicationObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_ApplicationObjects.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_ApplicationObjects.Name = "tableLayout_ApplicationObjects";
            this.tableLayout_ApplicationObjects.RowCount = 2;
            this.tableLayout_ApplicationObjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_ApplicationObjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_ApplicationObjects.Size = new System.Drawing.Size(200, 478);
            this.tableLayout_ApplicationObjects.TabIndex = 10;
            // 
            // tableLayout_Content
            // 
            this.tableLayout_Content.ColumnCount = 1;
            this.tableLayout_Content.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Content.Controls.Add(this.lblContent, 0, 0);
            this.tableLayout_Content.Controls.Add(this.rtfContent, 0, 1);
            this.tableLayout_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Content.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_Content.Name = "tableLayout_Content";
            this.tableLayout_Content.RowCount = 2;
            this.tableLayout_Content.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Content.Size = new System.Drawing.Size(645, 478);
            this.tableLayout_Content.TabIndex = 10;
            // 
            // tableLayout_Main
            // 
            this.tableLayout_Main.ColumnCount = 2;
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout_Main.Controls.Add(this.flowLayout_Buttons, 1, 1);
            this.tableLayout_Main.Controls.Add(this.splitContainer, 0, 1);
            this.tableLayout_Main.Controls.Add(this.groupBox_Project, 0, 0);
            this.tableLayout_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Main.Location = new System.Drawing.Point(3, 3);
            this.tableLayout_Main.Name = "tableLayout_Main";
            this.tableLayout_Main.RowCount = 2;
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Main.Size = new System.Drawing.Size(986, 560);
            this.tableLayout_Main.TabIndex = 11;
            // 
            // flowLayout_Buttons
            // 
            this.flowLayout_Buttons.AutoSize = true;
            this.flowLayout_Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayout_Buttons.Controls.Add(this.cmdImportXpo);
            this.flowLayout_Buttons.Controls.Add(this.cmdFont);
            this.flowLayout_Buttons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayout_Buttons.Location = new System.Drawing.Point(857, 79);
            this.flowLayout_Buttons.Name = "flowLayout_Buttons";
            this.flowLayout_Buttons.Size = new System.Drawing.Size(126, 58);
            this.flowLayout_Buttons.TabIndex = 12;
            // 
            // groupBox_Project
            // 
            this.tableLayout_Main.SetColumnSpan(this.groupBox_Project, 2);
            this.groupBox_Project.Controls.Add(this.lblXpoFilelocation);
            this.groupBox_Project.Controls.Add(this.label3);
            this.groupBox_Project.Controls.Add(this.lblXpoFilename);
            this.groupBox_Project.Controls.Add(this.label1);
            this.groupBox_Project.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Project.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Project.Name = "groupBox_Project";
            this.groupBox_Project.Size = new System.Drawing.Size(980, 70);
            this.groupBox_Project.TabIndex = 0;
            this.groupBox_Project.TabStop = false;
            this.groupBox_Project.Text = " Project ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename:";
            // 
            // lblXpoFilename
            // 
            this.lblXpoFilename.AutoSize = true;
            this.lblXpoFilename.Location = new System.Drawing.Point(80, 22);
            this.lblXpoFilename.Name = "lblXpoFilename";
            this.lblXpoFilename.Size = new System.Drawing.Size(11, 14);
            this.lblXpoFilename.TabIndex = 1;
            this.lblXpoFilename.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Location:";
            // 
            // lblXpoFilelocation
            // 
            this.lblXpoFilelocation.AutoSize = true;
            this.lblXpoFilelocation.Location = new System.Drawing.Point(80, 40);
            this.lblXpoFilelocation.Name = "lblXpoFilelocation";
            this.lblXpoFilelocation.Size = new System.Drawing.Size(11, 14);
            this.lblXpoFilelocation.TabIndex = 3;
            this.lblXpoFilelocation.Text = ".";
            // 
            // axtForm_XpoFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 566);
            this.Controls.Add(this.tableLayout_Main);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(550, 350);
            this.Name = "axtForm_XpoFileViewer";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "XPO-File Viewer";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayout_ApplicationObjects.ResumeLayout(false);
            this.tableLayout_Content.ResumeLayout(false);
            this.tableLayout_Main.ResumeLayout(false);
            this.tableLayout_Main.PerformLayout();
            this.flowLayout_Buttons.ResumeLayout(false);
            this.groupBox_Project.ResumeLayout(false);
            this.groupBox_Project.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdImportXpo;
        private System.Windows.Forms.TreeView tvApplicationObjects;
        private System.Windows.Forms.Label lblApplicationObjects;
        private System.Windows.Forms.RichTextBox rtfContent;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Button cmdFont;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayout_ApplicationObjects;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Content;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Main;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Buttons;
        private System.Windows.Forms.GroupBox groupBox_Project;
        private System.Windows.Forms.Label lblXpoFilelocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblXpoFilename;
        private System.Windows.Forms.Label label1;
    }
}