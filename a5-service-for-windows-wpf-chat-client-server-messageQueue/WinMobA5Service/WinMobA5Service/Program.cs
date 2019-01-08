/*
	FILE: 			Program.cs
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
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using loggingClass;

namespace WinMobA5Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServerService()
            };
            ServiceBase.Run(ServicesToRun);
        }

    }
}
