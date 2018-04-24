using RegexPower.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RegexPower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel { get; set; }



        public MainWindow(MainViewModel viewModel)
        {
            ViewModel = viewModel;

            DataContext = ViewModel;

            InitializeComponent();
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchHightlighted(PatternBox.Text);
        }

        private void LoadFile_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadFile();
        }

        private void ValidateEmail_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsValidEmail(GetTheWholeString(this.RawUserInput_RichTextBox)) == true)
                MessageBox.Show(string.Format("Email is valid:{0}", GetTheWholeString(RawUserInput_RichTextBox)));
            else
                MessageBox.Show(string.Format("Email is  not valid:{0}", GetTheWholeString(RawUserInput_RichTextBox)));
        }

        private string GetTheWholeString(RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text.Trim();
        }

        private void RegexInput_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = ownPatter.Text;

            if (ViewModel.IsValidRegex(pattern))
                ViewModel.SearchHightlighted(pattern);
            else
                ClearHighlighting();
        }

        public void ClearHighlighting()
        {
            RawUserInput_RichTextBox.SelectAll();

            RawUserInput_RichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
            RawUserInput_RichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        }

        public void HighlightText(TextRange textRange)
        {
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
    }
}
