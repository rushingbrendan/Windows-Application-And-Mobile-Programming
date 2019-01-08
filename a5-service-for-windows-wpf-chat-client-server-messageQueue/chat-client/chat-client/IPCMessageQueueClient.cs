/*
*  FILE          : IPCMessageQueueClient.cs
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
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace IPCMQClient
{
    /// <summary>
    /// Class for IPC Message Queue Client to send messages to a IPC MEssage Queue Server
    /// </summary>
    public class IPCMessageQueueClient
    {

        MessageQueue mq;

        /// <summary>
        /// enum for different types of messages
        /// </summary>
        public enum messageType
        {
            text = 0,
            listUsers,
            connect,
            disconnect
        };

        /// <summary>
        /// structure for data in message
        /// </summary>
        public struct message
        {
            public messageType type;
            public string body;
        }


        /// <summary>
        /// Paramaterized constructor for IPCMEssageQueueClient class object
        /// </summary>
        /// <param name="inputQueueName"></param>
        public IPCMessageQueueClient(string inputQueueName)
        {
            //instantiate class
            mq = new MessageQueue(inputQueueName);

            //format message with type of message structure
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(message) });
        }


        /// <summary>
        /// This method writes a message to the IPC message queue on the server
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        public string WriteToQueue(message inputMessage)
        {
            string result = "OK";

            //send message in paramater
            mq.Send(inputMessage);
            //close queue
            mq.Close();

            //return result
            return result;
            
        }
    }
}
