using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using HaleyIconsExplorer.ViewModels;

namespace HaleyIconsExplorer {
    /// <summary>
    /// Interaction logic for IconsExplorerControl.
    /// </summary>
    public partial class IconsExplorerControl : UserControl {
        /// <summary>
        /// Initializes a new instance of the <see cref="IconsExplorerControl"/> class.
        /// </summary>
        public IconsExplorerControl() {
            this.InitializeComponent();
            this.DataContext = ContainerStore.DI.Resolve<IconsExplorerVM>();
        }
    }
}