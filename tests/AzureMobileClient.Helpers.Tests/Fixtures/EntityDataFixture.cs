using System;
using System.Collections.Generic;
using System.Text;

namespace AzureMobileClient.Helpers.Tests.Fixtures
{
    using Xunit;

    public class EntityDataFixture
    {
        public class MyEntity : EntityData
        {
        }

        [Fact]
        public void EntityData_CompareWithoutIdDoesNotFail()
        {
            var data1=new MyEntity();
            var data2=new MyEntity();

            Assert.True(data1.Equals(data2));
        }
    }
}
