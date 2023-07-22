using System;
using System.Globalization;
using System.Windows.Data;

namespace TestWpf.Converter
{
    public class NowSwitchToLabelTextConverter : IValueConverter
    {
        private string _leftText;
        private string _middleText;
        private string _RightText;
        private int _nowSwicth;

        public NowSwitchToLabelTextConverter(int nowSwicth, string leftText, string middleText, string rightText)
        {
            _leftText = leftText;
            _middleText = middleText;
            _RightText = rightText;
            _nowSwicth = nowSwicth;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch (_nowSwicth)
            {
                case 0:
                    return _leftText;
                case 1:
                    return _middleText;
                case 2:
                    return _RightText;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
