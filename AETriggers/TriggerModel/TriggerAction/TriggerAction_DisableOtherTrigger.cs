using AEAssist.View;
using PropertyChanged;
using System;

namespace AEAssist.TriggerAction
{
    [Trigger("DisableOtherGroup 关闭其他组", Tooltip = "关闭另一组触发器", ParamTooltip = "[Other trigger's group id]")]
    [AddINotifyPropertyChangedInterface]
    public class TriggerAction_DisableOtherTrigger : ITriggerAction
    {
        [GUILabel("GroupId")]
        public string TriggerId { get; set; }

        public void WriteFromJson(string[] values)
        {
            TriggerId = values[0];
            Check();
        }

        public string[] Pack2Json()
        {
            return new string[]
            {
                TriggerId
            };
        }

        public void Check()
        {
#if Trigger
            if (!AETriggers.DataBinding.Instance.GroupIds.Contains(TriggerId))
                throw new Exception($"Id not found: {TriggerId}!\n");
#endif
        }
    }
}