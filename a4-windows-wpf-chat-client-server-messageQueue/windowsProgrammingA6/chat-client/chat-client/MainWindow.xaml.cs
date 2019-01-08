/*
*  FILE          : MainWindow.xaml.cs
*  PROJECT       : PROG2120 - Windows and Mobile Programming: Assignment #4 - IPC Chat Client Server
*  PROGRAMMER    : Brendan Rushing & Josh Rogers
*  FIRST VERSION : 2018-11-16
*  DESCRIPTION   : 
*  
*       This project is a Windows chat program that allows multiple clients to chat with each other
*           - The clients and server uses IPC Windows Message queues to send and receive messages
*           - The clients connect to a server that can be located on the same computer or another computer on the local network
*           - The clients send the server a message with their IP address, username and guid
*           - The server then creates a unique queue for each user
*           - The server and clients support threading and can respond to messages as they are sent
*           - The server keeps track of all the users connected and sends the list to the clients
*           - The clients can tag users in messages by clicking on their name in the user list box
*           - When the client application is closed, the client sends a disconnect message to the server who removes them from the list
*           
*           
*           TO DO:      1. Add logo icon to server and client
*                       2. Add about box
*                       3. Add check for ip valid?
*/




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
using IPCMessageQueue;
using IPCMQClient;
using System.Threading;
using System.Net;
using System.Messaging;

namespace chat_client
{
    /// <summary>
    /// Class logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private IPCMessageQueueClient mqClient;
        private Thread listenThread;
        private IPCMessageQueueServer mqServer;
        private string userName;
        bool connected = false;        
        string guid;
        string myIPAddress;
        string serverIPAddress;

        /// <summary>
        /// enum for different types of messages between server and client
        /// </summary>
        public enum messageType
        {
            text = 0,
            listUsers,
            connect,
            disconnect
        };

        /// <summary>
        /// struct for the message sent to the clients and server
        /// </summary>
        public struct message
        {
            public messageType type;
            public string body;
        }


        /// <summary>
        /// This method contains the code for the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //FIND MY IP ADDRESS
            #pragma warning disable CS0618 // Type or member is obsolete
            myIPAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

            //CREATE GUID STRING
            guid = System.Guid.NewGuid().ToString();

