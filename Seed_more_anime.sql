-- Extra test data (~50 anime) for trying search and paging.
-- Only inserts, safe to run more than once (existing rows are skipped).
-- Run after Manga_to_Anime.sql:
--   psql -h localhost -U postgres -d PraksaDB -v ON_ERROR_STOP=1 -f Seed_more_anime.sql

begin;

insert into Studio (Name_s, country, Established) values
('Toei Animation',   'Japan', '1948-01-23'),
('Kyoto Animation',  'Japan', '1981-07-12'),
('Production I.G',   'Japan', '1987-12-15'),
('A-1 Pictures',     'Japan', '2005-05-09'),
('CloverWorks',      'Japan', '2018-10-01'),
('Studio Trigger',   'Japan', '2011-08-22'),
('Studio Ghibli',    'Japan', '1985-06-15'),
('David Production', 'Japan', '2007-09-01'),
('White Fox',        'Japan', '2007-04-01')
on conflict (Name_s) do nothing;

insert into Manga (Author, Title, Volumes, Popularity, Studio_Id)
select v.author, v.title, v.volumes, v.popularity,
       (select Studio_Id from Studio where Name_s = v.studio)
from (values
    ('Eiichiro Oda',      'One Piece',                110, 'Very high', 'Toei Animation'),
    ('Akira Toriyama',    'Dragon Ball',               42, 'Very high', 'Toei Animation'),
    ('Kohei Horikoshi',   'My Hero Academia',          42, 'High',      'Bones'),
    ('Tatsuki Fujimoto',  'Chainsaw Man',              20, 'High',      'MAPPA'),
    ('Tatsuya Endo',      'Spy x Family',              14, 'High',      'Wit Studio'),
    ('Yoshihiro Togashi', 'Hunter x Hunter',           38, 'Very high', 'Madhouse'),
    ('ONE',               'One-Punch Man',             32, 'High',      'Madhouse'),
    ('Tite Kubo',         'Bleach',                    74, 'High',      'Studio Pierrot'),
    ('Haruichi Furudate', 'Haikyu!!',                  45, 'High',      'Production I.G'),
    ('Makoto Yukimura',   'Vinland Saga',              14, 'Medium',    'Wit Studio'),
    ('ONE',               'Mob Psycho 100',            16, 'Medium',    'Bones'),
    ('Hirohiko Araki',    'JoJo''s Bizarre Adventure', 63, 'High',      'David Production'),
    ('Sui Ishida',        'Tokyo Ghoul',               14, 'Medium',    'Studio Pierrot')
) as v(author, title, volumes, popularity, studio)
where not exists (select 1 from Manga m where m.Title = v.title);

insert into Anime (Title, Relese_date, Popularity, Genre, Number_of_seasons, Price, Manga_Id, Studio_Id)
select v.title, v.released::date, v.popularity, v.genre, v.seasons, v.price,
       (select Manga_Id from Manga where Title = v.manga limit 1),
       (select Studio_Id from Studio where Name_s = v.studio)
