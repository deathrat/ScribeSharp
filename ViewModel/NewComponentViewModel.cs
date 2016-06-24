using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Entitas;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScribeSharp.Messages;
using ScribeSharp.Model;

namespace ScribeSharp.ViewModel
{
    public class NewComponentViewModel : ViewModelBase
    {
        BlueprintItem _blueprintItem;

        List<string> _componentTypes;
        public List<string> ComponentTypes
        {
            get { return _componentTypes; }
            set { Set(() => ComponentTypes, ref _componentTypes, value); }
        }

        string _selectedType;
        public string SelectedType
        {
            get { return _selectedType; }
            set { Set(() => SelectedType, ref _selectedType, value); }
        }

        public NewComponentViewModel()
        {
            OkButtonCommand = new RelayCommand<Window>(OkButtonMethod);
            CancelButtonCommand = new RelayCommand<Window>(CancelButtonMethod);
            ClosingCommand = new RelayCommand<EventArgs>(ClosingMethod);

            MessengerInstance.Register<SendBlueprintMessage>(this, ReceiveBlueprintMessage);
        }

        void ReceiveBlueprintMessage(SendBlueprintMessage action)
        {
            _blueprintItem = action.Item;
            ComponentTypes = GetComponentTypes();
        }

        List<string> GetComponentTypes()
        {
            var componentPool = _blueprintItem.PoolName;

            return Pools.allPools.Where(pool => pool.metaData.poolName == componentPool)
                .Select(pool => pool.metaData.componentNames.ToList()).FirstOrDefault();
        }

        ComponentItem GetItem(string name)
        {
            var componentPool = _blueprintItem.PoolName;
            var pool = Pools.allPools.FirstOrDefault(tempPool => tempPool.metaData.poolName == componentPool);

            Type selectedType = null;
            for (int i = 0; i < pool.metaData.componentNames.Length; i++)
            {
                if (pool.metaData.componentNames[i] == name)
                {
                    selectedType = pool.metaData.componentTypes[i];
                    break;
                }
            }

            var componentItem = new ComponentItem()
            {
                ItemName = name,
                ItemType = selectedType
            };

            return componentItem;
        }

        void OkButtonMethod(Window window)
        {
            if (string.IsNullOrEmpty(SelectedType))
                return;

            var componentName = SelectedType;
            var componentItem = GetItem(componentName);

            var msg = new SendComponentMessage()
            {
                Item = componentItem
            };

            MessengerInstance.Send(msg);

            window.Close();
        }

        void CancelButtonMethod(Window window)
        {
            window.Close();
        }

        void ClosingMethod(EventArgs obj)
        {
            SelectedType = ComponentTypes[0];
        }

        public RelayCommand<Window> OkButtonCommand { get; set; }

        public RelayCommand<Window> CancelButtonCommand { get; set; }

        public RelayCommand<EventArgs> ClosingCommand { get; set; }
    }
}