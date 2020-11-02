using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace DATC.Function
{
    public static class DATC_QueueTrigger
    {
        [return: Table("studenti")]
        [FunctionName("DATC_QueueTrigger")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "datc2020_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        
            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}