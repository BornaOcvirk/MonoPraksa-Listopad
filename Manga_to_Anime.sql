drop table if exists Studio cascade;
drop table if exists Manga cascade;
drop table if exists Anime cascade;

Create table Studio(
	Studio_Id int primary key not null,
	Name_s varchar(255) unique not null,
	country varchar(255)
);

Create table Manga(
	Manga_Id int primary key not null,
	Author varchar(255) not null,
	Title varchar(255) not null,
	Volumes int not null,
	Popularity varchar(255),
	Studio_Id int not null,
	foreign key (Studio_Id) references Studio(Studio_Id)
);

Create table Anime(
	Anime_Id int primary key not null,
	Title varchar not null,
	Relese_date date not null,
	Popularity varchar(255),
	Manga_Id int,
	Studio_Id int not null,
	foreign key (Manga_Id) references Manga(Manga_Id),
	foreign key (Studio_Id) references Studio(Studio_Id) 
);

Insert into Studio (Studio_Id, Name_s, country) values
(1, 'Bones',         'Japan'),
(2, 'Wit Studio',    'Japan'),
(3, 'MAPPA',         'Japan'),
(4, 'Madhouse',      'Japan'),
(5, 'Studio Pierrot','Japan'),
(6, 'ufotable',      'Japan'),
(7, 'Sunrise',       'Japan');

Insert into Manga (Manga_Id, Author, Title, Volumes, Popularity, Studio_Id) values
(1, 'Hiromu Arakawa',               'Fullmetal Alchemist', 27, 'Very high', 1),
(2, 'Hajime Isayama',               'Attack on Titan',     34, 'Very high', 2),
(3, 'Tsugumi Ohba, Takeshi Obata',  'Death Note',          12, 'Very high', 4),
(4, 'Masashi Kishimoto',            'Naruto',              72, 'Very high', 5),
(5, 'Koyoharu Gotouge',             'Demon Slayer',        23, 'Very high', 6),
(6, 'Gege Akutami',                 'Jujutsu Kaisen',      30, 'High',      3);

Insert into Anime (Anime_Id, Title, Relese_date, Popularity, Manga_Id, Studio_Id) values
(1, 'Fullmetal Alchemist',               '2003-10-04', 'High',      1,    1),
(2, 'Fullmetal Alchemist: Brotherhood',  '2009-04-05', 'Very high', 1,    1),
(3, 'Attack on Titan',                   '2013-04-07', 'Very high', 2,    2),
(4, 'Attack on Titan: The Final Season', '2020-12-07', 'Very high', 2,    3),
(5, 'Death Note',                        '2006-10-04', 'Very high', 3,    4),
(6, 'Naruto',                            '2002-10-03', 'Very high', 4,    5),
(7, 'Demon Slayer',                      '2019-04-06', 'Very high', 5,    6),
(8, 'Jujutsu Kaisen',                    '2020-10-03', 'Very high', 6,    3),
(9, 'Cowboy Bebop',                      '1998-04-03', 'High',      null, 7);

Alter table Studio 
add column Established date;

update Studio set Established = '1998-10-01' where Studio_id = 1;  
update Studio set Established = '2012-06-01' where Studio_id = 2;  
update Studio set Established = '2011-06-14' where Studio_id = 3;  
update Studio set Established = '1972-10-17' where Studio_id = 4; 
update Studio set Established = '1979-05-01' where Studio_id = 5;  
update Studio set Established = '2000-10-01' where Studio_id = 6;  
update Studio set Established = '1972-09-01' where Studio_id = 7;

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