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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string image = System.IO.Path.GetFullPath("img.jpg");


        public MainWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(image);
            logo.EndInit();

            img.Source = logo;

            Extract();
        }

        private async void Extract()
        {
            await Task.Delay(100);

            TesseractEngine engine = new TesseractEngine(System.IO.Directory.GetCurrentDirectory(), "bul", EngineMode.Default);
            var pixImage = Pix.LoadFromFile(image);
            var page = engine.Process(pixImage);

            var iterator = page.GetIterator();
            iterator.Begin();

            while (iterator.Next(PageIteratorLevel.Word))
            {
                if (iterator.TryGetBoundingBox(PageIteratorLevel.Word, out Tesseract.Rect bounds))
                {
                    var label = new TextBox()
                    {
                        Background = Brushes.LightGray,
                        BorderThickness = new Thickness(0),
                        IsReadOnly = true,
                        FontWeight = FontWeights.Bold,
                        FontSize= 14,
                    };

                    int x = (bounds.X1 * (int)img.ActualWidth) / pixImage.Width + (int)((canvas.ActualWidth - img.ActualWidth) / 2) + 4;
                    int y = (bounds.Y1 * (int)img.ActualHeight) / pixImage.Height - 4;

                    Canvas.SetLeft(label, x);
                    Canvas.SetTop(label, y);

                    //label.Width = 100;
                    //label.Height = 40;
                    label.Text = iterator.GetText(PageIteratorLevel.Word);

                    canvas.Children.Add(label);
                }

                
            }

            iterator.Dispose();
        }
    }
}
