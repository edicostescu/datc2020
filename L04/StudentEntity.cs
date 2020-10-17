using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string university, string cnp)
        {
            this.PartitionKey = university;
            this.RowKey = cnp;
        }
        public StudentEntity() {}

        public string firstName {get; set;}
        public string lastName {get; set;}
        public string faculty {get; set;}
        public int year {get; set;}
        

    }
}