using System;
using System.Collections;
using System.ComponentModel.Design;
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

public class Vozilo : AutoDealer
{
    public int GodinaProizvodnje { get; set; }
    public double Cijena { get; set; }
    public Vozilo(string marka, string model, int godinaProizvodnje, double cijena) : base(marka, model)
    {
        GodinaProizvodnje = godinaProizvodnje;
        Cijena = cijena;
    }
}
    class Program
    {
        static void Main(string[] args)
        {
        List<Vozilo> vozila = new List<Vozilo>();
        vozila.Add(new Vozilo("Toyota", "Corolla", 2020, 13990.99));

        Console.Write("Unesite marku vozila: ");
        string Unesena_marka = Console.ReadLine();

        bool Marka_pronadena = false;

        for (int i = 0; i < vozila.Count; i++)
        {
            Vozilo v = vozila[i];
            if (v.Marka.Equals(Unesena_marka))
            {
                Marka_pronadena = true;
                Console.WriteLine($"Vozilo pronađeno: {v.Marka} {v.Model}");
            }
        }
        if (!Marka_pronadena)
        {
            Console.WriteLine("Vozilo sa unesenom markom nije pronađeno.");
        }
        }
    }

