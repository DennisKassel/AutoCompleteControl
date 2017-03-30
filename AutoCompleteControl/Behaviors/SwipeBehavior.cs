using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace AutoCompleteControl.Behaviors
{
    public class SwipeBehavior : Behavior<ContentControl>
    {
        private Point originPosition;
        private TouchDevice currentTouchDevice;
        private Point initialPoint;
        private Border highlightVisualElement;

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(SwipeBehavior), new PropertyMetadata(default(bool), OnIsAttachedPropertyChanged));

        public static void SetIsAttached(DependencyObject element, bool value)
        {
            element.SetValue(IsAttachedProperty, value);
        }

        public static bool GetIsAttached(DependencyObject element)
        {
            return (bool)element.GetValue(IsAttachedProperty);
        }



        private static bool handlerRegistered = false;

        private static void OnIsAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var contentControl = (ContentControl)dependencyObject;
            //MouseTouchDevice.RegisterEvents(ContentControl);
            if (contentControl != null)
            {
                var isAttached = (bool)e.NewValue;
                var behaviors = Interaction.GetBehaviors(contentControl);
                var swipeBehavior = behaviors.OfType<SwipeBehavior>().FirstOrDefault();

                if (!isAttached && swipeBehavior != null)
                {
                    behaviors.Remove(swipeBehavior);
                }
                else if (isAttached && swipeBehavior == null)
                {
                    behaviors.Add(new SwipeBehavior());
                }
            }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.PreviewTouchDown += AssociatedObject_PreviewTouchDown;
            this.AssociatedObject.TouchDown += AssociatedObjectOnTouchDown;
            this.AssociatedObject.TouchMove+=AssociatedObjectOnTouchMove;
            this.AssociatedObject.LostTouchCapture += AssociatedObject_LostTouchCapture;
            this.AssociatedObject.PreviewTouchUp += AssociatedObject_PreviewTouchUp;
            this.AssociatedObject.Loaded += (sender, args) =>
            {
                this.highlightVisualElement = (Border)this.AssociatedObject.Template?.FindName("HighlightVisualElement", this.AssociatedObject);
                if (this.highlightVisualElement != null)
                this.highlightVisualElement.Visibility = Visibility.Visible;
            };

        }

        private void AssociatedObject_LostTouchCapture(object sender, TouchEventArgs e)
        {
            if (this.currentTouchDevice != null)
            {
                this.AssociatedObject.ReleaseTouchCapture(currentTouchDevice);
            }
        }

        private void AssociatedObject_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (this.currentTouchDevice != null)
            {
                this.AssociatedObject.ReleaseTouchCapture(currentTouchDevice);
            }
            var touchPoint = e.GetTouchPoint(Application.Current.MainWindow);
            Point relativePoint = AssociatedObject.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            Rect bounds = new Rect(relativePoint,
                new Point(relativePoint.X + AssociatedObject.ActualWidth, relativePoint.Y + AssociatedObject.ActualHeight));

            if (initialPoint != null && bounds.Contains(touchPoint.Position) && touchPoint.Position.X - initialPoint.X > 100)
            {
                this.highlightVisualElement= (Border) this.AssociatedObject.Template.FindName("HighlightVisualElement", this.AssociatedObject);
                this.highlightVisualElement.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }

        private void AssociatedObject_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (this.currentTouchDevice != null)
            {
                this.AssociatedObject.ReleaseTouchCapture(currentTouchDevice);
            }
            this.AssociatedObject.CaptureTouch((TouchDevice) e.Device);
        }

        private void AssociatedObjectOnTouchMove(object sender, TouchEventArgs e)
        {
            var touchPoint = e.GetTouchPoint(Application.Current.MainWindow);
            Point relativePoint = AssociatedObject.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            Rect bounds = new Rect(relativePoint,
                new Point(relativePoint.X + AssociatedObject.ActualWidth, relativePoint.Y + AssociatedObject.ActualHeight));

            if (initialPoint != null && bounds.Contains(touchPoint.Position) && touchPoint.Position.X - initialPoint.X > 30)
            {
                var margin = Math.Max(0, 100 - (touchPoint.Position.X - initialPoint.X));
                this.highlightVisualElement.Margin = new Thickness(margin, this.highlightVisualElement.Margin.Top, margin, this.highlightVisualElement.Margin.Bottom);
                this.highlightVisualElement.Visibility = Visibility.Visible;
                this.highlightVisualElement.Background = Brushes.LightGreen;
                e.Handled = true;
            }
        }
 
        private void AssociatedObjectOnTouchDown(object sender, TouchEventArgs touchEventArgs)
        {
            initialPoint = touchEventArgs.GetTouchPoint(Application.Current.MainWindow).Position;
            //AssociatedObject.CaptureMouse();
        }

        private void AssociatedObject_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            this.originPosition = new Point(e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
        }

        private void AssociatedObject_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.IsInertial)
            {
                Point position = new Point(e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
                if (position.X - position.X >= 300)
                {
                    e.Complete();
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}