using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace TestWpf
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
        // 样例:
        // StringSplit("aaaa[1111,2222,]bbbb",new char[]{'[',',',']'},2,false,false)
        // 返回 "aaaa" "1111,2222,]bbbb"
        //
        // StringSplit("aaaa[1111,2222,]bbbb",new char[]{'[',',',']'},5,false,false)
        // 返回 "aaaa" "1111" "2222" "" "bbbb"
        //
        // StringSplit("aaaa[1111,2222,]bbbb",new char[] { '[', ',', ']' },2,false,true)
        // 返回 "bbbb" "aaaa[1111,2222,"
        //
        // StringSplit("aaaa[1111,2222,]bbbb",new char[] { '[', ',', ']' },2,true,true)
        // 返回 "bbbb" "aaaa[1111,2222"
        //
        // StringSplit("aaaa[1111,2222,]bbbb",new char[] { '[', ',', ']' },5,true,true)
        // 返回 "bbbb" "2222" "1111" "aaaa" ""
        //
        // StringSplit("[[1111,2222,]bbbb],",new char[] { '[', ',', ']' },5,true,false)
        // 返回 "1111" "2222" "bbbb" "" ""
        //
        // StringSplit("[[1111,2222,]bbbb],",new char[] { '[', ',', ']' },5,false,false)
        // 返回 "" "" "1111" "2222" "]bbbb],"

        string[] StringSplit(string srcString, char[] separatorChars, int count, bool ignoreEmpty, bool isReverse)
        {
            List<string> ret = new List<string>();
            int stringCount = 0;
            string tempString = new string(srcString);
            if (isReverse) tempString = new string(tempString.Reverse().ToArray());
            string lastString = "";
            foreach (var i in tempString)
            {
                if (separatorChars.Contains(i))
                {
                    if (stringCount >= count - 1)
                    {
                        separatorChars = new char[] { };
                        continue;
                    }
                    if (lastString.Equals("") && ignoreEmpty == false)
                    {
                        ret.Add(lastString);
                        lastString = "";
                        stringCount++;
                    }
                    else if (!lastString.Equals(""))
                    {
                        ret.Add(lastString);
                        lastString = "";
                        stringCount++;
                    }
                }
                else
                {
                    lastString += i;
                }
            }
            if (!lastString.Equals(""))
            {
                ret.Add(lastString);
                lastString = "";
                stringCount++;
            }

            while (stringCount < count)
            {
                ret.Add("");
                stringCount++;
            }
            return ret.ToArray();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string[] ans = StringSplit(SrcString.Text, SeparatorChars.Text.ToCharArray(), int.Parse(Count.Text), (bool)IgnoreEmpty.IsChecked,
                (bool)IsReverse.IsChecked);
            string message = "";
            foreach (var str in ans)
            {
                string toAddString = "";
                if ((bool)IsReverse.IsChecked)
                {
                    toAddString = new string(str.Reverse().ToArray());
                }
                else
                {
                    toAddString = str;
                }
                string temp = "\"" + toAddString + "\"" + "\n";
                message += temp;
            }
            AnsText.Text = message;
        }

        private void CanvasButton1_Background_OnClick(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                var brush = new SolidColorBrush(Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));
                CanvasButton1.Background = brush;
                // 在这里使用所选的颜色
            }
        }
        private void CanvasButton1_Foreground_OnClick(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                var brush = new SolidColorBrush(Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));
                CanvasButton1.Foreground = brush;
                // 在这里使用所选的颜色
            }
        }

        private void CanvasButton2_Background_OnClick(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                var brush = new SolidColorBrush(Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));
                CanvasButton2.Background = brush;
                // 在这里使用所选的颜色
            }
        }
        private void CanvasButton2_Foreground_OnClick(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;
                var brush = new SolidColorBrush(Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));
                CanvasButton2.Foreground = brush;
                // 在这里使用所选的颜色
            }
        }
    }
}
