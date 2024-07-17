using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.FileIO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace File_Shredder
{

    public partial class FileShredder : Window
    {
        // UI Buttons
        private BitmapImage discordD;
        private BitmapImage discordH;
        private BitmapImage patreonD;
        private BitmapImage patreonH;
        private BitmapImage githubD;
        private BitmapImage githubH;
        private BitmapImage webD;
        private BitmapImage webH;
        private BitmapImage deviantArtD;
        private BitmapImage deviantArtH;
        private BitmapImage artstationD;
        private BitmapImage artstationH;
        private BitmapImage button2X;
        private BitmapImage button2D;
        private BitmapImage button2H;
        private BitmapImage button3D;
        private BitmapImage button3H;
        private BitmapImage button4D;
        private BitmapImage button4H;

        // Function
        private CancellationTokenSource cancellationTokenSource;
        private int totalFilesShredded = 0;
        private int totalFilesSkipped = 0;
        private int multiPassLevel = 1;

        public FileShredder()
        {
            InitializeComponent();

            // UI Buttons
            button2X = new BitmapImage(new Uri("/Images/Button2X.png", UriKind.Relative));
            button2D = new BitmapImage(new Uri("/Images/Button2D.png", UriKind.Relative));
            button2H = new BitmapImage(new Uri("/Images/Button2H.png", UriKind.Relative));
            button3D = new BitmapImage(new Uri("/Images/Button3D.png", UriKind.Relative));
            button3H = new BitmapImage(new Uri("/Images/Button3H.png", UriKind.Relative));
            button4D = new BitmapImage(new Uri("/Images/Button4D.png", UriKind.Relative));
            button4H = new BitmapImage(new Uri("/Images/Button4H.png", UriKind.Relative));

            discordD = new BitmapImage(new Uri("/Images/Discord.png", UriKind.Relative));
            discordH = new BitmapImage(new Uri("/Images/Discord_H.png", UriKind.Relative));
            patreonD = new BitmapImage(new Uri("/Images/Patreon.png", UriKind.Relative));
            patreonH = new BitmapImage(new Uri("/Images/Patreon_H.png", UriKind.Relative));
            githubD = new BitmapImage(new Uri("/Images/Github.png", UriKind.Relative));
            githubH = new BitmapImage(new Uri("/Images/Github_H.png", UriKind.Relative));
            webD = new BitmapImage(new Uri("/Images/Website.png", UriKind.Relative));
            webH = new BitmapImage(new Uri("/Images/Website_H.png", UriKind.Relative));
            deviantArtD = new BitmapImage(new Uri("/Images/DeviantArt.png", UriKind.Relative));
            deviantArtH = new BitmapImage(new Uri("/Images/DeviantArt_H.png", UriKind.Relative));
            artstationD = new BitmapImage(new Uri("/Images/Artstation.png", UriKind.Relative));
            artstationH = new BitmapImage(new Uri("/Images/Artstation_H.png", UriKind.Relative));

        }

        private void TargetFolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.Title = "Select the target folder for the process";

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ExportConsole.Text = folderDialog.FileName;
            }
        }

        // Basic Buttons
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        { ShowInformation(); }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        { System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized; }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        { System.Windows.Application.Current.Shutdown(); }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        { if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); } }

        // Button Highlight
        private void Return_MouseEnter(object sender, MouseEventArgs e)
        { Return.Source = button2H; }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        { Return.Source = button2D; }
        private void Discord_MouseEnter(object sender, MouseEventArgs e)
        { Discord.Source = discordH; }

        private void Discord_MouseLeave(object sender, MouseEventArgs e)
        { Discord.Source = discordD; }

        private void Patreon_MouseEnter(object sender, MouseEventArgs e)
        { Patreon.Source = patreonH; }

        private void Patreon_MouseLeave(object sender, MouseEventArgs e)
        { Patreon.Source = patreonD; }

        private void Github_MouseEnter(object sender, MouseEventArgs e)
        { Github.Source = githubH; }

        private void Github_MouseLeave(object sender, MouseEventArgs e)
        { Github.Source = githubD; }

        private void Website_MouseEnter(object sender, MouseEventArgs e)
        { Website.Source = webH; }

        private void Website_MouseLeave(object sender, MouseEventArgs e)
        { Website.Source = webD; }
        private void DeviantArt_MouseEnter(object sender, MouseEventArgs e)
        { DeviantArt.Source = deviantArtH; }

        private void DeviantArt_MouseLeave(object sender, MouseEventArgs e)
        { DeviantArt.Source = deviantArtD; }

        private void Artstation_MouseEnter(object sender, MouseEventArgs e)
        { Artstation.Source = artstationH; }

        private void Artstation_MouseLeave(object sender, MouseEventArgs e)
        { Artstation.Source = artstationD; }

        private void DisclaimerBtn_MouseEnter(object sender, MouseEventArgs e)
        { DisclaimerBtn.Source = button3H; }

        private void DisclaimerBtn_MouseLeave(object sender, MouseEventArgs e)
        { DisclaimerBtn.Source = button3D; }

        private void OpenLastLogsBtn_MouseEnter(object sender, MouseEventArgs e)
        { OpenLastLogsBtn.Source = button3H; }

        private void OpenLastLogsBtn_MouseLeave(object sender, MouseEventArgs e)
        { OpenLastLogsBtn.Source = button3D; }

        private void ShredBtn_MouseEnter(object sender, MouseEventArgs e)
        { ShredBtn.Source = button2H; }

        private void ShredBtn_MouseLeave(object sender, MouseEventArgs e)
        { ShredBtn.Source = button2D; }

        private void PickDirectory_MouseEnter(object sender, MouseEventArgs e)
        { PickDirectory.Source = button4H; }

        private void PickDirectory_MouseLeave(object sender, MouseEventArgs e)
        { PickDirectory.Source = button4D; }

        // Links
        private void Patreon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://www.patreon.com/msdysphoria";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void Website_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://msdysphoria.shop/";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void DeviantArt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://www.deviantart.com/msdysphoria";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void Artstation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://www.artstation.com/msdysphoria";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void Discord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://discord.gg/uQDPFt6WKn";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void Github_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string patreonLink = "https://github.com/MsDysphoria";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = patreonLink,
                UseShellExecute = true
            });
        }

        private void Return_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            Storyboard fadeOutStoryboard = this.FindResource("FadeOut_Info") as Storyboard;
            fadeOutStoryboard?.Begin();
        }
        private void ShowInformation()
        {

            cancellationTokenSource = new CancellationTokenSource();
            _ = Typewriter(0, cancellationTokenSource.Token);

            Storyboard fadeInStoryboard = this.FindResource("FadeIn_Info") as Storyboard;
            fadeInStoryboard.Begin();
        }
        public async Task Typewriter(int message, CancellationToken cancellationToken)
        {
            Author.Text = "";
            string msg;
            if (message == 0) { msg = ""; }
            else if (message == 1) { msg = "Created by Ms. Dysphoria"; }
            else { msg = "Discord: msdysphoria"; }

            Random randomDelay = new Random();

            for (int i = 0; i < msg.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Author.Text += msg[i].ToString();
                int delay = randomDelay.Next(35, 55);
                await Task.Delay(delay, cancellationToken);
            }

            if (message == 0)
            {
                await Task.Delay(2500, cancellationToken);
                await Typewriter(1, cancellationToken);
            }
            else if (message == 1)
            {
                Storyboard fadeInStoryboard = this.FindResource("GlowAuthor") as Storyboard;
                fadeInStoryboard.Begin();
                await Task.Delay(5000, cancellationToken);
                await Typewriter(2, cancellationToken);
            }
            else
            {

                await Task.Delay(5000, cancellationToken);
                await Typewriter(1, cancellationToken);
            }
        }

        // Function
        private async void ProcessFiles_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isDisclaimerChecked = DisclaimerCheckBox.IsChecked.GetValueOrDefault();

            if (!isDisclaimerChecked)
            {
                UpdateConsole(2);
                return;
            }
            int selectedType = Type.SelectedIndex;
            string findText = Find.Text;
            string folderPath = ExportConsole.Text;
            bool searchSubfolders = SubfolderCheckbox.IsChecked.GetValueOrDefault();
            bool isSensitivityChecked = SensitivityCheckBox.IsChecked.GetValueOrDefault();
            string logFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Latest.log");

            if (findText == "Pick a keyword to specify files for shredding")
            {
                UpdateConsole(3);
                return;
            }

            if (folderPath == "Pick the target directory")
            {
                UpdateConsole(4);
                return;
            }

            List<string> files = new List<string>();
            if (searchSubfolders)
            {
                files.AddRange(Directory.GetFiles(folderPath, "*", System.IO.SearchOption.AllDirectories));
            }
            else
            {
                files.AddRange(Directory.GetFiles(folderPath, "*", System.IO.SearchOption.TopDirectoryOnly));
            }

            totalFilesShredded = 0;
            totalFilesSkipped = 0;
            int totalFiles = files.Count;
            int currentProgress = 0;

            ProgressBar.Value = 0;

            File.WriteAllText(logFilePath, string.Empty);
            await Task.Run(() =>
            {
                foreach (string file in files)
                {
                    bool containsText = false;
                    if (selectedType == 0)
                    {
                        bool filenameMatches = System.IO.Path.GetFileName(file).Contains(findText, isSensitivityChecked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                        bool contentMatches = isSensitivityChecked ? File.ReadAllText(file).Contains(findText) : File.ReadAllText(file).ToLower().Contains(findText.ToLower());
                        containsText = filenameMatches || contentMatches;
                    }
                    else if (selectedType == 1)
                    {
                        containsText = System.IO.Path.GetFileName(file).Contains(findText, isSensitivityChecked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        containsText = isSensitivityChecked ? File.ReadAllText(file).Contains(findText) : File.ReadAllText(file).ToLower().Contains(findText.ToLower());
                    }

                    if (containsText)
                    {
                        for (int multiPassCount = 0; multiPassCount < multiPassLevel; multiPassCount++)
                        {
                            ReplaceFileContent(file);
                        }
                        FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                        LogFile(file, logFilePath);
                        Interlocked.Increment(ref totalFilesShredded);
                    }
                    else
                    {
                        Interlocked.Increment(ref totalFilesSkipped);
                    }

                    Interlocked.Increment(ref currentProgress);
                    this.Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Value = (currentProgress * 100) / totalFiles;
                    });
                }
            });

            this.Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = 100;
                UpdateConsole(1);
            });
        }
        void LogFile(string filePath, string logFilePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"Deleted file: {filePath} - {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging file deletion: {ex.Message}");
            }
        }
        private void ReplaceFileContent(string filePath)
        {
            string originalContent = File.ReadAllText(filePath);
            string newContent = new string(GenerateRandomString(originalContent.Length));
            File.WriteAllText(filePath, newContent);
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void UpdateConsole(int message)
        {
            ResultConsole.Document.Blocks.Clear();
            if (message == 1)
            {
                Paragraph p = new Paragraph();
                p.TextAlignment = TextAlignment.Center;

                Run r = new Run($"▰▰▰▰▰▰▰▰▰▰ Process completed ▰▰▰▰▰▰▰▰▰▰");
                r.Foreground = new SolidColorBrush(Colors.Green);

                p.Inlines.Add(r);
                ResultConsole.Document.Blocks.Add(p);

                Paragraph pt = new Paragraph();
                pt.TextAlignment = TextAlignment.Center;

                Run rt = new Run($"Shredded " + totalFilesShredded + " files in total");
                rt.Foreground = new SolidColorBrush(Colors.White);
                pt.Inlines.Add(rt);

                ResultConsole.Document.Blocks.Add(pt);

                Paragraph ps = new Paragraph();
                ps.TextAlignment = TextAlignment.Center;

                Run rs = new Run($"Open the latest log to view the details");
                rs.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0xD3, 0xF1));
                ps.Inlines.Add(rs);
                ResultConsole.Document.Blocks.Add(ps);
            }
            else
            {
                Paragraph p = new Paragraph();
                Run r = new Run($"");
                r.Foreground = new SolidColorBrush(Colors.White);

                p.Inlines.Add(r);
                ResultConsole.Document.Blocks.Add(p);
            }

            if (message == 2)
            {

                Paragraph pt = new Paragraph();
                pt.TextAlignment = TextAlignment.Center;

                Run rt = new Run($"You need to read the disclaimer and accept it by ticking the checkbox");
                rt.Foreground = new SolidColorBrush(Colors.IndianRed);
                pt.Inlines.Add(rt);

                ResultConsole.Document.Blocks.Add(pt);
            }
            if (message == 3)
            {
                Paragraph pt = new Paragraph();
                pt.TextAlignment = TextAlignment.Center;

                Run rt = new Run($"Please specify the keyword to find.");
                rt.Foreground = new SolidColorBrush(Colors.IndianRed);
                pt.Inlines.Add(rt);

                ResultConsole.Document.Blocks.Add(pt);
            }
            if (message == 4)
            {

                Paragraph pt = new Paragraph();
                pt.TextAlignment = TextAlignment.Center;

                Run rt = new Run($"Please select a directory.");
                rt.Foreground = new SolidColorBrush(Colors.IndianRed);
                pt.Inlines.Add(rt);

                ResultConsole.Document.Blocks.Add(pt);
            }
        }

        private void OpenLastLogsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string rootDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string logFilePath = System.IO.Path.Combine(rootDirectory, "Latest.log");

                Process.Start("notepad.exe", logFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening log file: {ex.Message}");
            }
        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MultiPassSlider != null && MultiPassText != null)
            {
                multiPassLevel = (int)MultiPassSlider.Value;
                MultiPassText.Text = multiPassLevel.ToString();
            }
        }

        private void DisclaimerBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isDisclaimerChecked = DisclaimerCheckBox.IsChecked.GetValueOrDefault();

                // Show a modal dialogue with "Yes" and "No" buttons
                MessageBoxResult result = MessageBox.Show(
                    "File Shredder is an experimental software created with the aim of shredding files in a quick and organized way. Nevertheless it doesn't guarantee that the shreded files won't be recoverable as the result is dependent on OS, filesystems and resource management. Users are advised to use it only in block based non-copy on write filesystems and test it beforehand with data recovery software.\n\nUsers are also advised to proceed with caution as the use of this software may cause the loss of any important data. The creator (Ms. Dysphoria) can't be held accountable for any loss of data and damage the usage of File Shredder may cause.\n\nBy clicking 'Yes', the user will be considered to have read and accepted the disclaimer.",
                    "Disclaimer Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DisclaimerCheckBox.IsChecked = true;
                    DisclaimerBtn.IsEnabled = false;
                    DisclaimerCheckBox.IsEnabled = false;
                }
        }


    }
}
