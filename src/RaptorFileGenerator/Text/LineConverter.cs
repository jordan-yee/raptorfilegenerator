using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaptorFileGenerator.Text
{
    public class LineConverter
    {
        private string _text;

        public LineConverter(string[] lines)
        {
            Lines = lines;
        }

        public LineConverter(string text)
        {
            _text = text;
        }

        public string[] Lines
        {
            get
            {
                return GetLinesFromText(_text);
            }

            set
            {
                _text = GetTextFromLines(value);
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        public static string[] GetLinesFromText(string text)
        {
            return text == null ? null : text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        public static string GetTextFromLines(string[] lines)
        {
            return lines == null ? null : string.Join(Environment.NewLine, lines);
        }
    }
}
