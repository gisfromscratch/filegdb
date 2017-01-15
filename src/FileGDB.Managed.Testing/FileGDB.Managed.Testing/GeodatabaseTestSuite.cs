/*
 * Copyright 2017 Jan Tschada
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Esri.FileGDB;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace FileGDB.Managed.Testing
{
    [TestClass]
    public class GeodatabaseTestSuite
    {
        private const string GeodatabaseName = @"EmptyGeodatabase.gdb";

        [TestInitialize]
        public void TestIntialize()
        {
            if (Directory.Exists(GeodatabaseName))
            {
                Geodatabase.Delete(GeodatabaseName);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(GeodatabaseName))
            {
                Geodatabase.Delete(GeodatabaseName);
            }
        }

        [TestMethod]
        public void TestCreateAndDeleteGeodatabase()
        {
            using (var gdb = Geodatabase.Create(GeodatabaseName))
            {
                gdb.Close();
            }
        }

        [TestMethod]
        public void TestCreateGeodatabaseAndForcingTableCreateException()
        {
            using (var gdb = Geodatabase.Create(GeodatabaseName))
            {
                const string tableName = @"TestTable";
                var fields = new List<FieldDef>(1);
                const string invalidFieldName = @"1_OID";
                var oidField = new FieldDef { Name = invalidFieldName, Type = FieldType.OID };
                fields.Add(oidField);
                var exceptionRaised = false;
                try
                {
                    using (var table = gdb.CreateTable(tableName, fields.ToArray(), string.Empty))
                    {
                    }
                }
                catch (FileGDBException)
                {
                    // Avoid disposed exception
                    // the table instance is created internally without having a native table handle
                    // because of the raised exception you are not able to access this created table instance
                    // when this pending table instance is finalized after the geodatabase was disposed
                    // a diposed exception is thrown
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    exceptionRaised = true;
                }
                Assert.IsTrue(exceptionRaised, @"A table having an invalid field name should not be created!");

                // Try again
                oidField.Name = @"OID";
                using (var table = gdb.CreateTable(tableName, fields.ToArray(), string.Empty))
                {
                    table.Close();
                }

                gdb.Close();
            }
        }
    }
}
