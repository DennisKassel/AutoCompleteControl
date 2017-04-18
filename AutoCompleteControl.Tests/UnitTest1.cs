using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoCompleteControl.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string name;
        private Color color;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }
        [TestMethod]
        public void TestMethod1()
        {
            ContentControl contentControl = new ContentControl();
            string template =
                @"<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                                   xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                                   xmlns:controls='clr-namespace:AutoCompleteControl.Controls;assembly=AutoCompleteControl'
                TargetType =""ContentControl""> 
                <Border x:Name=""B1"">
                <ContentPresenter Content=""{Binding RelativeSource={RelativeSource TemplatedParent}, Path=controls:ControlsHelper.Foreground}""/>
                </Border>
                </ControlTemplate>";
            contentControl.SetBinding(Controls.ControlsHelper.ForegroundProperty,
                new Binding("Color") {Source = this, Mode = BindingMode.TwoWay});
            var controlTemplate = XamlReader.Parse(template) as ControlTemplate;
            contentControl.Template = controlTemplate;
            this.color = Color.FromRgb(255, 0, 0);
            contentControl.ApplyTemplate();
            var border = contentControl.Template.FindName("B1", contentControl);
            Assert.IsNotNull(border);   
        }
    }
}
