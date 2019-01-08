/*
	FILE: 			Server.cs
    PROJECT:		PROG 2120 - A05 - IPC

    PROGRAMMER:		Josh Rogers and Brendan Rushing
	FIRST VERSION:	2018-Nov-09
	DESCRIPTION	:
		Runs the server side for a TCP MessageQueue based chat program.
        Listens for new chat clients, then creates a thread that will deal
        with various messages from multiple clients at once
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace WinMob_A4_Server
{
    public class Server
    {
        //An enum for what type of message is in the queue
        public enum messageType {
            text = 0,
            listUsers,
            connect,
            disconnect
        };

        //Struct to hold message queues to/from client, and client's username
        public struct userInfo
        {
            public MessageQueue toClient;
            public MessageQueue fromClient;
            public string username;
        }

        //A struct to hold message body and message type
        public struct message
        {
            public messageType type;
            public string body;
        }

        
        //Queue and queue path for accepting new client connections
        string incomingConnectionQueuePath = @".\Private$\clientConnectQueue";
        MessageQueue connectionQueue;

        //A list of all connected users
        List <userInfo> clientList = new List<userInfo>();

        

        public Server()
        {

            //Search for the queue that listens for clients trying to connect
            //If it exists, attach to it, otherwise create it
            if (!MessageQueue.Exists(incomingConnectionQueuePath)) {
                connectionQueue = MessageQueue.Create(incomingConnectionQueuePath);
            } else
            {
                connectionQueue = new MessageQueue(incomingConnectionQueuePath);
            }
        }




        /*
        *   FUNCTION : SendMessages
        *
        *   DESCRIPTION : Takes a message as a string and sends it to all active clients
        *
        *   PARAMETERS : string outMessage - The text to be sent to all clients
        *   
        *   RETURNS : none
        */
        private void SendMessages (string outMessage)
        {
            //Create a new message with type text and add the message text to the body
            message newMessage;
            newMessage.type = messageType.text;
            newMessage.body = outMessage;

            //Send the message to each client in the client list
            foreach (userInfo clientInfo in clientList)
            {
                clientInfo.toClient.Send(newMessage);

                clientInfo.toClient.Close();
            }
        }




        /*
        *   FUNCTION : ListUsers
        *
        *   DESCRIPTION : Sends a comma-delimited list of users to clients
        *
        *   PARAMETERS : MessageQueue client - The client to send the list to
        *                                       or null to send to all clients
        *   
        *   RETURNS : true if the list of users was generated successfully, otherwise false
        */
        private bool ListUsers(MessageQueue client)
        {
            message userList;

            string nameList = "";

            //Add each connected user to a comma-delimited string
            foreach (userInfo clientInfo in clientList)
            {
                nameList += clientInfo.username;
                nameList += ',';
            }

            //If there's at least one user in the userlist, send the data
            if (nameList.Length > 0)
            {
                //Remove last comma from string and add it to the message, along with a message type
                nameList = nameList.Remove(nameList.Length - 1);
                userList.body = nameList;
                userList.type = messageType.listUsers;

                //If no client was specified, send user list to all clients
                if (client == null)
                {
                    foreach (userInfo clientInfo in clientList)
                    {
                        clientInfo.toClient.Send(userList);
                        clientInfo.toClient.Close();
                    }
                }
                //If client was specified, send user list to that client
                else
                {
                    client.Send(userList);
                    client.Close();
                }

                return true;

            } else
            {
                return false;
            }
        }




        /*
        *   FUNCTION : ClientConnect
        *
        *   DESCRIPTION : Creates/connects to a client's message queues and adds the
        *                 client to the active client list
        *
        *   PARAMETERS : string clientConnectInfo - A comma-delimited string containing:
        *                           1: The client's IP
        *                           2: The client's GUID
        *                           3: The client's username
        *   
        *   RETURNS : A userInfo struct containing the client's information
        */
        private userInfo ClientConnect(string clientConnectInfo)
        {
            //Create a new blank client info struct
            userInfo newClient = new userInfo();

            //Split connection data into useful strings
            string[] tokens = clientConnectInfo.Split(',');

            //If not all data is present, zero out object and return
            if (tokens.Count() != 3)
            {
                newClient.toClient = null;
                newClient.fromClient = null;
                newClient.username = null;
                return newClient;
            }

            //Save connection data to strings for use later
            string clientIP = tokens[0];
            string clientGUID = tokens[1];
            newClient.username = tokens[2];

            string clientOutString = @".\Private$\" + clientGUID + "out";

            //Create a new message to confirm connection
            message confirmConnect;
            confirmConnect.type = messageType.connect;
            confirmConnect.body = "Connected";

            try
            {
                //Create local message queue, connect to remote message queue
                if (!MessageQueue.Exists(clientOutString))
                {
                    newClient.fromClient = MessageQueue.Create(@".\Private$\" + clientGUID + "out");
                } else
                {
                    newClient.fromClient = new MessageQueue(clientOutString);
                }
                newClient.toClient = new MessageQueue("FormatName:Direct=TCP:" + clientIP + @"\private$\" + clientGUID + "in");

                //Set up message queues to use correct format
                newClient.toClient.Formatter = new XmlMessageFormatter(new Type[] { typeof(message) });
                newClient.fromClient.Formatter = new XmlMessageFormatter(new Type[] { typeof(message) });
            }
            //In case of any exception, zero-out data fields and return
            catch (MessageQueueException mqex)
            {
                Console.WriteLine("MessageQueueException: " + mqex);
                newClient.toClient = null;
                newClient.fromClient = null;
                newClient.username = null;
                return newClient;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                newClient.toClient = null;
                newClient.fromClient = null;
                newClient.username = null;
                return newClient;
            }

            //Send conformation message to client and return client object
            newClient.toClient.Send(confirmConnect);
            return newClient;
        }




        /*
        *   FUNCTION : ClientThread
        *
        *   DESCRIPTION : The main loop that will listen for messages from the client
        *                   and deal with messages appropriately
        *
        *   PARAMETERS : userInfo client - A struct containing the client's information
        *   
        *   RETURNS : nothing
        */
        private void ClientThread (userInfo client)
        {
            clientList.Add(client);

            //Send a list of connected users to all clients
            ListUsers(null);

            while (true)
            {
                message newMessage = (message)client.fromClient.Receive().Body;

                if ((newMessage.body != null) || (newMessage.type == messageType.disconnect))
                {
                    switch (newMessage.type)
                    {
                        case messageType.text:

                            //If a message was reicieved, redistribute to all clients
                            SendMessages("[" + client.username + "]: " + newMessage.body);
                            break;

                        case messageType.listUsers:

                            //Send a list of connected users to only this client
                            ListUsers(client.toClient);
                            break;

                        case messageType.disconnect:

                            //Client DCing, delete local message queue and remove client from list
                            clientList.Remove(client);
                            MessageQueue.Delete(client.fromClient.Path);
                            //MessageQueue.Remove
                            //Send new client list to all clients to reflect DCed client
                            ListUsers(null);
                            return;

                    }
                }
            }
        }




        /*
        *   FUNCTION : RunServer
        *
        *   DESCRIPTION : Listens for new clients in a loop, then sends them off to a new
        *                   thread once they try and connect
        *
        *   PARAMETERS : nothing
        *   
        *   RETURNS : nothing
        */
        public void RunServer ()
        {
            //Set the format of the message body
            connectionQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(message) });

            //Loop and listen for new clients
            while (true)
            {
                try
                {
                    //Get a new message from the server connection request queue
                    message newMessage = (message)connectionQueue.Receive().Body;

                    userInfo newClient;

                    if (newMessage.body != null)
                    {
                        //If the message had a body, try to connect to the client with the info the client sent
                        newClient = ClientConnect(newMessage.body);

                        //If any info is missing, client connection is invalid so return to waiting for client
                        if ((newClient.username == "") || (newClient.toClient == null) || (newClient.fromClient == null))
                        {

                            Console.WriteLine("Connecting to client " + newMessage.body + " failed.");

                        } else
                        {

                            //If info was valid, create a new thread for that client, start it, return to listening for client
                            Thread newClientThread = new Thread(() => ClientThread(newClient));
                            newClientThread.Start();

                        }
                    }
                }

                //Error printing for client connections
                catch (MessageQueueException mqex)
                {
                    Console.WriteLine("MQ Exception: " + mqex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }

            }
        }
    }
}
