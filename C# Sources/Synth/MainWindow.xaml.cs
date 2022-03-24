using Synth.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Editor editor = new();
        public Midi midi = new();
        public Output output = new();
        public MainWindow()
        {
            MainWindowHolder.SetWindow(this);
            InitializeComponent();
            Info.Channels = 0;
            Info.SampleRate = (int)SampleRatesKHZ.FourtyEight;
            Info.Time = 4;
            SizeChanged += MainWindow_SizeChanged;
            RootNavigation.Items.Add(
                new WPFUI.Controls.NavigationItem()
                {
                    Type = editor.GetType(),
                    Content = "Editor",
                    Tag = "editor",
                    Icon = WPFUI.Common.Icon.Edit32
                });
            RootNavigation.Items.Add(
                new WPFUI.Controls.NavigationItem()
                {
                    Type = midi.GetType(),
                    Content = "Piano Roll",
                    Tag = "midi",
                    Icon = WPFUI.Common.Icon.MusicNote124
                });
            RootNavigation.Items.Add(
                new WPFUI.Controls.NavigationItem()
                {
                    Type = output.GetType(),
                    Content = "Output",
                    Tag = "output",
                    Icon = WPFUI.Common.Icon.ArrowExportUp24
                });
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Info.RenderX = e.NewSize.Width;
            Info.RenderY = e.NewSize.Height;
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("home");
        }
    }
}
