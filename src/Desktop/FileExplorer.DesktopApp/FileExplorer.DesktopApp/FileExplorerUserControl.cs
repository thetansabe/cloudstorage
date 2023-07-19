using System.ComponentModel;
using System.Data;
namespace FileExplorer.DesktopApp
{
    public partial class FileExplorerUserControl : UserControl
    {
        private BindingList<DirectoryItem> _directoryList = new BindingList<DirectoryItem>();
        private bool _isFirstClickOnTxtPath = true;
        private string _currentPath = string.Empty;
        private bool _resetSelectedRow = false;
        private int _currentRowIndex;
        private string _newEditedName = string.Empty;
        private bool _isCellContentChanged = false;

        public FileExplorerUserControl()
        {
            InitializeComponent();
            LoadComboBoxPartionsNavigationAndPath();
            LoadDirectoryItems();
        }

        private void HandleDtFoldersAndFilesOnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                e.CellStyle.SelectionBackColor = Color.Transparent;
                using (var pen = new Pen(e.CellStyle.BackColor))
                    e.Graphics.FillRectangle(pen.Brush, e.CellBounds);
                e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentBackground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.Focus);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

        private void LoadComboBoxPartionsNavigationAndPath()
        {
            string[] drive = Directory.GetLogicalDrives();

            foreach (string driveName in drive)
            {
                cmbPartition.Items.Add(driveName);
            }

            cmbPartition.SelectedIndex = 0;
            cmbPartition.DropDownStyle = ComboBoxStyle.DropDownList;

            txtPath.Text = cmbPartition.SelectedItem.ToString();
        }

        private void LoadDirectoryItems()
        {
            string? selectedItem = cmbPartition.SelectedItem.ToString();

            if (Directory.Exists(selectedItem))
                LoadDirectoryItemsByPath(selectedItem);
            else
                MessageBox.Show("Can't find \"" + selectedItem + "\"", "Open disk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void HandleDtFoldersAndFilesOnCellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.ForeColor = Color.Black;
            cellStyle.BackColor = Color.FromArgb(188, 220, 244);

            if (e.RowIndex > -1)
            {
                dtFoldersAndFiles.Rows[e.RowIndex].DefaultCellStyle = cellStyle;
                dtFoldersAndFiles.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
                dtFoldersAndFiles.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void HandleDtFoldersAndFilesOnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.ForeColor = Color.Black;
            cellStyle.BackColor = Color.White;

            if (e.RowIndex > -1)
                dtFoldersAndFiles.Rows[e.RowIndex].DefaultCellStyle = cellStyle;
        }

        private void HandleDtFoldersAndFilesOnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            dtFoldersAndFiles.Rows[e.RowIndex].Selected = true;
            dtFoldersAndFiles.CurrentCell = dtFoldersAndFiles[1, e.RowIndex];
            btnDelete.Enabled = true;
            btnRename.Enabled = true;
        }

        private void HandleFmMainOnLoad(object sender, EventArgs e)
        {
            if (dtFoldersAndFiles.Rows.Count > 0)
            {
                dtFoldersAndFiles.Rows[0].Selected = false;
            }

            btnDelete.Enabled = false;
            btnRename.Enabled = false;
            btnBack.Enabled = false;
        }

        private void HandleCmbPartitionOnSelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadDirectoryItems();
        }

        private void LoadDirectoryItemsByPath(string path)
        {
            try
            {
                dtFoldersAndFiles.SuspendLayout();
                _currentRowIndex = 0;
                DirectoryInfo disk = new DirectoryInfo(path);

                List<DirectoryItem> directories = disk.GetDirectories().Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
                                                        .Select(d => new DirectoryItem
                                                        {
                                                            Icon = new Bitmap(Properties.Resources.folder_icon, 12, 12),
                                                            Name = d.Name,
                                                            Type = "File folder",
                                                            CreatedDate = d.CreationTime,
                                                            ModifiedDate = d.LastWriteTime,
                                                            Size = null,
                                                            Path = d.FullName,
                                                        }).OrderBy(d => d.Name).ToList();

                List<DirectoryItem> files = disk.GetFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
                                                        .Select(f => new DirectoryItem
                                                        {
                                                            Icon = new Bitmap(Icon.ExtractAssociatedIcon(f.FullName)!.ToBitmap(), 12, 12),
                                                            Name = f.Name,
                                                            Type = f.Extension,
                                                            CreatedDate = f.CreationTime,
                                                            ModifiedDate = f.LastWriteTime,
                                                            Size = (f.Length / 1024).ToString() + " KB",
                                                            Path = f.FullName,
                                                        }).OrderBy(f => f.Name).ToList();
                _directoryList.Clear();
                AddRangeToDataSource(directories.Concat(files));
                _currentPath = disk.FullName;
                if (dtFoldersAndFiles.DataSource == null)
                    ResetDtFoldersAndFilesDataSource();
                else if (dtFoldersAndFiles.Rows.Count > 0)
                    dtFoldersAndFiles.Rows[0].Selected = false;
                ResetTxtPath(disk.FullName);
                ResetCmbPartition(disk.Root.FullName);
                ResetBtnBack(txtPath.Text, cmbPartition.SelectedItem.ToString() ?? string.Empty);
                ResetDeleteAndRenameButton();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not open this file: " + path, "Open file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                dtFoldersAndFiles.ResumeLayout(true);
            }
        }

