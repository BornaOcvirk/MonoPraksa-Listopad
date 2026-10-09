drop table if exists Studio cascade;
drop table if exists Manga cascade;
drop table if exists Anime cascade;


Create table Users(
	User_Id uuid default gen_random_uuid() primary key,
	Username varchar(255) unique not null,
	Password_hash text not null,
	Role varchar(255) not null default 'Customer'
					  check(role in('Customer', 'Admin'))
);

Create table Studio(
	Studio_Id uuid default gen_random_uuid() primary key,
	Name_s varchar(255) unique not null,
	country varchar(255)
);

Create table Manga(
	Manga_Id uuid default gen_random_uuid() primary key,
	Author varchar(255) not null,
	Title varchar(255) not null,
	Volumes int not null,
	Popularity varchar(255),
	Studio_Id uuid not null,
	foreign key (Studio_Id) references Studio(Studio_Id)
);

Create table Anime(
	Anime_Id uuid default gen_random_uuid() primary key,
	Title varchar not null,
	Relese_date date not null,
	Popularity varchar(255),
	Manga_Id uuid,
	Studio_Id uuid not null,
	foreign key (Manga_Id) references Manga(Manga_Id),
	foreign key (Studio_Id) references Studio(Studio_Id) 
);

Insert into Studio (Name_s, country) values
('Bones',         'Japan'),
('Wit Studio',    'Japan'),
('MAPPA',         'Japan'),
('Madhouse',      'Japan'),
('Studio Pierrot','Japan'),
('ufotable',      'Japan'),
('Sunrise',       'Japan');

Insert into Manga (Author, Title, Volumes, Popularity, Studio_Id) values
('Hiromu Arakawa',              'Fullmetal Alchemist', 27, 'Very high', (select Studio_Id from Studio where Name_s = 'Bones')),
('Hajime Isayama',              'Attack on Titan',     34, 'Very high', (select Studio_Id from Studio where Name_s = 'Wit Studio')),
('Tsugumi Ohba, Takeshi Obata', 'Death Note',          12, 'Very high', (select Studio_Id from Studio where Name_s = 'Madhouse')),
('Masashi Kishimoto',           'Naruto',              72, 'Very high', (select Studio_Id from Studio where Name_s = 'Studio Pierrot')),
('Koyoharu Gotouge',            'Demon Slayer',        23, 'Very high', (select Studio_Id from Studio where Name_s = 'ufotable')),
('Gege Akutami',                'Jujutsu Kaisen',      30, 'High',      (select Studio_Id from Studio where Name_s = 'MAPPA'));

Insert into Anime (Title, Relese_date, Popularity, Manga_Id, Studio_Id) values
('Fullmetal Alchemist',               '2003-10-04', 'High',
	(select Manga_Id from Manga where Title = 'Fullmetal Alchemist'), (select Studio_Id from Studio where Name_s = 'Bones')),
('Fullmetal Alchemist: Brotherhood',  '2009-04-05', 'Very high',
	(select Manga_Id from Manga where Title = 'Fullmetal Alchemist'), (select Studio_Id from Studio where Name_s = 'Bones')),
('Attack on Titan',                   '2013-04-07', 'Very high',
	(select Manga_Id from Manga where Title = 'Attack on Titan'),     (select Studio_Id from Studio where Name_s = 'Wit Studio')),
('Attack on Titan: The Final Season', '2020-12-07', 'Very high',
	(select Manga_Id from Manga where Title = 'Attack on Titan'),     (select Studio_Id from Studio where Name_s = 'MAPPA')),
('Death Note',                        '2006-10-04', 'Very high',
	(select Manga_Id from Manga where Title = 'Death Note'),          (select Studio_Id from Studio where Name_s = 'Madhouse')),
('Naruto',                            '2002-10-03', 'Very high',
	(select Manga_Id from Manga where Title = 'Naruto'),              (select Studio_Id from Studio where Name_s = 'Studio Pierrot')),
('Demon Slayer',                      '2019-04-06', 'Very high',
	(select Manga_Id from Manga where Title = 'Demon Slayer'),        (select Studio_Id from Studio where Name_s = 'ufotable')),
('Jujutsu Kaisen',                    '2020-10-03', 'Very high',
	(select Manga_Id from Manga where Title = 'Jujutsu Kaisen'),      (select Studio_Id from Studio where Name_s = 'MAPPA')),
('Cowboy Bebop',                      '1998-04-03', 'High',
	null,                                                             (select Studio_Id from Studio where Name_s = 'Sunrise'));

Alter table Studio 
add column Established date;

update Studio set Established = '1998-10-01' where Name_s = 'Bones';
update Studio set Established = '2012-06-01' where Name_s = 'Wit Studio';
update Studio set Established = '2011-06-14' where Name_s = 'MAPPA';
update Studio set Established = '1972-10-17' where Name_s = 'Madhouse';
update Studio set Established = '1979-05-01' where Name_s = 'Studio Pierrot';
update Studio set Established = '2000-10-01' where Name_s = 'ufotable';
update Studio set Established = '1972-09-01' where Name_s = 'Sunrise';

