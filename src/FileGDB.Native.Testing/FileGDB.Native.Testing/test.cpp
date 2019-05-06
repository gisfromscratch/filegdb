/*
 * Copyright 2019 Jan Tschada
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

#include "pch.h"

TEST(FileGDB, CreateFileGDB) {
	using namespace FileGDBAPI;
	Geodatabase gdb;
	const std::wstring workspace_path = L"Data/NewlyCreated.gdb";
	if (S_OK == OpenGeodatabase(workspace_path, gdb))
	{
		EXPECT_EQ(S_OK, CloseGeodatabase(gdb));
		EXPECT_EQ(S_OK, DeleteGeodatabase(workspace_path));
	}

	EXPECT_EQ(S_OK, CreateGeodatabase(workspace_path, gdb)) << "Create file geodatabase.";
	EXPECT_EQ(S_OK, CloseGeodatabase(gdb)) << "Close file geodatabase.";
}

TEST(FileGDB, CreateEmptyTable) {
	using namespace FileGDBAPI;
	Geodatabase gdb;
	const std::wstring workspace_path = L"Data/EmptyTable.gdb";
	if (S_OK == OpenGeodatabase(workspace_path, gdb))
	{
		EXPECT_EQ(S_OK, CloseGeodatabase(gdb));
		EXPECT_EQ(S_OK, DeleteGeodatabase(workspace_path));
	}

	EXPECT_EQ(S_OK, CreateGeodatabase(workspace_path, gdb)) << "Create file geodatabase.";

	FieldDef oidFieldDef;
	oidFieldDef.SetAlias(L"OBJECTID");
	oidFieldDef.SetName(L"OID");
	oidFieldDef.SetType(FieldType::fieldTypeOID);
	std::vector<FieldDef> fields;
	fields.push_back(oidFieldDef);
	Table empty_table;
	EXPECT_EQ(S_OK, gdb.CreateTable(L"EmptyTable", fields, L"", empty_table)) << "Create empty table having OID.";
	EXPECT_EQ(S_OK, gdb.CloseTable(empty_table)) << "Close empty table.";

	EXPECT_EQ(S_OK, CloseGeodatabase(gdb)) << "Close file geodatabase.";
}

int main(int argc, wchar_t* argv[])
{
	testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}