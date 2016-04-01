namespace UKService
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
            this.UKServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.UKServiceBabyTesting = new System.ServiceProcess.ServiceInstaller();
            // 
            // UKServiceInstaller
            // 
            this.UKServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.UKServiceInstaller.Password = null;
            this.UKServiceInstaller.Username = null;
            // 
            // UKServiceBabyTesting
            // 
            this.UKServiceBabyTesting.Description = "UKServiceBabyTesting";
            this.UKServiceBabyTesting.DisplayName = "UKServiceBabyTesing";
            this.UKServiceBabyTesting.ServiceName = "OmnimarkAmazonService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.UKServiceInstaller,
            this.UKServiceBabyTesting});

        }

        #endregion

        public System.ServiceProcess.ServiceProcessInstaller UKServiceInstaller;
        public System.ServiceProcess.ServiceInstaller UKServiceBabyTesting;
    }
}