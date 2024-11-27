using Dapper;
using Microsoft.Data.SqlClient;
using System;

class Produkt
{
    public int Id { get; set; }
    public string Nazwa { get; set; }
    public decimal Cena { get; set; }
    public string Opis { get; set; }
    public int Ilosc { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Połączenie z bazą danych
        using var connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\source\repos\ConsoleApp1\ConsoleApp1\Database1.mdf;Integrated Security=True");
        connection.Open();

        // Sprawdzanie, czy tabela istnieje, a jeśli nie, tworzenie jej
        var createTableQuery = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Produkty')
        BEGIN
            CREATE TABLE Produkty (
                Id INT PRIMARY KEY,
                Nazwa NVARCHAR(100),
                Cena DECIMAL(10, 2),
                Opis NVARCHAR(255),
                Ilosc INT
            );
        END;";

        connection.Execute(createTableQuery);

        // Dodanie danych do tabeli Produkty, jeśli nie istnieją
        var insertDataQuery = @"
        IF NOT EXISTS (SELECT 1 FROM Produkty WHERE Id = 1)
        BEGIN
            INSERT INTO Produkty (Id, Nazwa, Cena, Opis, Ilosc)
            VALUES 
            (1, 'Laptop', 2999.99, 'Wydajny laptop do pracy i gier', 10),
            (2, 'Smartfon', 1499.99, 'Nowoczesny smartfon z dużym ekranem', 20),
            (3, 'Słuchawki', 299.99, 'Bezprzewodowe słuchawki z funkcją ANC', 15),
            (4, 'Monitor', 799.99, 'Monitor 24 Full HD', 30),
            (5, 'Klawiatura', 199.99, 'Ergonomiczna klawiatura do biura', 50);
        END; ";


        connection.Execute(insertDataQuery);

        // Pobranie danych z tabeli Produkty
        var products = connection.Query<Produkt>("SELECT * FROM Produkty");

        // Wyświetlenie danych w konsoli
        Console.WriteLine("Produkty w bazie danych:");
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("ID\tNazwa\tCena\t\tOpis\t\t\t\tIlość");


        Console.WriteLine("Operacja zakończona!");
    }
}

