using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    AuditEntryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EntitySetName = table.Column<string>(maxLength: 255, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.AuditEntryID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PaymentId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    AmountToPay = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Status = table.Column<string>(nullable: false),
                    PaymentFor = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullTicketNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    Hash = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    BtcWalletAddress = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    IsMembershipFeePaid = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryProperties",
                columns: table => new
                {
                    AuditEntryPropertyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuditEntryID = table.Column<int>(nullable: false),
                    PropertyName = table.Column<string>(maxLength: 255, nullable: true),
                    RelationName = table.Column<string>(maxLength: 255, nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryProperties", x => x.AuditEntryPropertyID);
                    table.ForeignKey(
                        name: "FK_AuditEntryProperties_AuditEntries_AuditEntryID",
                        column: x => x.AuditEntryID,
                        principalTable: "AuditEntries",
                        principalColumn: "AuditEntryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_UserAccountsData_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserAccountsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMultiAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserAccountDataId = table.Column<Guid>(nullable: false),
                    SponsorId = table.Column<Guid>(nullable: true),
                    MultiAccountName = table.Column<string>(nullable: true),
                    RefLink = table.Column<string>(nullable: true),
                    IsMainAccount = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMultiAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserMultiAccounts_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserAccountsData_UserAccountDataId",
                        column: x => x.UserAccountDataId,
                        principalTable: "UserAccountsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatrixPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserMultiAccountId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    MatrixLevel = table.Column<int>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    Right = table.Column<int>(nullable: false),
                    DepthLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrixPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatrixPositions_UserMultiAccounts_UserMultiAccountId",
                        column: x => x.UserMultiAccountId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[,]
                {
                    { new Guid("7408a3e1-6f6f-49e6-8812-a4bccd1f2c9a"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1951), 3, 4, 0, new Guid("5cb0ee5a-e892-426b-b4a0-6eb8a68fff3f"), 5, null },
                    { new Guid("c8456d59-f31a-426c-80e6-d45274f505d5"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2568), 2, 11, 6, new Guid("fdd7fca0-63d2-4fbc-aed3-066c81da3262"), 12, null },
                    { new Guid("be45b1fc-8649-4ef6-9296-b6e785b21fb1"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2565), 2, 9, 6, new Guid("fdd7fca0-63d2-4fbc-aed3-066c81da3262"), 10, null },
                    { new Guid("6c28d050-e499-4908-873a-24d69b717d19"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2557), 2, 5, 6, new Guid("2da184da-69e7-430a-b303-2e478e55aeed"), 6, null },
                    { new Guid("75e8f7b6-7a14-4323-9a7d-23909251d80d"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2554), 2, 3, 6, new Guid("2da184da-69e7-430a-b303-2e478e55aeed"), 4, null },
                    { new Guid("fc91662d-e282-4529-b9f5-0b4e968da928"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2530), 2, 11, 5, new Guid("8f17ca0f-3438-4f54-b42c-e4fb38599786"), 12, null },
                    { new Guid("a463d564-63ab-4080-9f96-a87f6fbaac50"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2530), 2, 9, 5, new Guid("8f17ca0f-3438-4f54-b42c-e4fb38599786"), 10, null },
                    { new Guid("9b153ec7-5aee-4871-9872-4e047fd5263e"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2527), 2, 5, 5, new Guid("ae83af06-06fc-43c2-80b3-ef8deb73b689"), 6, null },
                    { new Guid("0a2949ca-6bf1-4a41-935d-06f40c258892"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2522), 2, 3, 5, new Guid("ae83af06-06fc-43c2-80b3-ef8deb73b689"), 4, null },
                    { new Guid("9385719f-7be6-4f43-8ef0-8f865752ac54"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2501), 2, 11, 4, new Guid("6b09c107-e332-4245-be21-a43c3a4496fe"), 12, null },
                    { new Guid("20b76e7e-d8cf-4f0e-abae-d7f54015d121"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2498), 2, 9, 4, new Guid("6b09c107-e332-4245-be21-a43c3a4496fe"), 10, null },
                    { new Guid("7a502e73-5d16-4bd0-8946-f835761663f9"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2495), 2, 5, 4, new Guid("443e45dd-eeea-4886-a72e-aae6f64b339f"), 6, null },
                    { new Guid("2dccbb21-75da-478c-9e50-0d66d207cbef"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2495), 2, 3, 4, new Guid("443e45dd-eeea-4886-a72e-aae6f64b339f"), 4, null },
                    { new Guid("46c9af90-3595-478e-9aba-f14aebc978be"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2422), 2, 11, 3, new Guid("33ff46a1-fb5d-4634-b5a5-86de8c84f6a1"), 12, null },
                    { new Guid("69b6e0c1-1f3c-417d-b19a-40d617c43753"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2419), 2, 9, 3, new Guid("33ff46a1-fb5d-4634-b5a5-86de8c84f6a1"), 10, null },
                    { new Guid("ffd650af-99a8-4103-b3d9-3b0c9682f4ba"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2416), 2, 5, 3, new Guid("9b22206e-ba7d-48a3-be72-26b7f6074db3"), 6, null },
                    { new Guid("ab1bd571-25f4-4f09-8ce3-154c2df8efac"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2416), 2, 3, 3, new Guid("9b22206e-ba7d-48a3-be72-26b7f6074db3"), 4, null },
                    { new Guid("afd2bf2c-06e5-4e39-80a1-c413f48a550b"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2390), 2, 11, 2, new Guid("7b400f63-441d-40cf-9bf9-00d55a6677b1"), 12, null },
                    { new Guid("bca10fe3-59e3-4c89-9a9a-c2471e01bd27"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1951), 3, 6, 0, new Guid("5cb0ee5a-e892-426b-b4a0-6eb8a68fff3f"), 7, null },
                    { new Guid("9322fc0f-b204-4d52-a2f5-aefe556b6e77"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1954), 3, 10, 0, new Guid("44247d80-7a01-4ba5-b7eb-91ea2fe9df86"), 11, null },
                    { new Guid("2b5e23b5-cd92-48e3-bbab-fd1309de9612"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1957), 3, 12, 0, new Guid("44247d80-7a01-4ba5-b7eb-91ea2fe9df86"), 13, null },
                    { new Guid("8ef18845-399b-45e4-9f79-cd7ce9a51880"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1957), 3, 18, 0, new Guid("27c9473f-77a4-4b8b-aba8-d4f974d0e864"), 19, null },
                    { new Guid("98c6ce9e-6d37-4f79-84b5-6f72f3bffbf3"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1960), 3, 20, 0, new Guid("27c9473f-77a4-4b8b-aba8-d4f974d0e864"), 21, null },
                    { new Guid("984781d1-e1aa-4d3d-af82-376cd2807c8b"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1963), 3, 24, 0, new Guid("ee94cf3e-199b-408e-a770-fa7f161e34f6"), 25, null },
                    { new Guid("801965bd-e13b-4078-a1c2-d00002ce5570"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1963), 3, 26, 0, new Guid("ee94cf3e-199b-408e-a770-fa7f161e34f6"), 27, null },
                    { new Guid("a464e75c-f6a5-4888-baba-92a6ba7f8a50"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2337), 2, 5, 1, new Guid("e80dab1b-52fb-4a82-8597-b385ab6e9b07"), 6, null },
                    { new Guid("ca744662-cf8e-4131-9ba0-52a575bc9de2"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2340), 2, 9, 1, new Guid("5640c90b-70ea-41d9-a361-f2cddc9bbd9b"), 10, null },
                    { new Guid("04a9e3ce-d632-4b3c-ae95-f163acad0957"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2340), 2, 11, 1, new Guid("5640c90b-70ea-41d9-a361-f2cddc9bbd9b"), 12, null },
                    { new Guid("3e7095fd-8d15-42ad-b22c-499cc51bd0cc"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2381), 2, 3, 2, new Guid("f31fc5a8-6f26-437f-8906-04d35caf90cf"), 4, null },
                    { new Guid("43d4ad62-830e-4e27-a346-776e1f939de2"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2384), 2, 5, 2, new Guid("f31fc5a8-6f26-437f-8906-04d35caf90cf"), 6, null },
                    { new Guid("6b3b8b8b-c494-45ed-88eb-9792c334e213"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2387), 2, 9, 2, new Guid("7b400f63-441d-40cf-9bf9-00d55a6677b1"), 10, null },
                    { new Guid("b435eef8-44f6-4145-9c5d-8a5761d4c877"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2337), 2, 3, 1, new Guid("e80dab1b-52fb-4a82-8597-b385ab6e9b07"), 4, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("d89d7a45-91f4-4347-8b65-f46cf61dbe5d"), "BtcWalletAddressRoot", "CityRoot", "CountryRoot", new DateTime(2019, 6, 30, 19, 47, 50, 971, DateTimeKind.Utc).AddTicks(8397), "root@bitclub7.com", "FirstNameRoot", "Sc8cdO+purS95CSscawE5gUtZ1n2073wEcExUdlFJvyykbZ8MrmB9JATz4p/gHK5IKb2QdZCdLxiwV1NmvE5haDHbD+G51OPBOxq5Bx9C9nwgdXtm+8gohkPvK+SyDtPIUiazZGsbiMP9GItdff8Tx1sgf0CqBw3s1WOsfPxZB7kCa6oLGlfME4pqiNiH12z4tO56iaDWmzxqV7wfRpJ1VwvNBFY+MCCLoRQVKQTRokrjZr7kV9V5fSqGOMUoM+ZfoxKEboMJdkQN5DKnuzE+/HaSVqKxrn0Y2DCCrwJLGmqdkeQSh4C4FJsyl39K/iuIC0bVohUOhdcxlSNOfW/GA==", true, "LastNameRoot", "root", "Root", "MtJ8oklAP5Q=", "StreetRoot", "ZipCodeRoot" },
                    { new Guid("0de42c7f-bc1c-4734-9a95-14ae590073b3"), "BtcWalletAddressAdmin", "CityAdmin", "CountryAdmin", new DateTime(2019, 6, 30, 19, 47, 51, 31, DateTimeKind.Utc).AddTicks(3611), "admin@bitclub7.com", "FirstNameAdmin", "QoI0a3MYeg/G8onsLCVElQfdy7byynXnpBs/GFWbDdFlz3Tnhecywq+ejsnBBgn3O/zOaJVDugVc4iGOV5t4ovOo7Vkqzuw5GpC+u82p1Hob19O+mjtk7g5gTzyyJnI3an+DuHZxC7Y4CWhO0/zVjLsxu5Fsi6s/dsLxdqVn2gx9wkf7T8dLQrzQWDwmuAP/zUcym7NMvbptat+dPxIH5nKq6r8QWwsQPFSuFYWsH7rrKtzRwYK2Y/1kZOlGrTyH+/tYlJnp9GCFMWch7fTX39fn4NA34sw9HHWhLGeRmGACsz5OXPuaESv0NqtabglJeU1js/U2jB38dScMyC5kKg==", true, "LastNameAdmin", "admin", "Admin", "a8sYtCTk5eY=", "StreetAdmin", "ZipCodeAdmin" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[,]
                {
                    { new Guid("f7283501-3c65-473f-b2c5-84b094c522a6"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(2203), true, "root-001", null, null, new Guid("d89d7a45-91f4-4347-8b65-f46cf61dbe5d") },
                    { new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3728), false, "root-002", null, null, new Guid("d89d7a45-91f4-4347-8b65-f46cf61dbe5d") },
                    { new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3739), false, "root-003", null, null, new Guid("d89d7a45-91f4-4347-8b65-f46cf61dbe5d") },
                    { new Guid("eb8c3fa0-4fb9-47ce-bd7e-92f5976f1c64"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3742), true, "admin-001", null, null, new Guid("0de42c7f-bc1c-4734-9a95-14ae590073b3") },
                    { new Guid("20b2dad2-b2be-42af-af47-afd0e936634f"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3751), false, "admin-002", null, null, new Guid("0de42c7f-bc1c-4734-9a95-14ae590073b3") },
                    { new Guid("577e391c-df4e-42cc-a5f2-c0111e8d2c83"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3754), false, "admin-003", null, null, new Guid("0de42c7f-bc1c-4734-9a95-14ae590073b3") },
                    { new Guid("ddf2f3ec-a989-418a-8460-2fbcb9281b1b"), new DateTime(2019, 6, 30, 19, 47, 51, 87, DateTimeKind.Utc).AddTicks(3763), false, "admin-004", null, null, new Guid("0de42c7f-bc1c-4734-9a95-14ae590073b3") }
                });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[,]
                {
                    { new Guid("5a5af626-cf3d-40c7-b05b-94da55551fff"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1335), 0, 1, 0, null, 30, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("44247d80-7a01-4ba5-b7eb-91ea2fe9df86"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1936), 2, 9, 0, new Guid("3dfb5c5d-af7e-4df0-a332-04b2893d23ab"), 14, new Guid("20b2dad2-b2be-42af-af47-afd0e936634f") },
                    { new Guid("5cb0ee5a-e892-426b-b4a0-6eb8a68fff3f"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1934), 2, 3, 0, new Guid("3dfb5c5d-af7e-4df0-a332-04b2893d23ab"), 8, new Guid("eb8c3fa0-4fb9-47ce-bd7e-92f5976f1c64") },
                    { new Guid("fdd7fca0-63d2-4fbc-aed3-066c81da3262"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2551), 1, 8, 6, new Guid("559717ea-163a-4f88-bfea-cfdce35bdc29"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("8f17ca0f-3438-4f54-b42c-e4fb38599786"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2519), 1, 8, 5, new Guid("907b592a-cc92-440c-a3d7-a851f0636023"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("6b09c107-e332-4245-be21-a43c3a4496fe"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2486), 1, 8, 4, new Guid("0df78f75-566f-4e02-87e2-8e030f1fb3c9"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("33ff46a1-fb5d-4634-b5a5-86de8c84f6a1"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2413), 1, 8, 3, new Guid("8633e62c-f439-408c-b3a7-09b98e12077b"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("7b400f63-441d-40cf-9bf9-00d55a6677b1"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2381), 1, 8, 2, new Guid("cc3f900a-b2f4-4b6f-b6b2-caa17af265ca"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("5640c90b-70ea-41d9-a361-f2cddc9bbd9b"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2334), 1, 8, 1, new Guid("6ea13f12-4f0f-4efd-876e-78566edce08f"), 13, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("1acb5125-97e2-4fe8-85d8-aeb260640f5f"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1931), 1, 16, 0, new Guid("5a5af626-cf3d-40c7-b05b-94da55551fff"), 29, new Guid("ff63a56b-7de8-4746-8c79-6e775d53582b") },
                    { new Guid("2da184da-69e7-430a-b303-2e478e55aeed"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2548), 1, 2, 6, new Guid("559717ea-163a-4f88-bfea-cfdce35bdc29"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("27c9473f-77a4-4b8b-aba8-d4f974d0e864"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1939), 2, 17, 0, new Guid("1acb5125-97e2-4fe8-85d8-aeb260640f5f"), 22, new Guid("577e391c-df4e-42cc-a5f2-c0111e8d2c83") },
                    { new Guid("ae83af06-06fc-43c2-80b3-ef8deb73b689"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2519), 1, 2, 5, new Guid("907b592a-cc92-440c-a3d7-a851f0636023"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("9b22206e-ba7d-48a3-be72-26b7f6074db3"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2405), 1, 2, 3, new Guid("8633e62c-f439-408c-b3a7-09b98e12077b"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("f31fc5a8-6f26-437f-8906-04d35caf90cf"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2378), 1, 2, 2, new Guid("cc3f900a-b2f4-4b6f-b6b2-caa17af265ca"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("e80dab1b-52fb-4a82-8597-b385ab6e9b07"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2331), 1, 2, 1, new Guid("6ea13f12-4f0f-4efd-876e-78566edce08f"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("3dfb5c5d-af7e-4df0-a332-04b2893d23ab"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1919), 1, 2, 0, new Guid("5a5af626-cf3d-40c7-b05b-94da55551fff"), 15, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("559717ea-163a-4f88-bfea-cfdce35bdc29"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2545), 0, 1, 6, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("907b592a-cc92-440c-a3d7-a851f0636023"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2516), 0, 1, 5, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("0df78f75-566f-4e02-87e2-8e030f1fb3c9"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2437), 0, 1, 4, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("8633e62c-f439-408c-b3a7-09b98e12077b"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2405), 0, 1, 3, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("cc3f900a-b2f4-4b6f-b6b2-caa17af265ca"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2369), 0, 1, 2, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("6ea13f12-4f0f-4efd-876e-78566edce08f"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2328), 0, 1, 1, null, 14, new Guid("f7283501-3c65-473f-b2c5-84b094c522a6") },
                    { new Guid("443e45dd-eeea-4886-a72e-aae6f64b339f"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(2437), 1, 2, 4, new Guid("0df78f75-566f-4e02-87e2-8e030f1fb3c9"), 7, new Guid("2c06de2f-a710-4642-8903-ab6b7c57b5f5") },
                    { new Guid("ee94cf3e-199b-408e-a770-fa7f161e34f6"), new DateTime(2019, 6, 30, 19, 47, 51, 88, DateTimeKind.Utc).AddTicks(1942), 2, 23, 0, new Guid("1acb5125-97e2-4fe8-85d8-aeb260640f5f"), 28, new Guid("ddf2f3ec-a989-418a-8460-2fbcb9281b1b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatorId",
                table: "Articles",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryProperties_AuditEntryID",
                table: "AuditEntryProperties",
                column: "AuditEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_MatrixPositions_UserMultiAccountId",
                table: "MatrixPositions",
                column: "UserMultiAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountsData_Email",
                table: "UserAccountsData",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountsData_Login",
                table: "UserAccountsData",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMultiAccounts_SponsorId",
                table: "UserMultiAccounts",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMultiAccounts_UserAccountDataId",
                table: "UserMultiAccounts",
                column: "UserAccountDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "AuditEntryProperties");

            migrationBuilder.DropTable(
                name: "MatrixPositions");

            migrationBuilder.DropTable(
                name: "PaymentHistories");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DropTable(
                name: "UserMultiAccounts");

            migrationBuilder.DropTable(
                name: "UserAccountsData");
        }
    }
}
