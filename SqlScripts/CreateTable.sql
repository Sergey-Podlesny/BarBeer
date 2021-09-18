create table Bars
(
	Id int primary key identity,
	BarName nvarchar(50) not null,
	BarImage nvarchar(50) not null,
	BarRating float null,
	BarLocation nvarchar(50) not null,
)

create table Users
(
	Id int primary key identity,
	UserLogin nvarchar(50) not null unique,
	UserPassword nvarchar(50) not null,
	UserRole nvarchar(50) not null	
)

create table PersonalBestBars
(
	Id int primary key identity,
	BarId int not null references Bars(Id),
	UserId int not null references Users(Id)
)

create table Comments
(
	Id int primary key identity,
	BarId int not null references Bars(Id),
	UserId int not null references Users(Id),
	Comment nvarchar(50) not null
)