namespace FKAroundTIA.Forms
{
    partial class MainForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblRuntime = new System.Windows.Forms.Label();
            this.cbRuntimes = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dgvInterface = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBinding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBatchAdd = new System.Windows.Forms.Button();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInterface)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnBatchAdd);
            this.pnlTop.Controls.Add(this.btnApplyChanges);
            this.pnlTop.Controls.Add(this.lblRuntime);
            this.pnlTop.Controls.Add(this.cbRuntimes);
            this.pnlTop.Controls.Add(this.lblStatus);
            this.pnlTop.Controls.Add(this.btnLoad);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 50);
            this.pnlTop.TabIndex = 0;
            // 
            // lblRuntime
            // 
            this.lblRuntime.AutoSize = true;
            this.lblRuntime.Location = new System.Drawing.Point(120, 18);
            this.lblRuntime.Name = "lblRuntime";
            this.lblRuntime.Size = new System.Drawing.Size(85, 13);
            this.lblRuntime.TabIndex = 3;
            this.lblRuntime.Text = "Unified Runtime:";
            this.lblRuntime.Visible = false;
            // 
            // cbRuntimes
            // 
            this.cbRuntimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRuntimes.FormattingEnabled = true;
            this.cbRuntimes.Location = new System.Drawing.Point(220, 15);
            this.cbRuntimes.Name = "cbRuntimes";
            this.cbRuntimes.Size = new System.Drawing.Size(250, 21);
            this.cbRuntimes.TabIndex = 2;
            this.cbRuntimes.Visible = false;
            this.cbRuntimes.SelectedIndexChanged += new System.EventHandler(this.cbRuntimes_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(490, 18);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(240, 26);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Not connected.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(90, 26);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvInterface);
            this.splitContainer1.Size = new System.Drawing.Size(984, 511);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(320, 511);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // dgvInterface
            // 
            this.dgvInterface.AllowUserToAddRows = false;
            this.dgvInterface.AllowUserToDeleteRows = false;
            this.dgvInterface.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInterface.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInterface.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDisplayName,
            this.colDirection,
            this.colDataType,
            this.colValue,
            this.colBinding,
            this.colComment});
            this.dgvInterface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInterface.Location = new System.Drawing.Point(0, 0);
            this.dgvInterface.MultiSelect = true;
            this.dgvInterface.Name = "dgvInterface";
            this.dgvInterface.ReadOnly = true;
            this.dgvInterface.RowHeadersVisible = false;
            this.dgvInterface.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInterface.Size = new System.Drawing.Size(660, 511);
            this.dgvInterface.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colDisplayName
            // 
            this.colDisplayName.DataPropertyName = "DisplayName";
            this.colDisplayName.HeaderText = "DisplayName";
            this.colDisplayName.Name = "colDisplayName";
            this.colDisplayName.ReadOnly = true;
            // 
            // colDirection
            // 
            this.colDirection.DataPropertyName = "Direction";
            this.colDirection.HeaderText = "Direction";
            this.colDirection.Name = "colDirection";
            this.colDirection.ReadOnly = true;
            // 
            // colDataType
            // 
            this.colDataType.DataPropertyName = "DataType";
            this.colDataType.HeaderText = "DataType";
            this.colDataType.Name = "colDataType";
            this.colDataType.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.DataPropertyName = "CurrentValue";
            this.colValue.HeaderText = "CurrentValue";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // colBinding
            // 
            this.colBinding.DataPropertyName = "Binding";
            this.colBinding.HeaderText = "Binding";
            this.colBinding.Name = "colBinding";
            this.colBinding.ReadOnly = true;
            // 
            // colComment
            // 
            this.colComment.DataPropertyName = "Comment";
            this.colComment.HeaderText = "Comment";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            // 
            // btnBatchAdd
            // 
            this.btnBatchAdd.Enabled = false;
            this.btnBatchAdd.Location = new System.Drawing.Point(744, 13);
            this.btnBatchAdd.Name = "btnBatchAdd";
            this.btnBatchAdd.Size = new System.Drawing.Size(96, 23);
            this.btnBatchAdd.TabIndex = 4;
            this.btnBatchAdd.Text = "Batch Add";
            this.btnBatchAdd.UseVisualStyleBackColor = true;
            this.btnBatchAdd.Click += new System.EventHandler(this.btnBatchAdd_Click);
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Location = new System.Drawing.Point(850, 13);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(122, 23);
            this.btnApplyChanges.TabIndex = 5;
            this.btnApplyChanges.Text = "Apply Changes";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyToAll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlTop);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinCC Unified Faceplate Analyzer";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInterface)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRuntime;
        private System.Windows.Forms.ComboBox cbRuntimes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dgvInterface;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBinding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.Button btnBatchAdd;
        private System.Windows.Forms.Button btnApplyChanges;
    }
}
