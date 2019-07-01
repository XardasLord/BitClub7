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
                    { new Guid("2daf2a6f-7cec-4ff3-b15d-5c8356031d04"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2897), 3, 4, 0, new Guid("4d87f944-4a53-4431-abee-0a64e1d8a0f4"), 5, null },
                    { new Guid("7d871840-f354-4ec8-be1c-86958874c70f"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3444), 2, 11, 6, new Guid("0191e80e-66e7-49da-ad48-0ae26717c0aa"), 12, null },
                    { new Guid("f7b96740-38c7-4c18-8172-13e350b783fc"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3444), 2, 9, 6, new Guid("0191e80e-66e7-49da-ad48-0ae26717c0aa"), 10, null },
                    { new Guid("868c959a-3d3b-4f98-9ad1-1afdcbc3e692"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3441), 2, 5, 6, new Guid("70b21a08-2d7c-4afe-9dc1-79e365880cd7"), 6, null },
                    { new Guid("fbe104b5-364d-435f-b7c3-c79a6b013cd8"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3406), 2, 3, 6, new Guid("70b21a08-2d7c-4afe-9dc1-79e365880cd7"), 4, null },
                    { new Guid("2672defa-e166-4c9b-b9b4-6b559473143b"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3389), 2, 11, 5, new Guid("1b2c7f23-962c-43e3-9367-54df4f17b7e0"), 12, null },
                    { new Guid("607cc017-54e8-478a-9ef5-e92278aa870a"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3386), 2, 9, 5, new Guid("1b2c7f23-962c-43e3-9367-54df4f17b7e0"), 10, null },
                    { new Guid("9a348e77-da5e-4f09-bda8-5cb0555eea2e"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3386), 2, 5, 5, new Guid("e82a3015-3bd5-4664-aac1-32090e2091a9"), 6, null },
                    { new Guid("f3e5b60f-cd06-4a88-98fa-8fff6e17b346"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3383), 2, 3, 5, new Guid("e82a3015-3bd5-4664-aac1-32090e2091a9"), 4, null },
                    { new Guid("ad847c40-882a-410c-8b07-6458dc1f33b3"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3359), 2, 11, 4, new Guid("1babc389-43c0-4e1f-9506-f251e6372b17"), 12, null },
                    { new Guid("cd62fe7d-c451-4df7-8a8c-3d69a7ef51a8"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3356), 2, 9, 4, new Guid("1babc389-43c0-4e1f-9506-f251e6372b17"), 10, null },
                    { new Guid("2fd26ed3-c8e8-455d-ad71-cd1952b889ee"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3354), 2, 5, 4, new Guid("2036142b-541f-4124-b09d-b1b39c2c1ba0"), 6, null },
                    { new Guid("29a74bb5-013e-40a1-8b7a-58b7b11dd398"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3354), 2, 3, 4, new Guid("2036142b-541f-4124-b09d-b1b39c2c1ba0"), 4, null },
                    { new Guid("928f6f4f-d6b7-4f7f-a791-efa9152078e9"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3327), 2, 11, 3, new Guid("98c5b782-a1d8-49cb-99b1-d98c9900b8ad"), 12, null },
                    { new Guid("8a3a8474-1233-4621-b7ba-2c4570518135"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3324), 2, 9, 3, new Guid("98c5b782-a1d8-49cb-99b1-d98c9900b8ad"), 10, null },
                    { new Guid("ba73bbb5-0d9d-4f56-9ee3-94450e4c3824"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3324), 2, 5, 3, new Guid("cc96d0e4-74c8-4752-a330-989939af3579"), 6, null },
                    { new Guid("5c3a20e3-334b-4020-a1d1-2bc2257a8938"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3321), 2, 3, 3, new Guid("cc96d0e4-74c8-4752-a330-989939af3579"), 4, null },
                    { new Guid("77492f5f-da59-4b08-b732-ac902827d8d6"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3295), 2, 11, 2, new Guid("b298bd5e-cca8-4c88-accc-cb9daebbac43"), 12, null },
                    { new Guid("f52b3c6f-bfba-4090-bc8d-7bc0f04dc87a"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2900), 3, 6, 0, new Guid("4d87f944-4a53-4431-abee-0a64e1d8a0f4"), 7, null },
                    { new Guid("438ebaf1-9835-46ed-b37b-4abe905848c1"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2900), 3, 10, 0, new Guid("9bfba368-c480-48d0-b8a5-7669af6123fe"), 11, null },
                    { new Guid("ec2bacb3-fa9a-46eb-a998-5c1218ae8aab"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2903), 3, 12, 0, new Guid("9bfba368-c480-48d0-b8a5-7669af6123fe"), 13, null },
                    { new Guid("43d3a359-e00e-4379-9e89-671da81e7a1e"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2906), 3, 18, 0, new Guid("00f8f3ec-f9cf-478b-b3d1-2a338cd35054"), 19, null },
                    { new Guid("c4c58868-ef2d-4c2e-b6d3-db48d46cdd5d"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2906), 3, 20, 0, new Guid("00f8f3ec-f9cf-478b-b3d1-2a338cd35054"), 21, null },
                    { new Guid("06a52c28-e984-4a93-bac8-8f515480a2ac"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2909), 3, 24, 0, new Guid("2e8d71f8-24a4-48ac-8088-12b1351327cf"), 25, null },
                    { new Guid("b2e1041f-26a7-4b65-9ada-908f16ff53f7"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2915), 3, 26, 0, new Guid("2e8d71f8-24a4-48ac-8088-12b1351327cf"), 27, null },
                    { new Guid("20847eaf-a4ae-47a4-9ca1-11666c52ed5e"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3242), 2, 5, 1, new Guid("0dacc2f1-c2b2-42aa-95bc-cc090ac0781e"), 6, null },
                    { new Guid("d24790ea-aad8-45e0-9bdf-32095094c072"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3245), 2, 9, 1, new Guid("a50347be-ef95-47cc-8001-d65120945ae1"), 10, null },
                    { new Guid("f3c6bc8a-cac9-4cc2-832b-fe7621c32e70"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3245), 2, 11, 1, new Guid("a50347be-ef95-47cc-8001-d65120945ae1"), 12, null },
                    { new Guid("bbf68af4-33bd-4141-9d52-70577a056416"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3289), 2, 3, 2, new Guid("cacc8dcc-711a-4527-a95b-eb8704d92572"), 4, null },
                    { new Guid("ef6af0b3-4efd-431a-96c9-7951db8ff4ee"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3289), 2, 5, 2, new Guid("cacc8dcc-711a-4527-a95b-eb8704d92572"), 6, null },
                    { new Guid("aea0c28c-8733-48fb-8cdd-e96c776ba543"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3292), 2, 9, 2, new Guid("b298bd5e-cca8-4c88-accc-cb9daebbac43"), 10, null },
                    { new Guid("0dd4a5b4-136b-4d67-ae2e-9cc0b0e86426"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3239), 2, 3, 1, new Guid("0dacc2f1-c2b2-42aa-95bc-cc090ac0781e"), 4, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("870a7b28-8564-4b0d-9190-054381e29c94"), "BtcWalletAddressRoot", "CityRoot", "CountryRoot", new DateTime(2019, 7, 1, 13, 38, 57, 989, DateTimeKind.Utc).AddTicks(4320), "root@bitclub7.com", "FirstNameRoot", "RTZC5gPA3PvU1wh5v8OZcU0CDxUAA+ULZ5QPK+UF2D7BoBGsiZNZTFEP9TL9jCGEsMkHFWZne6YpQGoZQcl2oFFy6DsHIaliyP3BOdGlzlp1XnhN4tnNakD9ExU3hM/6tKtOzhlWAiRsrNAU6wbNKrKofWKTMx9VM1sPsHKUoICWuZuio/BQJLV4ShApxL1MNssD5pTsHZMiIar7H5IEkJyndiXTEEVOcQnUF09KCIEjAOzsqOSv8Z1fPkDc6QXyIRKSHcOiVyS0ZdRaoCjpofvPc5zBAnqA2K/cHIjliIeTdYwQXAltOIKU2Xnr6lxKiE5OhO3B0VVaYvUw9S5Fvw==", true, "LastNameRoot", "root", "Root", "VQHv7XzASWI=", "StreetRoot", "ZipCodeRoot" },
                    { new Guid("b34995c4-8a31-49df-81cc-66159918700d"), "BtcWalletAddressAdmin", "CityAdmin", "CountryAdmin", new DateTime(2019, 7, 1, 13, 38, 58, 48, DateTimeKind.Utc).AddTicks(3918), "admin@bitclub7.com", "FirstNameAdmin", "jvGNNvIDW80tDzgGFlJV6xzxgPYtDm4CyX3wbIiv7lTBG8otFDXYjcVZ9kx7ArwkO6SMQ7L3JxZyrhb/EKtGPEV0oAQEm6owUXBCBO+aS+8+6h4soVel4ynY+uQMCWTvzbzdDOjj4jBhkBwzJB+kbvGwDolP4WLTRhKNvYJNuEUZFxZs4f6alqjJWjIWu9EY/IjmJWp4MXl78Qer7Wou7OUyOE7A/vh3Au/4zVN2aqd3Zz1xQL7elvmTKze3jAE0lNxH63OHrTySq6MSdPQv87Atodl2QMx5ZyXEzhA4mupk6PToWWPU1ddR9noEBbCHCgaj9dB6rZI86vBuhA8CcA==", true, "LastNameAdmin", "admin", "Admin", "0jG6iqjmF+0=", "StreetAdmin", "ZipCodeAdmin" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[,]
                {
                    { new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(866), true, "root-001", null, null, new Guid("870a7b28-8564-4b0d-9190-054381e29c94") },
                    { new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(3292), false, "root-002", null, null, new Guid("870a7b28-8564-4b0d-9190-054381e29c94") },
                    { new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(3303), false, "root-003", null, null, new Guid("870a7b28-8564-4b0d-9190-054381e29c94") },
                    { new Guid("bde6962c-a69f-4070-b9c7-d5a3c5421a6e"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(3306), true, "admin-001", "xm3dgjTbckuxSfk0", null, new Guid("b34995c4-8a31-49df-81cc-66159918700d") },
                    { new Guid("6d1d38db-8f4d-48c8-af76-7bc5e50b1cb1"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(4778), false, "admin-002", "CbJGE3bl65zWhUwK", null, new Guid("b34995c4-8a31-49df-81cc-66159918700d") },
                    { new Guid("f5d34aaa-455b-4be0-8d77-7017ee3ffde5"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(4796), false, "admin-003", "nB1Mw99LCQuKXUY2", null, new Guid("b34995c4-8a31-49df-81cc-66159918700d") },
                    { new Guid("e374d579-ef95-46ab-99b9-c947a0eb62de"), new DateTime(2019, 7, 1, 13, 38, 58, 104, DateTimeKind.Utc).AddTicks(4799), false, "admin-004", "2ERrHKzmA7bigBeY", null, new Guid("b34995c4-8a31-49df-81cc-66159918700d") }
                });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[,]
                {
                    { new Guid("f85b8f04-5d1f-4261-bf8b-626c1bdd392c"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(1911), 0, 1, 0, null, 30, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("9bfba368-c480-48d0-b8a5-7669af6123fe"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2882), 2, 9, 0, new Guid("2bde30b3-599a-4e41-b820-9dc589937071"), 14, new Guid("6d1d38db-8f4d-48c8-af76-7bc5e50b1cb1") },
                    { new Guid("4d87f944-4a53-4431-abee-0a64e1d8a0f4"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2880), 2, 3, 0, new Guid("2bde30b3-599a-4e41-b820-9dc589937071"), 8, new Guid("bde6962c-a69f-4070-b9c7-d5a3c5421a6e") },
                    { new Guid("0191e80e-66e7-49da-ad48-0ae26717c0aa"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3403), 1, 8, 6, new Guid("b3bcc637-b794-4263-8ef5-e29c4f222a04"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("1b2c7f23-962c-43e3-9367-54df4f17b7e0"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3377), 1, 8, 5, new Guid("89cf4d81-0226-401e-9a49-8b1211caf441"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("1babc389-43c0-4e1f-9506-f251e6372b17"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3351), 1, 8, 4, new Guid("b147e664-c083-4110-8aeb-b54e78365aa1"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("98c5b782-a1d8-49cb-99b1-d98c9900b8ad"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3318), 1, 8, 3, new Guid("55b831a0-fede-4d1d-b1de-afb300331242"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("b298bd5e-cca8-4c88-accc-cb9daebbac43"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3286), 1, 8, 2, new Guid("50ac185b-e596-4469-9b84-af83420e6bc0"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("a50347be-ef95-47cc-8001-d65120945ae1"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3239), 1, 8, 1, new Guid("de35ee8e-5217-41f3-9902-407b082d5c7a"), 13, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("93f69513-67b1-4c34-befa-e81707335d35"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2877), 1, 16, 0, new Guid("f85b8f04-5d1f-4261-bf8b-626c1bdd392c"), 29, new Guid("975ec706-0f87-4c21-adbf-215f0b36d5cf") },
                    { new Guid("70b21a08-2d7c-4afe-9dc1-79e365880cd7"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3403), 1, 2, 6, new Guid("b3bcc637-b794-4263-8ef5-e29c4f222a04"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("00f8f3ec-f9cf-478b-b3d1-2a338cd35054"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2885), 2, 17, 0, new Guid("93f69513-67b1-4c34-befa-e81707335d35"), 22, new Guid("f5d34aaa-455b-4be0-8d77-7017ee3ffde5") },
                    { new Guid("e82a3015-3bd5-4664-aac1-32090e2091a9"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3374), 1, 2, 5, new Guid("89cf4d81-0226-401e-9a49-8b1211caf441"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("cc96d0e4-74c8-4752-a330-989939af3579"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3318), 1, 2, 3, new Guid("55b831a0-fede-4d1d-b1de-afb300331242"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("cacc8dcc-711a-4527-a95b-eb8704d92572"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3283), 1, 2, 2, new Guid("50ac185b-e596-4469-9b84-af83420e6bc0"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("0dacc2f1-c2b2-42aa-95bc-cc090ac0781e"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3236), 1, 2, 1, new Guid("de35ee8e-5217-41f3-9902-407b082d5c7a"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("2bde30b3-599a-4e41-b820-9dc589937071"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2818), 1, 2, 0, new Guid("f85b8f04-5d1f-4261-bf8b-626c1bdd392c"), 15, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("b3bcc637-b794-4263-8ef5-e29c4f222a04"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3400), 0, 1, 6, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("89cf4d81-0226-401e-9a49-8b1211caf441"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3371), 0, 1, 5, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("b147e664-c083-4110-8aeb-b54e78365aa1"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3342), 0, 1, 4, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("55b831a0-fede-4d1d-b1de-afb300331242"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3310), 0, 1, 3, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("50ac185b-e596-4469-9b84-af83420e6bc0"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3280), 0, 1, 2, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("de35ee8e-5217-41f3-9902-407b082d5c7a"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3234), 0, 1, 1, null, 14, new Guid("441c799c-e2b7-4f1c-b141-db3c6c1af034") },
                    { new Guid("2036142b-541f-4124-b09d-b1b39c2c1ba0"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(3342), 1, 2, 4, new Guid("b147e664-c083-4110-8aeb-b54e78365aa1"), 7, new Guid("c1b6ef5c-6e70-4982-bca3-f5407d1e431e") },
                    { new Guid("2e8d71f8-24a4-48ac-8088-12b1351327cf"), new DateTime(2019, 7, 1, 13, 38, 58, 105, DateTimeKind.Utc).AddTicks(2894), 2, 23, 0, new Guid("93f69513-67b1-4c34-befa-e81707335d35"), 28, new Guid("e374d579-ef95-46ab-99b9-c947a0eb62de") }
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
