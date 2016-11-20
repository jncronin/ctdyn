using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ctdyn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StackPanel_LayoutUpdated(object sender, object e)
        {

        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawEllipse(155, 115, 80, 30, Windows.UI.Colors.Black);
        }

        private async void inputdirbtn_Click(object sender, RoutedEventArgs e)
        {
            var fp = new Windows.Storage.Pickers.FolderPicker();
            fp.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.ComputerFolder;
            fp.FileTypeFilter.Add("*");
            var folder = await fp.PickSingleFolderAsync();

            try
            {
                inputdir.Text = folder.Path;
            }
            catch (Exception) { }
        }
    }
}