INSERT INTO users (username, password_hash, role) VALUES
    ('borna', 'AQAAAAIAAYagAAAAEApjXMSimG9fH5v5O5eKG2orG3bQcaQBtn/HB/ruCPE3u9WTjvpEjgr3kXYBRbMJyQ==', 'Admin'),
    ('sakura_fan', 'AQAAAAIAAYagAAAAEA551aZWWd/cXhnER7YY4ldAgmHhIEWHdyS3O9ZIVQWzvZ7u08DJDGSWzxqcVX59ww==', 'Customer'),
    ('naruto_uzumaki', 'AQAAAAIAAYagAAAAEDh2142QukQD1aNu4dknAo6GwUrhLsMlBfekBjnZ3MLQl+5mUgOJpn52ebFfBcjYGA==', 'Customer'),
    ('mikasa_a', 'AQAAAAIAAYagAAAAEMR1ip1GWtcNBc82fSldDE6hezcdpcnPOfTxtSwFNBloTmvSH1xxo5E0q2tdT05hiQ==', 'Customer'),
    ('luffy_d', 'AQAAAAIAAYagAAAAEOo4VXNRfGJZBR65Khb2dnbyFp3BUruok3D7YkM1eRZo4u3iIU4cmzySfMVszh8ksA==', 'Customer'),
    ('tanjiro_k', 'AQAAAAIAAYagAAAAENl2WfsCsUpfOpUC1AJearWXqKepCcp1PtWtxBCQ/LFTRcpfjyCLHXqhS5slSNXEFg==', 'Customer'),
    ('goku_ssj', 'AQAAAAIAAYagAAAAEDNGbKvVniP4okkmU8utk8qVRSARR95ClQhWI6N1R0x0WByGhhJGFDMlTvusxagwzg==', 'Customer'),
    ('levi_ack', 'AQAAAAIAAYagAAAAEGlzHqidIUvlD9wyzUbUqEtR3L+2MevgtF77SYdweHSVLit3eo/sIQfr3cw0l9ZrmQ==', 'Customer'),
    ('nami_nav', 'AQAAAAIAAYagAAAAEAsT9IX5+5F60j/O2Lj+k4gx7YQuF616enyu1tz7yMez2+PsAbGWhcAEOD9fCJyNlA==', 'Customer'),
    ('eren_y', 'AQAAAAIAAYagAAAAEDyfBik5XtS543SJW0Xpw4CCW7534RGHONBwA1/gEzKDNL95DKMfbF3YVN8eRVjg3g==', 'Customer')
ON CONFLICT (username) DO NOTHING;

--order with where
Select M.Title as Manga_title from Manga M
where Volumes between 20 and 50
order by M.Title

--inner join
Select Name_s as Ime_Studia, Author, A.Popularity as Anime_Popularity from Studio
inner join Anime A using (Studio_Id)
inner join Manga using (Manga_Id)
order by Author;

--left join (promijena je da se vidi i popularnost za anime bez autora)
Select Name_s as Ime_Studia, Author, A.Popularity as Anime_Popularity from Studio
left join Anime A using(Studio_Id)
left join Manga using (Manga_Id)
order by Author;

--group by
Select Name_s as Ime_Studia from Studio
inner join Anime using (Studio_Id)
inner join Manga using (Manga_Id)
where Volumes > 20
group by Name_s

--group by and count
select Studio_id, Name_s, count(*) as number_of_anime
from Anime inner join Studio using(Studio_Id)
group by Studio_Id, Name_s
order by count(*);

--transaction
begin;
drop table if exists Studio cascade;
rollback;

select * from Studio
select * from Anime
select * from Manga

alter table Anime
add column Genre varchar(100),
add column Number_of_seasons int,
add column Price numeric(10,2);

update Anime set Genre = 'Fantasy',         Number_of_seasons = 1, Price = 29.99 where Title = 'Fullmetal Alchemist';
update Anime set Genre = 'Fantasy',         Number_of_seasons = 1, Price = 39.99 where Title = 'Fullmetal Alchemist: Brotherhood';
update Anime set Genre = 'Action',          Number_of_seasons = 3, Price = 49.99 where Title = 'Attack on Titan';
update Anime set Genre = 'Action',          Number_of_seasons = 1, Price = 34.99 where Title = 'Attack on Titan: The Final Season';
update Anime set Genre = 'Thriller',        Number_of_seasons = 1, Price = 24.99 where Title = 'Death Note';
update Anime set Genre = 'Action',          Number_of_seasons = 5, Price = 59.99 where Title = 'Naruto';
update Anime set Genre = 'Action',          Number_of_seasons = 4, Price = 44.99 where Title = 'Demon Slayer';
update Anime set Genre = 'Action',          Number_of_seasons = 2, Price = 39.99 where Title = 'Jujutsu Kaisen';
update Anime set Genre = 'Science fiction', Number_of_seasons = 1, Price = 27.99 where Title = 'Cowboy Bebop';

select column_name from information_schema.columns where table_name = 'anime';
select * from Users