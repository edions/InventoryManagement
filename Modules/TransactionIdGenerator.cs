using System.Collections.Generic;
using System;

namespace InventoryApp.Modules
{
    class TransactionIdGenerator
    {
        private readonly HashSet<int> generatedIds = new HashSet<int>();

        public int GenerateTransactionId()
        {
            int uniqueTransactionId;

            do
            {
                uniqueTransactionId = GenerateUniqueId();
            }
            while (generatedIds.Contains(uniqueTransactionId));

            generatedIds.Add(uniqueTransactionId);

            return uniqueTransactionId;
        }
    }
}
