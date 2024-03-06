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