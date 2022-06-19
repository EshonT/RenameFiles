using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace RenameFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static List<string> logs = new List<string>();

        private void Btn_Find_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Btn_Rename_Click(object sender, RoutedEventArgs e)
        {
            String dir = this.Tbx_DirPath.Text;
            Done(dir);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < logs.Count; i++)
            {
                logs[i] = logs[i].TrimStart().TrimEnd();
                sb.AppendLine(logs[i]);
            }

            Tbx_Message.Text=sb.ToString();
        }

        private static void Done(String dirPath)
        {
            List<string> files = Directory.EnumerateFiles(dirPath).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                RenameByRule(i, file);
            }

        }

        /// <summary>
        /// 按最后修改时间重命名照片
        /// </summary>
        /// <param name="i"></param>
        /// <param name="file"></param>
        private static void RenameByRule(int i, string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    FileInfo fileInfo = new(file);
                    string? currentDir = fileInfo.DirectoryName;

                    if (currentDir is null)
                    {
                        return;
                    }

                    DateTime lastWriteTime = File.GetLastWriteTime(file);
                    string newName = $"IMG_{lastWriteTime:yyyyMMdd_HHmmss_}{i.ToString().PadLeft(3, '0')}{fileInfo.Extension}";
                    string archiveDir = Path.Combine(currentDir, "Done");
                    if (!Directory.Exists(archiveDir))
                    {
                        Directory.CreateDirectory(archiveDir);
                    }

                    string targetFilePath = Path.Combine(archiveDir, newName);

                    fileInfo.MoveTo(targetFilePath);

                }
                catch (Exception e)
                {
                    string message = DateTime.Now.ToString("F")+"\t" + file +"\t"+ e.Message;
                    logs.Add(message);
                }
            }
        }

    }
}
