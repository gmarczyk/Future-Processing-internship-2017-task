using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ArcPrime_Marczyk.Model;

namespace ArcPrime_Marczyk.Logging
{
    public class ReportWriter
    {
        string pathToCurrentReport;
        string email;
        string token;

        public ReportWriter(string email, string token)
        {
            this.email = email;
            this.token = token;
        }

        public void SetUpNewReport() // %PATH TO .EXE DIRECTORY%/Reports month_day_hour/minute_second.txt
        {
            var currentMonthDayHourDirectory = "Reports " + DateTime.Now.ToString("MM_dd_HH");  
            System.IO.Directory.CreateDirectory(currentMonthDayHourDirectory);

            var reportName = DateTime.Now.ToString("mm_ss") + ".txt"; 

            pathToCurrentReport = AppDomain.CurrentDomain.BaseDirectory                
                                + currentMonthDayHourDirectory + "\\" + reportName;

            writeHeaderToReport(pathToCurrentReport);
        }

        private void writeHeaderToReport(string pathToReport)
        {
            StreamWriter file = new StreamWriter(pathToReport, true);
            file.WriteLine("Report created on: " + DateTime.Now.ToString() + ", by ArcPrime_Marczyk");
            file.WriteLine("Email: " + email);
            file.WriteLine("Token: " + token);
            file.WriteLine("");
            file.Close();
        }

        public void UpdateCurrentReport(ArcologyState state, string[] events, string actionPerformed)
        {
            StreamWriter file = new StreamWriter(pathToCurrentReport,true);

            file.WriteLine("========================================================================");
            file.WriteLine("");

            file.WriteLine($"Turn: [{state.Turn}] Action performed: [{actionPerformed}]");
            file.WriteLine($"IsTerminated: [{state.IsTerminated}] Should restart: [{state.ShouldIRestartExperimentAndCry}]");
            file.WriteLine("");

            file.WriteLine(String.Format("{0,-18} [{1}]", "Total score:", state.TotalScore));
            file.WriteLine(String.Format("{0,-18} [{1}]", "Experiment score:", state.ExperimentScore));
            file.WriteLine(String.Format("{0,-18} [{1}]", "Event score:", state.EventScore)); ;
            file.WriteLine("");

            file.WriteLine("State of archology: ");
            file.WriteLine(String.Format("   - {0,-20} {1}", "Food quantity:", state.FoodQuantity));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Food capacity:", state.FoodCapacity));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Waste:", state.Waste));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Social capital:",state.SocialCapital));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Production:", state.Production));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Arcology integrity:", state.ArcologyIntegrity));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Population:", state.Population));
            file.WriteLine(String.Format("   - {0,-20} {1}", "Population capacity:", state.PopulationCapacity));
            file.WriteLine("");
            

            file.WriteLine("Resulted in events: ");
            foreach (var singleEvent in events)
            {
                file.WriteLine("   - " + singleEvent);
            }
            file.WriteLine("");

            file.WriteLine("Neho runes: ");
            if(state.NehoRunes != null)
            {
                foreach (var rune in state.NehoRunes)
                {
                    file.WriteLine($"[{rune}]");
                }
            }
            file.WriteLine("");

            file.Close();
        }
    }
}
