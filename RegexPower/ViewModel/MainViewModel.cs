using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace RegexPower.ViewModel
{
    public class MainViewModel
    {
        private PatternRepository Pattern     { get; set; }
        private MainWindow        View        { get; set; }
        public  ObservableCollection<string>      MatchGroups { get; set; }



        public MainViewModel()
        {
            Pattern     = new PatternRepository();
            MatchGroups = new ObservableCollection<string>();

            View = new MainWindow(this);
            View.Show();
        }



        public void SearchHightlighted(string pattern)
        {
            View.ClearHighlighting();
            MatchGroups.Clear();

            var        regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var textPosition = View.RawUserInput_RichTextBox.Document.ContentStart;
            var       ranges = new List<TextRange>();

            while (textPosition != null)
            {
                if (textPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string text = textPosition.GetTextInRun(LogicalDirection.Forward);
                    var  matchs = regex.Matches(text);

                    foreach (Match match in matchs)
                    {
                        MatchGroups.Add(match.ToString());

                        var start = textPosition.GetPositionAtOffset(match.Index);
                        var   end = start.GetPositionAtOffset(match.Length);

                        var textrange = new TextRange(start, end);
                        ranges.Add(textrange);
                    }
                }
                textPosition = textPosition.GetNextContextPosition(LogicalDirection.Forward);
            }

            foreach (var textRange in ranges)
                View.HighlightText(textRange);
        }

        public void LoadFile()
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Length > 0)
            {
                View.RawUserInput_RichTextBox.Document.Blocks.Clear();
                View.RawUserInput_RichTextBox.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(openFileDialog.FileName)))); ;
            }
        }

        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
                && Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*");
        }
        
        public bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return false;

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}
