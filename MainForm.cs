
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
namespace JImgView
{

    public partial class MainForm : Form
    {
        private static string AppDir =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        private static string SettingsFile
            = Path.Combine(AppDir, "settings.json");
        private AppSettings settings;
        private List<string> imageFiles = new List<string>();
        private int currentIndex = -1;

        public MainForm(string[] args)
        {
            InitializeComponent();
            LoadSettings();
            SetupEventHandlers();
            ProcessCommandLineArgs(args);
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    settings = JsonConvert.DeserializeObject<AppSettings>(json)!;
                }
            }
            catch
            {
                // Обработка ошибок загрузки
            }

            settings ??= new AppSettings();
        }

        private void SaveSettings()
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFile, json);
            }
            catch
            {
                // Обработка ошибок сохранения
            }
        }

        private void SetupEventHandlers()
        {
            btnPrev.Click += (s, e) => ShowPreviousImage();
            btnNext.Click += (s, e) => ShowNextImage();
            FormClosing   += (s, e) => SaveSettings();
            pictureBox.DoubleClick += (s, e) => DblClk(s, e);
            pictureBox.MouseWheel += (s, e) => Wheel(s, e);
            KeyDown += MainForm_KeyDown;
        }

        private void Wheel(object s, MouseEventArgs e)
        {
            pictureBox.Image?.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox.Invalidate();
        }

        private void DblClk(object s,  EventArgs e)
        {
            MouseEventArgs? ee = e as MouseEventArgs;
            
            if (currentIndex < 0 || currentIndex >= imageFiles.Count)
                return;

            if (ee?.Button != MouseButtons.Left)
                return;

            try
            {
                var file = imageFiles[currentIndex];
                Process.Start("explorer.exe", $"/select,\"{file}\"");
            }
            catch 
            {
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {                       
            MouseEventArgs? ee = e as MouseEventArgs;   
            
            if(ee?.Button == MouseButtons.Right)
                OpenNewFolder();

           
        }

        private void ProcessCommandLineArgs(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                OpenImage(args[0]);
            }
            else if (settings.RecentFolders.Count > 0)
            {
                LoadFolder(settings.RecentFolders.Last());
                currentIndex = 0;
                DisplayCurrentImage();
            }
            else if (Directory.Exists(args[0]))
            {
                LoadFolder(args[0]);
                currentIndex = 0;
                DisplayCurrentImage();
            }

        }

        private void OpenImage(string filePath)
        {
            try
            {
                var folder = Path.GetDirectoryName(filePath);
                if (!settings.RecentFolders.Contains(folder))
                {
                    settings.RecentFolders.Add(folder);
                    if (settings.RecentFolders.Count > 10)
                        settings.RecentFolders.RemoveAt(0);
                }

                LoadFolder(folder);
                currentIndex = imageFiles.IndexOf(filePath);
                DisplayCurrentImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия файла: {ex.Message}");
            }
        }

        private void LoadFolder(string folder)
        {
            if (!Directory.Exists(folder)) return;

            imageFiles = Directory.GetFiles(folder)
                .Where(f => IsSupportedImage(f.ToLower()))
                .OrderBy(f => f)
                .ToList();

            lblStatus.Text = $"Файлов: {imageFiles.Count}";
        }

        private bool IsSupportedImage(string file)
        {
            var extensions = new[] { ".webp", ".jpg", ".jpeg", ".png", ".gif", ".apng", ".bmp" };
            return extensions.Any(ext => file.EndsWith(ext));
        }

        private void DisplayCurrentImage()
        {
            if (currentIndex < 0 || currentIndex >= imageFiles.Count) return;

            try
            {
                var file = imageFiles[currentIndex];
                using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                var image = Image.FromStream(stream);
                pictureBox.Image?.Dispose();
                pictureBox.Image = new Bitmap(image);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                Text = $"{Path.GetFileName(file)} - Image Viewer";
            }
            catch (Exception ex)
            {
                //  MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                Text = $"Ошибка загрузки изображения: {ex.Message}";
                pictureBox.Image?.Dispose();
                pictureBox.Image = this.Icon.ToBitmap();
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }

        private void ShowPreviousImage()
        {
            if (imageFiles.Count == 0) return;
            currentIndex = (currentIndex - 1 + imageFiles.Count) % imageFiles.Count;
            DisplayCurrentImage();
        }

        private void ShowNextImage()
        {
            if (imageFiles.Count == 0) return;
            currentIndex = (currentIndex + 1) % imageFiles.Count;
            DisplayCurrentImage();
        }

        private void OpenNewFolder()
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadFolder(dialog.SelectedPath);
                currentIndex = imageFiles.Count > 0 ? 0 : -1;
                DisplayCurrentImage();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) ShowPreviousImage();
            else if (e.KeyCode == Keys.Right) ShowNextImage();
        }

    }

    public class AppSettings
    {
        public List<string> RecentFolders { get; set; } = new List<string>();
    }
}

