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

namespace TextExtractor
{
    class TextEntry
    {
        public Point Position;
        public string Text;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string image = System.IO.Path.GetFullPath("t.jpg");

        bool canvasVisible = true;

        List<TextEntry> texts = new List<TextEntry>();
        Point imageSize;

        public MainWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(image);
            logo.EndInit();

            img.Source = logo;

            Extract();
            Render();

        }

        private async void Render()
        {
            await Task.Delay(100);

            foreach(var entry in texts)
            {
                var label = new TextBox()
                {
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(0),
                    IsReadOnly = true,
                    FontWeight = FontWeights.Bold,
                    FontSize = 10,
                };


                double x = (entry.Position.X * (int)img.ActualWidth) / imageSize.X + (int)((canvas.ActualWidth - img.ActualWidth) / 2);
                double y = (entry.Position.Y * (int)img.ActualHeight) / imageSize.Y;

                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, y);

                label.Text = entry.Text;

                canvas.Children.Add(label);
            }
        }

        private void Extract()
        {

            PageIteratorLevel mode = PageIteratorLevel.Word;

            TesseractEngine engine = new TesseractEngine(System.IO.Directory.GetCurrentDirectory(), "eng", EngineMode.Default);
            var pixImage = Pix.LoadFromFile(image);
            var page = engine.Process(pixImage);
            imageSize = new Point(pixImage.Width, pixImage.Height);

            var iterator = page.GetIterator();
            iterator.Begin();

            while (iterator.Next(mode))
            {
                if (iterator.TryGetBoundingBox(mode, out Tesseract.Rect bounds))
                {
                    texts.Add(new TextEntry()
                    {
                        Position = new Point(bounds.X1, bounds.Y1),
                        Text = iterator.GetText(mode)
                    });
                }
            }

            iterator.Dispose();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if(e.Key== Key.V)
            {
                canvasVisible = !canvasVisible;
                canvas.Visibility = canvasVisible ? Visibility.Visible : Visibility.Hidden;
            }

            base.OnKeyUp(e);
        }

    }
}
