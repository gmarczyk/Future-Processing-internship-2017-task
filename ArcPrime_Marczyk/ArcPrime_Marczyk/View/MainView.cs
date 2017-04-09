using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ArcPrime_Marczyk.Communication;
using ArcPrime_Marczyk.Model;
using ArcPrime_Marczyk.Logging;

namespace ArcPrime_Marczyk.View
{
    public partial class MainView : Form
    {
        CommandExecutor commandExecutor;
        ArcStateReceiver arcStateReceiver;
        ReportWriter reporter;

        int eventsSoFar = 0;
        string[] postExecutionEvents;
        ArcologyState currentState;

        public MainView(CommandExecutor commandExecutor, ArcStateReceiver arcStateReceiver, ReportWriter reporter)
        {
            InitializeComponent();

            this.commandExecutor = commandExecutor;
            this.arcStateReceiver = arcStateReceiver;
            this.reporter = reporter;

            resetSimulation();
            currentState = arcStateReceiver.GetState();
            updateViewArcState(currentState);
        }

        private void resetSimulation()
        {
            eventsSoFar = 0;
            postExecutionEvents = null;        // instantly filled after executeCommand (2 lines further), even if null, listBox wont fail using it. But if used somewhere else, be aware
            reporter.SetUpNewReport();
            executeCommand(Command.Restart, "");
            resetViewParamsDelta();
        }

        private void executeCommand(Command command, string parameter)
        {
            /** Note for FP recruiter:
              Please check the 6th point in document, I've been thinking about how to make
              this method, contemplations were done. Check why the method is done like it is done.
            **/

            bool wasExecutionSuccessful = commandExecutor.TryExecuting(command, parameter);
            try
            {
                var postExecutionState = arcStateReceiver.GetState();
                updateViewAfterExecuting(postExecutionState);
                reporter.UpdateCurrentReport(postExecutionState, postExecutionEvents, $"{command.ToString()} {parameter}");

                if (postExecutionState.Turn.Equals(25))
                    MessageBox.Show("Experiment has ended");
                else if (postExecutionState.IsTerminated)
                    MessageBox.Show("Experiment is terminated! Look what you've done you...");
                else if (!wasExecutionSuccessful)
                    MessageBox.Show("Execution was unsuccessful - Houston, we have a problem!");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Receiving of the arcology state failed: {ex.Message}");
            }
        }

        private void updateViewAfterExecuting(ArcologyState postExecState)
        {
            postExecutionEvents = getPostExecutionEvents(postExecState);
            updateViewArcState(postExecState);
            updateViewParamsDelta(postExecState);
        }

        private string[] getPostExecutionEvents(ArcologyState arcState)
        {
            return arcState.Events.Skip(eventsSoFar).ToArray<string>();
        }

        private void updateViewArcState(ArcologyState arcState)
        {
            labelFoodQuantityVal.Text       =  Math.Round(arcState.FoodQuantity, 3).ToString();
            labelFoodCapacityVal.Text       =  Math.Round(arcState.FoodCapacity, 2).ToString();
            labelPopulationVal.Text         =  Math.Round(arcState.Population, 2).ToString();
            labelPopulationCapacityVal.Text =  Math.Round(arcState.PopulationCapacity, 2).ToString();
            labelProductionVal.Text         =  Math.Round(arcState.Production, 2).ToString();
            labelArcologyIntegrityVal.Text  =  Math.Round(arcState.ArcologyIntegrity, 2).ToString();
            labelSocialCapitalVal.Text      =  Math.Round(arcState.SocialCapital, 2).ToString();
            labelWasteVal.Text              =  Math.Round(arcState.Waste, 2).ToString();
            labelTotalScoreVal.Text         =  Math.Round(arcState.TotalScore, 5).ToString();
            labelExperimentScoreVal.Text    =  Math.Round(arcState.ExperimentScore, 5).ToString();
            labelEventScoreVal.Text         =  Math.Round(arcState.EventScore, 5).ToString();
            labelTurnVal.Text               =  arcState.Turn.ToString();
            labelTerminatedVal.Text         =  arcState.IsTerminated.ToString();
            labelShouldRestartVal.Text      =  arcState.ShouldIRestartExperimentAndCry.ToString();

            listBoxRunes.DataSource  = arcState.NehoRunes;
            listBoxAllEvents.DataSource = arcState.Events;
            listBoxPostExecutionEvents.DataSource = postExecutionEvents;                    

            eventsSoFar = arcState.Events.Count();
        }

