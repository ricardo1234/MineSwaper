using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private Game Game { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StartFlashing = false;
        }

        private void GenerateGame(int lines)
        {
            for (var x = 0; x < lines; x++)
            {
                GameArea.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(100 / lines, type: GridUnitType.Star)
                });
                for (var y = 0; y < lines; y++)
                {
                    GameArea.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(496 / lines, GridUnitType.Pixel)
                    });
                    var text = new Button
                    {
                        Name = "x" + x + "y" + y,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0))
                    };
                    text.Click += Played_Click;
                    Grid.SetColumn(text, x);
                    Grid.SetRow(text, y);
                    GameArea.Children.Add(text);
                }
            }
        }

        void Played_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.IsEnabled == false)
                return;

            (sender as Button).IsEnabled = false;

            int.TryParse((sender as Button)?.Name.Split('y')[0].Remove(0, 1), out var x);
            int.TryParse((sender as Button)?.Name.Split('y')[1], out var y);

            var type = Game.GetCamp(x, y);
            switch (type)
            {
                case CampType.Bomb:
                    Game.End();
                    TimerNumber.Content = Game.GameTime.Minutes + ":" + Game.GameTime.Seconds;
                    GameArea.IsEnabled = false;
                    foreach (var coord in Game.BombXY)
                        GameArea.FindChild<Button>($"x{coord.Split(',')[0]}y{coord.Split(',')[1]}").Content = new Image { Source = new BitmapImage(new Uri($"Pics/bomb.png", UriKind.Relative)), Stretch = Stretch.Fill };
                    break;
                case CampType.Blanck:
                    //x -1 y
                    if (x - 1 >= 0)
                        GameArea.FindChild<Button>($"x{x - 1}y{y}").RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    //x + 1 y
                    if (x + 1 < Game.Size)
                        GameArea.FindChild<Button>($"x{x + 1}y{y}").RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    //x y + 1
                    if (y + 1 < Game.Size)
                        GameArea.FindChild<Button>($"x{x}y{y + 1}").RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    //x y - 1
                    if (y - 1 >= 0)
                        GameArea.FindChild<Button>($"x{x}y{y - 1}").RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                default:
                        ((Button)sender).Content = new Image{Source = new BitmapImage(new Uri($"Pics/number{type.GetHashCode()}.png", UriKind.Relative)),Stretch = Stretch.Fill};
                        break;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var size = Beginner.IsChecked != null && (bool)Beginner.IsChecked ? 8 : 16;
            GenerateGame(size);
            Game = new Game(size, size == 8 ? 10 : 40);
            BombsNumber.Content = Game.Bombs;
            GameArea.IsEnabled = true;
            Game.Start();
            TimerNumber.Visibility = Visibility.Visible;
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