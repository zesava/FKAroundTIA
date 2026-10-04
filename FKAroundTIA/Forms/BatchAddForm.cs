using FKAroundTIA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FKAroundTIA.Forms
{
    public partial class BatchAddForm : Form
    {
        public BatchAddForm(
            IList<BatchAddFaceplateTypeInfo> faceplateTypes,
            IList<BatchAddScreenInfo> screens)
        {
            InitializeComponent();

            cbFaceplateType.DataSource = (faceplateTypes ?? new List<BatchAddFaceplateTypeInfo>()).ToList();
            cbTargetScreen.DataSource = (screens ?? new List<BatchAddScreenInfo>()).ToList();
        }

        public BatchAddRequest Request { get; private set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!(cbFaceplateType.SelectedItem is BatchAddFaceplateTypeInfo faceplateType))
            {
                ShowValidationMessage("Select a faceplate type.", cbFaceplateType);
                return;
            }

            if (!(cbTargetScreen.SelectedItem is BatchAddScreenInfo targetScreen))
            {
                ShowValidationMessage("Select a target screen.", cbTargetScreen);
                return;
            }

            string prefix = txtPrefix.Text.Trim();
            if (string.IsNullOrEmpty(prefix))
            {
                ShowValidationMessage("Enter an instance name prefix.", txtPrefix);
                return;
            }

            Request = new BatchAddRequest
            {
                FaceplateType = faceplateType,
                TargetScreen = targetScreen,
                Prefix = prefix,
                CounterDigits = (int)nudCounterDigits.Value,
                StartNumber = (int)nudStartNumber.Value,
                InstanceCount = (int)nudInstanceCount.Value,
                StartX = (int)nudStartX.Value,
                StartY = (int)nudStartY.Value,
                HorizontalGap = (int)nudHorizontalGap.Value,
                VerticalGap = (int)nudVerticalGap.Value
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private static void ShowValidationMessage(string message, Control control)
        {
            MessageBox.Show(message, "Batch Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
            control.Focus();
        }
    }
}
