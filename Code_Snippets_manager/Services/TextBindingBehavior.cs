using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit;
using System.Windows;

namespace Code_Snippets_manager.Services
{
    public static class TextBindingBehavior
    {
        public static readonly DependencyProperty BoundTextProperty =
            DependencyProperty.RegisterAttached("BoundText", typeof(string), typeof(TextBindingBehavior),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundTextChanged));

        public static string GetBoundText(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundTextProperty);
        }

        public static void SetBoundText(DependencyObject obj, string value)
        {
            obj.SetValue(BoundTextProperty, value);
        }

        private static void OnBoundTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor editor)
            {
                if (!editor.Text.Equals(e.NewValue))
                {
                    editor.Text = (string)e.NewValue;
                }
            }
        }
    }
}
