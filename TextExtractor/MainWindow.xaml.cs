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
using Tesseract;
using TextExtractor.OCR;

namespace TextExtractor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string image = System.IO.Path.GetFullPath("t.jpg");
        IOCR engine = new AspriseOCR();

        bool canvasVisible = true;

        IEnumerable<TextEntry> texts;

        public MainWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(image);
            logo.EndInit();

            img.Source = logo;

            Render();
        }

        private async void Render()
        {
            await Extract();

            await Task.Delay(100);

            foreach (var entry in texts)
            {
                var label = new TextBox()
                {
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(0),
                    IsReadOnly = true,
                    FontWeight = FontWeights.Bold,
                    FontSize = 10,
                };


                double x = (entry.Position.X * (int)img.ActualWidth) + (int)((canvas.ActualWidth - img.ActualWidth) / 2);
                double y = (entry.Position.Y * (int)img.ActualHeight);

                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, y);

                label.Text = entry.Text;

                canvas.Children.Add(label);
            }
        }

        private async Task Extract()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Run(() =>
            {
                texts = engine.Extract(image);
            });
            Mouse.OverrideCursor = null;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.V)
            {
                canvasVisible = !canvasVisible;
                canvas.Visibility = canvasVisible ? Visibility.Visible : Visibility.Hidden;
            }

            base.OnKeyUp(e);
        }

    }
}
