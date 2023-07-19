namespace FileExplorer.DesktopApp
{
    partial class FileExplorerUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmbPartition = new ComboBox();
            txtPath = new TextBox();
            dtFoldersAndFiles = new DataGridView();
            btnDelete = new Button();
            btnRename = new Button();
            btnBack = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dtFoldersAndFiles).BeginInit();
            SuspendLayout();
            // 
            // cmbPartition
            // 
            cmbPartition.FormattingEnabled = true;
            cmbPartition.Location = new Point(12, 12);
            cmbPartition.Name = "cmbPartition";
            cmbPartition.Size = new Size(67, 23);
            cmbPartition.TabIndex = 0;
            cmbPartition.SelectedIndexChanged += HandleCmbPartitionOnSelectedIndexChanged;
            // 
            // txtPath
            // 
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPath.Location = new Point(52, 41);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(706, 23);
            txtPath.TabIndex = 4;
            txtPath.Click += HandleTxtPathOnClick;
            txtPath.KeyDown += HandleTxtPathOnKeyDown;
            txtPath.Leave += HandleTxtPathOnLeave;
            // 
            // dtFoldersAndFiles
            // 
            dtFoldersAndFiles.AllowUserToAddRows = false;
            dtFoldersAndFiles.AllowUserToDeleteRows = false;
            dtFoldersAndFiles.AllowUserToResizeRows = false;
            dtFoldersAndFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dtFoldersAndFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtFoldersAndFiles.BackgroundColor = SystemColors.ControlLightLight;
            dtFoldersAndFiles.BorderStyle = BorderStyle.None;
            dtFoldersAndFiles.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtFoldersAndFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtFoldersAndFiles.Location = new Point(12, 70);
            dtFoldersAndFiles.MultiSelect = true;
            dtFoldersAndFiles.Name = "dtFoldersAndFiles";
            dtFoldersAndFiles.RowHeadersVisible = false;
            dtFoldersAndFiles.RowTemplate.Height = 25;
            dtFoldersAndFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtFoldersAndFiles.Size = new Size(776, 368);
            dtFoldersAndFiles.StandardTab = true;
            dtFoldersAndFiles.TabIndex = 6;
            dtFoldersAndFiles.CellBeginEdit += HandleCellBeginEdit;
            dtFoldersAndFiles.CellClick += HandleDtFoldersAndFilesOnCellClick;
            dtFoldersAndFiles.CellDoubleClick += HandleDtFoldersAndFilesOnCellDoubleClick;
            dtFoldersAndFiles.CellEndEdit += HandleCellEndEdit;
            dtFoldersAndFiles.CellMouseLeave += HandleDtFoldersAndFilesOnCellMouseLeave;
            dtFoldersAndFiles.CellMouseMove += HandleDtFoldersAndFilesOnCellMouseMove;
            dtFoldersAndFiles.CellPainting += HandleDtFoldersAndFilesOnCellPainting;
            dtFoldersAndFiles.CellValueChanged += HandleRenameFileOrFolder;
            dtFoldersAndFiles.SelectionChanged += HandleSelectionChanged;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Image = Properties.Resources.delete_icon;
            btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
            btnDelete.Location = new Point(637, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(66, 23);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.TextAlign = ContentAlignment.MiddleRight;
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += HandleBtnDeleteOnClick;
            // 
            // btnRename
            // 
            btnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRename.Image = Properties.Resources.rename_icon;
            btnRename.ImageAlign = ContentAlignment.MiddleLeft;
            btnRename.Location = new Point(713, 12);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(75, 23);
            btnRename.TabIndex = 2;
            btnRename.Text = "Rename";
            btnRename.TextAlign = ContentAlignment.MiddleRight;
            btnRename.UseVisualStyleBackColor = true;
            btnRename.Click += HandleBtnRenameOnClick;
            // 
            // btnBack
            // 
            btnBack.Image = Properties.Resources.back_icon;
            btnBack.Location = new Point(12, 40);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(34, 24);
            btnBack.TabIndex = 3;
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += HandleBtnBackOnClick;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Image = Properties.Resources.refresh_icon;
            btnRefresh.Location = new Point(764, 41);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(24, 24);
            btnRefresh.TabIndex = 5;
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += HandleBtnRefreshOnClick;
            // 
            // FileExplorerUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRefresh);
            Controls.Add(btnBack);
            Controls.Add(btnRename);
            Controls.Add(btnDelete);
            Controls.Add(dtFoldersAndFiles);
            Controls.Add(txtPath);
            Controls.Add(cmbPartition);
            Name = "FileExplorerUserControl";
            Size = new Size(800, 450);
            Load += HandleFmMainOnLoad;
            ((System.ComponentModel.ISupportInitialize)dtFoldersAndFiles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbPartition;
        private TextBox txtPath;
        private DataGridView dtFoldersAndFiles;
        private Button btnDelete;
        private Button btnRename;
        private Button btnBack;
        private Button btnRefresh;
    }
}
