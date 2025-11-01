# Accounting for Computers and Components 🖥️🔧

Windows Forms приложение для учёта компьютеров и комплектующих, написанное на **C#** (.NET Framework) с использованием **SQL Server** и **Windows Forms**.

## 💬 Обсуждения
Есть вопросы или предложения? Присоединяйтесь к [обсуждениям](https://github.com/DazaiOsamu132/Accounting-for-computers-and-components-/discussions) репозитория!

## 📸 Скриншоты интерфейса

![ окно авторизации ](https://github.com/user-attachments/assets/78d09166-0ec7-4a65-8ee7-ec67de15b926)  
![Главное окно приложения)(компьютеры)](https://github.com/user-attachments/assets/db358a61-16c7-4a7a-8359-6525ada8147f)  
![Главное окно приложения)(комплектующие)](https://github.com/user-attachments/assets/658944c2-63b5-4003-950e-ba6a64f547b3)  
![Окно создания пользователя](https://github.com/user-attachments/assets/c7198995-2229-4e11-9f20-362b17ed829b)  
![Окно управления пользователями](https://github.com/user-attachments/assets/2dd01bf4-2562-4de1-9853-fa1a3e4d4b54)

## 🗃️ Структура базы данных

![Схема базы данных](https://github.com/user-attachments/assets/4daf76ef-899b-434d-a178-e130c1577b04)

## 📌 Основные возможности
- Полнофункциональный графический интерфейс (Windows Forms)
- Полный цикл работы с данными:
  - Добавление новых записей
  - Редактирование существующих данных
  - Удаление информации
- Интеллектуальный поиск по:
  - Наименованию оборудования
  - Техническим характеристикам
  - Прочим параметрам
- Формирование отчетов в различных форматах
  - Экспорт данных (Excel)


## 🛠️ Системные требования
| Компонент | Минимальные требования |
|-----------|------------------------|
| Операционная система | Windows 10/11 |
| .NET Framework | Версия 4.7.2 или выше |
| Сервер БД | Microsoft SQL Server 2019+ |
| Утилиты | SQL Server Management Studio 18+ |

## 🚀 Установка и настройка

### 1. Скачивание проекта
1. На главной странице репозитория нажмите кнопку `<> Code`
2. Выберите опцию `Download ZIP`
3. Распакуйте архив в удобное место на диске

### 2. Восстановление базы данных
1. Запустите **SQL Server Management Studio (SSMS)**
2. Подключитесь к вашему SQL Server
3. В обозревателе объектов:
   - Кликните правой кнопкой на `Databases`
   - Выберите `Import Data-tier Application`
   - Укажите путь к файлу `HRYBD.bacpac`
   - Завершите процесс импорта

### 3. Настройка подключения
Откройте файл `database.cs` и измените строку подключения:

```csharp
// Для Windows-аутентификации:
SqlConnection sqlConnection = new SqlConnection(
    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HRYBD;Integrated Security=True;");

// Для SQL-аутентификации:
SqlConnection sqlConnection = new SqlConnection(
    @"Data Source=ИМЯ_СЕРВЕРА;Initial Catalog=HRYBD;User ID=логин;Password=пароль;");
```

📜 License: See [![License](https://img.shields.io/badge/License-Custom-blue)](./LICENSE.md)


