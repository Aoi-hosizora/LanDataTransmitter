using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Globalization;
using System.Threading;

namespace LanDataTransmitter {

    class Program : WindowsFormsApplicationBase {

        [STAThread]
        static void Main(string[] args) {
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new MainForm());
            var app = new Program();
            app.Run(args);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Program() {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            // IsSingleInstance = true;
            EnableVisualStyles = true;
            SaveMySettingsOnExit = true;
            ShutdownStyle = ShutdownMode.AfterAllFormsClose;
        }

        [System.Diagnostics.DebuggerStepThrough]
        protected override void OnCreateMainForm() {
            MainForm = InitForm.Instance;
        }
    }
}
