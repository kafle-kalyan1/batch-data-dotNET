using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;

namespace batch_data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dataController : ControllerBase
    {
        private readonly IConfiguration _config;

        public dataController(IConfiguration config)
        {
            this._config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Data>>> GetAllData()
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = @"
                SELECT d.*
                FROM data d
                JOIN ""Batch"" b ON d.batch = b.id
ORDER BY b.id DESC";


            var datas = await connection.QueryAsync<Data>(query);

            return Ok(datas);
        }




}




}
