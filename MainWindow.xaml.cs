using System;
using System.Collections.Generic;
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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();      //创建一个计时器
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();                //调用SetUpGame方法
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = "记录：" + timeTextBlock.Text + " - 再来?";
            }
        }

        private void SetUpGame()       //创建SetUpGame方法
        {
            List<string> animalEmoji = new List<string>()           //创建一个列表
            {
                "🐳", "🐳",
                "🦞", "🦞",
                "🐇", "🐇",
                "🦏", "🦏",
                "🦄", "🦄",
                "🐼", "🐼",
                "🦍", "🦍",
                "🦔", "🦔",
            };

            Random random = new Random();               // 创建一个随机数生成器

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())          // 创建一个循环；找到主网格中的各个TextBlock，重复如下操作
            {
                if (textBlock.Name != "timeTextBlock")          //跳过名为timeTextBlock的TextBlock控件
                {
                    textBlock.Visibility = Visibility.Visible;      //将所有的TextBlock控件都设置为显示(当重新运行程序时，默认所有控件都是隐藏的)
                    int index = random.Next(animalEmoji.Count);         // 选择介于0到列表中剩余表情符号数之间的一个随机数，把它赋给“index”
                    string nextEmoji = animalEmoji[index];              // 使用名为“index”的数从列表中得到一个表情符号
                    textBlock.Text = nextEmoji;                         // 将表情符号更新到TextBlock
                    animalEmoji.RemoveAt(index);                        // 从列表中删除这个表情符号
                }
            }

            timer.Start();                  //启动计时器，并重置字段
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;         //当玩家找到一个配对时，将matchesFound加1
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)      //如果8对动物已经找到，就重置游戏
            {
                SetUpGame();
            }
        }
    }
}
