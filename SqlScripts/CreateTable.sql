create table Bars
(
	Id int primary key identity,
	BarName varchar(50) not null,
	BarImage varchar(50) not null,
	BarRating float null,
	BarLocation varchar(50) not null,
)

create table Users
(
	Id int primary key identity,
	UserLogin varchar(50) not null unique,
	UserPassword varchar(50) not null,
	UserRole varchar(50) not null	
)

create table PersonalBestBars
(
	Id int primary key identity,
	BarId int not null references Bars(Id) on delete cascade,
	UserId int not null references Users(Id) on delete cascade
)

create table Comments
(
	Id int primary key identity,
	BarId int not null references Bars(Id) on delete cascade,
	UserId int not null references Users(Id) on delete cascade,
	Comment varchar(50) not null
)