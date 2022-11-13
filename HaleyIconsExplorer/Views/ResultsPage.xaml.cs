using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using HaleyIconsExplorer.Models;
using HaleyIconsExplorer.Enums;


namespace HaleyIconsExplorer.Views {
    /// <summary>
    /// Interaction logic for ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : UserControl {
        public ResultsPage() {
            InitializeComponent();
        }

        public ObservableCollection<IconInfo> IconsProvider {
            get { return (ObservableCollection<IconInfo>)GetValue(IconsProviderProperty); }
            set { SetValue(IconsProviderProperty, value); }
        }

        public static readonly DependencyProperty IconsProviderProperty =
            DependencyProperty.Register("IconsProvider", typeof(ObservableCollection<IconInfo>), typeof(ResultsPage), new PropertyMetadata(null));
    }
}
