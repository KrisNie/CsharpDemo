using System;
using System.Data;
using System.Data.SqlClient;
using Collections;
using Microsoft.Extensions.DependencyInjection;
using Services.Finance;

namespace Services
{
    public static class UnbelievableClass
    {
        public static void UnbelievableMethod()
        {
            Utilities.Excel2Json.ConvertExcel2Json();
        }

        #region Dependency Injection Demo

        private static void DependencyInjectionDemo()
        {
            var ba = new CompositionRoot().GetService<IBankAccount>();
            if (ba == null) return;
            ba.Create("Mr. Bryan Walton", 11.99);
            ba.Credit(5.77);
            ba.Debit(11.22);
            Console.WriteLine("Current balance is ${0}", ba.Balance);
        }

        #endregion

        #region Database Access Demo

        private static DataSet GetDataSet()
        {
            var ds = new DataSet();
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = "LOCALHOST", UserID = "sa", Password = "123456Zz",
                    InitialCatalog = "CSI10"
                };

                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                var sql = "SELECT TOP 10 * FROM item_mst;";

                using var dataAdapter = new SqlDataAdapter(sql, connection);
                dataAdapter.Fill(ds);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return ds;
        }

        #endregion

        #region CircularBuffer Demo

        private static void CircularBufferDemo()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2 });
            Console.WriteLine("Initial buffer {0,1,2}:");
            PrintBuffer(buffer);

            for (int i = 3; i < 7; i++)
            {
                buffer.PushBack(i);
            }

            Console.WriteLine("\nAfter adding a 7 elements to a 5 elements capacity buffer:");
            PrintBuffer(buffer);

            buffer.PopFront();
            Console.WriteLine("\nbuffer.PopFront():");
            PrintBuffer(buffer);

            buffer.PopBack();
            Console.WriteLine("\nbuffer.PopBack():");
            PrintBuffer(buffer);

            for (int i = 2; i >= 0; i--)
            {
                buffer.PushFront(i);
            }

            Console.WriteLine("\nbuffer.PushFront() {2,1,0} respectively:");
            PrintBuffer(buffer);

            buffer.Clear();
            Console.WriteLine("\nbuffer.Clear():");
            PrintBuffer(buffer);
        }

        private static void PrintBuffer(CircularBuffer<int> buffer)
        {
            Console.WriteLine($"{{{string.Join(",", buffer.ToArray())}}}");
        }

        #endregion
    }
}