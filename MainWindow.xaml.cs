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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace RenameFiles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
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
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "选择文件夹";
            fbd.ShowDialog();
            Tbx_DirPath.Text = fbd.SelectedPath;
        }

        private void Btn_Rename_Click(object sender, RoutedEventArgs e)
        {
            String dir = this.Tbx_DirPath.Text;

            if (String.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                Tbx_Message.Text = "文件夹异常";
                return;
            }
            Done(dir);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < logs.Count; i++)
            {
                logs[i] = logs[i].TrimStart().TrimEnd();
                sb.AppendLine(logs[i]);
            }

            Tbx_Message.Text = sb.ToString();
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
                    FileInfo fileInfo = new FileInfo(file);
                    string currentDir = fileInfo.DirectoryName;

                    if (CheckPicSource(file))
                    {
                        DealMicroMsgPic(fileInfo);
                        return;
                    }

                    DateTime lastWriteTime = File.GetLastWriteTime(file);

                    string newName = $"IMG_{lastWriteTime:yyyyMMdd_HHmmss_}{i.ToString().PadLeft(3, '0')}{fileInfo.Extension}";
                    string archiveDir = Path.Combine(currentDir, "Done");

                    RenameMoveFile(fileInfo, newName, archiveDir);

                }
                catch (Exception e)
                {
                    string message = DateTime.Now.ToString("F") + "\t" + file + "\t" + e.Message;
                    logs.Add(message);
                }
            }
        }

        private static void RenameMoveFile(FileInfo file, string newName, string newDirPath)
        {
            try
            {
                if (!Directory.Exists(newDirPath))
                {
                    Directory.CreateDirectory(newDirPath);
                }

                string targetFilePath = Path.Combine(newDirPath, newName);

                file.MoveTo(targetFilePath);
                
            }
            catch (Exception )
            {
                throw;
            }
        }

        private static bool CheckPicSource(String fullName)
        {
            if (!String.IsNullOrEmpty(fullName))
            {
                FileInfo fileInfo = new FileInfo(fullName);
                if (fileInfo.Exists)
                {
                    string fileName=fileInfo.Name;
                    if (fileName.StartsWith("mmexport"))
                    {
                        logs.Add("微信保存图片 " + fullName);

                        return true;

                    }
                }
            }
            return false;
        }

        private static void DealMicroMsgPic(FileInfo file)
        {
            string name = file.Name;
            string ext = file.Extension;

            name.Replace("mmexport", "IMG_");
            String[] namePart = name.Split(new char[] { '_', '.' });
            String timeString = namePart[1];
            long timeStamp = Int64.Parse(timeString);
            DateTime time = FromTimeStamp(timeStamp);

            namePart[1] = time.ToString("yyyyMMdd_HHmmss");

            string newName = String.Join("_", namePart);

            string archiveDir = Path.Combine(file.DirectoryName, "Done");

            RenameMoveFile(file, newName, archiveDir);

        }


        private static DateTime FromTimeStamp(long timestamp)
        {
            long begtine = timestamp * 10000;
            DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long tine_tricks = dt_1970.Ticks + begtine;
            return new DateTime(tine_tricks);
        }
    }
}
