using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Models;

namespace L05
{
    class Program
    {
        private static CloudTable studentsTable;
        private static CloudTableClient studentsTableClient;
        private static CloudTable metricsTable;
        private static CloudTableClient metricsTableClient;
        static void Main(string[] args)
        {
            Task.Run(async () => { await init(); })
                .GetAwaiter()
                .GetResult();
        }

        static async Task init()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;" +
            "AccountName=datc2020;" +
            "AccountKey=kH1WiapUmbzNFe1XjWF36ahwMX7sa7hEz+1bKnYCSZv8dVwqFeyN6krfLHdRp03lPAH6HP79ZJWN0yBV89gusQ==;" + // invalid access key
            "EndpointSuffix=core.windows.net";              

            var studentsAccount = CloudStorageAccount.Parse(storageConnectionString);
            studentsTableClient = studentsAccount.CreateCloudTableClient();
            studentsTable = studentsTableClient.GetTableReference("Students");
            await studentsTable.CreateIfNotExistsAsync();
            await displayStudents(storageConnectionString);
        }

        private static async Task<List<StudentEntity>> getAllStudents()
        {
            List<StudentEntity> listOfStudents = new List<StudentEntity>();
            TableQuery<StudentEntity> studentsQuery = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;

            do {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(studentsQuery, token);
                token = resultSegment.ContinuationToken;
                listOfStudents.AddRange(resultSegment.Results);
            }while(token != null);

            return listOfStudents;
        }

        private static async Task displayStudents(string storageConnectionString) {
            var listOfStudents = await getAllStudents();

            var metricsAccount = CloudStorageAccount.Parse(storageConnectionString);
            metricsTableClient = metricsAccount.CreateCloudTableClient();
            metricsTable = metricsTableClient.GetTableReference("StudentsMetrics");
            await metricsTable.CreateIfNotExistsAsync();

            int uptStudents = 0, uvtStudents = 0;

            foreach (StudentEntity std in listOfStudents) {
                if (std.PartitionKey == "UPT")
                    uptStudents += 1;
                else
                    uvtStudents += 1;
            }

            var timeSpanUPT = DateTime.Now.ToString("o");
            Statistics uptStats = new Statistics("UPT", timeSpanUPT);
            uptStats.numberOfStudents_ = uptStudents;
            var insertUptMetrics = TableOperation.Insert(uptStats);
            await metricsTable.ExecuteAsync(insertUptMetrics);

            var timeSpanUVT = DateTime.Now.ToString("o");
            Statistics uvtStats = new Statistics("UVT", timeSpanUVT);
            uvtStats.numberOfStudents_ = uvtStudents;
            var insertUvtMetrics = TableOperation.Insert(uvtStats);
            await metricsTable.ExecuteAsync(insertUvtMetrics);

            var timeSpanGENERAL = DateTime.Now.ToString("o");
            Statistics generalStats = new Statistics("General", timeSpanGENERAL);
            generalStats.numberOfStudents_ = uptStudents + uvtStudents;
            var insertGeneralMetrics = TableOperation.Insert(generalStats);
            await metricsTable.ExecuteAsync(insertGeneralMetrics);
        }
    }
}
