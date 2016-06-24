using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScribeSharp.Messages;
using ScribeSharp.Model;
using ScribeSharp.View;

namespace ScribeSharp.ViewModel
{
    public class BlueprintContainerViewModel : ViewModelBase
    {
        ProjectItem _currentProject;

        public BlueprintContainerViewModel()
        {
            AddBlueprintCommand = new RelayCommand(AddBlueprintMethod);
            RemoveBlueprintCommand = new RelayCommand(RemoveBlueprintMethod);

            AddComponentCommand = new RelayCommand(AddComponentMethod);
            RemoveComponentCommand = new RelayCommand(RemoveComponentMethod);

            ComponentDoubleClickCommand = new RelayCommand(ComponentDoubleClickMethod);

            MessengerInstance.Register<AddBlueprintMessage>(this, ReceiveAddBlueprintMessage);
            MessengerInstance.Register<SendComponentMessage>(this, ReceiveAddComponentMessage);
            MessengerInstance.Register<SetProjectMessage>(this, ReceiveSetProjectMessage);
            MessengerInstance.Register<OpenProjectMessage>(this, ReceiveOpenProjectMessage);
            MessengerInstance.Register<SaveProjectMessage>(this, ReceiveSaveProjectMessage);
            MessengerInstance.Register<ExportProjectMessage>(this, ReceiveExportProjectMessage);
        }

        void ReceiveAddBlueprintMessage(AddBlueprintMessage action)
        {
            var blueprintItem = action.Item;
            bool hasName = false;
            foreach (BlueprintItem item in BlueprintItems)
            {
                hasName = item.ItemName == blueprintItem.ItemName;
            }

            if (!hasName)
            {
                BlueprintItems.Add(blueprintItem);
                action.Callback(true, null);
            }
            else
            {
                action.Callback(false, "That name already exists, enter a different one.");
            }
        }

        void ReceiveAddComponentMessage(SendComponentMessage action)
        {
            var componentItem = action.Item;
            SelectedBlueprint.ComponentItems.Add(componentItem);
            componentItem.UpdateFields();
            componentItem.UpdateFieldString();
        }

        void ReceiveSetProjectMessage(SetProjectMessage action)
        {
            _currentProject = action.ProjectItem;
            _currentProject.Save(BlueprintItems);
        }

        void ReceiveOpenProjectMessage(OpenProjectMessage action)
        {
            var tuple = ProjectItem.Load(action.Location);
            _currentProject = tuple.Item1;
            BlueprintItems = new ObservableCollection<BlueprintItem>(tuple.Item2);
        }

        void ReceiveSaveProjectMessage(SaveProjectMessage action)
        {
            _currentProject?.Save(BlueprintItems);
        }

        void ReceiveExportProjectMessage(ExportProjectMessage obj)
        {
            _currentProject?.Save(BlueprintItems);
            var ex = _currentProject?.Export(BlueprintItems);
        }

        void AddBlueprintMethod()
        {
            var addBlueprintDialogBox = new NewBlueprintView();

            addBlueprintDialogBox.ShowDialog();
        }

        void RemoveBlueprintMethod()
        {
            if (SelectedBlueprint != null)
            {
                BlueprintItems.Remove(SelectedBlueprint);
            }
        }

        void AddComponentMethod()
        {
            if (SelectedBlueprint != null)
            {
                var addComponentDialogBox = new NewComponentView();

                var msg = new SendBlueprintMessage()
                {
                    Item = SelectedBlueprint
                };

                MessengerInstance.Send(msg);

                addComponentDialogBox.Show();
            }
        }

        void RemoveComponentMethod()
        {
            if (SelectedBlueprint != null && SelectedComponent != null)
            {
                SelectedBlueprint.ComponentItems.Remove(SelectedComponent);
            }
        }

        void ComponentDoubleClickMethod()
        {
            var editComponentDialog = new EditComponentView();

            editComponentDialog.Show();

            var msg = new EditComponentMessage();
            msg.ComponentItem = SelectedComponent;

            MessengerInstance.Send(msg);
        }

        ObservableCollection<BlueprintItem> _blueprintItems = new ObservableCollection<BlueprintItem>();
        public ObservableCollection<BlueprintItem> BlueprintItems
        {
            get { return _blueprintItems; }
            set { Set(() => BlueprintItems, ref _blueprintItems, value); }
        }

        BlueprintItem _selectedBlueprint;
        public BlueprintItem SelectedBlueprint
        {
            get { return _selectedBlueprint; }
            set { Set(() => SelectedBlueprint, ref _selectedBlueprint, value); }
        }

        ComponentItem _selectedComponent;
        public ComponentItem SelectedComponent
        {
            get { return _selectedComponent; }
            set { Set(() => SelectedComponent, ref _selectedComponent, value); }
        }

        public RelayCommand AddBlueprintCommand { get; set; }
        public RelayCommand RemoveBlueprintCommand { get; set; }

        public RelayCommand AddComponentCommand { get; set; }
        public RelayCommand RemoveComponentCommand { get; set; }

        public RelayCommand ComponentDoubleClickCommand { get; set; }
        }
}