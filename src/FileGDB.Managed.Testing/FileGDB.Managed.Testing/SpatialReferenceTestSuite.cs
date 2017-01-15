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

namespace FileGDB.Managed.Testing
{
    [TestClass]
    public class SpatialReferenceTestSuite
    {
        [TestMethod]
        public void TestQueryingSpatialReferences()
        {
            const int wgs84Srid = 4326;
            var wgsFound = false;
            using (var geographicInfos = SpatialReferenceInfo.GeographicSpatialReference())
            {
                var geographicInfoCount = 0;
                foreach (var geographicInfo in geographicInfos)
                {
                    var name = geographicInfo.srname;
                    Assert.IsNotNull(name, @"The name of a spatial reference must not be null!");
                    if (wgs84Srid == geographicInfo.auth_srid)
                    {
                        wgsFound = true;
                    }
                    geographicInfoCount++;
                }
                Assert.IsTrue(0 < geographicInfoCount, @"At least one geographic spatial reference should exists!");
            }
            Assert.IsTrue(wgsFound, @"WGS84 spatial reference was not found!");

            using (var projectedInfos = SpatialReferenceInfo.ProjectedSpatialReference())
            {
                var projectedInfoCount = 0;
                foreach (var projectedInfo in projectedInfos)
                {
                    var name = projectedInfo.srname;
                    Assert.IsNotNull(name, @"The name of a spatial reference must not be null!");
                    projectedInfoCount++;
                }
                Assert.IsTrue(0 < projectedInfoCount, @"At least one projected spatial reference should exists!");
            }
        }
    }
}
