using ExcelDataReader;
using GemBox.Spreadsheet;
using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Helper
{
    public class ExcelImportHelper
    {

        private readonly IItemRepository ItemRepository;
        private readonly IConfiguration configuration;
        public ExcelImportHelper(IConfiguration _configuration, IItemRepository _ItemRepository)
        {
            configuration = _configuration;
            ItemRepository = _ItemRepository;
        }

        //public System.Data.DataTable ReadExcelFileData(string fileName)
        //{
        //    System.Data.DataTable dtexcel = new System.Data.DataTable();
        //    try
        //    {
        //        string filePath = configuration.GetSection("InRetailConfig").GetSection("ExcelFilePath").Value;
        //        fileName = filePath + fileName;
        //        string conn = string.Empty;
        //        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        //        var workbook = ExcelFile.Load(fileName);
        //        var worksheet = workbook.Worksheets[0];

        //        dtexcel = worksheet.CreateDataTable(new CreateDataTableOptions()
        //        {
        //            ColumnHeaders = true,
        //            StartRow = 1,
        //            NumberOfColumns = 2,
        //            NumberOfRows = worksheet.Rows.Count - 1,
        //            Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture
        //        });
        //    }
        //    catch(Exception ex)
        //    { 
        //    }
        //    return dtexcel;
        //}

        //public System.Data.DataTable ReadExcelFileData(string fileName)
        //{
        //    string[] fileData = fileName.Split('.');
        //    string fileExt = fileData[fileData.Length - 1];
        //    string filePath = configuration.GetSection("InRetailConfig").GetSection("ExcelFilePath").Value;
        //    fileName = filePath + fileName;
        //    string conn = string.Empty;
        //    System.Data.DataTable dtexcel = new System.Data.DataTable();
        //    if (fileExt.CompareTo(".xls") == 0)
        //        conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
        //    else
        //        conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
        //    using (OleDbConnection con = new OleDbConnection(conn))
        //    {
        //        try
        //        {
        //            OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
        //            oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
        //        }
        //        catch { }
        //    }
        //    return dtexcel;
        //}

        public System.Data.DataTable ReadExcelFileData(string fileName)
        {
            System.Data.DataTable dtexcel = new System.Data.DataTable();

            try
            {
                string filePath = configuration.GetSection("InRetailConfig").GetSection("ExcelFilePath").Value;
                fileName = filePath + fileName;
                string conn = string.Empty;
                dtexcel.Columns.Add(ConstHelper.CategoryName);
                dtexcel.Columns.Add(ConstHelper.ItemName);

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        while (reader.Read()) //Each row of the file
                        {
                            var row = dtexcel.NewRow();
                            row[ConstHelper.CategoryName] = reader.GetValue(0).ToString();
                            row[ConstHelper.ItemName] = reader.GetValue(1).ToString();
                            dtexcel.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
            return dtexcel;
        }

        bool VerifyColumns(System.Data.DataTable dataTable)
        {
            bool flag = true;
            try
            {
                var categoryName = dataTable.Rows[0][ConstHelper.CategoryName];
                var itemName = dataTable.Rows[0][ConstHelper.ItemName];

                if (categoryName == null || itemName == null)
                    flag = false;
            }
            catch (Exception e)
            {
                flag = false;
            }
            return flag;
        }

        int GetCategoryCount(System.Data.DataTable dataTable)
        {
            int count = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                string categoryName = row[ConstHelper.CategoryName].ToString();
                if (!string.IsNullOrEmpty(categoryName) && !categoryName.Equals(ConstHelper.CategoryName))
                {
                    count++;
                    break;
                }
            }

            return count;
        }

        async Task<Item> AddCategory(string categoryName, int branchId)
        {
            Item category = await ItemRepository.GetCatgoryByNameAsync(categoryName, branchId);
            if (category == null)
            {
                category = new Item();
                category.Name = categoryName;
                category.Code = await ItemRepository.GetItemCode(branchId);
                category.Type = ConstHelper.TYPE_CATEGORY;
                category.CreatedOn = DateTime.Now;
                category.CreatedBy = ConstHelper.APP_ADMIN_USER_ID;
                category.IsActive = true;
                category.UpdatedOn = DateTime.Now;
                category.UpdatedBy = ConstHelper.APP_ADMIN_USER_ID;
                category.ParentId = 0;
                category.BranchId = branchId;
                category = await ItemRepository.AddAsync(category);
            }

            return category;
        }

        async Task<Item> AddItem(string itemName, int categoryId, int branchId)
        {
            Item item = await ItemRepository.GetItemByNameAsync(itemName, branchId);
            if (item == null)
            {
                item = new Item();
                item.Name = itemName;
                item.Type = ConstHelper.TYPE_PRODUCT;
                item.Code = await ItemRepository.GetItemCode(branchId);
                item.CreatedOn = DateTime.Now;
                item.CreatedBy = ConstHelper.APP_ADMIN_USER_ID;
                item.IsActive = true;
                item.UpdatedOn = DateTime.Now;
                item.UpdatedBy = ConstHelper.APP_ADMIN_USER_ID;
                item.ParentId = categoryId;
                item.BranchId = branchId;
                item = await ItemRepository.AddAsync(item);
            }

            return item;
        }

        public async Task<int> ImportExceFileData(string fileName, int branchId)
        {
            int flag = ConstHelper.ImportSuccesful;

            try
            {
                List<int> a = new List<int>();
                a.FindIndex(x => x == 1);
                System.Data.DataTable dataTable = ReadExcelFileData(fileName);
                if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 1)
                {
                    flag = ConstHelper.ImpEmptyFile;
                    return flag;
                }
                bool columnFlag = VerifyColumns(dataTable);
                if (columnFlag)
                {
                    int categoryId = 0;
                    int categoryCount = GetCategoryCount(dataTable);
                    if (categoryCount > 0)
                    {
                        if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 1)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string categoryName = row[ConstHelper.CategoryName].ToString();
                                string itemName = row[ConstHelper.ItemName].ToString();

                                if (!string.IsNullOrEmpty(categoryName) && !categoryName.Equals(ConstHelper.CategoryName))
                                {
                                    Item category = await AddCategory(categoryName, branchId);
                                    categoryId = category.Id;
                                }

                                if (!string.IsNullOrEmpty(itemName) && !itemName.Equals(ConstHelper.ItemName))
                                {
                                    Item item = await AddItem(itemName, categoryId, branchId);
                                }
                            }
                        }
                    }
                    else
                    { 
                        flag = ConstHelper.ImpZeroCategory;
                    }
                }
                else
                {
                    flag = ConstHelper.ImpInvalidColumns;
                }
            }
            catch (Exception ex)
            {
                flag = ConstHelper.ImportError;
            }

            return flag;
        }
    }
}
