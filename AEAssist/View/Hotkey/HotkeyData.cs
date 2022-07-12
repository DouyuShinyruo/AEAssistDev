using PropertyChanged;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace AEAssist.View.Hotkey
{
    [AddINotifyPropertyChangedInterface]
    public class HotkeyData
    {
        public HotkeyData(string name)
        {
            this.Name = name;
            this.TypeName = name;
        }

        public Keys Key { get; set; }
        public ModifierKeys ModifierKey { get; set; }

        public string Name { get; set; }

        public string TypeName;

        public string GetDisplayString()
        {
            var b = $"{Key.ToString()}";
            switch (ModifierKey)
            {
                case ModifierKeys.None:
                    return b;
                case ModifierKeys.Alt:
                    return $"A+{b}";
                case ModifierKeys.Control:
                    return $"C+{b}";
                case ModifierKeys.Shift:
                    return $"S+{b}";
                case ModifierKeys.Windows:
                    return $"Win+{b}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset()
        {
            Key = Keys.None;
            ModifierKey = ModifierKeys.None;
        }
    }
}