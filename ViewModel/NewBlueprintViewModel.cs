using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScribeSharp.Messages;
using ScribeSharp.Model;

namespace ScribeSharp.ViewModel
{
    public class NewBlueprintViewModel : ViewModelBase
    {
        List<string> _poolItems;

        public List<string> PoolItems
        {
            get { return _poolItems;}
            set { Set(() => PoolItems, ref _poolItems, value); }
        }

        string _blueprintName;

        public string BlueprintName
        {
            get { return _blueprintName;}
            set { Set(() => BlueprintName, ref _blueprintName, value); }
        }

        string _selectedPool;

        public string SelectedPool
        {
            get { return _selectedPool;}
            set { Set(() => SelectedPool, ref _selectedPool, value); }
        }

        public NewBlueprintViewModel()
        {
            OkButtonCommand = new RelayCommand<Window>(OkButtonMethod);
            CancelButtonCommand = new RelayCommand<Window>(CancelButtonMethod);
            ClosingCommand = new RelayCommand<EventArgs>(ClosingMethod);
            PoolItems = GetPoolItems();
        }

        public RelayCommand<Window> OkButtonCommand { get; private set; }

        public RelayCommand<Window> CancelButtonCommand { get; private set; }

        public RelayCommand<EventArgs> ClosingCommand { get; set; }

        public void OkButtonMethod(Window window)
        {
            if (string.IsNullOrEmpty(SelectedPool) && string.IsNullOrEmpty(BlueprintName))
            {
                MessageBox.Show(window, "You didn't select anything, idiot", "Error");
                return;
            }
            if (string.IsNullOrEmpty(SelectedPool))
            {
                MessageBox.Show(window, "Please select a Pool.", "Error");
                return;
            }

            if (string.IsNullOrEmpty(BlueprintName))
            {
                MessageBox.Show(window, "Invalid Blueprint name.", "Error");
                return;
            }

            var newBlueprint = new BlueprintItem
            {
                ItemName = BlueprintName,
                PoolName = SelectedPool
            };

            var msg = new AddBlueprintMessage((b, message) => OkButtonCallback(b, message, window))
            {
                Item = newBlueprint
            };

            MessengerInstance.Send(msg);
        }

        void OkButtonCallback(bool b, string message, Window window)
        {
            if (b)
            {
                window.Close();
            }
            else
            {
                MessageBox.Show(window, message, "Error");
            }
        }

        public void CancelButtonMethod(Window window)
        {
            window.Close();
        }

        void ClosingMethod(EventArgs obj)
        {
            BlueprintName = null;
            SelectedPool = PoolItems[0];
        }

        List<string> GetPoolItems()
        {
            return Pools.allPools.Select(pool => pool.metaData.poolName).ToList();
        }
    }
}