namespace VerifyCloudConnection
{
	using System;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DcpChatIntegrationHelper.Common;
	using Skyline.DataMiner.DcpChatIntegrationHelper.Teams;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public static class VerifyCloudConnection
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public static void Run(IEngine engine)
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

		private static void RunSafe(IEngine engine)
		{
			TestResult result = new TestResult();
			try
			{
				var chatIntegrationHelper = new ChatIntegrationHelperBuilder().Build();
				var identity = chatIntegrationHelper.GetDataMinerServicesDmsIdentity();

				result.ParameterName = "Verify Cloud Connection";
				result.DmsId = Convert.ToString(identity.DmsId);
				result.ReceivedValue = "Connected";
			}
			catch (ChatIntegrationException chatEx)
			{
				result.ParameterName = "Verify Cloud Connection";
				result.DmsId = "N/A";
				result.ReceivedValue = $"{chatEx}";
			}
			catch (Exception ex)
			{
				result.ParameterName = "Verify Cloud Connection";
				result.DmsId = "N/A";
				result.ReceivedValue = $"{ex}";
			}
			finally
			{
				engine.AddScriptOutput("result", JsonConvert.SerializeObject(result));
			}
		}
	}
}
