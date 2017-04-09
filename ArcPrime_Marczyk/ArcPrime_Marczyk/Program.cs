using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

using ArcPrime_Marczyk.Communication;
using ArcPrime_Marczyk.View;
using ArcPrime_Marczyk.Logging;

namespace ArcPrime_Marczyk
{
    /**
     * IMPORTANT: This application was made as a task for Future Processing internship program 2017.
     * 
     * This is github version of the application. It won't work properly!
     * Connection with the server won't happen, it requires unique account.
     * Also, the server won't be running infinitely.
     */
    static class Program
    {
        private const string EMAIL_ADRESS   = "xgithubversion";
        private const string TOKEN          = "xgithubversion";
        private const string SERVICE_ADRESS = "http://xgithubversion";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CommandExecutor  commandExecutor  = new CommandExecutor(Program.getClientUrlResponse(), EMAIL_ADRESS, TOKEN);
            ArcStateReceiver arcStateReceiver = new ArcStateReceiver(Program.getClientUrlRequest());
            ReportWriter     reportWriter     = new ReportWriter(EMAIL_ADRESS, TOKEN);

            Application.Run(new MainView(commandExecutor, arcStateReceiver, reportWriter));
        }


        private static string getClientUrlRequest()
        {
            string encodedLogin = HttpUtility.UrlEncode(EMAIL_ADRESS);
            return $"{SERVICE_ADRESS}/describe?login={encodedLogin}&token={TOKEN}";
        }

        private static string getClientUrlResponse()
        {
            return $"{SERVICE_ADRESS}/execute";
        }
    }
}
