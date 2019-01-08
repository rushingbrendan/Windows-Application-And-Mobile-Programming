/*
*  FILE          : AboutBox1.cs
*  PROJECT       : PROG2120 - Windows and Mobile Programming - Assignment #2
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-10-07
*  DESCRIPTION   : Windows WPF & C# notepad replica application.
*  REQUIREMENTS  :
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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
//eo INCLUDES


namespace a2
{

/*
*   NAME : AboutBox1
*
*   PURPOSE: Class for Aboutbox that shows assembly information for project
*			
*/

    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();   //close about box when ok button is pressed
        }
    }
}
