using System.Collections.Generic;
using ProductManagement.API.Entities;

namespace ProductManagement.Tests.TestData
{
    class WeightTypeTestData
    {
        private IEnumerable<WeightType> _weightTypes;
        public WeightTypeTestData()
        {
            SetWeightTypesTestData();
        }

        public IEnumerable<WeightType> GetWeightTypes() => _weightTypes;

        private void SetWeightTypesTestData()
        {
            _weightTypes = new List<WeightType>
            {
                new WeightType
                {
                    Id = 1,
                    Name = "weightType1"
                },
                new WeightType
                {
                    Id = 2,
                    Name = "weightType2"
                }
            };
        }
    }
}
