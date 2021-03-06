using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Extensions
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListBoxHelper), new PropertyMetadata(default(IList), OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject d) => (IList)d.GetValue(SelectedItemsProperty);

        public static void SetSelectedItems(DependencyObject d, IList value) => d.SetValue(SelectedItemsProperty, value);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;
            ReSetSelectedItems(listBox);
            listBox.SelectionChanged += delegate
            {
                ReSetSelectedItems(listBox);
            };
        }

        private static void ReSetSelectedItems(ListBox listBox)
        {
            var selectedItems = GetSelectedItems(listBox);
            selectedItems.Clear();
            if (listBox.SelectedItems != null)
            {
                foreach (var item in listBox.SelectedItems)
                {
                    _ = selectedItems.Add(item);
                }
            }
        }
    }
}