using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCompleteControl
{
    public interface ISelectionAdapter
    {
        event EventHandler SelectionChanged ;

        object SelectedItem { get; set; }

        IEnumerable ItemsSource { get; set; }
    }
}