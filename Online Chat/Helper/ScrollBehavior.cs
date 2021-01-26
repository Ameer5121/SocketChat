using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Online_Chat.Helper
{
    class ScrollBehavior
    {
        private static ListBox _listbox;
        private static INotifyCollectionChanged _collection;
        public static bool GetScrollOnNewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollOnNewItemProperty);
        }

        public static void SetScrollOnNewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollOnNewItemProperty, value);
        }

        public static readonly DependencyProperty ScrollOnNewItemProperty =
            DependencyProperty.RegisterAttached(
                "ScrollOnNewItem",
                typeof(bool),
                typeof(ScrollBehavior),
                new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

        private static void OnScrollOnNewItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            _listbox = d as ListBox;
            _listbox.Loaded += OnLoaded;         
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            _collection = _listbox.ItemsSource as INotifyCollectionChanged;           
            if (_collection != null)
            {
                _collection.CollectionChanged += OnCollectionChanged;
                _listbox.Loaded -= OnLoaded;
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           if(e.Action == NotifyCollectionChangedAction.Add)
           {
               _listbox.ScrollIntoView(e.NewItems[0]);
               _listbox.SelectedItem = e.NewItems[0];
           }
        }
    }
}
