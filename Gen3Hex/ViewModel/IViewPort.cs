﻿using HavenSoft.Gen3Hex.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace HavenSoft.Gen3Hex.ViewModel {
   public interface IViewPort : ITabContent, INotifyCollectionChanged, INotifyPropertyChanged {
      int Width { get; set; }
      int Height { get; set; }

      int MinimumScroll { get; }
      int ScrollValue { get; }
      int MaximumScroll { get; }
      ObservableCollection<string> Headers { get; }
      ICommand Scroll { get; }

      HexElement this[int x, int y] { get; }
      bool IsSelected(Point point);

      IReadOnlyList<int> Find(string search);
      IViewPort CreateChildView(int offset);
   }
}
