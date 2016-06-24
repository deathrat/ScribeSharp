using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace ScribeSharp.Model
{
    public class BlueprintItem : ObservableObject
    {
        public string ItemName;

        public string PoolName;


        ObservableCollection<ComponentItem> _componentItems; 
        public ObservableCollection<ComponentItem> ComponentItems
        {
            get { return _componentItems; }
            set { _componentItems = value; RaisePropertyChanged(() => ComponentItems); }
        }

        public BlueprintItem()
        {
            ComponentItems = new ObservableCollection<ComponentItem>();
        }

        public override string ToString()
        {
            return $"{ItemName} ({PoolName})";
        }
    }
}