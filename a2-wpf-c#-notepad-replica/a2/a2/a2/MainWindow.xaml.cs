/*
*  FILE          : MainWindow.xaml.cs
*  PROJECT       : PROG2120 - Windows and Mobile Programming - Assignment #2
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-10-07
*  DESCRIPTION   : Windows WPF & C# notepad replica application.
*  REQUIREMENTS  :
*  
*   Notepad icon is borrowed from microsoft windows notepad application.
*   
*   1. The overall user interface should have a menu at the top, a status bar at the bottom, and the
        remaining area in between as the work area for editing.
    2. Implement a menu that has “File Options Help” as the top level item.
    3. Implement “New”, “Open”, “Save As” and “Close” as the menu items in “File”.
        a. “New” allows you to start a new file for editing. If there is text in the work area, you
            must give the user the chance to save the file. Use the Save File Dialog if a save is requested.
        b. “Open” must display the Open File Dialog and allow you to choose a text file to Open
            and load into the work area of your application.
        c. “Save As” must display the Save File Dialog and allow you to save a text file with the
            content from your work area.
        d. “Close” closes the application.
    4. Implement “About” as the menu item in “Help”
        a. About should bring up a modal About Box with the standard information about your
           application. It should behave like most common about boxes in windows applications.
    5. Implement a Status Bar at the bottom of the window that displays the current count of
       characters in the work area. As you type, or delete, you should have this number updated.    
    6. There is no implementation requirement for “Options”. However, as a challenge, you may
       consider implementing options like Word Wrap or Font.
    7. Follow SET coding standards.

*/

//INCLUDES
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
//eo INCLUDES

namespace a2
{
    /*
    *   NAME : MainWindow
    *
    *   PURPOSE: Class for the MainWindow for the project.
    *            All events for the main window are handled here.
    *			
    */
    public partial class MainWindow : Window
    {
        /*
        *   FUNCTION : MainWindow
        *
        *   DESCRIPTION : This function is the constructor for the mainwindow to initialize the components
        *
        *   PARAMETERS : none
        *   
        *   RETURNS : none
        */
        public MainWindow()
        {
            InitializeComponent();

        }


        /*
        *   FUNCTION : MainWindow_Loaded
        *
        *   DESCRIPTION : This function is called when the main window is loaded
        *
        *   PARAMETERS : object sender, RoutedEventArgs e
        *   
        *   RETURNS : none
        */
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textbox);    //set focus on textbox right away so user can type as soon as program is launched.
        }



        /*
        *   FUNCTION : MenuItem_Click
        *
        *   DESCRIPTION : This function is called any of the items in the menu bar at the top of the program are clicked
        *
        *   PARAMETERS : object sender, RoutedEventArgs e
        *   
        *   RETURNS : none
        */
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            //instantiate fileMenuItem as object to determine item name
            MenuItem fileMenuItem = e.Source as MenuItem;

            switch (fileMenuItem.Name)
            {

                case "menuAbout":
                    AboutBox1 NotepadAboutBox = new AboutBox1();    //create aboutbox and show it
                    NotepadAboutBox.Show();
                    break;


                case "menuClose":
                    Close();            //call application closing function
                    break;

                case "menuNew":
                    NewNotepad();       //new notepad
                    break;
                        



                case "menuOpen":    // Create OpenFileDialog
                    Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                    // Launch OpenFileDialog by calling ShowDialog method
                    Nullable<bool> resultOpen = openFileDlg.ShowDialog();
                 
                    if (resultOpen == true)
                    {
                        //read text from file and put in the textbox
                        textbox.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
                    }

                    break;

                case "menuSaveAs":    // Create SaveFileDialog

                    SaveAsNotepad();

                    break;


                case "menuFont":

                    System.Windows.Forms.FontDialog fontDialog1 = new System.Windows.Forms.FontDialog();

                    fontDialog1.ShowColor = false;  //disable color
                    fontDialog1.ShowEffects = false;    //disable effects
                                                       
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


                case "menuWordWrap":    //toggle word wrap
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


                case "menuSpellCheck":  //toggle spell check
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


        /*
        *   FUNCTION : TextBox_TextChanged
        *
        *   DESCRIPTION : This function is called when the contents of the textbox are changed
        *
        *   PARAMETERS : object sender, TextChangedEventArgs e
        *   
        *   RETURNS : none
        */
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            int whiteSpaceCount = 0;

            foreach (char character in textbox.Text)
            {
                //step through textbox contents and count white space characters (space, new line, etc)
                if (char.IsWhiteSpace(character))
                {
                    whiteSpaceCount++;
                }
            }

            statusBarText.Text = "Character Count: " + (textbox.Text.Length - whiteSpaceCount);
        }


        /*
        *   FUNCTION : SaveAsNotepad
        *
        *   DESCRIPTION : This function is called to open the save dialog to save the text
        *
        *   PARAMETERS : none
        *   
        *   RETURNS : none
        */
        private void SaveAsNotepad()
        {
            //create save file dialog
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            //filter txt files and .cs files
            saveFileDialog1.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";

            //default as .txt
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.AddExtension = true;
            
            //show dialog
            System.Windows.Forms.DialogResult resultSave = saveFileDialog1.ShowDialog();

            if (resultSave == System.Windows.Forms.DialogResult.OK)
            {
                //write contents of textbox to file
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textbox.Text);

            }

            
        }



        /*
        *   FUNCTION : NewNotepad
        *
        *   DESCRIPTION : This function is called to clear the textbox for a new file
        *
        *   PARAMETERS : none
        *   
        *   RETURNS : none
        */
        private void NewNotepad()
        {
            //ask user if they want to save file if there is content in message box
            if (textbox.Text.Length != 0)
            {

                MessageBoxResult promptUser = MessageBox.Show("Do you want to save the file?", "Brendan Rushing's Textpad", MessageBoxButton.YesNoCancel);
                switch (promptUser)
                {
                    case MessageBoxResult.Yes:
                        SaveAsNotepad();
                        break;

                }
            }
            
            //clear textbox contents
            textbox.Text = "";

        }


        /*
        *   FUNCTION : SaveAsNotepad
        *
        *   DESCRIPTION : This function is called when the window closing event is called (x button)
        *
        *   PARAMETERS : object sender, System.ComponentModel.CancelEventArgs e
        *   
        *   RETURNS : none
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if there are contents in the textbox then prompt user to save file
            if (textbox.Text.Length != 0)
            {
                MessageBoxResult promptUser = MessageBox.Show("Do you want to save the file?", "Brendan Rushing's Textpad", MessageBoxButton.YesNoCancel);
                switch (promptUser)
                {
                    case MessageBoxResult.Yes:
                        SaveAsNotepad();
                        break;

                    case MessageBoxResult.No:
                        break;

                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;

                }

            }          
        }
    }
}
