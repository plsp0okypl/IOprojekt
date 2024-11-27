using System;

public class Produkt
{
    // Właściwości klasy Produkt
    public int Id { get; set; }
    public string Nazwa { get; set; }
    public decimal Cena { get; set; }
    public string Opis { get; set; }

    public int Ilosc { get; set; } //ilosc produktu w magazynie

    public Produkt()
    {

    }

    // Konstruktor klasy Produkt
    public Produkt(int id, string nazwa, decimal cena, string opis)
    {
        Id = id;
        Nazwa = nazwa;
        Cena = cena;
        Opis = opis;
    }

    // Metoda do wyświetlania szczegółów produktu
    public void WyswietlSzczegoly()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Nazwa: {Nazwa}");
        Console.WriteLine($"Cena: {Cena:C}");
        Console.WriteLine($"Opis: {Opis}");
    }
}


