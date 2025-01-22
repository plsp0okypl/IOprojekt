using System;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\studentath\Documents\GitHub\IOprojekt\baza danych i klasy\Database1.mdf;Integrated Security=True";

        var dbHelper = new DatabaseHelper(connectionString);

        try
        {
            // Upewnij się, że baza danych i tabela istnieją
            dbHelper.EnsureDatabaseAndTableExist();

            // Dodaj domyślne produkty
            dbHelper.AddDefaultProducts();

            // Wyświetl menu z opcjami
            dbHelper.DisplayMenu();

            // Opcjonalnie: wyświetl rachunek
            dbHelper.DisplayReceipt();

            Console.WriteLine("Operacja zakończona!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }

}
