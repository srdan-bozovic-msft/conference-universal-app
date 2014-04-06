using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class TabPageViewModel : ViewModelBase
    {
        public class TabViewModel : ViewModelBase 
        {
            private TabPageViewModel _pageViewModel;

            private bool _isActive;
            public bool IsActive
            {
                get
                {
                    return _isActive;
                }
                set
                {
                    if (value != _isActive)
                    {
                        foreach (var tab in _pageViewModel._tabs)
                        {
                            tab._isActive = tab == this;
                            tab.RaisePropertyChanged("IsActive");
                        }
                        _pageViewModel.RaisePropertyChanged(()=>_pageViewModel.SelectedIndex);
                    }
                }
            }

            private string _title;
            public string Title
            {
                get
                {
                    return _title;
                }
                set
                {
                    if (value != _title)
                    {
                        _title = value;
                        RaisePropertyChanged(() => Title);
                    }
                }
            }

            public TabViewModel(TabPageViewModel pageViewModel, bool isActive, string title)
            {
                _pageViewModel = pageViewModel;
                _isActive = isActive;
                _title = title;
            }
        }

        private ObservableCollection<TabViewModel> _tabs;
        public ObservableCollection<TabViewModel> Tabs
        {
            get
            {
                return _tabs;
            }
            set
            {
                _tabs = value;
                RaisePropertyChanged("Tabs");
            }
        }

        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < _tabs.Count; i++)
                {
                    if (_tabs[i].IsActive)
                        return i;
                }
                return -1;
            }
            set
            {
                for (int i = 0; i < _tabs.Count; i++)
                {
                    _tabs[i].IsActive = i == value;
                }
            }
        }

        public TabPageViewModel()
        {
            _tabs = new ObservableCollection<TabViewModel>();
        }

        public void InitializeTabs(params string[] tabs)
        {
            _tabs.Clear();
            foreach (var tab in tabs)
            {
                _tabs.Add(new TabViewModel(this, false, tab));
            }
            if(_tabs.Count>0)
            {
                _tabs[0].IsActive = true;
            }
        }
    }
}
