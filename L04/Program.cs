using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Models;

namespace L04
{
    class Program
    {

        private static CloudTable studentsTable;
        private static CloudTableClient tableClient;
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
            "AccountKey=b7ZB4AvbY6+I6ncZx/NIKBl2bMI7/QvfJ9Ye4yoiym2NopoW7dBldx4PNOAj9yDiPx9scbgee9lGqrPxqv6FwQ==;" + // invalid access key
            "EndpointSuffix=core.windows.net";              

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();
            studentsTable = tableClient.GetTableReference("Students");

            await studentsTable.CreateIfNotExistsAsync();
            await addStudent();
            await editStudent();
            await deleteStudent();
            await displayStudents();
        }

        private static async Task addStudent()
        {
            System.Console.WriteLine("university:");
            string university = Console.ReadLine();
            System.Console.WriteLine("cnp:");
            string cnp = Console.ReadLine();
            System.Console.WriteLine("firstName:");
            string firstName = Console.ReadLine();
            System.Console.WriteLine("lastName:");
            string lastName = Console.ReadLine();
            System.Console.WriteLine("faculty:");
            string faculty = Console.ReadLine();
            System.Console.WriteLine("year:");
            string year = Console.ReadLine();

            var student = new StudentEntity(university, cnp);
            student.firstName = firstName;
            student.lastName = lastName;
            student.faculty = faculty;
            student.year = Convert.ToInt32(year);

            var insertOperation = TableOperation.Insert(student);
            await studentsTable.ExecuteAsync(insertOperation);
        }

        private static async Task editStudent()
        {
            System.Console.WriteLine("university:");
            string university = Console.ReadLine();
            System.Console.WriteLine("cnp:");
            string cnp = Console.ReadLine();

            var getStudent = TableOperation.Retrieve<StudentEntity>(university, cnp);
            TableResult result = await studentsTable.ExecuteAsync(getStudent);
            var student = (StudentEntity)result.Result;

            if (student != null) 
            {
                System.Console.WriteLine("firstName:");
                student.firstName = Console.ReadLine();
                System.Console.WriteLine("lastName:");
                student.lastName = Console.ReadLine();
                System.Console.WriteLine("faculty:");
                student.faculty = Console.ReadLine();
                System.Console.WriteLine("year:");
                student.year = Convert.ToInt32(Console.ReadLine());

                var editOperation = TableOperation.Replace(student);
                await studentsTable.ExecuteAsync(editOperation);
            }
            else
            {
                Console.WriteLine("Record does not exist!");
            }
        }

        private static async Task deleteStudent()
        {
            System.Console.WriteLine("university:");
            string university = Console.ReadLine();
            System.Console.WriteLine("cnp:");
            string cnp = Console.ReadLine();

            var getStudent = TableOperation.Retrieve<StudentEntity>(university, cnp);
            TableResult result = await studentsTable.ExecuteAsync(getStudent);
            var student = (StudentEntity)result.Result;

            if (student != null)
            {
                var deleteOperation = TableOperation.Delete(student);
                await studentsTable.ExecuteAsync(deleteOperation);

            }
            else
            {
                Console.WriteLine("Record does not exist!");
            }
        }

        private static async Task displayStudents()
        {
            TableQuery<StudentEntity> displayStudents = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;

            do {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(displayStudents, token);
                token = resultSegment.ContinuationToken;

                foreach(StudentEntity student in resultSegment)
                {
                    Console.WriteLine("{0}\t{1}\t{2} {3}\t{4}\t{5}", student.PartitionKey, student.RowKey, student.firstName, student.lastName
                    , student.faculty, student.year);
                }
            }while(token != null);
        }
    }
}
