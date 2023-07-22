using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace TestWpf
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestWpf"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TestWpf;assembly=TestWpf"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:TriStateButton/>
    ///
    /// </summary>
    public class TriStateButton : Control
    {
        private Grid _grid;
        private Rectangle _rectangle;
        private TextBlock _textBlock;
        static TriStateButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TriStateButton), new FrameworkPropertyMetadata(typeof(TriStateButton)));
        }

        public TriStateButton()
        {
            AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsThree == true)
            {
                ThreeMove(sender, e);
            }
            else
            {
                TwoMove(sender, e);
            }
        }

        private void MoveRectangle(Rectangle rectangle, int oldPos, int newPos)
        {
            _grid.Children.Remove(rectangle);

        }

        private void ThreeMove(object sender, MouseButtonEventArgs e)
        {
            // Point position = e.GetPosition(this);
            _rectangle = GetTemplateChild("PART_Rectangle") as Rectangle;
            _grid = GetTemplateChild("PART_Grid") as Grid;
            _textBlock = GetTemplateChild("PART_TextBlock") as TextBlock;
            // 获取鼠标点击位置相对于 Grid 控件的位置
            Point position = e.GetPosition(_grid);

            int oldPosition, newPosition;
            oldPosition = NowSwitch;


            // 获取 Grid 控件的宽度和高度
            double width = _grid.ActualWidth;
            double height = _grid.ActualHeight;
            // 计算矩形应该移动到的位置
            if (position.X < width / 3)
            {
                // 移动到第一个区域
                NowSwitch = 0;
                // Grid.SetColumn(_rectangle, NowSwitch);
                // Grid.SetColumn(_textBlock, NowSwitch);
                _textBlock.Text = LeftText;

            }
            else if (position.X < width * 2 / 3)
            {
                // 移动到第二个区域
                NowSwitch = 1;
                // Grid.SetColumn(_rectangle, NowSwitch);
                // Grid.SetColumn(_textBlock, NowSwitch);
                _textBlock.Text = MiddleText;
            }
            else
            {
                // 移动到第三个区域
                NowSwitch = 2;
                // Grid.SetColumn(_rectangle, NowSwitch);
                // Grid.SetColumn(_textBlock, NowSwitch);
                _textBlock.Text = RightText;
            }
            newPosition = NowSwitch;



            // DoubleAnimation positionAnimation = new DoubleAnimation();
            // positionAnimation.From = oldPosition;
            // positionAnimation.To = newPosition;
            // positionAnimation.Duration = TimeSpan.FromSeconds(0.5);
            // DoubleAnimation positionAnimation = new DoubleAnimation(oldPosition, newPosition, TimeSpan.FromSeconds(0.5));
            // _rectangle.BeginAnimation(Grid.ColumnProperty, positionAnimation);
        }

        private void TwoMove(object sender, MouseButtonEventArgs e)
        {
            // Point position = e.GetPosition(this);
            _rectangle = GetTemplateChild("PART_Rectangle") as Rectangle;
            _grid = GetTemplateChild("PART_Grid") as Grid;
            _textBlock = GetTemplateChild("PART_TextBlock") as TextBlock;
            // 获取鼠标点击位置相对于 Grid 控件的位置
            Point position = e.GetPosition(_grid);

            // 获取 Grid 控件的宽度和高度
            double width = _grid.ActualWidth;
            double height = _grid.ActualHeight;
            // 计算矩形应该移动到的位置
            if (position.X < width / 2)
            {
                // 移动到第一个区域
                NowSwitch = 0;
                Grid.SetColumn(_rectangle, NowSwitch);
                Grid.SetColumn(_textBlock, NowSwitch);
                _textBlock.Text = LeftText;
                // MessageBox.Show("当前选择位置" + NowSwitch);

            }
            else
            {
                // 移动到第二个区域
                NowSwitch = 1;
                Grid.SetColumn(_rectangle, NowSwitch);
                Grid.SetColumn(_textBlock, NowSwitch);
                _textBlock.Text = MiddleText;
                // MessageBox.Show("当前选择位置" + NowSwitch);
            }
        }

        private static void OnWidthHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TriStateButton button = (TriStateButton)d;
            button.RectangleWidth = button.IsThree ? button.Width / 3 : button.Width / 2;
        }


        public static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(TriStateButton),
                new FrameworkPropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ForegroundProperty =
            Control.ForegroundProperty.AddOwner(typeof(TriStateButton),
                new FrameworkPropertyMetadata(Brushes.Blue));

        public static readonly DependencyProperty WidthProperty =
            FrameworkElement.WidthProperty.AddOwner(typeof(TriStateButton),
                new FrameworkPropertyMetadata(150.0, FrameworkPropertyMetadataOptions.AffectsMeasure, OnWidthHeightPropertyChanged));

        public static readonly DependencyProperty RectangleWidthProperty =
            DependencyProperty.Register("RectangleWidth", typeof(double), typeof(TriStateButton),
                new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsThreeProperty =
            DependencyProperty.Register("IsThree", typeof(bool), typeof(TriStateButton),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register("LeftText", typeof(string), typeof(TriStateButton),
                new FrameworkPropertyMetadata("低", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MiddleTextProperty =
            DependencyProperty.Register("MiddleText", typeof(string), typeof(TriStateButton),
                new FrameworkPropertyMetadata("中", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register("RightText", typeof(string), typeof(TriStateButton),
                new FrameworkPropertyMetadata("高", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ThirdGridWidthProperty =
            DependencyProperty.Register("ThirdGridWidth", typeof(string), typeof(TriStateButton),
                new FrameworkPropertyMetadata("*", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty NowSwitchProperty =
            DependencyProperty.Register("NowSwitch", typeof(int), typeof(TriStateButton),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Brush Background
        {
            get
            {
                return (Brush)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }

        public Brush Foreground
        {
            get
            {
                return (Brush)GetValue(ForegroundProperty);
            }
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }
        public int NowSwitch
        {
            get
            {
                return (int)GetValue(NowSwitchProperty);
            }
            set
            {
                SetValue(NowSwitchProperty, value);
            }
        }

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public string ThirdGridWidth
        {
            get
            {
                return (string)GetValue(ThirdGridWidthProperty);
            }
            set
            {
                SetValue(ThirdGridWidthProperty, value);
            }
        }

        public double RectangleWidth
        {
            get
            {
                return (double)GetValue(RectangleWidthProperty);
            }
            set
            {
                SetValue(RectangleWidthProperty, value);
            }
        }


        [Description("设置按钮的状态数量")]
        public bool IsThree
        {
            get
            {
                return (bool)GetValue(IsThreeProperty);
            }
            set
            {
                SetValue(IsThreeProperty, value);
            }
        }

        [Description("设置1状态的值")]
        public string LeftText
        {
            get
            {
                return (string)GetValue(LeftTextProperty);
            }
            set
            {
                SetValue(LeftTextProperty, value);
            }
        }

        [Description("设置2状态的值")]
        public string MiddleText
        {
            get
            {
                return (string)GetValue(MiddleTextProperty);
            }
            set
            {
                SetValue(MiddleTextProperty, value);
            }
        }

        [Description("设置3状态的值（当只有两个状态时该属性无效）")]
        public string RightText
        {
            get
            {
                return (string)GetValue(RightTextProperty);
            }
            set
            {
                SetValue(RightTextProperty, value);
            }
        }
    }
}
