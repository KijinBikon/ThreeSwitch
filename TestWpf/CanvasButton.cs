using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
    ///     <MyNamespace:CanvasButton/>
    ///
    /// </summary>
    public class CanvasButton : Control
    {
        private Rectangle _rectangle;
        private TextBlock _textBlock;
        private Canvas _canvas;


        static CanvasButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasButton),
                new FrameworkPropertyMetadata(typeof(CanvasButton)));
        }

        public CanvasButton()
        {
            AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown), true);

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBlock myLabel = (TextBlock)GetTemplateChild("PART_TextBlock");
            if (NowSwitch == 0)
            {
                myLabel.Text = LeftText;
            }
            else if (NowSwitch == 1)
            {
                myLabel.Text = MiddleText;
            }
            else if (NowSwitch == 2)
            {
                myLabel.Text = RightText;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsThree)
            {
                ThreeMove(sender, e);
            }
            else
            {
                TwoMove(sender, e);
            }
        }


        private void ThreeMove(object sender, MouseButtonEventArgs e)
        {
            _rectangle = GetTemplateChild("PART_Rectangle") as Rectangle;
            _canvas = GetTemplateChild("PART_Canvas") as Canvas;
            _textBlock = GetTemplateChild("PART_TextBlock") as TextBlock;

            Point position = e.GetPosition(this);
            string[] switchText = { LeftText, MiddleText, RightText };
            int oldPosition = NowSwitch, newPosition;
            double width = _canvas.ActualWidth;
            double height = _canvas.ActualHeight;
            double[] rectPositions = new double[3];
            double p = width / 3.0;
            rectPositions[0] = 0;
            for (int i = 1; i < 3; i++)
            {
                rectPositions[i] = rectPositions[i - 1] + p;
            }

            if (position.X < width / 3)
            {
                newPosition = 0;
            }
            else if (position.X < width * 2 / 3)
            {
                newPosition = 1;
            }
            else
            {
                newPosition = 2;
            }

            string targetText = switchText[newPosition];
            oldPosition = NowSwitch;
            NowSwitch = newPosition;
            SetPosition(rectPositions, oldPosition, newPosition, targetText);

        }

        private void TwoMove(object sender, MouseButtonEventArgs e)
        {
            _rectangle = GetTemplateChild("PART_Rectangle") as Rectangle;
            _canvas = GetTemplateChild("PART_Canvas") as Canvas;
            _textBlock = GetTemplateChild("PART_TextBlock") as TextBlock;

            Point position = e.GetPosition(this);
            int oldPosition = NowSwitch, newPosition;
            string[] switchText = { LeftText, MiddleText, RightText };
            double width = _canvas.ActualWidth;
            double height = _canvas.ActualHeight;
            double[] rectPositions = new double[3];
            double p = width / 2.0;

            rectPositions[0] = 0;
            for (int i = 1; i < 2; i++)
            {
                rectPositions[i] = rectPositions[i - 1] + p;
            }

            if (position.X < width / 2)
            {
                newPosition = 0;
            }
            else
            {
                newPosition = 1;
            }

            string targetText = switchText[newPosition];
            oldPosition = NowSwitch;
            NowSwitch = newPosition;
            SetPosition(rectPositions, oldPosition, newPosition, targetText);
        }

        private void SetPosition(double[] rectPositions, int oldPosition, int newPosition, string targetText)
        {
            if (oldPosition == newPosition) return;
            DoubleAnimation positionAnimation = new DoubleAnimation();
            positionAnimation.From = rectPositions[oldPosition];
            positionAnimation.To = rectPositions[newPosition];
            positionAnimation.Duration = TimeSpan.FromSeconds(0.25);
            positionAnimation.Changed += (sender, args) => { _textBlock.Text = ""; };
            positionAnimation.Completed += (sender, args) =>
            {
                this.TextBlockPosition = NowSwitch * RectangleWidth + this.RectangleWidth / 2;
                Canvas.SetLeft(_textBlock, this.TextBlockPosition);
                _textBlock.Text = targetText;
            };
            _rectangle.BeginAnimation(Canvas.LeftProperty, positionAnimation);
            Canvas.SetLeft(_rectangle, rectPositions[newPosition]);
        }

        private static void OnWidthHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CanvasButton button = (CanvasButton)d;
            button.RectangleWidth = button.IsThree ? button.Width / 3 : button.Width / 2;
            button.RectangleStartLeft = button.RectangleWidth * button.NowSwitch;
            button.TextBlockPosition = button.RectangleStartLeft + button.RectangleWidth / 2;
        }


        public static readonly DependencyProperty NowSwitchProperty =
            DependencyProperty.Register("NowSwitch", typeof(int), typeof(CanvasButton),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnWidthHeightPropertyChanged));

        [Description("按钮所处的现位置")]
        public int NowSwitch
        {
            get { return (int)GetValue(NowSwitchProperty); }
            set { SetValue(NowSwitchProperty, value); }
        }

        public static readonly DependencyProperty RectangleWidthProperty =
            DependencyProperty.Register("RectangleWidth", typeof(double), typeof(CanvasButton),
                new FrameworkPropertyMetadata(50.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        [Description("滑块宽度")]
        public double RectangleWidth
        {
            get { return (double)GetValue(RectangleWidthProperty); }
            private set { SetValue(RectangleWidthProperty, value); }
        }

        public static readonly DependencyProperty RectangleStartLeftProperty =
            DependencyProperty.Register("RectangleStartLeft", typeof(double), typeof(CanvasButton),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        [Description("滑块的启动位置")]
        public double RectangleStartLeft
        {
            get { return (double)GetValue(RectangleStartLeftProperty); }
            private set { SetValue(RectangleStartLeftProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            Control.BackgroundProperty.AddOwner(typeof(CanvasButton),
                new FrameworkPropertyMetadata(Brushes.Gray));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
            Control.ForegroundProperty.AddOwner(typeof(CanvasButton),
                new FrameworkPropertyMetadata(Brushes.Blue));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty WidthProperty =
            FrameworkElement.WidthProperty.AddOwner(typeof(CanvasButton),
                new FrameworkPropertyMetadata(150.0, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnWidthHeightPropertyChanged));

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public static readonly DependencyProperty IsThreeProperty =
            DependencyProperty.Register("IsThree", typeof(bool), typeof(CanvasButton),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                    FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnWidthHeightPropertyChanged));

        [Description("设置按钮的状态数量")]
        public bool IsThree
        {
            get { return (bool)GetValue(IsThreeProperty); }
            set { SetValue(IsThreeProperty, value); }
        }



        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register("LeftText", typeof(string), typeof(CanvasButton),
                new FrameworkPropertyMetadata("低", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MiddleTextProperty =
            DependencyProperty.Register("MiddleText", typeof(string), typeof(CanvasButton),
                new FrameworkPropertyMetadata("中", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register("RightText", typeof(string), typeof(CanvasButton),
                new FrameworkPropertyMetadata("高", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [Description("设置1状态的值")]
        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        [Description("设置2状态的值")]
        public string MiddleText
        {
            get { return (string)GetValue(MiddleTextProperty); }
            set { SetValue(MiddleTextProperty, value); }
        }

        [Description("设置3状态的值（当只有两个状态时该属性无效）")]
        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }


        public static readonly DependencyProperty TextBlockPositionProperty =
            DependencyProperty.Register("TextBlockPosition", typeof(double), typeof(CanvasButton),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double TextBlockPosition
        {
            get { return (double)GetValue(TextBlockPositionProperty); }
            private set { SetValue(TextBlockPositionProperty, value); }
        }
    }
}

