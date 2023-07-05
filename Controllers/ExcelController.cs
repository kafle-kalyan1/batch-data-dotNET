using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection.PortableExecutable;
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
            FinalData data1 = new FinalData
            {
                property_name = "PPPPPP",
                report_name = "RRRRRR",
                filters = new List<ReportFilters>
                {
                    new ReportFilters
                    {
                        key = "From Date",
                        value = "2020-01-01"
                    },
                    new ReportFilters
                    {
                        key = "To Date",
                        value = "2020-01-01"
                    }
                },
                primary_table = new TableData
                {
                    header = new TableHeader
                    {
                        value = new List<string> {
                            "Order Date",
                            "Menu Type"
                        }
                    },
                    rows = new List<TableRow> {
                        new TableRow{
                            value=new List<string> {
                                "Kathmandu Foods"
                            },
                            is_bold=true,
                            is_cell_merged=true
                        },
                        new TableRow{
                            value=new List<string> {
                                "Administration"
                            },
                            is_bold=true,
                            is_cell_merged=true
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "2020-01-01",
                                "Food"
                            }
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "2020-01-01",
                                "Food"
                            }
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=true,
                            value=new List<string>{
                                "",
                                "100"
                            }
                        }
                    }
                },
                secondary_table = new List<TableData>
                {

                }

            };



            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using ExcelPackage package = new();
            var worksheet = package.Workbook.Worksheets.Add("User Data");
            var header = worksheet.Cells[2, 3, 2, 6];

            worksheet.View.SplitPanes(4, 5);
            worksheet.View.FreezePanes(5, 1);



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
                    tabless.Value = "Batch: " + data.batch;
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

            //worksheet.Protection.AllowSelectLockedCells = false;

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



        [HttpGet("exportPro")]
        public IActionResult ExportToExcelPro()
        {
            FinalData data1 = new FinalData
            {
                property_name = "Expenses Tracker Pro",
                report_name = "Expenses",
                filters = new List<ReportFilters>
                {
                    new ReportFilters
                    {
                        key = "From Date",
                        value = "04-07-2023"
                    },
                    new ReportFilters
                    {
                        key = "To Date",
                        value = " 05-07-2023"
                    },
                    new ReportFilters
                    {
                        key = "User",
                        value = "User001"
                    },
                    new ReportFilters
                    {
                        key = "Total Days",
                        value = "30"
                    },
                    new ReportFilters
                    {
                        key = "NC Authority",
                        value = "ALL"
                    },
                },
                primary_table = new TableData
                {
                    header = new TableHeader
                    {
                        is_bold = true,
                        value = new List<string> {
                            "Date",
                            "Title",
                            "Category",
                            "Money",
                        }

                    },
                    rows = new List<TableRow> {
                        new TableRow{
                            bgColor="#f4f4f4f",
                            value=new List<string> {
                                "04-07-2023"
                            },
                            is_bold=true,
                            is_cell_merged=true
                        },
                        new TableRow{
                              bgColor="#24132",
                            value=new List<string> {
                                "Transportation"
                            },
                            is_bold=true,
                            is_cell_merged=true
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "04-07-2023",
                                "Ghar jada",
                                "Transportation",
                                "2000",

                            }
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "04-07-2023",
                                "Ghar aauda",
                                "Transportation",
                                "10",
                            }
                        },
                        new TableRow{

                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "04-07-2023",
                                "Uta jada",
                                "Transportation",
                                "30000",

                            }
                        },
                         new TableRow{
                            value=new List<string> {
                                "Food"
                            },
                            bgColor="#f4f4f4f",

                            is_bold=true,
                            is_cell_merged=true
                        },
                           new TableRow
                           {
                               is_cell_merged = false,
                               is_bold = false,
                               value = new List<string>{
                                "04-07-2023",
                                "Khana",
                                "Food",
                                "900",
                            }
                           },
                             new TableRow
                           {
                               is_cell_merged = false,
                               is_bold = false,
                               value = new List<string>{
                                "04-07-2023",
                                "Khaja",
                                "Food",
                                "300",
                            }
                           },


                }
                },
                secondary_table = new List<TableData>
                {
                    
                    new TableData
                    {

                        rows = new List<TableRow> {
                   
                        new TableRow{
                            value=new List<string> {
                                "Summary-1"
                            },
                            is_bold=true,
                            is_cell_merged=true
                        },
                        new TableRow{
                            is_cell_merged = true,
                            is_bold=true,
                            value=new List<string>{
                                "Date",
                                "Total",
                              

                            }
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "04-07-2023",
                                "33240"
                            }
                        },
                        new TableRow{
                            is_cell_merged = false,
                            is_bold=false,
                            value=new List<string>{
                                "04-07-2023",
                                "1340"

                            }
                        }

                }
                    },


                }

            };
            var headerColor = ColorTranslator.FromHtml("#CCCCCC");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage package = new();
            var worksheet = package.Workbook.Worksheets.Add("User Data");


            int row  = 1;
            int col = 1;
            int colCount = data1.primary_table.header.value.Count+4;

            var propName = worksheet.Cells[row, col, row, colCount];
            propName.Value = data1.property_name;
            propName.Merge = true;
            propName.Style.Font.Size = 14;
            propName.Style.Font.Bold = true;
            propName.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            propName.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            row++;

            var repName = worksheet.Cells[row, col, row, colCount];
            repName.Value = data1.report_name;
            repName.Merge = true;
            repName.Style.Font.Size = 11;
            repName.Style.Font.Bold = true;
            repName.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            repName.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            row+=2;






            var filters = worksheet.Cells[row, col];
            var keyval = data1.filters;
            int currentColumn = col;
            filters.Style.Font.Size = 10;

            foreach (var item in keyval)
            {
                filters = worksheet.Cells[row, col];
        
                filters.Style.Font.Bold = true;
                filters.Value = item.key;
                col++;

                //filters.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //filters.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                filters = worksheet.Cells[row, col];  
                filters.Value = item.value;
             

                col+=2;


                if (col >= colCount)
                {
                    col = 1; 
                    row++;  
                    filters = worksheet.Cells[row, col]; 
                }
            }

            col= 3;
            row += 2;
            var tableHeaders = data1.primary_table.header.value;
            var tableLocation = worksheet.Cells[row, col];
            foreach (var item in tableHeaders)
            {
                tableLocation = worksheet.Cells[row, col];

                //Console.ReadLine();
                //Console.WriteLine(item);
                tableLocation.Value = item;
                worksheet.View.FreezePanes(row+1, 1);
                filters.Style.Fill.PatternType = ExcelFillStyle.Solid;
                filters.Style.Fill.BackgroundColor.SetColor(headerColor);

                if (data1.primary_table.header.is_bold)
                {
                    tableLocation.Style.Font.Bold = true;
                }
                col++;
            }
            col = 1;

            var tableBodys = data1.primary_table.rows;
            var tablebodylocation = worksheet.Cells[row, col];

            foreach (var item in tableBodys)
            {
               
                    if (col > colCount)
                    {
                        col = 1;
                        row++;
                    }
                row++;
                tablebodylocation = worksheet.Cells[row, col];

                    if (item.value.Count != data1.primary_table.header.value.Count)
                    {
                    col = 1;
                    for (int i = 0; i < item.value.Count; i++)
                    {
                        tablebodylocation = worksheet.Cells[row, col];

                        tablebodylocation.Value = item.value[i];
                        if (item.is_bold)
                        {
                            tablebodylocation.Style.Font.Bold = true;
                        }
                        if (item.is_cell_merged)
                        {
                            worksheet.Cells[row, col, row, col + colCount - 1].Merge = true;
                            tablebodylocation.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            tablebodylocation.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        if (item.bgColor != null)
                        {
                            tablebodylocation.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            tablebodylocation.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(item.bgColor));
                        }
                    }
                    }
                    else
                    {

                    for (int i = 0; i < item.value.Count; i++)
                    {
                        if (col <= 2 || col >= colCount-1)
                        {
                            col = 3;
                        }
                        tablebodylocation = worksheet.Cells[row, col];
                        tablebodylocation.Value = item.value[i];
                        if (item.is_bold)
                        {
                            tablebodylocation.Style.Font.Bold = true;
                        }
                        if (item.is_cell_merged)
                        {
                            worksheet.Cells[row, col, row, col + 2].Merge = true;
                            tablebodylocation.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            tablebodylocation.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        if (item.bgColor != null)
                        {
                            tablebodylocation.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            tablebodylocation.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(item.bgColor));
                        }

                        col += 1;
                       

                    }


                }
            }














            worksheet.Cells.AutoFitColumns();

            var fileName = "UserData.xlsx";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            package.SaveAs(new FileInfo(filePath));



          //   return Ok(data1.primary_table);
            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


        }
    }
}
