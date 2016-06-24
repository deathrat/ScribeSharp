using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.Serialization.Blueprints;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace ScribeSharp.Model
{
    public class ProjectItem : ObservableObject
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public void Save(List<BlueprintItem> blueprints)
        {
            if (blueprints == null || blueprints.Count == 0)
            {
                File.Create(GetProjectFile);
                return;
            }
            var json = JsonConvert.SerializeObject(blueprints, Formatting.Indented);
            File.WriteAllText(GetProjectFile, json);
        }

        public void Save(ObservableCollection<BlueprintItem> blueprints)
        {
            Save(blueprints.ToList());
        }

        public bool Export(ObservableCollection<BlueprintItem> blueprintItems)
        {
            var blueprints = blueprintItems.ToList();
            if (blueprints.Count == 0)
            {
                return false;
            }
            foreach (BlueprintItem blueprintItem in blueprints)
            {
                var poolName = blueprintItem.PoolName;
                var components = blueprintItem.ComponentItems;
                Pool pool = Pools.allPools.FirstOrDefault(p => p.metaData.poolName == poolName);
                if (pool != null)
                {
                    var ent = pool.CreateEntity();
                    foreach (ComponentItem componentItem in components)
                    {
                        var componentType = componentItem.ItemType;
                        int componentIndex = 0;
                        foreach (Type type in pool.metaData.componentTypes)
                        {
                            if (type == componentType)
                                break;
                            componentIndex++;
                        }
                        var comp = ent.CreateComponent(componentIndex, componentType);
                        for (int i = 0; i < componentItem.FieldItems.Count; i++)
                        {
                            var field = componentItem.FieldItems[i];
                            object value = field.Value;
                            FieldInfo fieldInfo = componentItem.GetFields[i];
                            Type type = fieldInfo.FieldType;

                            //Special handling cases for converting bool and int
                            if (type == typeof(bool))
                            {
                                value = bool.Parse(value.ToString());
                            }
                            else if (type == typeof(int))
                            {
                                value = int.Parse((string)value);
                            }

                            fieldInfo.SetValue(comp, value);
                        }
                        ent.AddComponent(componentIndex, comp);
                    }

                    var blueprint = new Blueprint(poolName, blueprintItem.ItemName, ent);
                    var json = JsonConvert.SerializeObject(blueprint, Formatting.Indented);
                    var file = File.CreateText($"{Location}\\{blueprintItem.ItemName}.json");
                    file.Write(json);
                    file.Close();
                }
            }

            return true;
        }

        public static Tuple<ProjectItem,List<BlueprintItem>>  Load(string filePath)
        {
            var file = File.OpenText(filePath);
            var proj = new ProjectItem()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Location = Path.GetDirectoryName(filePath)
            };
            var list = JsonConvert.DeserializeObject<List<BlueprintItem>>(file.ReadToEnd()) ?? new List<BlueprintItem>();
            file.Close();
            return new Tuple<ProjectItem, List<BlueprintItem>>(proj, list);
        }

        string GetProjectFile => $"{Location}\\{Name}.Scribe";
    }
}