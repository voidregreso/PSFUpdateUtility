using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;

namespace Hacer
{
    public class CollapseBehavior : Behavior<ProgressRing>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            SetVisibility();
            DependencyPropertyDescriptor.FromProperty(ProgressRing.IsActiveProperty, typeof(ProgressRing))
                .AddValueChanged(AssociatedObject, OnIsActiveChanged);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DependencyPropertyDescriptor.FromProperty(ProgressRing.IsActiveProperty, typeof(ProgressRing))
                .RemoveValueChanged(AssociatedObject, OnIsActiveChanged);
        }

        private void OnIsActiveChanged(object sender, EventArgs eventArgs)
        {
            SetVisibility();
        }

        private void SetVisibility()
        {
            AssociatedObject.Visibility = AssociatedObject.IsActive ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
    }
}