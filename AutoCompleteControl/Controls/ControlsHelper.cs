using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutoCompleteControl.Controls
{
    public class ControlsHelper
    {

        public static Color GetForeground(DependencyObject obj)
        {
            return (Color)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Color value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Color), typeof(FrameworkElement), new PropertyMetadata(null));


    }
}
