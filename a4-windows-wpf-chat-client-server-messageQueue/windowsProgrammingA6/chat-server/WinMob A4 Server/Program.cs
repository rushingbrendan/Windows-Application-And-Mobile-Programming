/*
	FILE: 			Program.cs
    PROJECT:		PROG 2120 - A05 - IPC

    PROGRAMMER:		Josh Rogers and Brendan Rushing
	FIRST VERSION:	2018-Nov-09
	DESCRIPTION	:
		Runs the chat server
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace WinMob_A4_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server chatServer = new Server();

            chatServer.RunServer();

            Console.WriteLine("Program ended.");
            Console.ReadKey();
        }
    }
}
