using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using MahApps.Metro.Controls;

namespace MineSwaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private bool _startFlashing;
        public bool StartFlashing
        {
            get { return _startFlashing; }
            set
            {
                _startFlashing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartFlashing"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StartFlashing = false;
        }

        private void GenerateGame(int lines)
        {
            
            for (var i = 0; i < lines; i++)
            {
                GameArea.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(100 / lines, type: GridUnitType.Star)
                });
                for (var j = 0; j < lines; j++)
                {
                    GameArea.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(496 / lines, GridUnitType.Pixel)
                    });
                    var text = new Button {
                        Name = "x" +i+ "y"+j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0,0,0))
                    };
                    text.Click += Played_Click;
                    Grid.SetColumn(text,i);
                    Grid.SetRow(text,j);
                    GameArea.Children.Add(text);
                }
        }
        }
        void Played_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse((sender as Button)?.Name.Split('y')[0].Remove(0, 1), out var x);
            int.TryParse((sender as Button)?.Name.Split('y')[1], out var y);
            TimerNumber.Visibility = Visibility.Visible;
            StartFlashing = !StartFlashing;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            GenerateGame(16);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            GameArea.Children.Clear();
            GameArea.RowDefinitions.Clear();
            GameArea.ColumnDefinitions.Clear();
        }
    }
}
