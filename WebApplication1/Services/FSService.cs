using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication1.API.Services;
using WebApplication1.Data.DTOs;
using WebApplication1.Extensions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace WebApplication1.Services
{
    public class FsService : IFSService
    {
        private readonly HttpClient _client;
        private readonly string _connectionString = "connection string value";

        public FsService(HttpClient client)
        {
            _client = client ?? new HttpClient();
            //_client.BaseAddress = new Uri(string.Empty);
        }

        public async Task<IEnumerable<SampleContent>> GetSampleContents()
        {
            List<SampleContent> result = new List<SampleContent>();

            var urlString = @"https://samplerspubcontent.blob.core.windows.net/public/properties.json";

            var request = new HttpRequestMessage(HttpMethod.Get, urlString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                byte[] jsonBytes = await response.Content.ReadAsByteArrayAsync();
                var jsonDoc = JsonDocument.Parse(jsonBytes);
                var root = jsonDoc.RootElement;
                var myStringList = root.GetProperty("properties");

                for (var i = 0; i < myStringList.GetArrayLength(); i++)
                {
                    var address = myStringList[i].GetProperty("address").ToObject<Address>();

                    var sampleContent = new SampleContent();
                    sampleContent.Id = myStringList[i].GetProperty("id").ToString();
                    sampleContent.Address = address;

                    var financialData = myStringList[i].GetProperty("financial");
                    var physicalData = myStringList[i].GetProperty("physical");
                    ;

                    if (financialData.ValueKind.ToString() != "Null")
                    {
                        sampleContent.FinancialData = new Financial();
                        sampleContent.FinancialData.ListPrice = financialData.GetProperty("listPrice").ToString();
                        sampleContent.FinancialData.MonthlyRent = financialData.GetProperty("monthlyRent").ToString();
                    }

                    if (physicalData.ValueKind.ToString() != "Null")
                    {
                        sampleContent.PhysicalData = new Physical();
                        sampleContent.PhysicalData.YearBuilt = financialData.GetProperty("monthlyRent").ToString();
                    }

                    result.Add(sampleContent);
                }

                return result;
            }
        }

        public async Task<string> SaveRecord(SampleContent newRecord)
        {
            int Id = 0;
            var recordInsertQuery =
                @"INSERT INTO SampleData(id
                                        ,YearBuilt
                                        ,ListPrice
                                        ,MonthlyRent
                                        ,GrossYield 
                                        ,LastUpdatedOnUtc,
                                        ,Address1
                                        ,Address2
                                        ,City
                                        ,Country
                                        ,County
                                        ,District
                                        ,State
                                        ,Zip)
                                    VALUES( @YearBuilt
                                            ,@ListPrice
                                            ,@MonthlyRent
                                            ,@GrossYield
                                            ,@SYSDATETIME()
                                            ,@Address1
                                            ,@Address2
                                            ,@City
                                            ,@Country
                                            ,@County
                                            ,@District
                                            ,@State
                                            ,@Zip)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Id = await connection.ExecuteScalarAsync<int>(recordInsertQuery, new
                {
                    ID = newRecord.Id,
                    YearBuilt = newRecord.PhysicalData.YearBuilt,
                    ListPrice = newRecord.FinancialData.ListPrice,
                    MonthlyRent = newRecord.FinancialData.MonthlyRent,
                    GrossYield = newRecord.GrossYield,
                    Address1 = newRecord.Address.address1,
                    Address2 = newRecord.Address.address2,
                    City = newRecord.Address.city,
                    District = newRecord.Address.district,
                    State = newRecord.Address.state,
                    Zip = newRecord.Address.zip
                });
            }

            return Id.ToString();
        }
    }
}
