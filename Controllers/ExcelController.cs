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

            using ExcelPackage package = new();
            var worksheet = package.Workbook.Worksheets.Add("User Data");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Hobbies";

            var row = 2;
            foreach (var data in dataList)
            {
                worksheet.Cells[row, 1].Value = data.id;
                worksheet.Cells[row, 2].Value = data.name;
                worksheet.Cells[row, 3].Value = data.gender;
                worksheet.Cells[row, 4].Value = string.Join(", ", data.hobbies);
                row++;
            }
            worksheet.Cells.AutoFitColumns();

            var fileName = "UserData.CSV";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            package.SaveAs(new FileInfo(filePath));

            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/ok")]
        public IActionResult Export1()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using ExcelPackage package = new();
            var worksheet = package.Workbook.Worksheets.Add("User Data");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Hobbies";
            worksheet.Cells[2, 1].Value = 1;
            worksheet.Cells[2, 2].Value = "USERR";
            worksheet.Cells[2, 3].Value = "Female";
            worksheet.Cells[2, 4].Value = "Eating";

            worksheet.Cells.AutoFitColumns();

            var fileName = "UserDataa.CSV";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            package.SaveAs(new FileInfo(filePath));

            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
