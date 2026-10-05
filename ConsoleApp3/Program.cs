using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;


interface IVozilo
{
    void Detalji();
}
public class AutoDealer
{
    public string Marka { get; set; }
    public string Model { get; set; }

    public AutoDealer(string marka, string model)
    {
        Marka = marka;
        Model = model;
    }
}

public class Vozilo : AutoDealer, IVozilo
{
    public int GodinaProizvodnje { get; set; }
    public double Cijena { get; set; }
    private string brojSasije;

    public void Detalji()
    {
        Console.WriteLine("Automobil je nov i vrijedi ga kupiti.");
    }

    public Vozilo(string marka, string model, int godinaProizvodnje, double cijena, string brojSasije)
        : base(marka, model)
    {
        GodinaProizvodnje = godinaProizvodnje;
        Cijena = cijena;
        this.brojSasije = brojSasije;
    }

    public string SkrivenaSasija()
    {
        return "***************";
    }
}

class Program
{
    static void Main(string[] args)
    {

        List<Vozilo> vozila = new List<Vozilo>();
        vozila.Add(new Vozilo("Toyota", "Corolla", 2020, 13990.99, "IITNFDIF83929"));

        Console.Write("Unesite marku vozila: ");
        string unesenaMarka = (Console.ReadLine() ?? "").Trim();

        bool markaPronadena = false;

        foreach (Vozilo v in vozila)
        {
            if (v.Marka.Equals(unesenaMarka, StringComparison.OrdinalIgnoreCase))
            {
                markaPronadena = true;
                Console.WriteLine($"Vozilo pronađeno: {v.Marka} {v.Model} ({v.GodinaProizvodnje}), {v.Cijena}");
                Console.WriteLine($"Broj Sasije: {v.SkrivenaSasija()}");
                v.Detalji();
            }
        }

        if (!markaPronadena)
        {
            Console.WriteLine("Vozilo s unesenom markom nije pronađeno.");
        }

    }
}