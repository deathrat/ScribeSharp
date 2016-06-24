using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScribeSharp.Messages;
using ScribeSharp.Model;
using ScribeSharp.View.ComponentViews;
using ScribeSharp.ViewModel.ComponentViewModels;

namespace ScribeSharp.ViewModel
{
    public class EditComponentViewModel : ViewModelBase
    {
        public EditComponentViewModel()
        {
            MessengerInstance.Register<EditComponentMessage>(this, ReceiveEditComponentMessage);
            OkButtonCommand = new RelayCommand<Window>(OkButtonMethod);
            CancelButtonCommand = new RelayCommand<Window>(CancelButtonMethod);
            ClosingCommand = new RelayCommand<EventArgs>(ClosingMethod);
        }

        void ReceiveEditComponentMessage(EditComponentMessage action)
        {
            SelectedItem = action.ComponentItem;
        }

        ComponentItem _selectedItem;

        public ComponentItem SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }

        FieldInfo _selectedFieldInfo;

        public FieldInfo SelectedFieldInfo
        {
            get { return _selectedFieldInfo; }
            set
            {
                SwitchView(value.FieldType);
                Set(() => SelectedFieldInfo, ref _selectedFieldInfo, value);
            }
        }

        FrameworkElement _selectedFieldValueControl;

        public FrameworkElement SelectedFieldValueControl
        {
            get
            {
                return _selectedFieldValueControl;
            }
            set { Set(() => SelectedFieldValueControl, ref _selectedFieldValueControl, value); }
        }

        void SwitchView(Type type)
        {
            if (type == typeof(string) || type == typeof(int))
            {
                SelectedFieldValueControl = new StringControlView();
            }
            if (type == typeof(bool))
            {
                SelectedFieldValueControl = new BoolControlView();
            }
        }

        void OkButtonMethod(Window window)
        {
            var componentViewModel = (ComponentControlViewModel)SelectedFieldValueControl.DataContext;
            object objValue = componentViewModel.ComponentFieldValue;

            if (objValue is int)
            {
                componentViewModel.ComponentFieldValue = int.Parse((string)objValue);
            }

            SelectedItem.SetValue(SelectedFieldInfo.Name, objValue);


            window.Close();
        }

        void CancelButtonMethod(Window window)
        {
            window.Close();
        }

        void ClosingMethod(EventArgs obj)
        {
            var componentViewModel = (ComponentControlViewModel)SelectedFieldValueControl.DataContext;
            componentViewModel.ComponentFieldValue = null;
            SelectedItem = null;
        }

        public RelayCommand<Window> OkButtonCommand { get; set; }

        public RelayCommand<Window> CancelButtonCommand { get; set; }

        public RelayCommand<EventArgs> ClosingCommand { get; set; }
    }
}