from (values
    -- Manga adaptations
    ('One Piece',                                     '1999-10-20', 'Very high', 'Action',          21, 69.99, 'One Piece',                 'Toei Animation'),
    ('One Piece Film: Red',                           '2022-08-06', 'High',      'Action',           1, 14.99, 'One Piece',                 'Toei Animation'),
    ('One Piece: Stampede',                           '2019-08-09', 'Medium',    'Action',           1, 12.99, 'One Piece',                 'Toei Animation'),
    ('Dragon Ball',                                   '1986-02-26', 'High',      'Action',           1, 29.99, 'Dragon Ball',               'Toei Animation'),
    ('Dragon Ball Z',                                 '1989-04-26', 'Very high', 'Action',           9, 54.99, 'Dragon Ball',               'Toei Animation'),
    ('Dragon Ball Super',                             '2015-07-05', 'High',      'Action',           5, 39.99, 'Dragon Ball',               'Toei Animation'),
    ('Dragon Ball GT',                                '1996-02-07', 'Low',       'Action',           1, 19.99, 'Dragon Ball',               'Toei Animation'),
    ('Naruto Shippuden',                              '2007-02-15', 'Very high', 'Action',          21, 64.99, 'Naruto',                    'Studio Pierrot'),
    ('Boruto: Naruto Next Generations',               '2017-04-05', 'Medium',    'Action',           1, 29.99, 'Naruto',                    'Studio Pierrot'),
    ('The Last: Naruto the Movie',                    '2014-12-06', 'Medium',    'Romance',          1, 12.99, 'Naruto',                    'Studio Pierrot'),
    ('Attack on Titan: No Regrets',                   '2014-12-09', 'Medium',    'Action',           1,  9.99, 'Attack on Titan',           'Wit Studio'),
    ('Attack on Titan: The Final Chapters',           '2023-03-04', 'Very high', 'Action',           1, 19.99, 'Attack on Titan',           'MAPPA'),
    ('My Hero Academia',                              '2016-04-03', 'Very high', 'Action',           7, 49.99, 'My Hero Academia',          'Bones'),
    ('My Hero Academia: Two Heroes',                  '2018-08-03', 'Medium',    'Action',           1, 12.99, 'My Hero Academia',          'Bones'),
    ('Chainsaw Man',                                  '2022-10-12', 'High',      'Action',           1, 24.99, 'Chainsaw Man',              'MAPPA'),
    ('Spy x Family',                                  '2022-04-09', 'Very high', 'Comedy',           2, 34.99, 'Spy x Family',              'Wit Studio'),
    ('Hunter x Hunter',                               '2011-10-02', 'Very high', 'Action',           1, 39.99, 'Hunter x Hunter',           'Madhouse'),
    ('One-Punch Man',                                 '2015-10-05', 'Very high', 'Comedy',           2, 29.99, 'One-Punch Man',             'Madhouse'),
    ('Bleach',                                        '2004-10-05', 'High',      'Action',          16, 54.99, 'Bleach',                    'Studio Pierrot'),
    ('Bleach: Thousand-Year Blood War',               '2022-10-11', 'Very high', 'Action',           3, 34.99, 'Bleach',                    'Studio Pierrot'),
    ('Haikyu!!',                                      '2014-04-06', 'High',      'Sports',           4, 39.99, 'Haikyu!!',                  'Production I.G'),
    ('Vinland Saga',                                  '2019-07-08', 'High',      'Drama',            2, 29.99, 'Vinland Saga',              'Wit Studio'),
    ('Mob Psycho 100',                                '2016-07-12', 'High',      'Comedy',           3, 29.99, 'Mob Psycho 100',            'Bones'),
    ('JoJo''s Bizarre Adventure',                     '2012-10-06', 'High',      'Action',           6, 49.99, 'JoJo''s Bizarre Adventure', 'David Production'),
    ('JoJo''s Bizarre Adventure: Stone Ocean',        '2021-12-01', 'Medium',    'Action',           1, 24.99, 'JoJo''s Bizarre Adventure', 'David Production'),
    ('Tokyo Ghoul',                                   '2014-07-04', 'Medium',    'Thriller',         4, 29.99, 'Tokyo Ghoul',               'Studio Pierrot'),
    ('Tokyo Ghoul:re',                                '2018-04-03', 'Low',       'Thriller',         2, 19.99, 'Tokyo Ghoul',               'Studio Pierrot'),
    ('Jujutsu Kaisen 0',                              '2021-12-24', 'Very high', 'Action',           1, 14.99, 'Jujutsu Kaisen',            'MAPPA'),
    ('Demon Slayer: Mugen Train',                     '2020-10-16', 'Very high', 'Action',           1, 14.99, 'Demon Slayer',              'ufotable'),
    ('Death Note: Relight',                           '2007-08-31', 'Low',       'Thriller',         1,  9.99, 'Death Note',                'Madhouse'),
    ('Fullmetal Alchemist: The Sacred Star of Milos', '2011-07-02', 'Medium',    'Fantasy',          1, 12.99, 'Fullmetal Alchemist',       'Bones'),
    -- Originals / not based on a manga
    ('Cowboy Bebop: The Movie',                       '2001-09-01', 'Medium',    'Science fiction',  1, 12.99, null, 'Bones'),
    ('Code Geass',                                    '2006-10-05', 'Very high', 'Science fiction',  2, 39.99, null, 'Sunrise'),
    ('Mobile Suit Gundam',                            '1979-04-07', 'High',      'Science fiction',  1, 24.99, null, 'Sunrise'),
    ('Mobile Suit Gundam: The Witch from Mercury',    '2022-10-02', 'Medium',    'Science fiction',  2, 29.99, null, 'Sunrise'),
    ('Steins;Gate',                                   '2011-04-06', 'Very high', 'Science fiction',  1, 34.99, null, 'White Fox'),
    ('Steins;Gate 0',                                 '2018-04-12', 'Medium',    'Science fiction',  1, 24.99, null, 'White Fox'),
    ('Re:Zero',                                       '2016-04-04', 'High',      'Fantasy',          3, 34.99, null, 'White Fox'),
    ('Fate/Zero',                                     '2011-10-02', 'High',      'Fantasy',          2, 34.99, null, 'ufotable'),
    ('Fate/stay night: Unlimited Blade Works',        '2014-10-04', 'High',      'Fantasy',          2, 34.99, null, 'ufotable'),
    ('Violet Evergarden',                             '2018-01-11', 'High',      'Drama',            1, 29.99, null, 'Kyoto Animation'),
    ('Clannad',                                       '2007-10-04', 'Medium',    'Romance',          2, 24.99, null, 'Kyoto Animation'),
    ('Sound! Euphonium',                              '2015-04-08', 'Medium',    'Slice of life',    3, 29.99, null, 'Kyoto Animation'),
    ('Ghost in the Shell: Stand Alone Complex',       '2002-10-01', 'High',      'Science fiction',  2, 34.99, null, 'Production I.G'),
    ('Psycho-Pass',                                   '2012-10-12', 'High',      'Thriller',         3, 34.99, null, 'Production I.G'),
    ('Sword Art Online',                              '2012-07-08', 'High',      'Fantasy',          4, 39.99, null, 'A-1 Pictures'),
    ('Anohana',                                       '2011-04-15', 'Medium',    'Drama',            1, 19.99, null, 'A-1 Pictures'),
    ('Wonder Egg Priority',                           '2021-01-13', 'Low',       'Drama',            1, 14.99, null, 'CloverWorks'),
    ('Kill la Kill',                                  '2013-10-04', 'High',      'Action',           1, 24.99, null, 'Studio Trigger'),
    ('Cyberpunk: Edgerunners',                        '2022-09-13', 'High',      'Science fiction',  1, 24.99, null, 'Studio Trigger'),
    ('Promare',                                       '2019-05-24', 'Low',       'Action',           1, 12.99, null, 'Studio Trigger'),
    ('Spirited Away',                                 '2001-07-20', 'Very high', 'Fantasy',          1, 14.99, null, 'Studio Ghibli'),
    ('My Neighbor Totoro',                            '1988-04-16', 'High',      'Fantasy',          1, 12.99, null, 'Studio Ghibli'),
    ('Princess Mononoke',                             '1997-07-12', 'High',      'Fantasy',          1, 14.99, null, 'Studio Ghibli')
) as v(title, released, popularity, genre, seasons, price, manga, studio)
where not exists (select 1 from Anime a where a.Title = v.title);

commit;

select count(*) as anime_total from Anime;
