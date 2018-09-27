using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;

namespace TextExtractor.OCR
{
    public class TesseractOCR : IOCR
    {
        public IEnumerable<TextEntry> Extract(string imagePath)
        {
            List<TextEntry> texts = new List<TextEntry>();

            PageIteratorLevel mode = PageIteratorLevel.Word;

            TesseractEngine engine = new TesseractEngine(System.IO.Directory.GetCurrentDirectory(), "eng", EngineMode.Default);
            var pixImage = Pix.LoadFromFile(imagePath);
            var page = engine.Process(pixImage);
            Point imageSize = new Point(pixImage.Width, pixImage.Height);

            var iterator = page.GetIterator();
            iterator.Begin();

            while (iterator.Next(mode))
            {
                if (iterator.TryGetBoundingBox(mode, out Tesseract.Rect bounds))
                {
                    texts.Add(new TextEntry()
                    {
                        Position = new Point(bounds.X1 / (double)pixImage.Width, bounds.Y1/ (double)pixImage.Height ),
                        Text = iterator.GetText(mode)
                    });
                }
            }

            iterator.Dispose();

            return texts;
        }
    }
}
