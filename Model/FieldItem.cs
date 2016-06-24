using GalaSoft.MvvmLight;

namespace ScribeSharp.Model
{
    public class FieldItem : ObservableObject
    {
        string _fieldInfoKey;
        public string FieldInfoKey
        {
            get { return _fieldInfoKey; }
            set
            {
                Set(() => FieldInfoKey, ref _fieldInfoKey, value);
                UpdateString();
            }
        }

        string _fieldInfoType;
        public string FieldInfoType
        {
            get
            {
                return _fieldInfoType;
            }
            set
            {
                Set(() => FieldInfoType, ref _fieldInfoType, value);
                UpdateString();
            }
        }

        object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                Set(() => Value, ref _value, value);
                UpdateString();
            }
        }

        string _string;
        public string String { get { return _string; } set { Set(() => String, ref _string, value);} }

        void UpdateString()
        {
            string extra = "";
            if (Value != null)
            {
                extra = $": {Value}";
            }
            String = $"{FieldInfoKey} ({FieldInfoType}){extra}";
        }

        public override string ToString()
        {
            return String;
        }
    }
}