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