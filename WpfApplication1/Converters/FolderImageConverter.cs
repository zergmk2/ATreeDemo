using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using WpfApplication1.Model;

namespace WpfApplication1.Converters
{
    public class FolderImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //TreeViewItem item = (TreeViewItem)value;
            if (value is FolderItem)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }

    }
}
