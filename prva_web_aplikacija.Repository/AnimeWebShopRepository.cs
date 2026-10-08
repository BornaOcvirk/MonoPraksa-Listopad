using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;

namespace prva_web_aplikacija.Repository
{
    public class AnimeWebShopRepository : IAnimeWebShopRepository
    {
        private const string SelectColumns =
            "select Anime_Id, Title, Genre, Number_of_seasons, Price, Relese_date, Studio_Id from Anime";

        private readonly NpgsqlDataSource _dataSource;

        public AnimeWebShopRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public List<AnimeWebShop> GetAll()
        {
            using var cmd = _dataSource.CreateCommand(SelectColumns + " order by Title");
            return ReadList(cmd);
        }

        public AnimeWebShop? GetById(Guid id)
        {
            using var cmd = _dataSource.CreateCommand(SelectColumns + " where Anime_Id = $1");
            cmd.Parameters.Add(new NpgsqlParameter { Value = id });

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadAnime(reader) : null;
        }

        public List<AnimeWebShop> GetByGenre(string genre, int minSeasons)
        {
            using var cmd = _dataSource.CreateCommand(
                SelectColumns + " where Genre = $1 and Number_of_seasons >= $2 order by Title");
            cmd.Parameters.Add(new NpgsqlParameter { Value = genre });
            cmd.Parameters.Add(new NpgsqlParameter { Value = minSeasons });
            return ReadList(cmd);
        }

        public void Add(AnimeWebShop anime)
        {
            using var cmd = _dataSource.CreateCommand(
                @"insert into Anime (Anime_Id, Title, Genre, Number_of_seasons, Price, Relese_date, Studio_Id)
                  values ($1, $2, $3, $4, $5, $6, $7)");
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.Id });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.Name });
            cmd.Parameters.Add(new NpgsqlParameter { Value = (object?)anime.Genre ?? DBNull.Value });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.NumberOfSeasons });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.Price });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.ReleaseDate });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.StudioId });

            cmd.ExecuteNonQuery();
        }

        public bool Update(Guid id, AnimeWebShop anime)
        {
            using var cmd = _dataSource.CreateCommand(
                @"update Anime
                  set Title = $1, Genre = $2, Number_of_seasons = $3, Price = $4,
                      Relese_date = $5, Studio_Id = $6
                  where Anime_Id = $7");
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.Name });
            cmd.Parameters.Add(new NpgsqlParameter { Value = (object?)anime.Genre ?? DBNull.Value });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.NumberOfSeasons });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.Price });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.ReleaseDate });
            cmd.Parameters.Add(new NpgsqlParameter { Value = anime.StudioId });
            cmd.Parameters.Add(new NpgsqlParameter { Value = id });

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(Guid id)
        {
            using var cmd = _dataSource.CreateCommand("delete from Anime where Anime_Id = $1");
            cmd.Parameters.Add(new NpgsqlParameter { Value = id });

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool StudioExists(Guid studioId)
        {
            using var cmd = _dataSource.CreateCommand(
                "select exists(select 1 from Studio where Studio_Id = $1)");
            cmd.Parameters.Add(new NpgsqlParameter { Value = studioId });

            return (bool)cmd.ExecuteScalar()!;
        }
        private static List<AnimeWebShop> ReadList(NpgsqlCommand cmd)
        {
            var animelist = new List<AnimeWebShop>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                animelist.Add(ReadAnime(reader));
            }
            return animelist;
        }

        private static AnimeWebShop ReadAnime(NpgsqlDataReader reader) => new AnimeWebShop
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Genre = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            NumberOfSeasons = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
            Price = reader.IsDBNull(4) ? 0m : reader.GetDecimal(4),
            ReleaseDate = reader.GetFieldValue<DateOnly>(5),
            StudioId = reader.GetGuid(6)
        };
    }
}