        private void AddRangeToDataSource(IEnumerable<DirectoryItem> source)
        {
            foreach (var item in source)
            {
                _directoryList.Add(item);
            }
        }

        private void ResetDtFoldersAndFilesDataSource()
        {
            dtFoldersAndFiles.DataSource = _directoryList;
            dtFoldersAndFiles.ClearSelection();

            dtFoldersAndFiles.Columns["Size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtFoldersAndFiles.Columns["Path"].Visible = false;
            dtFoldersAndFiles.Columns["Icon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dtFoldersAndFiles.Columns["Icon"].Width = 20;
            dtFoldersAndFiles.Columns["Icon"].Resizable = DataGridViewTriState.False;

            dtFoldersAndFiles.CellValueChanged -= HandleRenameFileOrFolder;
            dtFoldersAndFiles.Columns["CreatedDate"].HeaderText = "Date created";
            dtFoldersAndFiles.Columns["ModifiedDate"].HeaderText = "Date modified";
            dtFoldersAndFiles.CellValueChanged += HandleRenameFileOrFolder;

            dtFoldersAndFiles.TabStop = false;
            dtFoldersAndFiles.Refresh();
        }

        private void ResetTxtPath(string path)
        {
            txtPath.Text = path;
            txtPath.SelectionStart = txtPath.Text.Length;
            txtPath.SelectionLength = 0;
        }

        private void HandleTxtPathOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Directory.Exists(txtPath.Text))
                {
                    LoadDirectoryItemsByPath(txtPath.Text);
                    _isFirstClickOnTxtPath = true;
                }
                else
                {
                    MessageBox.Show("Can not find the path: \"" + txtPath.Text + "\"", "Navigate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetTxtPath(_currentPath);
                }
            }
        }

        private void ResetCmbPartition(string diskName)
        {
            cmbPartition.SelectedIndexChanged -= HandleCmbPartitionOnSelectedIndexChanged;
            foreach (var item in cmbPartition.Items)
            {
                if (item.ToString() == diskName)
                {
                    cmbPartition.SelectedItem = item;
                    break;
                }
            }
            cmbPartition.SelectedIndexChanged += HandleCmbPartitionOnSelectedIndexChanged;
        }

        private void HandleDtFoldersAndFilesOnCellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            LoadDirectoryItemsByPath(dtFoldersAndFiles.Rows[e.RowIndex].Cells["Path"].Value.ToString() ?? string.Empty);
        }

        private void HandleBtnDeleteOnClick(object sender, EventArgs e)
        {
            DialogResult result;
            string deleteMessage;
            string deleteCaption;

            HandleDeleteMessage(out deleteMessage, out deleteCaption);
            result = MessageBox.Show(deleteMessage, deleteCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                HandleDeleteSelectedItem();
        }

        private void HandleDeleteMessage(out string deleteMessage, out string deleteCaption)
        {
            deleteMessage = string.Empty;
            deleteCaption = string.Empty;

            if (dtFoldersAndFiles.SelectedRows.Count > 1)
            {
                deleteMessage = $"Are you sure you want to delete these {dtFoldersAndFiles.SelectedRows.Count} items?";
                deleteCaption = MessageConstant.DELETE_ITEMS_CAPTION;
            }
            else if (dtFoldersAndFiles.SelectedRows.Count == 1)
                if (dtFoldersAndFiles.Rows[dtFoldersAndFiles.SelectedRows[0].Index].Cells["Type"].Value.ToString() == "File folder")
                {
                    deleteMessage = MessageConstant.CONFIRM_DELETE_FOLDER_MESSAGE;
                    deleteCaption = MessageConstant.DELETE_FOLDER_CAPTION;
                }
                else
                {
                    deleteMessage = MessageConstant.CONFIRM_DELETE_FILE_MESSAGE;
                    deleteCaption = MessageConstant.DELETE_FILE_CAPTION;
                }
        }

        private void HandleDeleteSelectedItem()
        {
            try
            {
                foreach (DataGridViewRow selectedRow in dtFoldersAndFiles.SelectedRows)
                {
                    string? deletedPath = dtFoldersAndFiles.Rows[selectedRow.Index].Cells["Path"].Value.ToString();

                    if (Directory.Exists(deletedPath))
                        Directory.Delete(deletedPath, true);
                    else if (File.Exists(deletedPath))
                        File.Delete(deletedPath);
                    else
                        MessageBox.Show("Can not find path \"" + deletedPath + "\"", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                LoadDirectoryItemsByPath(_currentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HandleBtnRenameOnClick(object sender, EventArgs e)
        {
            int rowIndex = dtFoldersAndFiles.CurrentRow.Index;
            dtFoldersAndFiles.BeginEdit(true);

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.ForeColor = Color.Black;
            cellStyle.BackColor = Color.FromArgb(188, 220, 244);

            dtFoldersAndFiles.Rows[rowIndex].DefaultCellStyle = cellStyle;
            dtFoldersAndFiles.DefaultCellStyle.SelectionBackColor = Color.FromArgb(188, 220, 244);
            dtFoldersAndFiles.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void HandleRenameFileOrFolder(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string oldPath = dtFoldersAndFiles.Rows[e.RowIndex].Cells["Path"].Value.ToString() ?? string.Empty;
                if (dtFoldersAndFiles.CurrentCell.Value == null)
                {
                    dtFoldersAndFiles.CellValueChanged -= HandleRenameFileOrFolder;
                    ResetFileOrDirectoryName(e);
                    dtFoldersAndFiles.CellValueChanged += HandleRenameFileOrFolder;
                    return;
                }
                string newDirectoryName = dtFoldersAndFiles.CurrentCell.Value.ToString() ?? string.Empty;
                string newPath = Path.Combine(_currentPath, newDirectoryName);
                Directory.Move(oldPath, newPath);
                _newEditedName = newDirectoryName;
                _isCellContentChanged = true;
                dtFoldersAndFiles.CellValueChanged -= HandleRenameFileOrFolder;
                dtFoldersAndFiles.Rows[e.RowIndex].Cells["Path"].Value = newPath;
                dtFoldersAndFiles.CellValueChanged += HandleRenameFileOrFolder;
            }
            catch (Exception ex)
            {
                string renameCaption = MessageConstant.DELETE_FILE_CAPTION;

                if (dtFoldersAndFiles.Rows[e.RowIndex].Cells["Type"].Value.ToString() == "File folder")
                    renameCaption = MessageConstant.RENAME_FOLDER_CAPTION;

                MessageBox.Show(ex.Message, renameCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                dtFoldersAndFiles.CellValueChanged -= HandleRenameFileOrFolder;
                ResetFileOrDirectoryName(e);
                dtFoldersAndFiles.CellValueChanged += HandleRenameFileOrFolder;
            }
        }

        private void ResetFileOrDirectoryName(DataGridViewCellEventArgs e)
        {
            string? path = dtFoldersAndFiles.Rows[e.RowIndex].Cells["Path"].Value.ToString();
            dtFoldersAndFiles.Rows[e.RowIndex].Cells["Name"].Value = Path.GetFileName(path);
        }

        private void HandleBtnBackOnClick(object sender, EventArgs e) => LoadDirectoryItemsByPath(Directory.GetParent(_currentPath)?.FullName ?? string.Empty);

        private void ResetBtnBack(string txtPath, string cmbItem)
        {
            btnBack.Enabled = txtPath != cmbItem;
        }

        private void HandleTxtPathOnClick(object sender, EventArgs e)
        {
            if (_isFirstClickOnTxtPath == true)
            {
                txtPath.SelectAll();
                _isFirstClickOnTxtPath = false;
            }
        }

        private void HandleTxtPathOnLeave(object sender, EventArgs e) => _isFirstClickOnTxtPath = true;

        private void HandleBtnRefreshOnClick(object sender, EventArgs e) => LoadDirectoryItemsByPath(_currentPath);

        private void HandleCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dtFoldersAndFiles.CellDoubleClick -= HandleDtFoldersAndFilesOnCellDoubleClick;
        }

        private void HandleCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            LoadDirectoryItemsByPath(_currentPath);
            if (_isCellContentChanged)
            {
                _currentRowIndex = FindRowIndexOfDirectoryName(_newEditedName);
                ResetSelectedRowAfterRenamed();
                if (dtFoldersAndFiles.Rows[e.RowIndex].Index != dtFoldersAndFiles.Rows.Count - 1)
                {
                    _resetSelectedRow = true;
                }
                _isCellContentChanged = false;
            }
            dtFoldersAndFiles.CellDoubleClick += HandleDtFoldersAndFilesOnCellDoubleClick;
        }

        private int FindRowIndexOfDirectoryName(string dirname)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in dtFoldersAndFiles.Rows)
            {
                if (row.Cells["Name"].Value.ToString()!.Equals(dirname))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            return rowIndex;
        }

        private void HandleSelectionChanged(object sender, EventArgs e)
        {
            if (_resetSelectedRow)
            {
                _resetSelectedRow = false;
                ResetSelectedRowAfterRenamed();
            }
            ResetDeleteAndRenameButton();
        }

        private void ResetSelectedRowAfterRenamed()
        {
            if (_currentRowIndex != -1)
            {
                dtFoldersAndFiles.CurrentCell = dtFoldersAndFiles.Rows[_currentRowIndex].Cells[0];
            }
        }

        private void ResetDeleteAndRenameButton()
        {
            btnDelete.Enabled = dtFoldersAndFiles.SelectedRows.Count >= 1;
            btnRename.Enabled = dtFoldersAndFiles.SelectedRows.Count == 1;
        }
    }
}
