USE VR_CLUB_DATABASE

CREATE TABLE ������������(
[Id ������������] INT NOT NULL PRIMARY KEY,
��� NCHAR(30) NOT NULL,
����� NCHAR(30) NOT NULL,
������� NCHAR(12) NOT NULL,
������ NCHAR(15) NOT NULL,
������ NCHAR(1) NOT NULL
)

CREATE TABLE ������(
[ID ������] INT NOT NULL PRIMARY KEY,
����� nchar(400) NOT NULL,
���� DATE NOT NULL,
�������� NCHAR(30) NOT NULL,
[Id ������������] INT NOT NULL,

FOREIGN KEY ([Id ������������]) REFERENCES ������������ ([Id ������������])
)


CREATE TABLE ������(
[ID ����������] INT NOT NULL PRIMARY KEY,
���������� NCHAR(50) NOT NULL
)


CREATE TABLE [������ ���](
[ID ������ ���] INT NOT NULL PRIMARY KEY,
������� INT NOT NULL,
���� NCHAR(50) NOT NULL,
���������� INT NOT NULL
)

CREATE TABLE ����(
[ID �����] INT NOT NULL PRIMARY KEY,
����� NCHAR(50) NOT NULL,
������� NCHAR(12) NOT NULL,
����� NCHAR(30) NOT NULL,

[ID ������ ���] INT NOT NULL,
[ID ����������] INT NOT NULL,

FOREIGN KEY ([ID ������ ���]) REFERENCES [������ ���] ([ID ������ ���]),
FOREIGN KEY ([ID ����������]) REFERENCES ������ ([ID ����������])
)


CREATE TABLE �����(
�������� NCHAR(20) NOT NULL PRIMARY KEY,
���� NCHAR(15) NOT NULL,

[ID �����] INT NOT NULL,

FOREIGN KEY ([ID �����]) REFERENCES ���� ([ID �����])
)

CREATE TABLE ������(
[ID ������] INT NOT NULL PRIMARY KEY,
[���������� �������] INT NOT NULL,
���� DATE NOT NULL,
[���� ����������] DATE NOT NULL,
���� NCHAR(15) NOT NULL,

[ID �����] INT NOT NULL,
[Id ������������] INT NOT NULL,

FOREIGN KEY ([ID �����]) REFERENCES ���� ([ID �����]),
FOREIGN KEY ([Id ������������]) REFERENCES ������������ ([Id ������������])
)


