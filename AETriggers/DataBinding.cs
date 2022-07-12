using AEAssist;
using AEAssist.View;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AETriggers
{
    [AddINotifyPropertyChangedInterface]
    public class DataBinding
    {
        public static DataBinding Instance = new DataBinding();

        [AddINotifyPropertyChangedInterface]
        public class Trigger
        {

            public Trigger(string typeName)
            {
                this.TypeName = typeName;
                SetTriggerObj();
                ParamTooltip = ParamToolTip();
                NeedParam = IsNeedParam();
                Tooltip = ToolTip();
            }

            public string TypeName { get; set; }
            public string Param { get; set; }
            public string ParamTooltip { get; set; }

            public string Tooltip { get; set; }

            public Visibility NeedParam { get; set; }

            public Visibility DelButton { get; set; } = Visibility.Hidden;

            public bool DefaultStyle { get; set; }
            public ITriggerBase TriggerObj { get; set; }

            void SetTriggerObj()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = t.GetCustomAttributes(typeof(GUIDefaultAttribute), false);
                if (attr != null && attr.Length > 0)
                {
                    DefaultStyle = true;
                }
                else
                {
                    TriggerObj = Activator.CreateInstance(t) as ITriggerBase;
                }
            }

            public Visibility IsNeedParam()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = TriggerMgr.Instance.AllAttrs[t];
                return attr.NeedParams ? Visibility.Visible : Visibility.Hidden;
            }


            public string ParamToolTip()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = TriggerMgr.Instance.AllAttrs[t];
                var toolTip = attr.ParamTooltip;
                if (attr.Example != null)
                {
                    toolTip += "\nExamples:\n\t" + attr.Example;
                }

                return toolTip;
            }

            public string ToolTip()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = TriggerMgr.Instance.AllAttrs[t];
                return attr.Tooltip;
            }
        }

        public class GroupData
        {
            public ObservableCollection<Trigger> CondTriggers { get; set; } = new ObservableCollection<Trigger>();
            public ObservableCollection<Trigger> ActionTriggers { get; set; } = new ObservableCollection<Trigger>();

            public void AddTrigger(string type)
            {
                var t = TriggerMgr.Instance.Name2Type[type];
                if (TriggerMgr.Instance.AllCondType.Contains(t))
                {
                    CondTriggers.Add(new Trigger(type));
                }
                else
                {
                    ActionTriggers.Add(new Trigger(type));
                }
            }
        }



        public void Reset()
        {
            Author = string.Empty;
            Name = string.Empty;
            GroupIds = new ObservableCollection<string>();
            AllGroupData = new Dictionary<string, GroupData>();
            CurrChoosedId = string.Empty;
            TargetJob = Jobs.Any.ToString();
        }


        public TriggerLine Export()
        {
            try
            {
                var triggleLine = new TriggerLine
                {
                    Author = Author,
                    ConfigVersion = TriggerLine.CurrConfigVersion,
                    CurrZoneId = 0,
                    Name = Name,
                    SubZoneId = 0,
                    TargetJob = TargetJob,
                };
                foreach (var v in AllGroupData)
                {
                    var trigger = new AEAssist.Trigger
                    {
                        Id = v.Key
                    };
                    foreach (var cond in v.Value.CondTriggers)
                    {
                        trigger.TriggerConds.Add(ConvertToData(cond) as ITriggerCond);
                    }

                    foreach (var action in v.Value.ActionTriggers)
                    {
                        trigger.TriggerActions.Add(ConvertToData(action) as ITriggerAction);
                    }

                    triggleLine.Triggers.Add(trigger);
                }

                return triggleLine;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }


        ITriggerBase ConvertToData(Trigger trigger)
        {

            if (trigger.DefaultStyle)
            {

                var typeName = trigger.TypeName;
                var type = TriggerMgr.Instance.Name2Type[typeName];
                var instance = Activator.CreateInstance(type) as ITriggerBase;
                string[] values = null;
                if (trigger.Param != null)
                {
                    values = trigger.Param.Split(',');
                }
                instance.WriteFromJson(values);

                return instance as ITriggerBase;
            }
            else
            {
                trigger.TriggerObj.Check();
                return trigger.TriggerObj;
            }
        }


        public void Load(TriggerLine triggerLine)
        {
            this.Reset();

            this.Author = triggerLine.Author;
            this.Name = triggerLine.Name;
            this.TargetJob = triggerLine.TargetJob;

            foreach (var v in triggerLine.Triggers)
            {
                this.GroupIds.Add(v.Id);
                this.AllGroupData[v.Id] = new GroupData();
                var groupData = this.AllGroupData[v.Id];

                foreach (var cond in v.TriggerConds)
                {
                    var attr = TriggerMgr.Instance.AllAttrs[cond.GetType()];
                    string str = string.Empty;
                    var pack = cond.Pack2Json();
                    if (pack != null && pack.Length > 0)
                    {
                        foreach (var p in pack)
                        {
                            str += p + ",";
                        }

                        str = str.TrimEnd(',');
                    }

                    var trigger = new Trigger(attr.Name)
                    {
                        Param = str
                    };
                    if (!trigger.DefaultStyle)
                    {
                        trigger.TriggerObj = cond;
                    }

                    groupData.CondTriggers.Add(trigger);

                }

                foreach (var action in v.TriggerActions)
                {
                    var attr = TriggerMgr.Instance.AllAttrs[action.GetType()];
                    string str = string.Empty;
                    var pack = action.Pack2Json();
                    if (pack != null && pack.Length > 0)
                    {
                        foreach (var p in pack)
                        {
                            str += p + ",";
                        }

                        str = str.TrimEnd(',');
                    }


                    var trigger = new Trigger(attr.Name)
                    {
                        Param = str
                    };
                    if (!trigger.DefaultStyle)
                    {
                        trigger.TriggerObj = action;
                    }

                    groupData.ActionTriggers.Add(trigger);
                }

            }
        }


        public string Author { get; set; }
        public string Name { get; set; }

        public string TargetJob { get; set; } = Jobs.Any.ToString();

        public ObservableCollection<string> GroupIds { get; set; } = new ObservableCollection<string>();
        public Dictionary<string, GroupData> AllGroupData { get; set; } = new Dictionary<string, GroupData>();

        public string CurrChoosedId { get; set; }

        public Dictionary<int, string> SpellsDictionary { get; set; } = new Dictionary<int, string>()
        {
            // Sage
            { 24302, "PhysisII"},
            {24292, "EukrasianPrognosis"},
            {24298, "Kerachole"},
            {24299, "Ixochole"},
            {24300, "Zoe"},
            {24301, "Pepsis"},
            {24303, "Taurochole"},
            {24305, "Haima"},
            {24309, "24309"},
            {24310, "Holos"},
            {24311, "Panhaima"},
            {24317, "Krasis"},
            {24318, "Pneuma"}
        };

        // Sort  observalble collection GroupsIds by id.
        public void SortGroupIds(ObservableCollection<string> groupsIds)
        {
            var list = groupsIds.ToList();
            list.Sort();
            groupsIds.Clear();
            foreach (var v in list)
            {
                groupsIds.Add(v);
            }
        }
    }

}