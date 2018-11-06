using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ExpertManagment.Classes
{
    public class MathModuleController
    {
        public static string rumModule() {
            Process myProcess = new Process();
            string parameters = "";
            AppDomain domain = AppDomain.CreateDomain("/");
            string base_dir = domain.BaseDirectory;
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c pq.exe");
            try
            {
                processInfo.UseShellExecute = false;
                processInfo.WorkingDirectory = base_dir + "\\Mixing\\Example\\";

                myProcess.StartInfo = processInfo;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.StandardInput.WriteLine("/c exit");
                string output = myProcess.StandardOutput.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
