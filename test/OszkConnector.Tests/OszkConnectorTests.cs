using OszkConnector;
using OszkConnector.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class OszkConnectorTests
    {
        [Fact]
        public async void MekCatalog() 
        {
            var client = new Client();
            var result = await client.FindAudioBook("gardonyi");
            Assert.NotNull(result);
        }
    }
}
