using System.IO;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using ScribeSharp.Messages;
using ScribeSharp.View;

namespace ScribeSharp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            NewProjectCommand = new RelayCommand(NewProjectMethod);
            SaveProjectCommand = new RelayCommand(SaveProjectMethod);
            OpenProjectCommand = new RelayCommand<Window>(OpenProjectMethod);
            ExportProjectCommand = new RelayCommand(ExportProjectMethod);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        void NewProjectMethod()
        {
            var newProjectDialog = new NewProjectView();

            newProjectDialog.ShowDialog();
        }

        void OpenProjectMethod(Window window)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Project Location";
            dlg.IsFolderPicker = false;
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            var openDlg = dlg.ShowDialog(window);
            if (openDlg == CommonFileDialogResult.Ok)
            {
                if (dlg.FileName.EndsWith(".scribe"))
                {
                    var msg = new OpenProjectMessage()
                    {
                        Location = dlg.FileName
                    };

                    MessengerInstance.Send(msg);
                }
            }
        }

        void SaveProjectMethod()
        {
            var msg = new SaveProjectMessage();
            MessengerInstance.Send(msg);
        }

        void ExportProjectMethod()
        {
            var msg = new ExportProjectMessage();
            MessengerInstance.Send(msg);
        }

        public RelayCommand NewProjectCommand { get; set; }
        public RelayCommand<Window> OpenProjectCommand { get; set; }
        public RelayCommand SaveProjectCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public RelayCommand ExportProjectCommand { get; set; }
    }
}