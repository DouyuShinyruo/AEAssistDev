using AEAssist.View;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AETriggers
{
    public partial class DynamicTriggerContent : UserControl
    {
        public DataBinding.Trigger Data { get; set; }

        public DynamicTriggerContent()
        {
            InitializeComponent();
        }


        public void Clear()
        {
            this.Data = null;
            this.Panel.Children.Clear();
        }

        public void Init(DataBinding.Trigger trigger)
        {
            Clear();
            if (trigger.NeedParam != Visibility.Visible)
            {
                return;
            }

            if (trigger.DefaultStyle || trigger.TriggerObj == null)
            {
                var textBox = new TextBox();
                textBox.Width = 150;
                textBox.ToolTip = trigger.ParamTooltip;
                textBox.Background = Brushes.Khaki;
                ToolTipService.SetShowDuration(textBox, 60000);
                var prop = new Binding("Param");
                prop.Source = trigger;
                prop.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(textBox, TextBox.TextProperty, prop);
                this.Panel.Children.Add(textBox);
                return;
            }

            this.Data = trigger;
            var obj = trigger.TriggerObj;
            foreach (var v in obj.GetType().GetProperties())
            {
                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                var element0 = new TextBlock();
                element0.Text = v.Name;
                element0.Foreground = Brushes.Aqua;

                var label = v.GetCustomAttribute<GUILabelAttribute>();
                if (label != null)
                {
                    element0.Text = label.LabelName;
                }

                var tooltip = v.GetCustomAttribute<GUIToolTipAttribute>();
                if (tooltip != null)
                {
                    element0.ToolTip = tooltip.tip;
                }

                stackPanel.Children.Add(element0);
                CreateUI(v, obj, stackPanel);
                this.Panel.Children.Add(stackPanel);
            }
        }

        void CreateUI(PropertyInfo v, object obj, StackPanel panel)
        {
            if (v.PropertyType == typeof(bool))
            {
                CreateBool(v, obj, panel);
            }
            else if (v.PropertyType == typeof(string))
            {
                CreateString(v, obj, panel);
            }
            else if (v.PropertyType == typeof(int))
            {
                CreateInt(v, obj, panel);
            }
            else if (v.PropertyType == typeof(float))
            {
                CreateFloat(v, obj, panel);
            }
            else if (v.PropertyType == typeof(uint))
            {
                var element1 = new TextBox();
                element1.Margin = new Thickness(15, 0, 5, 0);
                element1.Width = 150;
                element1.Text = v.GetValue(obj).ToString();
                element1.TextChanged += (s, o) =>
                {
                    object value_obj = null;
                    if (v.PropertyType == typeof(uint))
                    {
                        uint.TryParse(element1.Text, out var value);
                        value_obj = value;
                    }
                    else if (v.PropertyType == typeof(float))
                    {
                        float.TryParse(element1.Text, out var value);
                        value_obj = value;
                    }

                    v.SetValue(obj, value_obj);
                };

                panel.Children.Add(element1);
            }
        }

        void CreateBool(PropertyInfo v, object obj, StackPanel panel)
        {
            var element1 = new CheckBox();
            element1.Width = 30;
            element1.Margin = new Thickness(15, 0, 5, 0);
            var prop = new Binding(v.Name);
            prop.Source = obj;
            prop.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(element1, CheckBox.IsCheckedProperty, prop);
            panel.Children.Add(element1);
        }

        void CreateString(PropertyInfo v, object obj, StackPanel panel)
        {
            var element1 = new TextBox();
            element1.Margin = new Thickness(15, 0, 5, 0);
            element1.Width = 150;
            var prop = new Binding(v.Name);
            prop.Source = obj;
            prop.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(element1, TextBox.TextProperty, prop);
            panel.Children.Add(element1);
        }

        void CreateInt(PropertyInfo v, object obj, StackPanel panel)
        {
            var toolTip = v.GetCustomAttribute<GUIToolTipAttribute>();
            var rangeAttr = v.GetCustomAttribute<GUIIntRangeAttribute>();
            if (rangeAttr == null)
            {
                var element1 = new TextBox();
                element1.Margin = new Thickness(15, 0, 5, 0);
                element1.Width = 150;
                element1.Text = v.GetValue(obj).ToString();
                if (toolTip != null)
                    element1.ToolTip = toolTip.tip;
                element1.KeyDown += (s, o) =>
                {
                    if (o.Key != Key.Enter)
                        return;
                    object value_obj = null;

                    int.TryParse(element1.Text, out var value);
                    value_obj = value;
                    v.SetValue(obj, value_obj);
                    element1.Text = value.ToString();
                };
                // element1.TextChanged += (s, o) =>
                // {
                //     if(o.)
                //     object value_obj = null;
                //
                //     int.TryParse(element1.Text, out var value);
                //     value_obj = value;
                //     v.SetValue(obj, value_obj);
                // };
                panel.Children.Add(element1);
            }
            else
            {
                var element2 = new TextBlock();
                element2.Margin = new Thickness(5, 0, 5, 0);
                element2.Foreground = Brushes.Aqua;
                element2.Text = v.GetValue(obj).ToString();

                var element1 = new Slider();
                element1.Margin = new Thickness(5, 0, 5, 0);
                element1.Width = 80;
                element1.TickFrequency = 1;
                element1.Maximum = rangeAttr.MaxValue;
                element1.Minimum = rangeAttr.MinValue;
                element1.Value = (int)v.GetValue(obj);
                if (toolTip != null)
                    element1.ToolTip = toolTip.tip;
                element1.ValueChanged += (s, o) =>
                {
                    object value_obj = null;
                    value_obj = (int)element1.Value;
                    element2.Text = value_obj.ToString();
                    v.SetValue(obj, value_obj);
                };



                panel.Children.Add(element1);
                panel.Children.Add(element2);

            }
        }

        void CreateFloat(PropertyInfo v, object obj, StackPanel panel)
        {

            var rangeAttr = v.GetCustomAttribute<GUIFloatRangeAttribute>();

            {
                var element1 = new TextBox();
                element1.Margin = new Thickness(15, 0, 5, 0);
                element1.Width = 80;
                element1.Text = v.GetValue(obj).ToString();
                element1.KeyDown += (s, o) =>
                {
                    if (o.Key != Key.Enter)
                        return;
                    object value_obj = null;

                    float.TryParse(element1.Text, out var value);
                    if (rangeAttr != null && (value < rangeAttr.MinValue || value > rangeAttr.MaxValue))
                    {
                        return;
                    }
                    value_obj = value;
                    v.SetValue(obj, value_obj);
                    element1.Text = value_obj.ToString();
                };
                panel.Children.Add(element1);
            }
        }
    }
}