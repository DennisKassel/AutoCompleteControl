using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AutoCompleteControl.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Selector", Type = typeof(Selector))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(TextBox))]
    public class AutoCompleteControl : Control
    {
        private TextBox textBox;
        private Popup popup;

        public static readonly DependencyProperty PopulateDelayProperty = DependencyProperty.Register("PopulateDelay", typeof(int), typeof(AutoCompleteControl), new PropertyMetadata(0, OnPopulateDelayChanged));

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteControl), new PropertyMetadata());

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoCompleteControl), new PropertyMetadata());

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteControl), new PropertyMetadata());

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteControl), new PropertyMetadata());

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteControl), new PropertyMetadata());

        public int PopulateDelay
        {
            get { return (int)GetValue(PopulateDelayProperty); }
            set { SetValue(PopulateDelayProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnPopulateDelayChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as AutoCompleteControl;
        }

        public override void OnApplyTemplate()
        {
            if (this.textBox != null)
            {
                this.textBox.GotFocus -= TextBox_GotFocus;
                this.textBox.LostFocus -= TextBox_LostFocus;
                this.textBox.TextChanged -= TextBox_TextChanged;
            }
            if (this.popup != null)
            {
                this.popup.Closed -= OnPopupClosed;
                this.popup.GotFocus -= OnPopupGotFocus;
                this.popup.LostFocus -= OnPopupLostFocus;
            }
            base.OnApplyTemplate();
            this.textBox = (TextBox)this.GetTemplateChild("PART_TextBox");
            this.popup = (Popup)this.GetTemplateChild("PART_Popup");
            this.textBox.GotFocus += TextBox_GotFocus;
            this.textBox.LostFocus += TextBox_LostFocus;
            this.textBox.TextChanged += TextBox_TextChanged;
            if (this.popup != null)
            {
                this.popup.Closed -= OnPopupClosed;
                this.popup.GotFocus += OnPopupGotFocus;
                this.popup.LostFocus += OnPopupLostFocus;

            }

            base.OnApplyTemplate();
        }


        private void OpenPopup()
        {
            if (this.popup != null)
            {
                this.popup.IsOpen = true;
            }
        }

        private void OnTextBoxTextChanged()
        {
            
        }

        private void OnPopupGotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
        }

        private void OnPopupLostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs routedEventArgs)
        {

        }
    }
}
