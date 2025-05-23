namespace Verify_Cloud_Connection
{
	using System;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DcpChatIntegrationHelper.Common;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private void RunSafe(IEngine engine)
		{
			var result = new TestResult();
			const string parameterName = "Verify Cloud Connection";

			try
			{
				var chatIntegrationHelper = new ChatIntegrationHelperBuilder().Build();
				var identity = chatIntegrationHelper.GetDataMinerServicesDmsIdentity();

				result.ParameterName = parameterName;
				result.DmsId = Convert.ToString(identity.DmsId);
				result.ReceivedValue = "Connected";
			}
			catch (Exception ex)
			{
				result.ParameterName = parameterName;
				result.DmsId = "N/A";
				result.ReceivedValue = ex.Message;
			}
			finally
			{
				engine.AddScriptOutput("result", JsonConvert.SerializeObject(result));
			}
		}
	}
}
