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


        

        private void Btn_Find_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Rename_Click(object sender, RoutedEventArgs e)
        {

        }

        private static void Done(String dirPath)
        {
            Directory directory = new Directory(dirPath);
            //文件夹
            File directoryFile = new File(directoryPath + "test");
            if (directoryFile.isDirectory())
            {
                File[] files = directoryFile.listFiles();
                for (File file : files)
                {

                    //获取时间
                    long modifyTime = file.lastModified();
                    System.out.println(modifyTime);
                    String trueTime = formatModifyTime(modifyTime);

                    //后缀
                    String name = file.getName();
                    String suffix = name.substring(name.lastIndexOf("."));

                    //重命名
                    File newName = new File(directoryFile.getAbsolutePath() + File.separator + trueTime + suffix);
                    if (!file.renameTo(newName))
                    {
                        System.out.println(file.getName() + "修改失败  有相同时间的文件");
                    }
                }
            }
        }


        private static String FormatModifyTime(long modifyTime)
        {
            Calendar calendar = Calendar.getInstance();

            calendar.setTimeInMillis(modifyTime);
            String pattern = "yyyy-MM-dd HHmmssSSS";
            SimpleDateFormat sdf = new SimpleDateFormat(pattern);

            return sdf.format(calendar.getTime());
        }
    }
}
