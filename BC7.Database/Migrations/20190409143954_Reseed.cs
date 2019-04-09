using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Reseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("181a0761-ed8f-4c3b-993e-f69e6390cfbf"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("1c7f1963-4cf6-4430-897b-dd79521e6755"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("975da247-9300-4afe-a70b-fdf0d342d295"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("bf75e510-5f20-43ed-b693-e5d9c5fefcbb"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("d445b122-047b-444c-9238-c61fb2c28dda"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("6d338237-3001-45b4-bed9-4f45356cb238"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"));

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("d56a72b9-494f-49ce-a8b5-990a227ef075"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 4, 9, 14, 39, 54, 461, DateTimeKind.Utc).AddTicks(8934), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("83295631-a9e8-4405-9027-6acfb5d1a59c"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 4, 9, 14, 39, 54, 462, DateTimeKind.Utc).AddTicks(181), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("1371b998-0d76-4669-9cad-904330fe7ca9"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 4, 9, 14, 39, 54, 462, DateTimeKind.Utc).AddTicks(193), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("e7785b1e-3d98-4b2c-8fff-e6ad0b213f1a"), true, "LoginRoot1", "111111", new Guid("d56a72b9-494f-49ce-a8b5-990a227ef075"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("0732561e-f258-42dd-a103-6561bf898d0a"), true, "LoginRoot2", "222222", new Guid("83295631-a9e8-4405-9027-6acfb5d1a59c"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("9b56bc1e-b4e6-4dbc-834c-ae623b37fbbe"), true, "LoginRoot3", "333333", new Guid("1371b998-0d76-4669-9cad-904330fe7ca9"), null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("67c0e361-ddd7-413b-86a4-fcf11230fd91"), 0, 1, 0, null, 14, new Guid("e7785b1e-3d98-4b2c-8fff-e6ad0b213f1a") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("2f814a3b-ef74-4f61-8e12-0836fee67eff"), 1, 2, 0, new Guid("67c0e361-ddd7-413b-86a4-fcf11230fd91"), 7, new Guid("0732561e-f258-42dd-a103-6561bf898d0a") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("727212aa-48be-4a02-a8b5-af6d6bba5b3c"), 1, 8, 0, new Guid("67c0e361-ddd7-413b-86a4-fcf11230fd91"), 13, new Guid("9b56bc1e-b4e6-4dbc-834c-ae623b37fbbe") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("6aa80c12-db7d-4910-b3b8-b606684c8979"), 2, 3, 0, new Guid("2f814a3b-ef74-4f61-8e12-0836fee67eff"), 4, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("232b377a-4dc8-49eb-9e1a-e0546066fb9e"), 2, 5, 0, new Guid("2f814a3b-ef74-4f61-8e12-0836fee67eff"), 6, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("defa67e9-9cd1-40ef-9049-9cd43042c028"), 2, 9, 0, new Guid("727212aa-48be-4a02-a8b5-af6d6bba5b3c"), 10, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("8d819e4c-053c-47e9-b58a-33325ccad788"), 2, 11, 0, new Guid("727212aa-48be-4a02-a8b5-af6d6bba5b3c"), 12, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("232b377a-4dc8-49eb-9e1a-e0546066fb9e"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("6aa80c12-db7d-4910-b3b8-b606684c8979"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("8d819e4c-053c-47e9-b58a-33325ccad788"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("defa67e9-9cd1-40ef-9049-9cd43042c028"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("2f814a3b-ef74-4f61-8e12-0836fee67eff"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("727212aa-48be-4a02-a8b5-af6d6bba5b3c"));

            migrationBuilder.DeleteData(
                table: "MatrixPositions",
                keyColumn: "Id",
                keyValue: new Guid("67c0e361-ddd7-413b-86a4-fcf11230fd91"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("0732561e-f258-42dd-a103-6561bf898d0a"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("9b56bc1e-b4e6-4dbc-834c-ae623b37fbbe"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("1371b998-0d76-4669-9cad-904330fe7ca9"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("83295631-a9e8-4405-9027-6acfb5d1a59c"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("e7785b1e-3d98-4b2c-8fff-e6ad0b213f1a"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("d56a72b9-494f-49ce-a8b5-990a227ef075"));

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", "EmailRoot1", "FirstNameRoot1", "", true, "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("6d338237-3001-45b4-bed9-4f45356cb238"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", "EmailRoot2", "FirstNameRoot2", "", true, "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", "EmailRoot3", "FirstNameRoot3", "", true, "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a"), true, "LoginRoot1", "111111", new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf"), true, "LoginRoot2", "222222", new Guid("6d338237-3001-45b4-bed9-4f45356cb238"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945"), true, "LoginRoot3", "333333", new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"), null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 0, 1, 0, null, 14, new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 1, 2, 0, new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 7, new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 1, 8, 0, new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 13, new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("181a0761-ed8f-4c3b-993e-f69e6390cfbf"), 2, 3, 0, new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 4, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("bf75e510-5f20-43ed-b693-e5d9c5fefcbb"), 2, 5, 0, new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 6, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("975da247-9300-4afe-a70b-fdf0d342d295"), 2, 9, 0, new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 10, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("1c7f1963-4cf6-4430-897b-dd79521e6755"), 2, 11, 0, new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 12, null });
        }
    }
}
