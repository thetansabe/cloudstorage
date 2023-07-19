namespace FileExplorer.DesktopApp
{
    partial class fmFileExplorerUserControl
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
            if (disposing && (components != null))
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
            splitContainer = new SplitContainer();
            ucLeftFileExplorerUserControl = new FileExplorerUserControl();
            ucRightFileExplorerUserControl = new FileExplorerUserControl();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer.Panel1.Controls.Add(ucLeftFileExplorerUserControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer.Panel2.Controls.Add(ucRightFileExplorerUserControl);
            splitContainer.Size = new Size(1447, 693);
            splitContainer.SplitterDistance = 742;
            splitContainer.TabIndex = 0;
            // 
            // ucLeftFileExplorerUserControl
            // 
            ucLeftFileExplorerUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ucLeftFileExplorerUserControl.Location = new Point(12, 12);
            ucLeftFileExplorerUserControl.Name = "fileExplorerUserControl1";
            ucLeftFileExplorerUserControl.Size = new Size(708, 669);
            ucLeftFileExplorerUserControl.TabIndex = 0;
            // 
            // ucRightFileExplorerUserControl
            // 
            ucRightFileExplorerUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ucRightFileExplorerUserControl.Location = new Point(27, 12);
            ucRightFileExplorerUserControl.Name = "fileExplorerUserControl2";
            ucRightFileExplorerUserControl.Size = new Size(662, 669);
            ucRightFileExplorerUserControl.TabIndex = 1;
            // 
            // fmFileExplorerUserControl
            // 
            ClientSize = new Size(1447, 693);
            Controls.Add(splitContainer);
            Name = "fmFileExplorerUserControl";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            Text = "File Explorer";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer;
        private FileExplorerUserControl ucLeftFileExplorerUserControl;
        private FileExplorerUserControl ucRightFileExplorerUserControl;
    }
}