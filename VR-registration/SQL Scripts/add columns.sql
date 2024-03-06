-- Добавление колонок в таблицу
ALTER TABLE Пользователь
ADD Пароль nchar(15) not null,
Статус nchar(1) not null