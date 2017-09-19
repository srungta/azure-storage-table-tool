using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace azure_storage_table_tool
{
    public class TableHelper
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudTableClient tableClient;
        private CloudTable table;
     
        public TableHelper(string connectionString)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task<bool> CheckIfTableExists(string tableName)
        {
            table = tableClient.GetTableReference(tableName);
            return await table.ExistsAsync();
        }

        public async Task AddEntity<T>(string tableName, T entity, InsertType insertType, bool createIfNotExist = false) where T : ITableEntity
        {
            table = tableClient.GetTableReference(tableName);
            if (!table.Exists() && !createIfNotExist) throw new Exception();
            await table.CreateIfNotExistsAsync();
            switch (insertType)
            {
                case InsertType.Insert:
                    await table.ExecuteAsync(TableOperation.Insert(entity));
                    break;
                case InsertType.InsertOrMerge:
                    await table.ExecuteAsync(TableOperation.InsertOrMerge(entity));
                    break;
                case InsertType.InsertOrReplace:
                    await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}
