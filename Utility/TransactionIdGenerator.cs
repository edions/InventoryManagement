using System.Collections.Generic;
using System;

namespace InventoryApp.Utility
{
    class TransactionIdGenerator
    {
        private readonly HashSet<string> generatedIds = new HashSet<string>();

        public string GenerateTransactionId()
        {
            string uniqueTransactionId;

            do
            {
                uniqueTransactionId = Guid.NewGuid().ToString();
            }
            while (generatedIds.Contains(uniqueTransactionId));

            generatedIds.Add(uniqueTransactionId);

            return uniqueTransactionId;
        }
    }
}
