using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace test2.ViewModels
{
    internal partial class Page2ViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string title = "Это 2 сттаница";

    }
}
