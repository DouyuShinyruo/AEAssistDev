using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AEAssist.View
{
    public partial class CommonOverlay : UserControl
    {
        public CommonOverlay()
        {
            InitializeComponent();
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            AEAssist.DataBinding.Instance.Reset();
        }

        private void ChangeTriggerLine_OnClick(object sender, RoutedEventArgs e)
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

                if (line != null) AEAssist.DataBinding.Instance.ChangeTriggerLine(line);
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }

        private void ClearTriggerLine_OnClick(object sender, RoutedEventArgs e)
        {
            AEAssist.DataBinding.Instance.ChangeTriggerLine(null);
        }
    }
}