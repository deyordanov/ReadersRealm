namespace ReadersRealm.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using Models.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(GenerateBooks());
    }

    public static IEnumerable<Book> GenerateBooks()
    {
        MongoClient client = new MongoClient("mongodb://localhost:27017");
        IMongoDatabase database = client.GetDatabase("ReadersRealm");
        GridFSBucket gridFsBucket = new GridFSBucket(database);

        List<Book> books = new List<Book>();

        byte[] imageBytes = File
                 .ReadAllBytes("wwwroot\\images\\bookSeedData\\9e84aff9-028a-4cd7-8e31-2e73eddce1d5.jpg");
        string imageId = SeedImageAsync(imageBytes, "NarutoVol1", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 1",
            ISBN = "9797415363864",
            Price = 19.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = @"""Naruto Vol 1"" introduces readers to Naruto Uzumaki, a young ninja with dreams of becoming the strongest ninja and leader of his village, the Hokage. Despite being ostracized by the village for housing a fearsome nine-tailed fox that attacked the village years ago, Naruto is determined to gain the village's recognition through his accomplishments and hard work. The volume covers his struggles in the Ninja Academy, his formation of friendships and rivalries, and the beginning of his journey to becoming a skilled ninja. It sets the stage for an expansive story of growth, battles, and the pursuit of dreams amidst the complexities of the ninja world.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\b2f6e888-402f-4851-9392-8931a567ea9a.jpg");
        imageId = SeedImageAsync(imageBytes, "NarutoVol8", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 8",
            ISBN = "9785421961182",
            Price = 19.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = @"In ""Naruto Vol 8"" the Chunin Exam's second phase plunges Naruto, Sasuke, and Sakura into the perilous Forest of Death. Amidst treacherous battles and survival challenges, the team confronts formidable rivals and their own fears. As hidden dangers emerge, Naruto's resolve is tested, unveiling new powers and deepening bonds. This volume is a thrilling journey of growth, teamwork, and unyielding spirit against life-and-death stakes.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\3ba49236-d6a4-43ec-b9d7-d77eeff8e812.webp");
        imageId = SeedImageAsync(imageBytes, "NarutoVol35", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 35",
            ISBN = "9794654155478",
            Price = 19.99M,
            Pages = 200,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = @"""Naruto Vol 35"" shifts the focus to the aftermath of Naruto's intense training under Jiraiya and the looming threat of the Akatsuki. Naruto returns to Konoha, showcasing newfound strengths and techniques. Meanwhile, the story delves into the formation of new alliances and the preparation for a confrontation that could shake the ninja world to its core. This volume encapsulates themes of evolution and anticipation, setting the stage for epic battles and strategic mind games.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81fuNV7XsnL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "JujutsuKaisenVol10", gridFsBucket);
        books.Add(new Book
        {
            Title = "Jujutsu Kaisen Vol 10",
            ISBN = "9799667150078",
            Price = 19.99M,
            Pages = 200,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId, 
            Description = "In 'Jujutsu Kaisen Vol 10', the plot escalates with intense battles and strategic maneuvering among the sorcerers. As the Shibuya Incident unfolds, Yuji Itadori and his allies face unprecedented challenges that test their strength and resolve. This volume delves deeper into the dark pasts of key characters, revealing motives and hidden powers, setting up high stakes for the series' future developments.",
            AuthorId = Guid.Parse("2d340d56-2594-4071-8878-019647b45cb4"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81KiIrm5ToL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "JujutsuKaisenVol13", gridFsBucket);
        books.Add(new Book
        {
            Title = "Jujutsu Kaisen Vol 13",
            ISBN = "9759367153649", 
            Price = 19.99M,
            Pages = 200,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId, 
            Description = "Volume 13 of 'Jujutsu Kaisen' heightens the tension as the battles intensify and new alliances are formed. In this volume, the focus shifts to the fallout of the Shibuya Incident, with Yuji Itadori and the other Jujutsu sorcerers grappling with the aftermath and the moral dilemmas posed by their decisions. The stakes are higher than ever, with new threats emerging that could alter the course of their world.",
            AuthorId = Guid.Parse("2d340d56-2594-4071-8878-019647b45cb4"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81+-9sRp2JL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "JujutsuKaisenVol20", gridFsBucket);
        books.Add(new Book
        {
            Title = "Jujutsu Kaisen Vol 20",
            ISBN = "9799462150364", 
            Price = 19.99M,
            Pages = 200,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'Jujutsu Kaisen Vol 20', the narrative continues to evolve as the characters face new challenges and enemies. This volume sees Yuji Itadori and his comrades confronting the consequences of the Culling Game, with alliances tested and strategies pushed to their limits. The volume delivers thrilling action, deeper explorations into the sorcerers' abilities, and crucial turning points that set the stage for future conflicts.",
            AuthorId = Guid.Parse("2d340d56-2594-4071-8878-019647b45cb4"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\61hwr9KxTCL._SY445_SX342_.jpg");
        imageId = SeedImageAsync(imageBytes, "NarutoVol27", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 27",
            ISBN = "9781421516489",
            Price = 19.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "Volume 27 of 'Naruto' marks the end of Part I and sets the stage for the mature themes of Part II. This pivotal volume concludes with Naruto's departure from Konoha to train with Jiraiya, setting up his future return as a more powerful ninja. The volume also explores Sasuke's struggle with his past and the fateful decision that leads him down a darker path, igniting a fierce rivalry that becomes central to the narrative.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\71dnAACq16L.jpg");
        imageId = SeedImageAsync(imageBytes, "NarutoVol63", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 63",
            ISBN = "9781421516632",
            Price = 19.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'Naruto Vol 63', the Fourth Great Ninja War continues to rage, bringing about pivotal confrontations. This volume features intense battles, strategic maneuvers, and deep emotional storylines as Naruto faces some of his most formidable enemies yet. As alliances shift and truths are revealed, Naruto's beliefs and resolve are put to the ultimate test, setting the stage for dramatic developments in the shinobi world.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81D4SPVL8iL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "OnePieceVol5", gridFsBucket);
        books.Add(new Book
        {
            Title = "One Piece Vol 5",
            ISBN = "9781421516724",
            Price = 15.99M, 
            Pages = 208,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'One Piece Vol 5', the Straw Hat Pirates face new challenges as they search for the elusive treasure, One Piece. This volume captures the excitement of their adventures in the Grand Line, featuring clashes with rival pirates and encounters with bizarre creatures. As Luffy and his crew venture deeper, they learn the complexities of loyalty and the harsh realities of the pirate world, all while pursuing their grand dream.",
            AuthorId = Guid.Parse("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81d8JssLMdL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "OnePieceVol105", gridFsBucket);
        books.Add(new Book
        {
            Title = "One Piece Vol 105",
            ISBN = "9781421516908",
            Price = 17.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'One Piece Vol 105', the epic journey of Luffy and the Straw Hat Pirates reaches new heights as they encounter formidable foes and uncover secrets that could change the world. This volume dives deeper into the mysteries of the One Piece world, with thrilling battles, strategic alliances, and moments of unexpected bravery that underscore the enduring themes of friendship and the pursuit of dreams.",
            AuthorId = Guid.Parse("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\81UC57rQ1NL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "OnePieceVol77", gridFsBucket);
        books.Add(new Book
        {
            Title = "One Piece Vol 77",
            ISBN = "9781421516854",
            Price = 16.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "Volume 77 of 'One Piece' continues the intense Dressrosa Arc, where Luffy and his allies battle against the tyrannical Doflamingo and his powerful crew. This volume highlights the culmination of many long-brewing conflicts and the dramatic transformations of key characters. The stakes are higher than ever as the Straw Hats strive to overthrow Doflamingo and bring freedom to the people of Dressrosa.",
            AuthorId = Guid.Parse("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\91FCVy2bZ6L._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "OnePieceVol80", gridFsBucket);
        books.Add(new Book
        {
            Title = "One Piece Vol 80",
            ISBN = "9781421516885",
            Price = 16.49M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'One Piece Vol 80', the narrative unfolds further into the intriguing Zou Arc. The Straw Hat Pirates, along with new allies, explore the mysterious and ancient elephant island, Zou. This volume reveals crucial details about the world's geopolitical landscape and the history of the Mink Tribe. The resilience and unity of the crew are tested as they delve deeper into the secrets of the Pirate World and the legendary Void Century.",
            AuthorId = Guid.Parse("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\91Gs6wk1TdL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "BerserkVol1", gridFsBucket);
        books.Add(new Book
        {
            Title = "Berserk Vol 1",
            ISBN = "9781421516700",
            Price = 14.99M,
            Pages = 224,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "Volume 1 of 'Berserk' sets the stage for a dark and gripping saga as it introduces Guts, the Black Swordsman. This volume plunges readers into a bleak world of formidable battles and relentless despair, revealing the beginning of Guts' quest for revenge against a cursed fate. The intense narrative and stark, yet detailed artwork lay the groundwork for a profound and challenging series.",
            AuthorId = Guid.Parse("4f82ee5a-9a3b-4d00-8027-e677e41e2527"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\A1TzB8k68UL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "BerserkVol3", gridFsBucket);
        books.Add(new Book
        {
            Title = "Berserk Vol 3",
            ISBN = "9781421516724",
            Price = 14.99M,
            Pages = 240,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "In 'Berserk Vol 3', the epic tale continues as Guts, the fearless Black Swordsman, delves deeper into the demonic forces manipulating the earthly world. This volume intensifies the exploration of dark fantasy elements and Guts’ complex relationship with his past. It features brutal conflicts, emotional depth, and expanding lore that enriches the narrative's grim and captivating universe.",
            AuthorId = Guid.Parse("4f82ee5a-9a3b-4d00-8027-e677e41e2527"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\91nz5EQTi1L._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "BerserkVol12", gridFsBucket);
        books.Add(new Book
        {
            Title = "Berserk Vol 12",
            ISBN = "9781421516809",
            Price = 14.99M,
            Pages = 256,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "Volume 12 of 'Berserk' reaches new heights in storytelling as Guts faces off against supernatural forces that threaten to consume the world. The narrative becomes increasingly intense as it delves into themes of isolation and resilience, showcasing Guts' struggle to maintain his humanity amidst chaos. This volume is a turning point that expands on the series' mythos and sets a new course for the saga's future developments.",
            AuthorId = Guid.Parse("4f82ee5a-9a3b-4d00-8027-e677e41e2527"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\91h+Z+WG8AL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "NarutoVol66", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 66",
            ISBN = "9781421516694",
            Price = 17.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId, 
            Description = "Volume 66 of 'Naruto' further escalates the drama and action of the Fourth Great Ninja War. As the battles intensify, Naruto's leadership and strategic mind are put to the test. This volume features pivotal moments that define the future of the ninja world, including crucial revelations and character developments that add depth to the story's overarching themes.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\917q2qj6wbL._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "NarutoVol64", gridFsBucket);
        books.Add(new Book
        {
            Title = "Naruto Vol 64",
            ISBN = "9781421516670",
            Price = 17.99M,
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId, 
            Description = "In 'Naruto Vol 64', the Fourth Great Ninja War continues, showcasing intense battles and deepening the bonds between the characters. Naruto and his allies face off against powerful enemies, uncovering new abilities and tactics. This volume highlights the strength and resolve of the Shinobi Alliance as they fight to protect their world from destruction.",
            AuthorId = Guid.Parse("A5E87971-53AD-40DF-97FF-79DCAEF4520A"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\91v4tKkWbtS._SL1500_.jpg");
        imageId = SeedImageAsync(imageBytes, "OnePieceVol98", gridFsBucket);
        books.Add(new Book
        {
            Title = "One Piece Vol 98",
            ISBN = "9781421516878",
            Price = 16.99M, 
            Pages = 192,
            BookCover = BookCover.Softcover,
            Used = false,
            ImageId = imageId,
            Description = "Volume 98 of 'One Piece' continues the high-stakes adventure in the Land of Wano, as Luffy and the allied forces prepare for the final battle against Kaido and his formidable army. This volume captures the intricate planning and the early clashes that set the stage for one of the most epic confrontations in the series. With alliances tested and strategies unveiled, the resilience and courage of the Straw Hat Pirates are highlighted as they fight for freedom and justice.",
            AuthorId = Guid.Parse("99aa315d-72fb-49fe-a18b-fc2b7c84ec51"),
            CategoryId = 1,
        });

        imageBytes = File
            .ReadAllBytes("wwwroot\\images\\bookSeedData\\defaultImage.png");
        SeedImageAsync(imageBytes, "DefaultImage", gridFsBucket);

        return books;
    }

    private static string SeedImageAsync(byte[] image, string imageName, GridFSBucket gridFsBucket)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq(info => info.Filename, imageName);
        var existingFiles = gridFsBucket.Find(filter).ToList();

        if (existingFiles.Any())
        {
            return existingFiles.First().Id.ToString()!;
        }

        ObjectId imageId = gridFsBucket.UploadFromBytes(imageName, image);
        return imageId.ToString()!;
    }
}