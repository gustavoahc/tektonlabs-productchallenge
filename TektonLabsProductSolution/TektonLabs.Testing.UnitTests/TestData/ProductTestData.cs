using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Testing.UnitTests.TestData
{
    public static class ProductTestData
    {
        public static Product GetProduct()
        {
            return new Product
            {
                ProductId = 1,
                Name = "Product",
                Price = 1,
                Status = 1,
                Stock = 1
            };
        }

        public static Product GetUpdatedProduct()
        {
            return new Product
            {
                ProductId = 1,
                Name = "Product 1 updated",
                Price = 1,
                Status = 1,
                Stock = 1
            };
        }

        public static Product GetInvalidProduct()
        {
            return new Product
            {
                Name = "",
                Price = 1,
                Status = 1,
                Stock = 1
            };
        }

        public static List<StatusData> GetStatusDataList()
        {
            List<StatusData> statusData = new List<StatusData>();
            statusData.Add(new StatusData { Id = 0, Name = "Inactive" });
            statusData.Add(new StatusData { Id = 1, Name = "Active" });

            return statusData;
        }
    }
}
