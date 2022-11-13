using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace HaleyIconsExplorer.Models {
    public class IconInfo : ChangeNotifier  {
        private string _name;
        public string Name {
            get { return _name; }
            set { SetProp(ref _name, value); }
        }

        private ImageSource _iconSource;
        public ImageSource IconSource {
            get { return _iconSource; }
            set { SetProp(ref _iconSource, value); }
        }

        private string _type;
        public string Type {
            get { return _type; }
            set { SetProp(ref _type, value); }
        }

        public IconInfo() {
        }
    }
}
