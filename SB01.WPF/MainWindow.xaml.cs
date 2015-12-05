using System;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using SB01.Core;
using MessageBox = System.Windows.MessageBox;

namespace SB01.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TextBoxSource.Text = Brew.ReadSetting("Source");
            TextBoxDestination.Text = Brew.ReadSetting("Destination");
            TextBoxArchive.Text = Brew.ReadSetting("Archive");
            bool useYearStructure;
            if (bool.TryParse(Brew.ReadSetting("UseYearStructure"), out useYearStructure))
                CheckBoxUseYearStructure.IsChecked = useYearStructure;
            bool useYearStructureLargeOnly;
            if (bool.TryParse(Brew.ReadSetting("UseYearStructureLargeOnly"), out useYearStructureLargeOnly))
                CheckBoxOnlyLarge.IsChecked = useYearStructureLargeOnly;
        }

        private void ButtonSource_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = TextBoxSource.Text;
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                TextBoxSource.Text = folderBrowserDialog.SelectedPath;
        }

        private void ButtonDestination_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                TextBoxDestination.Text = folderBrowserDialog.SelectedPath;
        }

        private void ButtonArchive_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                TextBoxArchive.Text = folderBrowserDialog.SelectedPath;
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            string source = TextBoxSource.Text;
            string destination = TextBoxDestination.Text;
            string archive = TextBoxArchive.Text;
            bool useYearStructure = CheckBoxUseYearStructure.IsChecked.GetValueOrDefault();
            bool onlyLarge = CheckBoxOnlyLarge.IsChecked.GetValueOrDefault();

            // save to config
            Brew.AddUpdateAppSettings("Source", source);
            Brew.AddUpdateAppSettings("Destination", destination);
            Brew.AddUpdateAppSettings("Archive", archive);
            Brew.AddUpdateAppSettings("UseYearStructure", useYearStructure.ToString());
            Brew.AddUpdateAppSettings("UseYearStructureLargeOnly", onlyLarge.ToString());

            MessageBoxResult messageBoxResult = MessageBox.Show(this, string.Format("Move and rename all media from {0} to {1}?", source, destination), "Confirm", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                Renamer renamer = new Renamer(destination, archive, useYearStructure);
                //renamer.Go(source);
                ButtonGo.IsEnabled = false;
                renamer.GoAsync(source, (percent, message) =>
                {
                    if (percent < 101)
                    {
                        LabelProgress.Content = message;
                        ProgressBarMain.Value = percent;
                    }
                    else
                    {
                        // complete
                        LabelProgress.Content = "Complete";
                        ProgressBarMain.Value = 100;
                        ButtonGo.IsEnabled = true;
                    }
                });
            }
        }

        private void CheckBoxUseYearStructure_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxOnlyLarge.IsEnabled = true;
        }

        private void CheckBoxUseYearStructure_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxOnlyLarge.IsEnabled = false;
        }

        private void ButtonShowConsole_Click(object sender, RoutedEventArgs e)
        {
            ConsoleWindow.ShowConsole();
        }
    }
}
