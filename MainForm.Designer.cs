namespace JImgView
{
    partial class MainForm
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
        private Button btnPrev;
        private Button btnNext;
        private PictureBox pictureBox;
        private Label lblStatus;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnPrev = new Button();
            btnNext = new Button();
            pictureBox = new PictureBox();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // btnPrev
            // 
            btnPrev.Dock = DockStyle.Left;
            btnPrev.Font = new Font("Arial", 14F);
            btnPrev.Location = new Point(0, 0);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(50, 238);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            // 
            // btnNext
            // 
            btnNext.Dock = DockStyle.Right;
            btnNext.Font = new Font("Arial", 14F);
            btnNext.Location = new Point(234, 0);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(50, 238);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.Control;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.InitialImage = (Image)resources.GetObject("pictureBox.InitialImage");
            pictureBox.Location = new Point(50, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(184, 238);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.LightGray;
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 238);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(284, 23);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Готово";
            // 
            // MainForm
            // 
            ClientSize = new Size(284, 261);
            Controls.Add(pictureBox);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            Controls.Add(lblStatus);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Image Viewer";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
