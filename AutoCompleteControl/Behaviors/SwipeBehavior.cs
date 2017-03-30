using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AutoCompleteControl.Behaviors
{
    public class SwipeBehavior : Behavior<ListBoxItem>
    {
        private Point originPosition;

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
            var listBoxItem = (ListBoxItem)dependencyObject;
            MouseTouchDevice.RegisterEvents(listBoxItem);
            if (listBoxItem != null)
            {
                var isAttached = (bool)e.NewValue;
                var behaviors = Interaction.GetBehaviors(listBoxItem);
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
            this.AssociatedObject.TouchDown += AssociatedObjectOnTouchDown;
            this.AssociatedObject.TouchUp += AssociatedObjectOnTouchUp;
            Application.Current.MainWindow.TouchUp += AssociatedObjectOnTouchUp;
        }

        private Point initialPoint;

        private void AssociatedObjectOnTouchUp(object sender, TouchEventArgs touchEventArgs)
        {
            var touchPoint = touchEventArgs.GetTouchPoint(Application.Current.MainWindow);
            Point relativePoint = AssociatedObject.TransformToAncestor(Application.Current.MainWindow)
               .Transform(new Point(0, 0));
            Rect bounds = new Rect(relativePoint,
                new Point(relativePoint.X + AssociatedObject.ActualWidth, relativePoint.Y + AssociatedObject.ActualHeight));
            if (initialPoint != null && bounds.Contains(touchPoint.Position) && touchPoint.Position.X - initialPoint.X > 100 )
            {
                AssociatedObject.IsSelected = !AssociatedObject.IsSelected;
                touchEventArgs.Handled = true;
                initialPoint = default(Point);
            }
        }

        private void AssociatedObjectOnTouchDown(object sender, TouchEventArgs touchEventArgs)
        {
            initialPoint = touchEventArgs.GetTouchPoint(Application.Current.MainWindow).Position;
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