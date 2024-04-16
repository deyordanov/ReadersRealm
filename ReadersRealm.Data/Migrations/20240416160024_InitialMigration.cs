using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadersRealm.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Author Identifier"),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Author First Name"),
                    MiddleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "Author Middle Name"),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Author Last Name"),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "Author Age"),
                    Gender = table.Column<int>(type: "int", nullable: false, comment: "Author Gender"),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false, comment: "Author Email"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Author Phone Number")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                },
                comment: "Readers Realm Author");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Category Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Category Name"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, comment: "Category Display Order")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "Readers Realm Category");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UIC = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Book Identifier"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Book Title"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true, comment: "Book Description"),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false, comment: "Book's International Standard Book Number"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Book Price"),
                    Pages = table.Column<int>(type: "int", nullable: true, comment: "Book Page Count"),
                    BookCover = table.Column<int>(type: "int", nullable: true, comment: "Book Cover Type"),
                    Used = table.Column<bool>(type: "bit", nullable: false, comment: "Book Condition"),
                    ImageId = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true, comment: "Book Image Id"),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Author Identifier"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Readers Realm Book");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdersDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Age", "Email", "FirstName", "Gender", "LastName", "MiddleName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("2d340d56-2594-4071-8878-019647b45cb4"), 32, "gegeakutami@gmail.com", "Gege", 0, "Akutami", null, "098-354-4364" },
                    { new Guid("4f82ee5a-9a3b-4d00-8027-e677e41e2527"), 54, "kentaromiura@gmail.com", "Kentaro", 0, "Miura", null, "098-646-4683" },
                    { new Guid("72fc4a67-9b6d-44e0-a21a-cc78ba323dea"), 59, "rriordan@gmail.com", "Rick", 0, "Riordan", null, "098-765-4321" },
                    { new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 49, "eiichirooda@gmail.com", "Eiichiro", 0, "Oda", null, "098-742-7674" },
                    { new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 49, "kmasashi@gmail.com", "Masashi", 0, "Kishimoto", null, "123-456-7890" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Manga" },
                    { 2, 2, "Action" },
                    { 3, 3, "SciFi" },
                    { 4, 4, "History" },
                    { 5, 5, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "BookCover", "CategoryId", "Description", "ISBN", "ImageId", "Pages", "Price", "Title", "Used" },
                values: new object[,]
                {
                    { new Guid("081beab9-bf93-4270-ac8d-4b659d584153"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "In 'Naruto Vol 64', the Fourth Great Ninja War continues, showcasing intense battles and deepening the bonds between the characters. Naruto and his allies face off against powerful enemies, uncovering new abilities and tactics. This volume highlights the strength and resolve of the Shinobi Alliance as they fight to protect their world from destruction.", "9781421516670", "661e9f13ff51d33c794cfea5", 192, 17.99m, "Naruto Vol 64", false },
                    { new Guid("0c1aa78c-7610-465f-af48-30f2b9afe436"), new Guid("2d340d56-2594-4071-8878-019647b45cb4"), 1, 1, "Volume 13 of 'Jujutsu Kaisen' heightens the tension as the battles intensify and new alliances are formed. In this volume, the focus shifts to the fallout of the Shibuya Incident, with Yuji Itadori and the other Jujutsu sorcerers grappling with the aftermath and the moral dilemmas posed by their decisions. The stakes are higher than ever, with new threats emerging that could alter the course of their world.", "9759367153649", "661e9f13ff51d33c794cfe84", 200, 19.99m, "Jujutsu Kaisen Vol 13", false },
                    { new Guid("2e6a6b62-c01c-479f-97db-62af801ff89c"), new Guid("4f82ee5a-9a3b-4d00-8027-e677e41e2527"), 1, 1, "In 'Berserk Vol 3', the epic tale continues as Guts, the fearless Black Swordsman, delves deeper into the demonic forces manipulating the earthly world. This volume intensifies the exploration of dark fantasy elements and Guts’ complex relationship with his past. It features brutal conflicts, emotional depth, and expanding lore that enriches the narrative's grim and captivating universe.", "9781421516724", "661e9f13ff51d33c794cfe9c", 240, 14.99m, "Berserk Vol 3", false },
                    { new Guid("2facd184-2f4c-4eec-95f3-348c2e0d98c5"), new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 1, 1, "In 'One Piece Vol 80', the narrative unfolds further into the intriguing Zou Arc. The Straw Hat Pirates, along with new allies, explore the mysterious and ancient elephant island, Zou. This volume reveals crucial details about the world's geopolitical landscape and the history of the Mink Tribe. The resilience and unity of the crew are tested as they delve deeper into the secrets of the Pirate World and the legendary Void Century.", "9781421516885", "661e9f13ff51d33c794cfe96", 192, 16.49m, "One Piece Vol 80", false },
                    { new Guid("36462fdc-afb3-4d79-9d22-7a477b07555f"), new Guid("2d340d56-2594-4071-8878-019647b45cb4"), 1, 1, "In 'Jujutsu Kaisen Vol 10', the plot escalates with intense battles and strategic maneuvering among the sorcerers. As the Shibuya Incident unfolds, Yuji Itadori and his allies face unprecedented challenges that test their strength and resolve. This volume delves deeper into the dark pasts of key characters, revealing motives and hidden powers, setting up high stakes for the series' future developments.", "9799667150078", "661e99cbc95aa93eba46d526", 200, 19.99m, "Jujutsu Kaisen Vol 10", false },
                    { new Guid("3c357f2f-ad1e-4f8d-8477-42e00dbbd541"), new Guid("4f82ee5a-9a3b-4d00-8027-e677e41e2527"), 1, 1, "Volume 1 of 'Berserk' sets the stage for a dark and gripping saga as it introduces Guts, the Black Swordsman. This volume plunges readers into a bleak world of formidable battles and relentless despair, revealing the beginning of Guts' quest for revenge against a cursed fate. The intense narrative and stark, yet detailed artwork lay the groundwork for a profound and challenging series.", "9781421516700", "661e9f13ff51d33c794cfe99", 224, 14.99m, "Berserk Vol 1", false },
                    { new Guid("3c875621-102a-4795-b1a0-728a69bd247a"), new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 1, 1, "In 'One Piece Vol 5', the Straw Hat Pirates face new challenges as they search for the elusive treasure, One Piece. This volume captures the excitement of their adventures in the Grand Line, featuring clashes with rival pirates and encounters with bizarre creatures. As Luffy and his crew venture deeper, they learn the complexities of loyalty and the harsh realities of the pirate world, all while pursuing their grand dream.", "9781421516724", "661e9f13ff51d33c794cfe8d", 208, 15.99m, "One Piece Vol 5", false },
                    { new Guid("6e651ab1-d34a-41ab-b436-f9b4f94641fc"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "Volume 27 of 'Naruto' marks the end of Part I and sets the stage for the mature themes of Part II. This pivotal volume concludes with Naruto's departure from Konoha to train with Jiraiya, setting up his future return as a more powerful ninja. The volume also explores Sasuke's struggle with his past and the fateful decision that leads him down a darker path, igniting a fierce rivalry that becomes central to the narrative.", "9781421516489", "661e9f13ff51d33c794cfe89", 192, 19.99m, "Naruto Vol 27", false },
                    { new Guid("71c75207-a547-4b34-8b6d-3de0f5e80c0e"), new Guid("4f82ee5a-9a3b-4d00-8027-e677e41e2527"), 1, 1, "Volume 12 of 'Berserk' reaches new heights in storytelling as Guts faces off against supernatural forces that threaten to consume the world. The narrative becomes increasingly intense as it delves into themes of isolation and resilience, showcasing Guts' struggle to maintain his humanity amidst chaos. This volume is a turning point that expands on the series' mythos and sets a new course for the saga's future developments.", "9781421516809", "661e9f13ff51d33c794cfe9f", 256, 14.99m, "Berserk Vol 12", false },
                    { new Guid("850aecd6-9982-4f20-936f-b9712668dc64"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "Volume 66 of 'Naruto' further escalates the drama and action of the Fourth Great Ninja War. As the battles intensify, Naruto's leadership and strategic mind are put to the test. This volume features pivotal moments that define the future of the ninja world, including crucial revelations and character developments that add depth to the story's overarching themes.", "9781421516694", "661e9f13ff51d33c794cfea2", 192, 17.99m, "Naruto Vol 66", false },
                    { new Guid("87e614c7-ef21-455e-b1ab-63981f6c1920"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "\"Naruto Vol 35\" shifts the focus to the aftermath of Naruto's intense training under Jiraiya and the looming threat of the Akatsuki. Naruto returns to Konoha, showcasing newfound strengths and techniques. Meanwhile, the story delves into the formation of new alliances and the preparation for a confrontation that could shake the ninja world to its core. This volume encapsulates themes of evolution and anticipation, setting the stage for epic battles and strategic mind games.", "9794654155478", "65e2d86a7714ad25c89d864a", 200, 19.99m, "Naruto Vol 35", false },
                    { new Guid("8d3fd4f5-50f5-463c-9464-7cdcfc7227c8"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "In 'Naruto Vol 63', the Fourth Great Ninja War continues to rage, bringing about pivotal confrontations. This volume features intense battles, strategic maneuvers, and deep emotional storylines as Naruto faces some of his most formidable enemies yet. As alliances shift and truths are revealed, Naruto's beliefs and resolve are put to the ultimate test, setting the stage for dramatic developments in the shinobi world.", "9781421516632", "661e9f13ff51d33c794cfe8b", 192, 19.99m, "Naruto Vol 63", false },
                    { new Guid("943627fb-87e8-4a95-a7db-56097cb7f0f7"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "In \"Naruto Vol 8\" the Chunin Exam's second phase plunges Naruto, Sasuke, and Sakura into the perilous Forest of Death. Amidst treacherous battles and survival challenges, the team confronts formidable rivals and their own fears. As hidden dangers emerge, Naruto's resolve is tested, unveiling new powers and deepening bonds. This volume is a thrilling journey of growth, teamwork, and unyielding spirit against life-and-death stakes.", "9785421961182", "65e2d86a7714ad25c89d8648", 192, 19.99m, "Naruto Vol 8", false },
                    { new Guid("9a1309ca-22c3-46f9-b8a5-02203a465c95"), new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 1, 1, "In 'One Piece Vol 105', the epic journey of Luffy and the Straw Hat Pirates reaches new heights as they encounter formidable foes and uncover secrets that could change the world. This volume dives deeper into the mysteries of the One Piece world, with thrilling battles, strategic alliances, and moments of unexpected bravery that underscore the enduring themes of friendship and the pursuit of dreams.", "9781421516908", "661e9f13ff51d33c794cfe90", 192, 17.99m, "One Piece Vol 105", false },
                    { new Guid("b1ba9a0a-13d8-49e3-b42c-9265c4413207"), new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 1, 1, "Volume 98 of 'One Piece' continues the high-stakes adventure in the Land of Wano, as Luffy and the allied forces prepare for the final battle against Kaido and his formidable army. This volume captures the intricate planning and the early clashes that set the stage for one of the most epic confrontations in the series. With alliances tested and strategies unveiled, the resilience and courage of the Straw Hat Pirates are highlighted as they fight for freedom and justice.", "9781421516878", "661e9f13ff51d33c794cfea8", 192, 16.99m, "One Piece Vol 98", false },
                    { new Guid("eeb87926-3145-43df-b182-c4a59171d1d3"), new Guid("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"), 1, 1, "Volume 77 of 'One Piece' continues the intense Dressrosa Arc, where Luffy and his allies battle against the tyrannical Doflamingo and his powerful crew. This volume highlights the culmination of many long-brewing conflicts and the dramatic transformations of key characters. The stakes are higher than ever as the Straw Hats strive to overthrow Doflamingo and bring freedom to the people of Dressrosa.", "9781421516854", "661e9f13ff51d33c794cfe93", 192, 16.99m, "One Piece Vol 77", false },
                    { new Guid("f86b0d78-66f2-46d4-9cab-d327263d1370"), new Guid("a5e87971-53ad-40df-97ff-79dcaef4520a"), 1, 1, "\"Naruto Vol 1\" introduces readers to Naruto Uzumaki, a young ninja with dreams of becoming the strongest ninja and leader of his village, the Hokage. Despite being ostracized by the village for housing a fearsome nine-tailed fox that attacked the village years ago, Naruto is determined to gain the village's recognition through his accomplishments and hard work. The volume covers his struggles in the Ninja Academy, his formation of friendships and rivalries, and the beginning of his journey to becoming a skilled ninja. It sets the stage for an expansive story of growth, battles, and the pursuit of dreams amidst the complexities of the ninja world.", "9797415363864", "65e2d86a7714ad25c89d8646", 192, 19.99m, "Naruto Vol 1", false },
                    { new Guid("ff803925-30f7-4e4c-a6aa-dd9564eb5f96"), new Guid("2d340d56-2594-4071-8878-019647b45cb4"), 1, 1, "In 'Jujutsu Kaisen Vol 20', the narrative continues to evolve as the characters face new challenges and enemies. This volume sees Yuji Itadori and his comrades confronting the consequences of the Culling Game, with alliances tested and strategies pushed to their limits. The volume delivers thrilling action, deeper explorations into the sorcerers' abilities, and crucial turning points that set the stage for future conflicts.", "9799462150364", "661e9f13ff51d33c794cfe86", 200, 19.99m, "Jujutsu Kaisen Vol 20", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderHeaderId",
                table: "Orders",
                column: "OrderHeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_BookId",
                table: "OrdersDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_OrderHeaderId",
                table: "OrdersDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ApplicationUserId",
                table: "ShoppingCarts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_BookId",
                table: "ShoppingCarts",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrdersDetails");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
