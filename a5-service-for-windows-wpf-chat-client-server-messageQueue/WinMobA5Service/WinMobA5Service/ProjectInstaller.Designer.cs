namespace WinMobA5Service
{
    partial class ProjectInstaller
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
            this.MyServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.MyServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // MyServiceProcessInstaller
            // 
            this.MyServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.MyServiceProcessInstaller.Password = null;
            this.MyServiceProcessInstaller.Username = null;
            // 
            // MyServiceInstaller
            // 
            this.MyServiceInstaller.Description = "Chat Server Service for WinMob A5";
            this.MyServiceInstaller.DisplayName = "Chat Server Service";
            this.MyServiceInstaller.ServiceName = "ServerService";
            this.MyServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.MyServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.MyServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MyServiceProcessInstaller,
            this.MyServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MyServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller MyServiceInstaller;
    }
}