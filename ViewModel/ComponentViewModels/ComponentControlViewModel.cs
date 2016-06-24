using GalaSoft.MvvmLight;

namespace ScribeSharp.ViewModel.ComponentViewModels
{
    public class ComponentControlViewModel : ViewModelBase
    {
        object _componentFieldValue;
        public object ComponentFieldValue
        {
            get { return _componentFieldValue; }
            set { Set(() => ComponentFieldValue, ref _componentFieldValue, value); }
        }
    }
}