using ECommerce.API.Models;
using ECommerce.Shared;

namespace ECommerce.API.Data;

public class DbSeeder
{
    public static async Task SeedItemsAsync(ApiDbContext db)
    {
        // Tags
        var westCoastHipHopTag = new Tag { TagName = "West Coast Hip-Hop" };
        var consciousRapTag = new Tag { TagName = "Conscious Rap" };
        var femaleEmpowermentTag = new Tag { TagName = "Female Empowerment" };
        var heartbreakTag = new Tag { TagName = "Heartbreak" };
        var rnbTag = new Tag { TagName = "R&B" };
        var soulTag = new Tag { TagName = "Soul" };
        var popTag = new Tag { TagName = "Pop" };
        var alternativeTag = new Tag { TagName = "Alternative" };
        var trapTag = new Tag { TagName = "Trap" };
        var melodicRapTag = new Tag { TagName = "Melodic Rap" };
        var indieTag = new Tag { TagName = "Indie" };
        var folkTag = new Tag { TagName = "Folk" };
        var rockTag = new Tag { TagName = "Rock" };
        var eastCoastHipHopTag = new Tag { TagName = "East Coast Hip-Hop" };
        var afrobeatTag = new Tag { TagName = "Afrobeats" };

        var sampleItems = new List<Item>
        {
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Kendrick Lamar",
                Name = "GNX", Price = 34.99m, Genre = "Hip-Hop",
                Tags = new List<Tag> { westCoastHipHopTag, consciousRapTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Beyoncé",
                Name = "Cowboy Carter", Price = 39.99m, Genre = "Country/R&B",
                Tags = new List<Tag> { femaleEmpowermentTag, rnbTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Sabrina Carpenter",
                Name = "Short n' Sweet", Price = 29.99m, Genre = "Pop",
                Tags = new List<Tag> { popTag, heartbreakTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Billie Eilish",
                Name = "HIT ME HARD AND SOFT", Price = 31.99m, Genre = "Alternative/Pop",
                Tags = new List<Tag> { alternativeTag, popTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Drake",
                Name = "For All The Dogs", Price = 34.99m, Genre = "Hip-Hop/R&B",
                Tags = new List<Tag> { trapTag, rnbTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Tyler, the Creator",
                Name = "Chromakopia", Price = 36.99m, Genre = "Hip-Hop",
                Tags = new List<Tag> { westCoastHipHopTag, alternativeTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Chappell Roan",
                Name = "The Rise and Fall of a Midwest Princess", Price = 29.99m, Genre = "Pop",
                Tags = new List<Tag> { popTag, indieTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Playboi Carti",
                Name = "Music", Price = 34.99m, Genre = "Hip-Hop",
                Tags = new List<Tag> { trapTag, melodicRapTag }
            },
            new()
            {
                Format = ItemFormat.Cd, Type = ItemType.Album, Artist = "Gracie Abrams",
                Name = "The Secret of Us", Price = 13.99m, Genre = "Indie/Pop",
                Tags = new List<Tag> { indieTag, heartbreakTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Vampire Weekend",
                Name = "Only God Was Above Us", Price = 31.99m, Genre = "Indie Rock",
                Tags = new List<Tag> { indieTag, rockTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Doechii",
                Name = "Alligator Bites Never Heal", Price = 29.99m, Genre = "Hip-Hop/R&B",
                Tags = new List<Tag> { femaleEmpowermentTag, trapTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Burna Boy",
                Name = "I Told Them...", Price = 33.99m, Genre = "Afrobeats",
                Tags = new List<Tag> { afrobeatTag, rnbTag }
            },
            new()
            {
                Format = ItemFormat.Cd, Type = ItemType.Single, Artist = "Kendrick Lamar",
                Name = "Not Like Us", Price = 5.99m, Genre = "Hip-Hop",
                Tags = new List<Tag> { westCoastHipHopTag, consciousRapTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Mk.gee",
                Name = "Two Star & The Dream Police", Price = 27.99m, Genre = "Alternative/R&B",
                Tags = new List<Tag> { alternativeTag, rnbTag }
            },
            new()
            {
                Format = ItemFormat.Vinyl, Type = ItemType.Album, Artist = "Lauryn Hill",
                Name = "The Miseducation of Lauryn Hill", Price = 29.98m, Genre = "Neo Soul",
                Tags = new List<Tag> { femaleEmpowermentTag, soulTag }
            }
        };

        db.Items.AddRange(sampleItems);
        await db.SaveChangesAsync();
    }
}