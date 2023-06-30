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

        [HttpPost("add")]
        public async Task<ActionResult<List<Data>>> CreateData(List<Data> dataList)
        {
            Console.WriteLine(dataList);
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            var insertedDataList = new List<Data>();
            foreach (var data in dataList)
            {
                var insertQuery = @"
            INSERT INTO public.data(
	                name, gender, batch, hobbies)
	            VALUES (@Name, @Gender, @Batch, @Hobbies)
	            RETURNING *";

                var insertedData = await connection.QueryFirstOrDefaultAsync<Data>(insertQuery, new
                {
                    Name = data.name,
                    Gender = data.gender,
                    Batch = data.batch,
                    Hobbies = data.hobbies
                });
                insertedDataList.Add(insertedData);
            }

            return Ok(insertedDataList);
        }
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<List<Data>>> EditData(int id, List<Data> dataList)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            var updatedDataList = new List<Data>();
            foreach (var data in dataList)
            {
                if (data.id == 0)
                {
                    var insertQuery = @"
                INSERT INTO public.data(
                    name, gender, batch, hobbies)
                VALUES (@Name, @Gender, @Batch, @Hobbies)
                RETURNING *";

                    var insertedData = await connection.QueryFirstOrDefaultAsync<Data>(insertQuery, new
                    {
                        Name = data.name,
                        Gender = data.gender,
                        Batch = id,
                        Hobbies = data.hobbies
                    });

                    updatedDataList.Add(insertedData);
                }
                else
                {
                   
                    var updateQuery = @"
                UPDATE public.data
                SET name = @Name, gender = @Gender, batch = @Batch, hobbies = @Hobbies
                WHERE id = @Id
                RETURNING *";

                    var updatedData = await connection.QueryFirstOrDefaultAsync<Data>(updateQuery, new
                    {
                        Name = data.name,
                        Gender = data.gender,
                        Batch = id,
                        Hobbies = data.hobbies,
                        Id = data.id
                    });

                    updatedDataList.Add(updatedData);
                }
            }

            return Ok(updatedDataList);
        }

        [HttpGet("view/{id}")]
        public async Task<ActionResult<Data>> GetBatchDataById(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = @"
        SELECT * FROM public.data WHERE batch = @Batch ";

            var dataBatch = await connection.QueryAsync<Data>(query, new { Batch = id });

            return Ok(dataBatch);
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Data>> GetDataForEdit(List<Data> dataList)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = @"
        UPDATE public.data
                SET name = @Name, gender = @Gender, hobbies = @Hobbies
                WHERE id = @Id
                RETURNING *";

            var dataBatch = await connection.QueryAsync<Data>(query, new {
                //Batch = dataList.id,
                //Name = dataList.name

            });

            return Ok(dataBatch);
        }


    }
}
