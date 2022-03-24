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
    /// Interaction logic for PianoRoll.xaml
    /// </summary>
    public partial class PianoRoll : UserControl
    {
        public Dictionary<int, int> ColToSelNote = new();
        private object handled;
        public PianoRoll()
        {
            InitializeComponent();
            for (int i = 0; i < 9; i++)
            {
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "C" + i.ToString(),
                    Height=70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "C#/Db" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "D" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "D#/Eb" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "E" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "F" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "F#/Gb" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "G" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "G#/Ab" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "A" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "A#/Bb" + i.ToString(),
                    Height = 70
                });
                Notes.Children.Add(new TextBox()
                {
                    IsReadOnly = true,
                    Text = "B" + i.ToString(),
                    Height = 70
                });
            }
            for (int i = 0; i < 32; i++)
            {
                
                int index = NotesGrid.Children.Add(new StackPanel()
                {
                    Name = "NoteColumn" + i.ToString(),
                    Orientation = Orientation.Vertical,
                });
                Grid.SetColumn(NotesGrid.Children[index], i);
                for (int j = 0; j < 108; j++)
                {
                    WPFUI.Controls.CardControl newNote = new WPFUI.Controls.CardControl()
                    {
                        Name = "_" + i.ToString() + "_" + j.ToString(),
                        
                        BorderBrush = this.Resources["CardBackgroundFillColorDefaultBrush"] as SolidColorBrush,
                        Margin = new Thickness(1, 1, 1, 1)
                        
                        

                };
                    newNote.ClickMode = ClickMode.Press;
                    newNote.Click += StopCapture;
                    newNote.MouseMove += MouseMove;
                    int nindex = ((StackPanel)NotesGrid.Children[index]).Children.Add(newNote);
                    
                }
            }
            for (int i = 0; i < 32; i++)
                ColToSelNote[i] = -1;
        }

        public void StopCapture(object sender, RoutedEventArgs e)
        {
            OnKeyPress(sender, e);
            Mouse.Capture(null);
        }

        public void MouseMove(object sender, RoutedEventArgs e)
        {
            
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Control control = (Control)sender;
                if (control.IsMouseCaptured)
                    Mouse.Capture(null);
                OnKeyPress(sender, e);
            }
               
        }

        public void OnKeyPress(object sender, RoutedEventArgs e)
        {
            if (handled == sender) { return; }
            handled = sender;
            
            StackPanel col = (sender as WPFUI.Controls.CardControl).Parent as StackPanel; 
            int colIndex = NotesGrid.Children.IndexOf(col);
            int noteIndex = col.Children.IndexOf(sender as WPFUI.Controls.CardControl);

            if (ColToSelNote[colIndex] == noteIndex)
            {
                
                (col.Children[ColToSelNote[colIndex]] as WPFUI.Controls.CardControl).BorderBrush = this.Resources["CardBackgroundFillColorDefaultBrush"] as SolidColorBrush;
                ColToSelNote[colIndex] = -1;
                return;
            }

            (sender as WPFUI.Controls.CardControl).BorderBrush = this.Resources["AccentFillColorDefaultBrush"] as SolidColorBrush;
            
            if (ColToSelNote[colIndex] != -1)
                (col.Children[ColToSelNote[colIndex]] as WPFUI.Controls.CardControl).BorderBrush = this.Resources["CardBackgroundFillColorDefaultBrush"] as SolidColorBrush;
            ColToSelNote[colIndex] = noteIndex;

        }
    }
}
