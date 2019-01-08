/*
	FILE: 			ServerService.Designer.cs
    PROJECT:		PROG 2120 - A05 - IPC

    PROGRAMMER:		Josh Rogers and Brendan Rushing
	FIRST VERSION:	2018-Nov-09
	DESCRIPTION	:
		Runs the server side for a TCP MessageQueue based chat program.
        Listens for new chat clients, then creates a thread that will deal
        with various messages from multiple clients at once
*/

namespace WinMobA5Service
{
    partial class ServerService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ServerService
            // 
            this.CanPauseAndContinue = true;
            this.ServiceName = "ServerService";

        }

        #endregion
    }
}
