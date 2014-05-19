namespace AXbusiness.XpoTools
{
    partial class axtForm_XpoContentSelection
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
            this.lblFilename = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.Label();
            this.tvXpoContent = new System.Windows.Forms.TreeView();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.flowLayout_Buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayout_Filename = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayout_Main = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayout_Buttons.SuspendLayout();
            this.flowLayout_Filename.SuspendLayout();
            this.tableLayout_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(3, 3);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(58, 14);
            this.lblFilename.TabIndex = 0;
            this.lblFilename.Text = "Filename:";
            // 
            // txtFilename
            // 
            this.txtFilename.AutoSize = true;
            this.txtFilename.Location = new System.Drawing.Point(67, 3);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(19, 14);
            this.txtFilename.TabIndex = 1;
            this.txtFilename.Text = "...";
            // 
            // tvXpoContent
            // 
            this.tvXpoContent.CheckBoxes = true;
            this.tvXpoContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvXpoContent.Location = new System.Drawing.Point(3, 31);
            this.tvXpoContent.Name = "tvXpoContent";
            this.tvXpoContent.Size = new System.Drawing.Size(435, 298);
            this.tvXpoContent.TabIndex = 2;
            this.tvXpoContent.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvXpoContent_AfterCheck);
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(3, 3);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(3, 32);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.tableLayout_Main.SetColumnSpan(this.chkSelectAll, 2);
            this.chkSelectAll.Location = new System.Drawing.Point(3, 335);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(74, 18);
            this.chkSelectAll.TabIndex = 5;
            this.chkSelectAll.Text = "Select all";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // flowLayout_Buttons
            // 
            this.flowLayout_Buttons.AutoSize = true;
            this.flowLayout_Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayout_Buttons.Controls.Add(this.cmdOK);
            this.flowLayout_Buttons.Controls.Add(this.cmdCancel);
            this.flowLayout_Buttons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayout_Buttons.Location = new System.Drawing.Point(444, 31);
            this.flowLayout_Buttons.Name = "flowLayout_Buttons";
            this.flowLayout_Buttons.Size = new System.Drawing.Size(81, 58);
            this.flowLayout_Buttons.TabIndex = 6;
            // 
            // flowLayout_Filename
            // 
            this.tableLayout_Main.SetColumnSpan(this.flowLayout_Filename, 2);
            this.flowLayout_Filename.Controls.Add(this.lblFilename);
            this.flowLayout_Filename.Controls.Add(this.txtFilename);
            this.flowLayout_Filename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout_Filename.Location = new System.Drawing.Point(3, 3);
            this.flowLayout_Filename.Name = "flowLayout_Filename";
            this.flowLayout_Filename.Padding = new System.Windows.Forms.Padding(0, 3, 0, 5);
            this.flowLayout_Filename.Size = new System.Drawing.Size(522, 22);
            this.flowLayout_Filename.TabIndex = 7;
            this.flowLayout_Filename.WrapContents = false;
            // 
            // tableLayout_Main
            // 
            this.tableLayout_Main.ColumnCount = 2;
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout_Main.Controls.Add(this.flowLayout_Filename, 0, 0);
            this.tableLayout_Main.Controls.Add(this.chkSelectAll, 0, 2);
            this.tableLayout_Main.Controls.Add(this.tvXpoContent, 0, 1);
            this.tableLayout_Main.Controls.Add(this.flowLayout_Buttons, 1, 1);
            this.tableLayout_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Main.Location = new System.Drawing.Point(3, 3);
            this.tableLayout_Main.Name = "tableLayout_Main";
            this.tableLayout_Main.RowCount = 3;
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout_Main.Size = new System.Drawing.Size(528, 356);
            this.tableLayout_Main.TabIndex = 2;
            // 
            // axtForm_XpoContentSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 362);
            this.Controls.Add(this.tableLayout_Main);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(550, 300);
            this.Name = "axtForm_XpoContentSelection";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "XPO Content Selection";
            this.Load += new System.EventHandler(this.frmXpoContentSelection_Load);
            this.flowLayout_Buttons.ResumeLayout(false);
            this.flowLayout_Filename.ResumeLayout(false);
            this.flowLayout_Filename.PerformLayout();
            this.tableLayout_Main.ResumeLayout(false);
            this.tableLayout_Main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Label txtFilename;
        private System.Windows.Forms.TreeView tvXpoContent;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Buttons;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Filename;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Main;
    }
}