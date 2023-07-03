using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using batch_data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Net.WebRequestMethods;

namespace BatchData.Controllers
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
            var header = worksheet.Cells[2, 3, 2, 6];

            worksheet.View.SplitPanes(4,5);
            worksheet.View.FreezePanes(5,1);



           header.Value = "Welcome! Here is the List of Data";
           header.Merge = true;
           header.Style.Font.Size = 20;
           header.Style.Font.Bold = true;
           header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
           header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           header.Style.Fill.PatternType = ExcelFillStyle.Solid;
           header.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            worksheet.Cells[4, 3].Value = "ID";
            worksheet.Cells[4, 4].Value = "Name";
            worksheet.Cells[4, 5].Value = "Gender";
            worksheet.Cells[4, 6].Value = "Hobbies";
            var headerRange = worksheet.Cells[4, 3, 4, 6];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.Size = 14;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);


            var row = 4;
            int batchh = 0;
            foreach (var data in dataList)
            {
                if (batchh != data.batch)
                {
                    row++;
                    var tabless = worksheet.Cells[row, 3, row, 6];
                    tabless.Value = "Batch: "+data.batch;
                    tabless.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    tabless.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCCCCC"));
                    tabless.Style.Font.Bold = true;
                    tabless.Style.Font.Size = 12;
                    tabless.Merge = true;
                    tabless.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    tabless.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    row++;
                }
                var datasss = worksheet.Cells[row, 3, row, 6];
                worksheet.Cells[row, 3].Value = data.id;
                datasss.Style.Font.Size = 12;
                datasss.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                datasss.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                datasss.Style.Fill.PatternType = ExcelFillStyle.Solid;
                datasss.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                datasss.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                datasss.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#0000"));
                datasss.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                datasss.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#0000"));
                datasss.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                datasss.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#0000"));  
                datasss.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                datasss.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#0000"));
                worksheet.Cells[row, 4].Value = data.name;
                worksheet.Cells[row, 5].Value = data.gender;
                worksheet.Cells[row, 6].Value = string.Join(", ", data.hobbies);
                batchh = data.batch;
                row++;
            }

        
            worksheet.Cells.AutoFitColumns();

          //  worksheet.Protection.AllowSelectLockedCells = false;

            var fileName = "UserData.xlsx";
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
            worksheet.Cells[2, 2].Value = "USER";
            worksheet.Cells[2, 3].Value = "Female";
            worksheet.Cells[2, 4].Value = "Eating";


            worksheet.Cells.AutoFitColumns();

            var fileName = "UserData.xlsx";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            package.SaveAs(new FileInfo(filePath));

            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
