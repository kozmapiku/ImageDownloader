using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace ImageDownloader.View
{
    public partial class ImageForm : Form
    {
        private PictureBox _pictureBox;
        private Panel panel1;
        private Button _button;

        public ImageForm(System.Drawing.Image i)
        {
            InitializeComponent();

            _pictureBox.Image = i;

        }

        private void InitializeComponent()
        {
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(999, 461);
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 361);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(999, 100);
            this.panel1.TabIndex = 1;
            // 
            // _button
            // 
            this._button.Dock = System.Windows.Forms.DockStyle.Fill;
            this._button.Location = new System.Drawing.Point(0, 0);
            this._button.Name = "_button";
            this._button.Size = new System.Drawing.Size(999, 100);
            this._button.TabIndex = 0;
            this._button.Text = "Letöltés";
            this._button.UseVisualStyleBackColor = true;
            this._button.Click += new System.EventHandler(this._button_Click);
            // 
            // ImageForm
            // 
            this.ClientSize = new System.Drawing.Size(999, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._pictureBox);
            this.Name = "ImageForm";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void _button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG files (*.png)|*.png";
            saveDialog.InitialDirectory = Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures);
            saveDialog.RestoreDirectory = true;


            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                _pictureBox.Image.Save(saveDialog.FileName, ImageFormat.Png);
            }

        }
    }
}
