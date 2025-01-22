using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

public class Produkt
{
    public int Id { get; set; }
    public string Nazwa { get; set; }
    public decimal Cena { get; set; }
    public string Opis { get; set; }
    public int Ilosc { get; set; }
}

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Produkt> GetProducts()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT * FROM Produkty";
        return connection.Query<Produkt>(query);
    }

    public void EnsureDatabaseAndTableExist()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

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
    }

    public void EnsureProductExists(Produkt produkt)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"
        IF NOT EXISTS (SELECT 1 FROM Produkty WHERE Id = @Id)
        BEGIN
            INSERT INTO Produkty (Id, Nazwa, Cena, Opis, Ilosc)
            VALUES (@Id, @Nazwa, @Cena, @Opis, @Ilosc);
        END";

        connection.Execute(query, produkt);
    }

    public void AddDefaultProducts()
    {
        // Dodajemy domyślne produkty
        EnsureProductExists(new Produkt { Id = 1, Nazwa = "Laptop", Cena = 4999.99m, Opis = "Nowoczesny laptop z 16GB RAM i 512GB SSD", Ilosc = 10 });
        EnsureProductExists(new Produkt { Id = 2, Nazwa = "Komputer", Cena = 3499.99m, Opis = "Stacjonarny komputer do gier z RTX 3060", Ilosc = 5 });
        EnsureProductExists(new Produkt { Id = 3, Nazwa = "Telefon", Cena = 2499.99m, Opis = "Smartfon z ekranem OLED 6.5 cala i 128GB pamięci", Ilosc = 20 });
    }

    public void DisplayReceipt()
    {
        var products = GetProducts().ToList();

        Console.WriteLine("\nRACHUNEK:");
        double sumPrice = 0;

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(new string('-', 60));
        Console.WriteLine("| {0,-5} | {1,-20} | {2,-10} | {3,-10} |", "Ilość", "Produkt", "Cena Jedn.", "Razem");
        Console.WriteLine(new string('-', 60));

        foreach (var product in products)
        {
            double totalPrice = (double)(product.Cena * product.Ilosc);

            Console.WriteLine("| {0,-5} | {1,-20} | {2,-10:C} | {3,-10:C} |", product.Ilosc, product.Nazwa, product.Cena, totalPrice);

            sumPrice += totalPrice;
        }

        Console.WriteLine(new string('-', 60));
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nSuma do zapłaty: {0:C}", sumPrice);

        HandlePayment(sumPrice);
    }

    private void HandlePayment(double sumPrice)
    {
        double cash = 0.0;
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Otrzymana gotówka:");
            if (double.TryParse(Console.ReadLine(), out cash) && cash > 0)
            {
                double rest = cash - sumPrice;
                if (rest >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Reszta: {0:C}", rest);
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nie stać Cię.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Błędna wartość, podaj kwotę.");
                Console.ResetColor();
            }
        } while (true);
    }

    public void DisplayMenu()
    {
        var products = GetProducts().ToList();
        bool close = false;

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n==================================");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("               MENU               ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("==================================");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("1. Pokaż najtańszy produkt");
        Console.WriteLine("2. Pokaż najdroższy produkt");
        Console.WriteLine("3. Pokaż liczbę wszystkich produktów");
        Console.WriteLine("4. Pokaż produkt, którego sprzedano najwięcej");
        Console.WriteLine("5. Pokaż produkt, którego sprzedano najmniej");
        Console.WriteLine("6. Wyjście");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("==================================");
        Console.ResetColor();

        do
        {
            int opcja;
            do
            {
                Console.WriteLine("Wybierz opcję (1-6): ");
                if (int.TryParse(Console.ReadLine(), out opcja) && opcja >= 1 && opcja <= 6)
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Błędna wartość, wybierz opcję z zakresu 1-6.");
                    Console.ResetColor();
                }
            } while (true);

            switch (opcja)
            {
                case 1:
                    var minPriceProduct = products.OrderBy(p => p.Cena).First();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Najtańszy produkt: {minPriceProduct.Nazwa}, cena: {minPriceProduct.Cena:C}");
                    break;
                case 2:
                    var maxPriceProduct = products.OrderByDescending(p => p.Cena).First();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Najdroższy produkt: {maxPriceProduct.Nazwa}, cena: {maxPriceProduct.Cena:C}");
                    break;
                case 3:
                    int totalCount = products.Sum(p => p.Ilosc);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Łączna liczba sprzedanych produktów: {totalCount}");
                    break;
                case 4:
                    var maxSoldProduct = products.OrderByDescending(p => p.Ilosc).First();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Produkt, którego sprzedano najwięcej: {maxSoldProduct.Nazwa}, ilość: {maxSoldProduct.Ilosc}");
                    break;
                case 5:
                    var minSoldProduct = products.OrderBy(p => p.Ilosc).First();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Produkt, którego sprzedano najmniej: {minSoldProduct.Nazwa}, ilość: {minSoldProduct.Ilosc}");
                    break;
                case 6:
                    close = true;
                    break;
            }
        } while (!close);
    }
}
