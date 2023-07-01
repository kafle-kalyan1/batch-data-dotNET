using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace batch_data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        [HttpPost("export")]
        public IActionResult ExportToExcel(List<Data> dataList)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("User Data");
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Hobbies";
                worksheet.Cells[2, 1].Value = 1;
                worksheet.Cells[2, 2].Value = "Ishan";
                worksheet.Cells[2, 3].Value = "Female";
                worksheet.Cells[2, 4].Value = "Eating Gu.";

                // var row = 2;
                // foreach (var data in dataList)
                // {
                //     worksheet.Cells[row, 1].Value = data.id;
                //     worksheet.Cells[row, 2].Value = data.name;
                //     worksheet.Cells[row, 3].Value = data.gender;
                //     worksheet.Cells[row, 4].Value = string.Join(", ", data.hobbies);
                //     row++;
                // }

                // Auto-fit columns
                // worksheet.Cells.AutoFitColumns();

                var fileName = "UserData_Ishan.xlsx";
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    package.SaveAs(fileStream);
                }

                return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
