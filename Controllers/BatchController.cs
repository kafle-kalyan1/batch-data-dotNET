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

            var query = "SELECT * FROM \"Batch\"";
            var batchData = await connection.QueryAsync<Batch>(query);

            return Ok(batchData);
        }

        [HttpPost]
        public async Task<ActionResult<Batch>> CreateBatch()
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var insertQuery = @"INSERT INTO public.""Batch"" DEFAULT VALUES RETURNING ""id""";

            var newId = await connection.ExecuteScalarAsync<int>(insertQuery);

            var createdBatch = new Batch { id = newId };
            return Ok(createdBatch);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Batch>> DeleteBatch(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var deleteQuery = @"DELETE FROM public.""Batch"" WHERE ""id"" = @Id";
            var parameters = new { Id = id };

            await connection.ExecuteAsync(deleteQuery, parameters);

            return NoContent();
        }

    }
}