        private void updateViewParamsDelta(ArcologyState postExecState)
        {
            labelDeltaArcologyIntegrity.Text = Math.Round((postExecState.ArcologyIntegrity - currentState.ArcologyIntegrity), 2).ToString();
            labelDeltaFoodQuantity.Text      = Math.Round((postExecState.FoodQuantity - currentState.FoodQuantity), 2).ToString();
            labelDeltaPopulation.Text        = Math.Round((postExecState.Population - currentState.Population), 2).ToString();
            labelDeltaProduction.Text        = Math.Round((postExecState.Production - currentState.Production), 2).ToString();
            labelDeltaSocialCapital.Text     = Math.Round((postExecState.SocialCapital - currentState.SocialCapital), 2).ToString();
            labelDeltaWaste.Text             = Math.Round((postExecState.Waste - currentState.Waste), 2).ToString();

            currentState = postExecState;
        }

        private void resetViewParamsDelta()
        {
            labelDeltaArcologyIntegrity.Text = "";
            labelDeltaFoodQuantity.Text = "";
            labelDeltaPopulation.Text = "";
            labelDeltaProduction.Text = "";
            labelDeltaSocialCapital.Text = "";
            labelDeltaWaste.Text = "";
        }

        private void buttonUpdateState_Click(object sender, EventArgs e)
        {
            try
            {
                updateViewArcState(arcStateReceiver.GetState());
            } 
            catch (Exception ex)
            {
                MessageBox.Show($"Receiving of the arcology state failed: {ex.Message}");
            }
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            if (arcStateReceiver.GetState().IsTerminated)
            {
                MessageBox.Show("Experiment is terminated!");
                return;
            }

            if (!tryGettingCheckedRadioButton(groupBoxRadioCommandList, out var checkedButton))
            {
                MessageBox.Show("No command was choosen!");
                return;
            }

            var command = getCommandFromRadioButton(checkedButton);
            var parameter = textBoxCommandParameter.Text;
            if (!command.IsParamValid(parameter))
            {
                MessageBox.Show("Invalid parameter was given for such command!");
                return;
            }

            executeCommand(command, parameter);
        }

        private bool tryGettingCheckedRadioButton(Control container, out RadioButton radioButton)
        {
            radioButton = container.Controls.OfType<RadioButton>()
                                          .FirstOrDefault(r => r.Checked); // Some linq magic

            if (radioButton == null)
                return false;
            else
                return true;
        }
        
        private Command getCommandFromRadioButton(RadioButton radio)
        {
            switch(radio.Text)
            {
                case "Import food":
                    return Command.ImportFood;
                case "Produce":
                    return Command.Produce;
                case "Propaganda":
                    return Command.Propaganda;
                case "Clean":
                    return Command.Clean;
                case "Build Arcology":
                    return Command.BuildArcology;
                case "Expand population capacity":
                    return Command.ExpandPopulationCapacity;
                case "Expand food capacity":
                    return Command.ExpandFoodCapacity;
                case "We are ready":
                    return Command.WeAreReady;
                default:
                    throw new Exception("No such RadioButton.Text matches command list! " +
                                        "Check if the radio button has proper text and if such command even exists");
            }
        }

        private void buttonResetSimulation_Click(object sender, EventArgs e)
        {
            resetSimulation();
        }     
    }
}
