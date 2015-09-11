using System;
using System.Windows;
using System.Windows.Threading;

namespace SB01.WPF
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        private static ConsoleWindow instance;

        public ConsoleWindow()
        {
            InitializeComponent();
        }

        internal static void ShowConsole()
        {
            if (instance == null || !instance.IsActive)
            {
                instance = new ConsoleWindow();
                instance.TextBlockConsole.Text = ConsoleText;
                instance.Show();
            }
        }

        public static string ConsoleText { get; set; }

        public static void AddConsoleText(string text)
        {
            if (instance != null)
                instance.AppendText(text);

            ConsoleText += text;
        }

        public void AppendText(string text)
        {
            Action action = delegate { TextBlockConsole.Text += text; };
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }
    }
}
