using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ScribeSharp.Util;

namespace ScribeSharp.Model
{
    public class ComponentItem : ObservableObject
    {
        [JsonProperty]
        public string ItemName;

        Type _itemType;
        [JsonProperty]
        public Type ItemType
        {
            get { return _itemType; }
            set
            {
                Set(() => ItemType, ref _itemType, value);
                //UpdateFields();
            }
        }

        public ComponentItem()
        {
            FieldItems = new ObservableCollection<FieldItem>();
        }

        public override string ToString()
        {
            return $"{ItemName} ({ItemType})";
        }

        ObservableCollection<FieldItem> _fieldItems;
        public ObservableCollection<FieldItem> FieldItems
        {
            get { return _fieldItems; }
            set
            {
                Set(() => FieldItems, ref _fieldItems, value);
                FieldsString = GetFieldsString();
            }
        }

        public void UpdateFields()
        {
            var fields = GetFields;
            foreach (FieldInfo fieldInfo in fields)
            {
                bool foundField = false;
                foreach (FieldItem fieldItem in FieldItems)
                {
                    if (fieldItem.FieldInfoKey == fieldInfo.Name)
                    {
                        foundField = true;
                        break;
                    }
                }
                if (!foundField)
                {
                    FieldItems.Add(new FieldItem()
                    {
                        FieldInfoType = fieldInfo.FieldType.Name,
                        FieldInfoKey = fieldInfo.Name
                    });
                }
            }
        }

        public void SetValue<T>(string key, object value)
        {
            SetValue(key, (T)value);
        }

        public void SetValue(string key, object value)
        {
            foreach (FieldItem fieldItem in FieldItems)
            {
                if (fieldItem.FieldInfoKey == key)
                {
                    fieldItem.Value = value;
                }
            }
            FieldsString = GetFieldsString();
        }

        string GetFieldsString()
        {
            string tempString = "";
            foreach (FieldItem fieldItem in FieldItems)
            {
                tempString = $"{tempString}{fieldItem.String}\r\n";
            }
            return tempString;
        }

        string _fieldsString; 
        public string FieldsString
        {
            get { return _fieldsString; }
            set { Set(() => FieldsString, ref _fieldsString, value); }
        }

        [JsonIgnore]
        public FieldInfo[] GetFields => ItemType.GetFields();

        public void UpdateFieldString()
        {
            FieldsString = GetFieldsString();
        }
    }
}