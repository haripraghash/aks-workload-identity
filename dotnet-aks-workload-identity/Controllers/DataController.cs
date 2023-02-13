using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace dotnet_aks_workload_identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public DataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // GET: api/Data
        [HttpGet]
        public async Task<IEnumerable<string?>> Get()
        {
            var defaultAzureCredential = new DefaultAzureCredential();
            var token = await defaultAzureCredential.GetTokenAsync( new TokenRequestContext(new string[]{"https://database.windows.net/.default"}));

            await using var sqlConnection =
                new SqlConnection(this._configuration.GetConnectionString("DefaultConnection"))
                {
                    //AccessToken = token.Token
                };
            var query = "Select count(*) from SalesLT.Customer";
            await using var sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                await sqlConnection.OpenAsync();
                var result = await sqlCommand.ExecuteScalarAsync();
                return new string?[] {result?.ToString()};

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
