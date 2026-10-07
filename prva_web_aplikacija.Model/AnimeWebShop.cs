namespace prva_web_aplikacija.Model
{
    public class AnimeWebShop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Genre { get; set; }

        public int NumberOfSeasons { get; set; }

        public float Price { get; set; }

        public AnimeWebShop(int id, string name, string genre, int numberofseasons, float price)
        {
            Id = id;
            Name = name;
            Genre = genre;
            NumberOfSeasons = numberofseasons;
            Price = price;
        }
    }
}
