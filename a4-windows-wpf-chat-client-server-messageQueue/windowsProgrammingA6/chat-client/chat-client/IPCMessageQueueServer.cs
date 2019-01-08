/*
*  FILE          : IPCMessageQueueServer.cs
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



namespace IPCMessageQueue
{
    /// <summary>
    /// Class for IPC Message Queue Server to receives messages from a IPC message queue client
    /// </summary>
    public class IPCMessageQueueServer
    {

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


        public MessageQueue mq;

        /// <summary>
        /// Paramaterized constructor for IPCMEssageQueueServer class object
        /// </summary>
        /// <param name="serverName"></param>
        public IPCMessageQueueServer(string serverName)
        {
           //instantiate class object
           mq = MessageQueue.Create(serverName);

           //format message with type of message structure
           mq.Formatter = new XmlMessageFormatter(new Type[] {typeof(message)});
              
        }

        /// <summary>
        /// This method is called to receive the first message from the IPC message queue server
        ///     - the mehod will time out and return a fail condition if it has not received a message in 10 seconds
        /// </summary>
        /// <returns></returns>
        public bool GetFirstMessage()
        {
            //create message
            message messageOutput;
            messageOutput.body = "";

            try
            {
                //wait to receive message for 10 seconds
                messageOutput = (message)mq.Receive(TimeSpan.FromSeconds(10)).Body;
                
                //if message output body is null then return a fail condition
                //else, return pass condition
                //close queue
                if (messageOutput.body == null)
                {
                    mq.Close();
                    return false;
                }
                else
                {
                    mq.Close(); 
                    return true;
                }               
            }
            //catch exceptions and then close queue and return fail condition
            catch (MessageQueueException)
            {
            }
            catch (Exception)
            {
            }
            mq.Close();
            return false;
        }


        /// <summary>
        /// This method receives IPC message queue messages from the server
        ///     - will wait forever until it receives messages or the thread is killed.
        ///     - will return the message in struct message form        
        /// </summary>
        /// <returns>struct message</returns>
        public message GetMessages()
        {
            //create message
            message messageOutput;
            messageOutput.body = "";

                try
                {
                messageOutput = (message)mq.Receive().Body;

                    mq.Close();
                    return messageOutput;

                }
                //catch exceptions and then close queue and return fail condition
                catch (MessageQueueException)
                {                    
                }
                catch (Exception )
                {                    
                }
            mq.Close();
            messageOutput = new message();
            return messageOutput;
        }
    }
}
