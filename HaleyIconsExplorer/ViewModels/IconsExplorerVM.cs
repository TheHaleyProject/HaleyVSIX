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
using System.Windows.Input;
using HaleyIconsExplorer.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HaleyIconsExplorer.Models;
using ipUtils= Haley.IconsPack.Utils;
using System.Windows;
using System.Windows.Threading;

namespace HaleyIconsExplorer.ViewModels {
    public class IconsExplorerVM : BaseVM {
        //Each thread has it's own UI Dispatcher.
        Dispatcher _uiDispatcher;
        DispatcherTimer _clipboardTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };
        bool _initiated = false;

        private ViewEnum _activeView;
        public ViewEnum ActiveView {
            get { return _activeView; }
            set { SetProp(ref _activeView, value); }
        }

        private bool _isCopyStatVisible = false;
        public bool IsCopyStatVisible {
            get { return _isCopyStatVisible; }
            set { SetProp(ref _isCopyStatVisible, value); }
        }

        private ObservableCollection<IconInfo> _brandIcons = new ObservableCollection<IconInfo>();
        public ObservableCollection<IconInfo> BrandIcons {
            get { return _brandIcons; }
            set { SetProp(ref _brandIcons, value); }
        }

        private ObservableCollection<IconInfo> _bsIcons = new ObservableCollection<IconInfo>();
        public ObservableCollection<IconInfo> BSIcons {
            get { return _bsIcons; }
            set { SetProp(ref _bsIcons, value); }
        }

        private ObservableCollection<IconInfo> _internalIcons = new ObservableCollection<IconInfo>();
        public ObservableCollection<IconInfo> InternalIcons {
            get { return _internalIcons; }
            set { SetProp(ref _internalIcons, value); }
        }

        private ObservableCollection<IconInfo> _faIcons = new ObservableCollection<IconInfo>();
        public ObservableCollection<IconInfo> FAIcons {
            get { return _faIcons; }
            set { SetProp(ref _faIcons, value); }
        }

        public ICommand CMDChangeView => new DelegateCommand<object>(changeView);
        public ICommand CmdClipBoardCopy => new DelegateCommand<string>((e) => {
            try {
                Clipboard.SetText(System.Net.WebUtility.HtmlDecode(e));
                InitiateCopyStatVisibility();
            } catch (Exception) {
            }
        });

        void InitiateCopyStatVisibility() {
            IsCopyStatVisible = true;
            _clipboardTimer.Start();
        }

        private void changeView(object obj) {
            if (!(obj is ViewEnum venum)) return;
            ActiveView = venum; //Set the view;
            InitiateIcons();
        }

        public override async void OnViewLoaded(object sender) {
            base.OnViewLoaded(sender);
            try {
                await Task.Run(() => InitiateIcons());
            } catch (Exception) {
                //Dont' do anything for the exception
            }
        }

        async Task InitiateIcons() {
            if (_initiated) return;

            foreach (ViewEnum viewtype in Enum.GetValues(typeof(ViewEnum))) {
                await Task.Run(()=>{ FillIcons(viewtype);});
            }
            _initiated = true;
        }

        private async Task FillIcons(ViewEnum @enum) {
            try {

                Func<Type, List<IconInfo>> listGenerator = (t) => {
                    Array values = Enum.GetValues(t);
                    string _type = t.Name;
                    if (values == null) return null;
                    List<IconInfo> infoList = new List<IconInfo>();

                    foreach (Enum item in values) {
                        var info = new IconInfo() { Type = _type, Name = item.ToString() };
                        if (@enum is ViewEnum.Internal) {
                            info.IconSource = IconFinder.GetIcon(info.Name);
                        } else {
                            info.IconSource = ipUtils.IconFinder.GetIcon(info.Name);
                        }
                        infoList.Add(info);
                    }
                    return infoList;
                };

                await _uiDispatcher?.BeginInvoke(new Action(() => {
                    switch (@enum) {
                        case ViewEnum.Brand:
                            BrandIcons = new ObservableCollection<IconInfo>(listGenerator(typeof(BrandKind)));
                            break;
                        case ViewEnum.Internal:
                            InternalIcons = new ObservableCollection<IconInfo>(listGenerator(typeof(IconKind)));
                            break;
                        case ViewEnum.BootStrap:
                            BSIcons = new ObservableCollection<IconInfo>(listGenerator(typeof(BootStrapKind)));
                            break;
                        case ViewEnum.FontAwe:
                            FAIcons = new ObservableCollection<IconInfo>(listGenerator(typeof(FAKind)));
                            break;
                    }
                }), DispatcherPriority.Send);
            } catch (Exception) {
            }
        }

        public IconsExplorerVM() {
            _uiDispatcher = Dispatcher.CurrentDispatcher;
            _clipboardTimer.Tick += HandleClipboardTimer;
        }

        private void HandleClipboardTimer(object sender, EventArgs e) {
            _clipboardTimer.Stop();
            IsCopyStatVisible = false;
        }
    }
}
