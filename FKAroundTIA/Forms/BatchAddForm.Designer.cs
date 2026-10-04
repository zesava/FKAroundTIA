namespace FKAroundTIA.Forms
{
    partial class BatchAddForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblFaceplateType = new System.Windows.Forms.Label();
            this.cbFaceplateType = new System.Windows.Forms.ComboBox();
            this.lblTargetScreen = new System.Windows.Forms.Label();
            this.cbTargetScreen = new System.Windows.Forms.ComboBox();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.lblCounterDigits = new System.Windows.Forms.Label();
            this.nudCounterDigits = new System.Windows.Forms.NumericUpDown();
            this.lblStartNumber = new System.Windows.Forms.Label();
            this.nudStartNumber = new System.Windows.Forms.NumericUpDown();
            this.lblInstanceCount = new System.Windows.Forms.Label();
            this.nudInstanceCount = new System.Windows.Forms.NumericUpDown();
            this.lblStartX = new System.Windows.Forms.Label();
            this.nudStartX = new System.Windows.Forms.NumericUpDown();
            this.lblStartY = new System.Windows.Forms.Label();
            this.nudStartY = new System.Windows.Forms.NumericUpDown();
            this.lblHorizontalGap = new System.Windows.Forms.Label();
            this.nudHorizontalGap = new System.Windows.Forms.NumericUpDown();
            this.lblVerticalGap = new System.Windows.Forms.Label();
            this.nudVerticalGap = new System.Windows.Forms.NumericUpDown();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCounterDigits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstanceCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorizontalGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVerticalGap)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lblFaceplateType, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.cbFaceplateType, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lblTargetScreen, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.cbTargetScreen, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lblPrefix, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.txtPrefix, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.lblCounterDigits, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.nudCounterDigits, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.lblStartNumber, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.nudStartNumber, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.lblInstanceCount, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.nudInstanceCount, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.lblStartX, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.nudStartX, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.lblStartY, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.nudStartY, 1, 7);
            this.tableLayoutPanel.Controls.Add(this.lblHorizontalGap, 0, 8);
            this.tableLayoutPanel.Controls.Add(this.nudHorizontalGap, 1, 8);
            this.tableLayoutPanel.Controls.Add(this.lblVerticalGap, 0, 9);
            this.tableLayoutPanel.Controls.Add(this.nudVerticalGap, 1, 9);
            this.tableLayoutPanel.Controls.Add(this.pnlButtons, 0, 10);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 11;
            for (int i = 0; i < 10; i++)
            {
                this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            }
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(440, 382);
            this.tableLayoutPanel.TabIndex = 0;
            this.tableLayoutPanel.SetColumnSpan(this.pnlButtons, 2);
            // 
            // labels
            // 
            ConfigureLabel(this.lblFaceplateType, "Faceplate type:", 0);
            ConfigureLabel(this.lblTargetScreen, "Target screen:", 1);
            ConfigureLabel(this.lblPrefix, "Name prefix:", 2);
            ConfigureLabel(this.lblCounterDigits, "Counter digits:", 3);
            ConfigureLabel(this.lblStartNumber, "Start number:", 4);
            ConfigureLabel(this.lblInstanceCount, "Instance count:", 5);
            ConfigureLabel(this.lblStartX, "Start X:", 6);
            ConfigureLabel(this.lblStartY, "Start Y:", 7);
            ConfigureLabel(this.lblHorizontalGap, "Horizontal gap:", 8);
            ConfigureLabel(this.lblVerticalGap, "Vertical gap:", 9);
            // 
            // combo boxes and text
            // 
            this.cbFaceplateType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbFaceplateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFaceplateType.FormattingEnabled = true;
            this.cbFaceplateType.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cbFaceplateType.Name = "cbFaceplateType";
            this.cbFaceplateType.TabIndex = 0;
            this.cbTargetScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTargetScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetScreen.FormattingEnabled = true;
            this.cbTargetScreen.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cbTargetScreen.Name = "cbTargetScreen";
            this.cbTargetScreen.TabIndex = 1;
            this.txtPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.TabIndex = 2;
            // 
            // numeric inputs
            // 
            ConfigureNumeric(this.nudCounterDigits, 1, 10, 2, 3);
            ConfigureNumeric(this.nudStartNumber, 0, 999999999, 1, 4);
            ConfigureNumeric(this.nudInstanceCount, 1, 10000, 1, 5);
            ConfigureNumeric(this.nudStartX, 0, 100000, 0, 6);
            ConfigureNumeric(this.nudStartY, 0, 100000, 0, 7);
            ConfigureNumeric(this.nudHorizontalGap, 0, 10000, 20, 8);
            ConfigureNumeric(this.nudVerticalGap, 0, 10000, 20, 9);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlButtons.Location = new System.Drawing.Point(0, 340);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlButtons.Size = new System.Drawing.Size(440, 42);
            this.pnlButtons.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(349, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(255, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // BatchAddForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(464, 406);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchAddForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Add Faceplates";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCounterDigits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstanceCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHorizontalGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVerticalGap)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void ConfigureLabel(System.Windows.Forms.Label label, string text, int tabIndex)
        {
            label.AutoSize = true;
            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            label.Name = "label" + tabIndex;
            label.TabIndex = 20 + tabIndex;
            label.Text = text;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        private static void ConfigureNumeric(
            System.Windows.Forms.NumericUpDown control,
            decimal minimum,
            decimal maximum,
            decimal value,
            int tabIndex)
        {
            control.Dock = System.Windows.Forms.DockStyle.Left;
            control.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            control.Maximum = maximum;
            control.Minimum = minimum;
            control.Name = "numeric" + tabIndex;
            control.Size = new System.Drawing.Size(120, 20);
            control.TabIndex = tabIndex;
            control.Value = value;
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblFaceplateType;
        private System.Windows.Forms.ComboBox cbFaceplateType;
        private System.Windows.Forms.Label lblTargetScreen;
        private System.Windows.Forms.ComboBox cbTargetScreen;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label lblCounterDigits;
        private System.Windows.Forms.NumericUpDown nudCounterDigits;
        private System.Windows.Forms.Label lblStartNumber;
        private System.Windows.Forms.NumericUpDown nudStartNumber;
        private System.Windows.Forms.Label lblInstanceCount;
        private System.Windows.Forms.NumericUpDown nudInstanceCount;
        private System.Windows.Forms.Label lblStartX;
        private System.Windows.Forms.NumericUpDown nudStartX;
        private System.Windows.Forms.Label lblStartY;
        private System.Windows.Forms.NumericUpDown nudStartY;
        private System.Windows.Forms.Label lblHorizontalGap;
        private System.Windows.Forms.NumericUpDown nudHorizontalGap;
        private System.Windows.Forms.Label lblVerticalGap;
        private System.Windows.Forms.NumericUpDown nudVerticalGap;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}
