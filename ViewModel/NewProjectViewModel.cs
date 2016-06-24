using System;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using ScribeSharp.Messages;
using ScribeSharp.Model;

namespace ScribeSharp.ViewModel
{
    public class NewProjectViewModel : ViewModelBase
    {
        public NewProjectViewModel()
        {
            OkCommand = new RelayCommand<Window>(OkMethod);
            CancelCommand = new RelayCommand<Window>(CancelMethod);
            BrowseCommand = new RelayCommand<Window>(BrowseMethod);
            ClosingCommand = new RelayCommand<EventArgs>(ClosingMethod);
        }

        void BrowseMethod(Window window)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Project Location";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            var openDlg = dlg.ShowDialog(window);
            if (openDlg == CommonFileDialogResult.Ok)
            {
                ProjectLocation = dlg.FileName;
            }
        }

        void OkMethod(Window window)
        {
            if (string.IsNullOrEmpty(ProjectName))
            {
                MessageBox.Show(window, "Please enter a valid project name.", "Error");
                return;
            }
            if (string.IsNullOrEmpty(ProjectLocation) || !Directory.Exists(ProjectLocation))
            {
                MessageBox.Show(window, "Please select a valid project location.", "Error");
                return;
            }


            var msg = new SetProjectMessage()
            {
                ProjectItem = new ProjectItem()
                {
                    Name = ProjectName,
                    Location = ProjectLocation
                }
            };

            MessengerInstance.Send(msg);
            
            window.Close();
        }

        void CancelMethod(Window window)
        {
            window.Close();
        }

        void ClosingMethod(EventArgs obj)
        {
            ProjectName = null;
            ProjectLocation = null;
        }

        string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set { Set(() => ProjectName, ref _projectName, value); }
        }

        string _projectLocation;

        public string ProjectLocation
        {
            get { return _projectLocation; }
            set { Set(() => ProjectLocation, ref _projectLocation, value); }
        }
        public RelayCommand<Window> BrowseCommand { get; set; }
        public RelayCommand<Window> CancelCommand { get; set; }
        public RelayCommand<Window> OkCommand { get; set; }

        public RelayCommand<EventArgs> ClosingCommand { get; set; }
    }
}