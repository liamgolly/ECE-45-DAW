using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using WPFUI.Controls;
using Synth.Controls;
using System.Text.RegularExpressions;

namespace Synth.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page, INavigable
    {
        public static Dictionary<string, HomeColumn> SynthColumns;
        public Home()
        {
            InitializeComponent();
            SynthColumns = new Dictionary<string, HomeColumn>();
        }

        public void AddChannel(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z_]+$");
            if (SynthColumns == null) { return; }
            if (!SynthColumns.ContainsKey(AddColumnName.Text) && regex.IsMatch(AddColumnName.Text))
            {
                HomeColumn newColumn = new(AddColumnName.Text, true);
                sStackPanel.Children.Add(newColumn);
                SynthColumns.Add(AddColumnName.Text, newColumn);
                Info.Channels += 1;
            }      
        }




        public void OnNavigationRequest(INavigation sender, object current)
        {
            
        }
    }
}
