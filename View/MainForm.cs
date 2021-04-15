using ImageDownloader.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace ImageDownloader.View
{
    public partial class MainForm : Form
    {
        private TextBox _textBox;
        private Button _button;
        private FlowLayoutPanel _layoutPanel;
        private ToolStripStatusLabel _statusLabel;
        private ToolStripProgressBar _progressBar;
        private StatusStrip _statusStrip;

        private WebPage _model;
        private int _images;
        private bool _isDownloading;

        public MainForm()
        {
            InitializeComponent();
            _images = 0;
            _isDownloading = false;
        }

        private void InitializeComponent()
        {
            this._textBox = new System.Windows.Forms.TextBox();
            this._button = new System.Windows.Forms.Button();
            this._layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this._textBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._textBox.Location = new System.Drawing.Point(0, 0);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(991, 23);
            this._textBox.TabIndex = 0;
            // 
            // _button
            // 
            this._button.Dock = System.Windows.Forms.DockStyle.Top;
            this._button.Location = new System.Drawing.Point(0, 23);
            this._button.Name = "_button";
            this._button.Size = new System.Drawing.Size(991, 23);
            this._button.TabIndex = 1;
            this._button.Text = "Képek letöltése";
            this._button.UseVisualStyleBackColor = true;
            this._button.Click += new System.EventHandler(this._button_Click);
            // 
            // _layoutPanel
            // 
            this._layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutPanel.Location = new System.Drawing.Point(0, 46);
            this._layoutPanel.Name = "_layoutPanel";
            this._layoutPanel.Size = new System.Drawing.Size(991, 373);
            this._layoutPanel.TabIndex = 2;
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._progressBar});
            this._statusStrip.Location = new System.Drawing.Point(0, 397);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(991, 22);
            this._statusStrip.TabIndex = 3;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _statusLabel
            // 
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(81, 17);
            this._statusLabel.Text = "Képek száma: ";
            // 
            // _progressBar
            // 
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(991, 419);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._layoutPanel);
            this.Controls.Add(this._button);
            this.Controls.Add(this._textBox);
            this.Name = "MainForm";
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private async void _button_Click(object sender, EventArgs e)
        {
            if(_isDownloading)
            {
                _model.CancelLoad();
            }
            else
            {
                _images = 0;
                this._statusLabel.Text = "Képek száma: " + _images;
                this._progressBar.Value = 0;
                this._progressBar.Visible = true;
                this._button.Text = "Letöltés megszakítása";
                _isDownloading = true;
                this._layoutPanel.Controls.Clear();
                try
                {
                    _model = new WebPage(new Uri(_textBox.Text));
                    _model.ImageLoaded += new EventHandler<WebImage>(Model_ImageLoaded);
                    _model.LoadProgress += new EventHandler<int>(Model_LoadProgress);

                    await _model.LoadImagesAsync();
                }
                catch
                {

                }
                this._progressBar.Visible = false;
                this._button.Text = "Letöltés";
                _isDownloading = false;
            }
            
        }
        private void Model_LoadProgress(object sender, int i)
        {
            this._progressBar.Value = i;
            Debug.WriteLine(i);
        }
        private void Model_ImageLoaded(object sender, WebImage i)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new System.Drawing.Size(100, 100);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = i.Image;

            pictureBox.Click += new System.EventHandler(this.ShowImage);

            this._layoutPanel.Controls.Add(pictureBox);
            _images++;
            this._statusLabel.Text = "Képek száma: " + _images;

        }
        private void ShowImage(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox == null)
                return;
            ImageForm form = new ImageForm(pictureBox.Image);
            form.Show();
        }
    }
}
