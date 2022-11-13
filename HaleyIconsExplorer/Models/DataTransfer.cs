using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaleyIconsExplorer.Enums;
using System.Windows;

namespace HaleyIconsExplorer.Models {
    public static class DataTransfer {
        public static ViewEnum GetCurrentView(DependencyObject obj) {
            return (ViewEnum)obj.GetValue(CurrentViewProperty);
        }

        public static void SetCurrentView(DependencyObject obj, ViewEnum value) {
            obj.SetValue(CurrentViewProperty, value);
        }

        public static readonly DependencyProperty CurrentViewProperty =
            DependencyProperty.RegisterAttached("CurrentView", typeof(ViewEnum), typeof(DataTransfer), new FrameworkPropertyMetadata(default(ViewEnum)));
    }
}
