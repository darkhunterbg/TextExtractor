using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextExtractor.OCR
{
    public class TextEntry
    {
        public Point Position;
        public string Text;
    }


    public interface IOCR
    {
        IEnumerable<TextEntry> Extract(string imagePath);
    }
}
