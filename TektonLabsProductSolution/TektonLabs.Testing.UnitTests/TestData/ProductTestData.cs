using TektonLabs.Core.Domain.Common;

namespace TektonLabs.Testing.UnitTests.TestData
{
    public static class ProductTestData
    {
        public static List<StatusData> GetStatusData()
        {
            List<StatusData> statusData = new List<StatusData>();
            statusData.Add(new StatusData { Id = 0, Name = "Inactive" });
            statusData.Add(new StatusData { Id = 1, Name = "Active" });

            return statusData;
        }
    }
}
