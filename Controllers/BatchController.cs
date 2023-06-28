using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Dapper;


namespace batch_data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : Controller
    {
        private readonly IConfiguration _config;

        public BatchController(IConfiguration config)
        {
            this._config = config;
        }

        [HttpGet]
            public async Task<ActionResult<List<Batch>>> GetAllBatch()
            {
                using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

                var query = @"
                SELECT * from ""Batch""";


                var batchData = await connection.QueryAsync<Batch>(query);

                return Ok(batchData);
            

             }
    }
}
