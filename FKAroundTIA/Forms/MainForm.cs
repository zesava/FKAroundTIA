using FKAroundTIA.Models;
using FKAroundTIA.Services;
using Microsoft.Win32;
using Siemens.Engineering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FKAroundTIA.Forms
{
    public partial class MainForm : Form
    {
        private readonly TiaPortalService _tiaPortalService = new TiaPortalService();
        private readonly RuntimeService _runtimeService = new RuntimeService();
        private readonly GridClipboardService _gridClipboardService = new GridClipboardService();
        private FaceplateGroupService _faceplateGroupService;
        private BatchAddService _batchAddService;

        private IList<RuntimeInfo> _runtimes;
        private RuntimeInfo _selectedRuntime;

        static MainForm()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveTiaAssembly;
        }

        public MainForm()
        {
            InitializeComponent();

            dgvInterface.MultiSelect = true;
            dgvInterface.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvInterface.KeyDown += dgvInterface_KeyDown;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Connecting to TIA Portal...";
                Application.DoEvents();

                var project = _tiaPortalService.Connect();
                lblStatus.Text = $"Connected to project: {project.Name}";

                _runtimes = _runtimeService.GetRuntimes(project);

                if (_runtimes.Count == 0)
                {
                    lblStatus.Text = "No WinCC Unified PC Runtimes found.";
                    MessageBox.Show("No WinCC Unified PC Runtime devices found in the active project.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearUI();
                    return;
                }

                if (_runtimes.Count == 1)
                {
                    lblRuntime.Visible = false;
                    cbRuntimes.Visible = false;
                    _selectedRuntime = _runtimes[0];
                    lblStatus.Text = $"Using Runtime: {_selectedRuntime.Name}";
                    LoadScreenAndFaceplates();
                }
                else
                {
                    cbRuntimes.Items.Clear();
                    foreach (var rt in _runtimes)
                    {
                        cbRuntimes.Items.Add($"{rt.DeviceName} -> {rt.Name}");
                    }
                    lblRuntime.Visible = true;
                    cbRuntimes.Visible = true;
                    _selectedRuntime = null;
                    lblStatus.Text = "Multiple runtimes found. Please select one.";
                    ClearUI();
                }
            }
            catch (Exception ex)
            {
                LogException("Failed to load TIA Portal project or runtimes.", ex);
            }
        }

        private void cbRuntimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRuntimes.SelectedIndex >= 0 && _runtimes != null)
            {
                _selectedRuntime = _runtimes[cbRuntimes.SelectedIndex];
                lblStatus.Text = $"Selected Runtime: {_selectedRuntime.Name}";
                LoadScreenAndFaceplates();
            }
        }

        private void LoadScreenAndFaceplates()
        {
            if (_selectedRuntime == null) return;

            btnBatchAdd.Enabled = false;
            _batchAddService = null;

            try
            {
                lblStatus.Text = "Scanning screens and loading faceplate groups...";
                Application.DoEvents();

                _faceplateGroupService = new FaceplateGroupService(_selectedRuntime);
                _batchAddService = new BatchAddService(
                    _selectedRuntime,
                    _tiaPortalService.AttachedPortal,
                    _tiaPortalService.ActiveProject);
                var screens = _faceplateGroupService.GetFaceplateGroupsByScreen();

                PopulateTreeView(screens);
                btnBatchAdd.Enabled = true;

                int groupCount = screens.Sum(screen => screen.Groups.Count);
                int instanceCount = screens.Sum(screen => screen.InstanceCount);
                lblStatus.Text = $"Loaded {screens.Count} screens, {groupCount} faceplate groups and {instanceCount} instances.";

                if (instanceCount == 0)
                {
                    MessageBox.Show("No Faceplate Instances found in the selected Runtime.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogException("Error loading screens or faceplates.", ex);
            }
        }

        private void PopulateTreeView(IList<ScreenFaceplateInfo> screens)
        {
            treeView1.Nodes.Clear();
            dgvInterface.DataSource = null;

            foreach (var screen in screens)
            {
                var screenNode = new TreeNode($"{screen.Name} [{screen.InstanceCount}]")
                {
                    Tag = screen
                };

                foreach (var group in screen.Groups)
                {
                    screenNode.Nodes.Add(new TreeNode($"{group.TypeName} [{group.Count}]")
                    {
                        Tag = group
                    });
                }

                treeView1.Nodes.Add(screenNode);
            }
            treeView1.ExpandAll();
        }

        private void ConfigureGridColumns()
        {
            dgvInterface.ReadOnly = false;
            dgvInterface.MultiSelect = true;
            dgvInterface.SelectionMode = DataGridViewSelectionMode.CellSelect;

            foreach (DataGridViewColumn col in dgvInterface.Columns)
            {
                if (col.Name == "Faceplate Name")
                {
                    col.ReadOnly = true;
                }
                else
                {
                    col.ReadOnly = false;
                }
            }
        }

        private void dgvInterface_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    _gridClipboardService.CopySelectedCells(dgvInterface);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.Control && e.KeyCode == Keys.V)
                {
                    int changedCells = _gridClipboardService.PasteClipboardValues(dgvInterface, "Faceplate Name");
                    lblStatus.Text = changedCells > 0
                        ? $"Pasted {changedCells} cells. Click Apply Changes to write them to TIA."
                        : "No editable cells were pasted.";
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception ex)
            {
                LogException("Clipboard operation failed.", ex);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is FaceplateTypeGroupInfo group)
            {
                try
                {
                    if (group.GridData == null)
                    {
                        group.GridData = _faceplateGroupService.GetInterfacePropertiesTable(group);
                    }

                    dgvInterface.Columns.Clear();
                    dgvInterface.AutoGenerateColumns = true;
                    dgvInterface.DataSource = group.GridData;

                    ConfigureGridColumns();
                }
                catch (Exception ex)
                {
                    LogException($"Failed to load properties for faceplate group '{group.TypeName}'.", ex);
                }
            }
            else
            {
                dgvInterface.DataSource = null;
            }
        }

        private void btnApplyToAll_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is FaceplateTypeGroupInfo group)
            {
                try
                {
                    dgvInterface.EndEdit();

                    var valuesByInstanceName = new Dictionary<string, IDictionary<string, string>>();
                    if (group.GridData != null)
                    {
                        foreach (DataRow row in group.GridData.Rows)
                        {
                            string instanceName = row["Faceplate Name"]?.ToString();
                            if (string.IsNullOrEmpty(instanceName))
                            {
                                continue;
                            }

                            var propertyValues = new Dictionary<string, string>();
                            foreach (DataColumn column in group.GridData.Columns)
                            {
                                if (column.ColumnName == "Faceplate Name")
                                {
                                    continue;
                                }

                                propertyValues[column.ColumnName] = row[column.ColumnName]?.ToString() ?? "";
                            }

                            valuesByInstanceName[instanceName] = propertyValues;
                        }
                    }

                    if (valuesByInstanceName.Count == 0)
                    {
                        MessageBox.Show("No faceplate instance rows found to apply.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    lblStatus.Text = $"Applying all property values for {valuesByInstanceName.Count} instances...";
                    Application.DoEvents();

                    _faceplateGroupService.ApplyChangesToAll(group, valuesByInstanceName);

                    lblStatus.Text = "Changes applied successfully.";
                }
                catch (Exception ex)
                {
                    LogException($"Failed to apply changes for group '{group.TypeName}'.", ex);
                }
            }
            else
            {
                MessageBox.Show("Please select a Faceplate Type group from the list first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            if (_batchAddService == null)
            {
                MessageBox.Show(
                    "Load a Runtime before adding faceplates.",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                var faceplateTypes = _batchAddService.GetFaceplateTypes();
                var screens = _batchAddService.GetScreens();

                using (var dialog = new BatchAddForm(faceplateTypes, screens))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    lblStatus.Text = "Adding faceplate instances...";
                    Application.DoEvents();

                    int createdCount = _batchAddService.AddFaceplates(dialog.Request);
                    string targetScreenName = dialog.Request.TargetScreen.Name;

                    LoadScreenAndFaceplates();
                    lblStatus.Text = $"Created {createdCount} faceplate instances on screen '{targetScreenName}'.";
                }
            }
            catch (Exception ex)
            {
                LogException("Failed to batch add faceplates.", ex);
            }
        }

        private void ClearUI()
        {
            treeView1.Nodes.Clear();
            dgvInterface.DataSource = null;
            btnBatchAdd.Enabled = false;
            _batchAddService = null;
        }

        private void LogException(string message, Exception ex)
        {
            lblStatus.Text = "An error occurred.";
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            try
            {
                string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}\nException: {ex.Message}\nStackTrace:\n{ex.StackTrace}\n\n";
                File.AppendAllText(logPath, logContent);
            }
            catch
            {
                // Ignore log writing errors
            }

            MessageBox.Show($"{message}\n\nDetails: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static Assembly ResolveTiaAssembly(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name;
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Siemens\Automation\Openness\21.0\PublicAPI\21.0.0.0\net48"))
            {
                if (key != null)
                {
                    foreach (var valueName in key.GetValueNames())
                    {
                        if (valueName.StartsWith(name))
                        {
                            var path = key.GetValue(valueName) as string;
                            if (!string.IsNullOrEmpty(path) && File.Exists(path)) return Assembly.LoadFrom(path);
                        }
                    }
                }
            }
            return null;
        }
    }
}
