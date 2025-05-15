namespace VerifyCloudConnection
{
	internal class TestResult
	{
		public string ParameterName { get; set; }

		public string DisplayName { get; set; }

		public string ElementName { get; set; }

		public string DmaName { get; set; }

		public string DmsId { get; set; }

		public string ReceivedValue { get; set; }

		public string ExpectedValue { get; set; }

		public bool Success { get; set; }
	}
}
