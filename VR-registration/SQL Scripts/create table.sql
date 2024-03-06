USE VR_CLUB_DATABASE

CREATE TABLE Пользователь(
[Id пользователя] INT NOT NULL PRIMARY KEY,
ФИО NCHAR(30) NOT NULL,
Почта NCHAR(30) NOT NULL,
Телефон NCHAR(12) NOT NULL,
Пароль NCHAR(15) NOT NULL,
Статус NCHAR(1) NOT NULL
)

CREATE TABLE Жалобы(
[ID жалобы] INT NOT NULL PRIMARY KEY,
Текст nchar(400) NOT NULL,
Дата DATE NOT NULL,
Название NCHAR(30) NOT NULL,
[Id пользователя] INT NOT NULL,

FOREIGN KEY ([Id пользователя]) REFERENCES Пользователь ([Id пользователя])
)


CREATE TABLE График(
[ID расписания] INT NOT NULL PRIMARY KEY,
Расписание NCHAR(50) NOT NULL
)


CREATE TABLE [Список игр](
[ID Списка игр] INT NOT NULL PRIMARY KEY,
Рейтинг INT NOT NULL,
Игра NCHAR(50) NOT NULL,
Совместная INT NOT NULL
)

CREATE TABLE Клуб(
[ID клуба] INT NOT NULL PRIMARY KEY,
Адрес NCHAR(50) NOT NULL,
Телефон NCHAR(12) NOT NULL,
Почта NCHAR(30) NOT NULL,

[ID Списка игр] INT NOT NULL,
[ID расписания] INT NOT NULL,

FOREIGN KEY ([ID Списка игр]) REFERENCES [Список игр] ([ID Списка игр]),
FOREIGN KEY ([ID расписания]) REFERENCES График ([ID расписания])
)


CREATE TABLE Тариф(
Название NCHAR(20) NOT NULL PRIMARY KEY,
Цена NCHAR(15) NOT NULL,

[ID клуба] INT NOT NULL,

FOREIGN KEY ([ID клуба]) REFERENCES Клуб ([ID клуба])
)

CREATE TABLE Запись(
[ID записи] INT NOT NULL PRIMARY KEY,
[Количество человек] INT NOT NULL,
Дата DATE NOT NULL,
[Дата проведения] DATE NOT NULL,
Цена NCHAR(15) NOT NULL,

[ID клуба] INT NOT NULL,
[Id пользователя] INT NOT NULL,

FOREIGN KEY ([ID клуба]) REFERENCES Клуб ([ID клуба]),
FOREIGN KEY ([Id пользователя]) REFERENCES Пользователь ([Id пользователя])
)


