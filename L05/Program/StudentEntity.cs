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

        public string firstName_ {get; set;}
        public string lastName_ {get; set;}
        public string faculty_ {get; set;}
        public int year_ {get; set;}
    }
}