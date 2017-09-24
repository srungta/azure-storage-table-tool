using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace azure_storage_table_tool
{
    static class Program
    {
        private const string connectionString = "<Replace Connection String here and then delete later>";
        static void Main(string[] args)
        {
            var tableName = "<Insert table name here>";
            var tableClient = new TableHelper(connectionString);

            //Simple message example
            var tableExists = tableClient.CheckIfTableExists(tableName).GetAwaiter().GetResult();
            if (tableExists)
            {
                Console.WriteLine($"A table named '{tableName}' exists.");
            }
            else
            {
                Console.WriteLine($"A table named '{tableName}' does not exist.");
            }

            // Insert a simple entity
            tableClient.AddEntity(tableName, new SampleClass("partitionKey", "rowKey") { MyProperty1 = "property1", MyProperty2 = "property2" }, InsertType.Insert, true).GetAwaiter().GetResult();
        }
    }

    public class SampleClass : TableEntity
    {
        public SampleClass()
        {
        }

        public SampleClass(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
    }
}