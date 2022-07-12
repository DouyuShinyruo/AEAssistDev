using AETriggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace AEAssist
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public class TriggerTypeData
        {
            public string Name { get; set; }
            public string Tooltip { get; set; }
        }

        private RenameWindow _renameWindow;

        public MainWindow()
        {
            InitializeComponent();
            /*var currCulture = CultureInfo.CurrentCulture;
            if (currCulture.Name.Contains("zh-CN"))
            {
                Load.Content = "加载Excel表";
                Export.Content = "导出";
            }*/
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata((object)60000));
            var contextMenu = new System.Windows.Controls.ContextMenu();
            Dictionary<string, MenuItem> _menuItemsDict = new Dictionary<string, MenuItem>();
            Dictionary<string, TriggerAttribute> path2Triggers = new Dictionary<string, TriggerAttribute>();

            foreach (var v in TriggerMgr.Instance.AllCondType)
            {
                var attr = TriggerMgr.Instance.AllAttrs[v];
                if (attr.Name.Contains("/"))
                {
                    var path = "Conds/" + attr.Name;
                    path2Triggers.Add(path, attr);
                }
                else
                {
                    var path = "Conds/General/" + attr.Name;
                    path2Triggers.Add(path, attr);
                }


            }

            foreach (var v in TriggerMgr.Instance.AllActionType)
            {
                var attr = TriggerMgr.Instance.AllAttrs[v];
                if (attr.Name.Contains("/"))
                {
                    var path = "Actions/" + attr.Name;
                    path2Triggers.Add(path, attr);
                }
                else
                {
                    var path = "Actions/General/" + attr.Name;
                    path2Triggers.Add(path, attr);
                }
            }

            foreach (var v in path2Triggers)
            {
                var afterSplit = v.Key.Split('/');
                for (int i = 0; i < afterSplit.Length; i++)
                {
                    var path = string.Empty;
                    for (int j = 0; j <= i; j++)
                    {
                        path += afterSplit[j] + "/";
                    }

                    if (path.Length > 0)
                        path = path.Remove(path.Length - 1);

                    if (!_menuItemsDict.TryGetValue(path, out var menuItem))
                    {
                        _menuItemsDict[path] = new MenuItem()
                        {
                            Header = new TextBlock()
                            {
                                Text = afterSplit[i],
                                TextTrimming = TextTrimming.None,
                                TextAlignment = TextAlignment.Left,
                                TextWrapping = TextWrapping.NoWrap,
                                HorizontalAlignment = HorizontalAlignment.Left
                            },
                            Height = 20,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            HorizontalContentAlignment = HorizontalAlignment.Right
                        };
                        menuItem = _menuItemsDict[path];

                        if (i > 0)
                        {
                            var lastLevelMenu = string.Empty;
                            for (int j = 0; j < i; j++)
                            {
                                lastLevelMenu += afterSplit[j] + "/";
                            }
                            if (lastLevelMenu.Length > 0)
                                lastLevelMenu = lastLevelMenu.Remove(lastLevelMenu.Length - 1);
                            var lastMenu = _menuItemsDict[lastLevelMenu];
                            if (afterSplit[i] == "General")
                            {
                                lastMenu.Items.Insert(0, menuItem);
                            }
                            else
                            {
                                lastMenu.Items.Add(menuItem);
                            }
                        }

                        if (i == 0)
                        {
                            contextMenu.Items.Add(menuItem);
                        }
                    }

                    if (i == afterSplit.Length - 1)
                    {
                        menuItem.ToolTip = v.Value.Tooltip;
                        var name = v.Value.Name;
                        menuItem.Click += (sender, args) =>
                        {
                            if (string.IsNullOrEmpty(DataBinding.Instance.CurrChoosedId))
                            {
                                MessageBox.Show("Pls choose a group on left\n请在左边选择一个Group");
                                return;
                            }

                            if (!DataBinding.Instance.AllGroupData.TryGetValue(DataBinding.Instance.CurrChoosedId, out var group))
                            {
                                return;
                            }

                            group.AddTrigger(name.ToString());
                        };
                    }
                }
            }

            mainStackPanel.ContextMenu = contextMenu;

            JobComboBox.SelectedValue = Jobs.Any;

            _renameWindow = new RenameWindow();

            Entry.Init();
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            var TriggerLine = DataBinding.Instance.Export();
            if (TriggerLine == null)
                return;
            var openFile = new SaveFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = new DirectoryInfo("../").FullName;
            openFile.FileName =
                $"[{TriggerLine.TargetJob}] [{TriggerLine.Name}] [{TriggerLine.Author}].json";
            var ret = openFile.ShowDialog();
            if (!ret.HasValue || !ret.Value)
                return;
            TriggerHelper.SaveTriggerLine(TriggerLine, openFile.FileName);
            MessageBox.Show("Export Success!");
        }

        private string ListToString(string[] pa)
        {
            var str = string.Empty;
            if (pa != null && pa.Length > 0)
                foreach (var v in pa)
                    str += v + ",";

            return $"[{str}]";
        }

        /*private void RefreshUI()
        {
            var TriggerLine = Entry.TriggerLine;
            var AllExcelData = Entry.AllExcelData;
            if (TriggerLine == null)
            {
                AuthorName.Content = string.Empty;
                Name.Content = string.Empty;
                TargetZone.Content = string.Empty;
                TargetJob.Content = string.Empty;
                return;
            }

            AuthorName.Content = TriggerLine.Author;
            Name.Content = TriggerLine.Name;
            TargetZone.Content = $"{TriggerLine.CurrZoneId} | {TriggerLine.SubZoneId}";
            TargetJob.Content = TriggerLine.TargetJob;
        }*/
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        private void Selector_OnSelected(object sender, EventArgs eventArgs)
        {
            DataBinding.Instance.TargetJob = JobComboBox.SelectedValue.ToString();
        }

        private void AddConditionOrActionBtn_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddGrouIdBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var newId = IdTextBox.Text;
            if (string.IsNullOrEmpty(newId) || DataBinding.Instance.GroupIds.Contains(newId))
            {
                MessageBox.Show("Repeat or Null!");
                return;
            }
            DataBinding.Instance.GroupIds.Add(newId);
            DataBinding.Instance.AllGroupData[newId] = new DataBinding.GroupData();
            ChooseGroup(newId);

        }

        private void IdTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var newId = IdTextBox.Text;
                if (string.IsNullOrEmpty(newId) || DataBinding.Instance.GroupIds.Contains(newId))
                {
                    MessageBox.Show("Repeat or Null!");
                    return;
                }
                DataBinding.Instance.GroupIds.Add(newId);
                DataBinding.Instance.AllGroupData[newId] = new DataBinding.GroupData();
                // reset text box value after enter.
                IdTextBox.Text = "";
            }
        }


        // private void Main_OnMouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     if (e.ChangedButton != MouseButton.Right)
        //     {
        //         _chooseTriggerWindow.Hide();
        //         return;
        //     }
        //
        //     var mousePos = e.GetPosition(this);
        //    var screenPos = this.PointToScreen(mousePos);
        //     _chooseTriggerWindow.Show(screenPos);
        // }

        private void AuthorTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TriggerOnClickHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null)
                return;

            if (string.IsNullOrEmpty(DataBinding.Instance.CurrChoosedId))
            {
                MessageBox.Show("Pls choose a group on left\n请在左边选择一个Group");
                return;
            }

            if (!DataBinding.Instance.AllGroupData.TryGetValue(DataBinding.Instance.CurrChoosedId, out var group))
            {
                return;
            }

            group.AddTrigger(menuItem.Header.ToString());
        }

        private void GroupIdRename_EventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var stackPanel = button.Parent as StackPanel;
                var textBlock = stackPanel.Children[0] as TextBlock;

                var oldId = textBlock.Text;
                var mousePos = Mouse.GetPosition(this);
                var screenPos = this.PointToScreen(mousePos);
                _renameWindow.Display(screenPos, v => Rename(oldId, v));
                //todo: get newId,
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        void Rename(string oldId, string newId)
        {
            bool currChoosed = false;
            if (DataBinding.Instance.CurrChoosedId == oldId)
            {
                DataBinding.Instance.CurrChoosedId = string.Empty;
                CondsListView.ItemsSource = null;
                currChoosed = true;
            }

            var oldIndex = DataBinding.Instance.GroupIds.IndexOf(oldId);

            DataBinding.Instance.GroupIds.Remove(oldId);
            var groupData = DataBinding.Instance.AllGroupData[oldId];
            DataBinding.Instance.AllGroupData.Remove(oldId);
            DataBinding.Instance.GroupIds.Insert(oldIndex, newId);
            DataBinding.Instance.AllGroupData[newId] = groupData;
            if (currChoosed)
            {
                DataBinding.Instance.CurrChoosedId = newId;
                CondsListView.ItemsSource = groupData.CondTriggers;
                ActionsListView.ItemsSource = groupData.ActionTriggers;
            }

        }

        private void GroupIdDel_EventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var stackPanel = button.Parent as StackPanel;
                var textBlock = stackPanel.Children[0] as TextBlock;

                var delId = textBlock.Text;
                if (DataBinding.Instance.CurrChoosedId == delId)
                {
                    Reset();
                }

                DataBinding.Instance.GroupIds.Remove(delId);
                DataBinding.Instance.AllGroupData.Remove(delId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void ShowGroup_EventHandler(object sender, MouseButtonEventArgs e)
        {
            var TextBlock = sender as TextBlock;
            var id = TextBlock.Text;
            ChooseGroup(id);
        }

        void ChooseGroup(string id)
        {
            DataBinding.Instance.CurrChoosedId = id;
            var groupData = DataBinding.Instance.AllGroupData[id];
            CondsListView.ItemsSource = groupData.CondTriggers;
            ActionsListView.ItemsSource = groupData.ActionTriggers;
            var content = TriggerContent.Children[0] as DynamicTriggerContent;
            content.Clear();
        }

        void Reset()
        {
            DataBinding.Instance.CurrChoosedId = string.Empty;
            CondsListView.ItemsSource = null;
            ActionsListView.ItemsSource = null;
            var content = TriggerContent.Children[0] as DynamicTriggerContent;
            content.Clear();
            ActionsListView.SelectedIndex = -1;
            CondsListView.SelectedIndex = -1;
        }

        private void LoadTriggerLine_OnClick(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Json(*.json)|*.json";
            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Multiselect = false;
            var ret = openFile.ShowDialog();

            if (!ret.HasValue || !ret.Value)
                return;
            var file = openFile.FileName;
            {
                var str = "";
                TriggerLine line = null;
                (str, line) = TriggerHelper.LoadTriggerLine(file);
                if (str != null && line == null) MessageBox.Show(str);

                if (line != null)
                {
                    Reset();
                    DataBinding.Instance.Load(line);
                }
            }
            // sort GroupIds collection, and update UI.
            DataBinding.Instance.SortGroupIds(DataBinding.Instance.GroupIds);

        }

        private void Cond_DeleteTriggerBehavior_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = CondsListView.SelectedValue as DataBinding.Trigger;
            CondsListView.SelectedValue = null;
            DataBinding.Instance.AllGroupData[DataBinding.Instance.CurrChoosedId].CondTriggers.Remove(selectedItem);
            var content = TriggerContent.Children[0] as DynamicTriggerContent;
            content.Clear();
        }

        private void Action_DeleteTriggerBehavior_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ActionsListView.SelectedValue as DataBinding.Trigger;
            ActionsListView.SelectedValue = null;
            DataBinding.Instance.AllGroupData[DataBinding.Instance.CurrChoosedId].ActionTriggers.Remove(selectedItem);
            var content = TriggerContent.Children[0] as DynamicTriggerContent;
            content.Clear();
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender == CondsListView)
            {
                ActionsListView.SelectedIndex = -1;
                foreach (var v in ActionsListView.Items)
                {
                    var trigger = v as DataBinding.Trigger;
                    trigger.DelButton = Visibility.Hidden;
                }
            }
            else
            {
                CondsListView.SelectedIndex = -1;
                foreach (var v in CondsListView.Items)
                {
                    var trigger = v as DataBinding.Trigger;
                    trigger.DelButton = Visibility.Hidden;
                }
            }


            foreach (var v in e.RemovedItems)
            {
                var trigger = v as DataBinding.Trigger;
                trigger.DelButton = Visibility.Hidden;
            }

            foreach (var v in e.AddedItems)
            {
                var trigger = v as DataBinding.Trigger;
                trigger.DelButton = Visibility.Visible;
            }

            if (e.AddedItems.Count > 0)
            {
                var content = TriggerContent.Children[0] as DynamicTriggerContent;
                content.Init(e.AddedItems[0] as DataBinding.Trigger);
            }
        }

        private bool notice;
        private void SaveTriggerline_OnClick(object sender, RoutedEventArgs e)
        {
            var TriggerLine = DataBinding.Instance.Export();
            if (TriggerLine == null)
                return;
            try
            {
                var dirPath = Directory.GetCurrentDirectory() + "/Triggerline/";
                var filePath = dirPath + "/TempFile_" + DateTimeOffset.Now.ToFileTime() + ".json";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                TriggerHelper.SaveTriggerLine(TriggerLine, filePath);
                if (!notice)
                    MessageBox.Show($"Export Success!Only first notice:\n{filePath}");
                notice = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            // The text box grabs all input.


            // Fetch the actual shortcut key.
            var key = e.Key == Key.System ? e.SystemKey : e.Key;

            switch (key)
            {
                case Key.Escape:
                    return;
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                    return;
            }

            // Ignore modifier keys.
            var ModKeySetting = ModifierKeys.None;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) ModKeySetting = ModifierKeys.Control;

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) ModKeySetting = ModifierKeys.Shift;

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0) ModKeySetting = ModifierKeys.Alt;

            if (Keyboard.Modifiers == 0) ModKeySetting = ModifierKeys.None;

            var newKey = (Keys)KeyInterop.VirtualKeyFromKey(key);

            if (ModKeySetting == ModifierKeys.Control && newKey == Keys.S)
            {
                SaveTriggerline_OnClick(sender, null);
                e.Handled = true;
            }
        }

        private void SortGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            // sort GroupIds collection, and update UI.
            DataBinding.Instance.SortGroupIds(DataBinding.Instance.GroupIds);
        }
    }
}