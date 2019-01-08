/*
	FILE: 			ServerService.cs
    PROJECT:		PROG 2120 - A05 - IPC

    PROGRAMMER:		Josh Rogers and Brendan Rushing
	FIRST VERSION:	2018-Nov-25
	DESCRIPTION	:
		Runs the server side for a TCP MessageQueue based chat program.
        Listens for new chat clients, then creates a thread that will deal
        with various messages from multiple clients at once

        This is run as a service. The service is named "Chat Server".

        The service can be started, paused, resumed and stopped.

        All new client connections, client disconnections and server status reports are
        logged to a "chat-server.log" text file in the location of the service.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using loggingClass;

namespace WinMobA5Service
{
    public partial class ServerService : ServiceBase
    {
        Thread serverThread = null;
        Server chatServer = null;

        public ServerService()
        {
            InitializeComponent();

            chatServer = new Server();
            ThreadStart st = new ThreadStart(chatServer.RunServer);
            serverThread = new Thread(st);
        }

        //method called when service has been started
        protected override void OnStart(string[] args)
        {
            //start server
            try
            {
                Logging.NewLogEvent("Server has been started.");
                serverThread.Start();
            }
            catch (Exception e)
            {
                Logging.NewLogEvent($"Server Exception: {e}");
            }
        }

        /// <summary>
        /// Method called when service has been stopped
        /// </summary>
        protected override void OnStop()
        {
            //stop server
            try
            {
                Logging.NewLogEvent("Server has been stopped.");
                serverThread.Abort();
            }
            catch (Exception e)
            {
                Logging.NewLogEvent($"Server Exception: {e}");
            }
        }

        /// <summary>
        /// Method called when service is paused
        /// </summary>
        protected override void OnPause()
        {
            //log that server is paused
            try
            {
                serverThread.Suspend();
                Logging.NewLogEvent(@"Server has been paused.");
            }
            catch (Exception e)
            {
                Logging.NewLogEvent($"Server Exception: {e}");
            }

        }

        /// <summary>
        /// Method called when service is resumed
        /// </summary>
        protected override void OnContinue()
        {
            //log that server is resumed
            try
            {
                Logging.NewLogEvent(@"Server has been resumed.");
                serverThread.Resume();
            }
            catch (Exception e)
            {
                Logging.NewLogEvent($"Server Exception: {e}");
            }           
        }
    }
}
