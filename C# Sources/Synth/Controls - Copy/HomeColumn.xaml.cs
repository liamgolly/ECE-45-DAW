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

namespace Synth.Controls
{
    /// <summary>
    /// Interaction logic for HomeColumn.xaml
    /// </summary>
    public partial class HomeColumn : UserControl
    {
        public static hSynth Synth { get; set; }
        public HomeColumn(string name, bool isSynth)
        {
            InitializeComponent();
            ColumnName.Text = name;
            
            hcStackPanel.Children.Add(new hSynth() { Name = name});
            Synth = (hSynth)hcStackPanel.Children[1];
        }



        public static LineGeometry generateLineGeometry(int i, uint x, uint x1, double scale_factor, uint max, uint min, double height) { 

            Point p1 = new Point(i * scale_factor + 25, height - scale_graph(x, max, min, height) + height/10);
            Point p2 = new Point(i * scale_factor + 25, height - scale_graph(x1, max, min, height) + height/10);
            return new LineGeometry(p1, p2);
        }
        public static double scale_graph(uint x, uint max, uint min, double height)
        {
            return height * ((x - min)/((double)(max - min)));
        }       
    }
}
