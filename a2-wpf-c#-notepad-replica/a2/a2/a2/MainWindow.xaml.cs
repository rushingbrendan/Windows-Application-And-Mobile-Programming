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
using System.Drawing;
//using System.Windows.Forms;

namespace a2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }



        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textbox);
        }




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            //instantiate fileMenuItem as object to determine item name
            MenuItem fileMenuItem = e.Source as MenuItem;

            switch (fileMenuItem.Name)
            {

                case "menuClose":
                    ExitNotepad();
                    break;

                case "menuNew":
                    NewNotepad();
                    break;
                        



                case "menuOpen":    // Create OpenFileDialog
                    Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                    // Launch OpenFileDialog by calling ShowDialog method
                    Nullable<bool> resultOpen = openFileDlg.ShowDialog();
                    // Get the selected file name and display in a TextBox.
                    // Load content of file in a TextBlockqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq
                    if (resultOpen == true)
                    {

                        textbox.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
                    }

                    break;

                case "menuSaveAs":    // Create SaveFileDialog

                    SaveAsNotepad();

                    break;


                case "menuFont":

                    System.Windows.Forms.FontDialog fontDialog1 = new System.Windows.Forms.FontDialog();

                    fontDialog1.ShowColor = false;  //disable color
                    //fontDlg.ShowApply = true;
                    fontDialog1.ShowEffects = false;    //disable effects
                                                        //fontDlg.ShowHelp = true;

                    //Set Current settings from Textbox to font diaglog box ******************
                    //var myCurrentFont = new System.Drawing.Font(textbox.FontFamily, textbox.FontSize, textbox.FontStyle, GraphicsUnit.Pixel);
                   // fontDialog1.Font = (myCurrentFont);



                    //fontDialog1.Font = textbox.FontFamily()



                    // Show the dialog.
                    System.Windows.Forms.DialogResult resultFont = fontDialog1.ShowDialog();
                    // See if OK was pressed.

                    if (resultFont == System.Windows.Forms.DialogResult.OK)
                    {
                        // Get Font object for dialog box
                        Font font = fontDialog1.Font;

                        // Set Font
                        textbox.FontFamily = new System.Windows.Media.FontFamily(fontDialog1.Font.Name);

                        //Set Font size
                        textbox.FontSize = fontDialog1.Font.Size;

                        //Set Font to Bold if Bold is selected
                        if (fontDialog1.Font.Bold)
                        {
                            textbox.FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            textbox.FontWeight = FontWeights.Normal;
                        }

                        //Set Font to Italic if Italic is selected
                        if (fontDialog1.Font.Italic)
                        {
                            textbox.FontStyle = FontStyles.Italic;
                        }
                        else
                        {
                            textbox.FontStyle = FontStyles.Normal;
                        }

                       
    
                    }

                    break;


                case "menuWordWrap":
                    if (textbox.TextWrapping.Equals(TextWrapping.NoWrap))
                    {
                        textbox.TextWrapping = TextWrapping.Wrap;
                        menuWordWrap.IsChecked = true;


                    }
                    else if (textbox.TextWrapping.Equals(TextWrapping.Wrap))
                    {
                        textbox.TextWrapping = TextWrapping.NoWrap;
                        menuWordWrap.IsChecked = false;
                    }

                    break;


                case "menuSpellCheck":
                    if (textbox.SpellCheck.IsEnabled.Equals(true))
                    {
                        textbox.SpellCheck.IsEnabled = false;
                        menuSpellCheck.IsChecked = false;


                    }
                    else
                    {
                        textbox.SpellCheck.IsEnabled = true;
                        menuSpellCheck.IsChecked = true;
                    }

                    break;

            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            statusBarText.Text = "Character Count: " + textbox.Text.Length;
        }


        private void SaveAsNotepad()
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";

            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.AddExtension = true;

            System.Windows.Forms.DialogResult resultSave = saveFileDialog1.ShowDialog();

            if (resultSave == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textbox.Text);

            }
        }



        private string ExitNotepad()
        {

            MessageBoxResult promptUser = MessageBox.Show("Do you want to save the file?", "Brendan Rushing's Textpad", MessageBoxButton.YesNoCancel);
            switch (promptUser)
            {
                case MessageBoxResult.Yes:
                    SaveAsNotepad();
                    return "exit";

                case MessageBoxResult.No:
                    NewNotepad();
                    return "exit";

                case MessageBoxResult.Cancel:
                    return "cancel";
            }

            return "cancel";
        }


        private void NewNotepad()
        {
            textbox.Text = "";

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (textbox.Text.Length != 0)
            {
                string retCode = ExitNotepad();

                if (retCode == "cancel")
                {
                    e.Cancel = true;
                }           
            }          
        }
    }
}