            //CREATE MESSAGE QUEUE SERVER CLASS - INPUT
            //INPUT IS MY GUID + "in" - so that it is unique even if users have same name and IP Adddress
            mqServer = new IPCMessageQueueServer(".\\private$\\"+guid+"in");
        }

        
        /// <summary>
        /// This function is called when the main window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            connected = false;

            //SET STATUSBAR TEXT
            statusBarText.Text = $"Status: Not Connected\t\t\t Client IP: {myIPAddress}\t\t\t Server IP: {serverIPAddress}";

            //set focus on textbox right away so user can type as soon as program is launched
            Keyboard.Focus(ServerIPAddressInput);  
        }


        /// <summary>
        /// This function is called any of the items in the menu bar at the top of the program are clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            //instantiate fileMenuItem as object to determine item name
            MenuItem fileMenuItem = e.Source as MenuItem;

            switch (fileMenuItem.Name)
            {
                case "menuAbout":
                    //create aboutbox and show it
                    chat_client_AboutBox chatClientAboutBox = new chat_client_AboutBox();
                    chatClientAboutBox.Show();                    
                    break;

                case "menuClose":
                    Close();            //call application closing function
                    break;
            }
        }


        /// <summary>
        /// This method is called if the textbox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        /// <summary>
        /// This function is called when the window closing event is called (x button)
        ///     -Disconnect message is sent to server
        ///     -Application is exited with code 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult promptUser = MessageBox.Show("Do you want to close?", "Brendan Rushing & Josh Roger's Chat-Client", MessageBoxButton.YesNo);
            switch (promptUser)
            {
                case MessageBoxResult.Yes:               
                    try
                    {
                        //Send disconnect message to server if connected
                        if (connected)
                        {
                            IPCMessageQueueClient.message outputMessage = new IPCMessageQueueClient.message();
                            outputMessage.type = IPCMessageQueueClient.messageType.disconnect;  //type = disconnect                            
                            mqClient.WriteToQueue(outputMessage);   //send disconnect message
                        }                        
                    }
                    catch
                    {
                        Environment.Exit(0);        //exit program
                    }                  
                    Environment.Exit(0);        //exit program                    
                    break;

                case MessageBoxResult.No:
                    e.Cancel = true;
                    return;
                   
            }
        }

        
        /// <summary>
        /// This mehod is called when a keyboard button is pressed in the input text box
        ///     If the enter button is pressed, the current text box is sent as a message to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            //CHECK IF ENTER BUTTON IS PRESSED
            if (e.Key == Key.Return)
            {
                if (connected)
                {
                    //write message text to server queue
                    IPCMessageQueueClient.message outputMessage;
                    outputMessage.type = IPCMessageQueueClient.messageType.text;    //type is text
                    outputMessage.body = messageInput.Text;                    
                    mqClient.WriteToQueue( outputMessage);  //write to queue

                    //clear textbox contents
                    messageInput.Text = "";
                }
            }
        }

    
        /// <summary>
        /// This method is a dedicated thread to listen to the message queue server on the local machine
        ///     - this message queue contains all incoming messages to the client
        ///     - the message can contain message text from a user
        ///     - the message text can contain a list of current users on the chat server
        /// </summary>
        private void MessageQueueServerListener()
        {
            //loop
            while (true)
            {
                IPCMessageQueue.IPCMessageQueueServer.message outputMessage;
                string currentTime = null;

                //wait for new message
                outputMessage = mqServer.GetMessages();
                
                //IF MESSAGAGE IS A LIST OF USERS  -- THEN PARSE AND ADD INTO LIST
                if (outputMessage.type == IPCMessageQueueServer.messageType.listUsers)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        //clear list box
                        UserListBox.Items.Clear();
                    }));

                    //break into strings
                    var elements = outputMessage.body.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    //step through
                    foreach (string items in elements)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            UserListBox.Items.Add(items);
                        }));
                    }
                }
                //IF MESSAGE IS CHAT TEXT FROM USER
                else if (outputMessage.type == IPCMessageQueueServer.messageType.text)
                {
                    //get current time
                    currentTime = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                    //get permission from UI thread to update UI textbox
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        //add message text to text output in chat client
                        messageOutput.Text = messageOutput.Text + currentTime + " - " + outputMessage.body + Environment.NewLine;
                    }));
                }
            }
        }


        /// <summary>
        /// This method is called when the connect button at the top of the window is pressed
        ///     - The IP Address text box and username textbox will be used to connect to the server
        ///     - The server will be attempted to connect to
        ///     - The connection will timeout if not connected within 10 seconds
        ///     - The connect button will then be greyed out and disabled if connection is successfull
        ///     - clientconnectqueue is used for all users to first connect to
        ///     - client tells the server their guid, ip and username
        ///     - server then makes a unique queue for that user to use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool connectionSuccessful = false;

                //USERNAME AND IP ADDRESS MUST HAVE TEXT IN THEM
                if ((UserNameInput.Text.Length > 0) && (ServerIPAddressInput.Text.Length >0))
                {

                    //SET LED TO YELLOW
                    StatusLEDRed.Visibility = System.Windows.Visibility.Hidden;
                    StatusLEDGreen.Visibility = System.Windows.Visibility.Hidden;
                    StatusLEDYellow.Visibility = System.Windows.Visibility.Visible;

                    //set username for client
                    userName = UserNameInput.Text;

                    //if username has comma in then tell the user and remove them.
                    if (userName.Contains(","))
                    {
                        userName = userName.Replace(",", "");
                        errorMessage.Content = "No Commas Allowed";
                    }

                    //update server ip address
                    serverIPAddress = ServerIPAddressInput.Text;

                    //CREATE MESSAGE QUEUE CLIENT CLASS                    
                    mqClient = new IPCMessageQueueClient("FormatName:Direct=TCP:" + serverIPAddress + @"\private$\clientconnectqueue"); 

                    //SEND FIRST MESSAGE TO SERVER                    
                    IPCMessageQueueClient.message outputMessage;
                    outputMessage.type = IPCMessageQueueClient.messageType.connect;
                    outputMessage.body = myIPAddress + "," + guid + "," + userName;               
                    mqClient.WriteToQueue(outputMessage);


                    //wait for first message - timeout after 10 seconds
                    if ( !(connectionSuccessful = mqServer.GetFirstMessage()))
                    {
                        errorMessage.Content = "Connection Failed";

                        MessageBoxResult result = MessageBox.Show("ERROR - COULD NOT CONNECT TO SERVER",
                                              "Confirmation",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Question);

                        return; //failed to connect, return to UI
                    }
                    
                    //connect to new queue for my client on server
                    mqClient = new IPCMessageQueueClient("FormatName:Direct=TCP:" + serverIPAddress + "\\private$\\" + guid + "out");

                    //CREATE TRHEAD FOR LISTENER
                    listenThread = new Thread(new ThreadStart(MessageQueueServerListener));

                    //START THREAD
                    listenThread.Start();

                    //SET LED TO GREEN
                    StatusLEDRed.Visibility = System.Windows.Visibility.Hidden;
                    StatusLEDYellow.Visibility = System.Windows.Visibility.Hidden;
                    StatusLEDGreen.Visibility = System.Windows.Visibility.Visible;

                    //DISABLE CONNECT BUTTON
                    connectButton.IsEnabled = false;
                    connectButton.Content = "Connected";

                    //DISABLE INPUT - IP ADDRESS & USERNAME
                    UserNameInput.IsReadOnly = true;
                    ServerIPAddressInput.IsReadOnly = true;

                    //SET STATUSBAR TEXT
                    statusBarText.Text = $"Status: Connected\t\t\t Client IP: {myIPAddress}\t\t\t Server IP: {serverIPAddress}";                    

                    //ENABLE INPUT TEXTBOX
                    messageInput.IsReadOnly = false;
                    connected = true;

                    //set focus on textbox right away so user can type as soon as program is launched.
                    Keyboard.Focus(messageInput);

                    connectionSuccessful = true;
                }
           
            
        }


        /// <summary>
        /// This method is called when the user list box item is selected
        ///     - This listbox shows a list of all connected users to the server
        ///     - If a user is selected then the selected username is added to the chat input text
        ///       with a @ symbol in front of their name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //select textbox again
            Keyboard.Focus(messageInput);

            //add selected name to textbox
            messageInput.Text += "@"+ UserListBox.SelectedItem.ToString() + " ";
            messageInput.Select(messageInput.Text.Length, 0);
        }
    }
}
