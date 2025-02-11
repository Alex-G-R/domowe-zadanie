using System;
using System.Collections.Generic;

public class Pojazd
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public int RokProdukcji { get; set; }

    public Pojazd(string marka, string model, int rokProdukcji)
    {
        Marka = marka;
        Model = model;
        RokProdukcji = rokProdukcji;
    }

    public virtual string OpisPojazdu()
    {
        return $"Marka: {Marka}, Model: {Model}, Rok produkcji: {RokProdukcji}";
    }
}

public abstract class PojazdSilnikowy : Pojazd
{
    public double PojemnoscSilnika { get; set; }

    public PojazdSilnikowy(string marka, string model, int rokProdukcji, double pojemnoscSilnika)
        : base(marka, model, rokProdukcji)
    {
        PojemnoscSilnika = pojemnoscSilnika;
    }

    public abstract double KosztPaliwa(int km);
}

public class Samochod : PojazdSilnikowy
{
    public int LiczbaDrzwi { get; set; }

    public Samochod(string marka, string model, int rokProdukcji, double pojemnoscSilnika, int liczbaDrzwi)
        : base(marka, model, rokProdukcji, pojemnoscSilnika)
    {
        LiczbaDrzwi = liczbaDrzwi;
    }

    public override double KosztPaliwa(int km)
    {
        return (5.0 * km / 100) * 6;
    }

    public override string OpisPojazdu()
    {
        return base.OpisPojazdu() + $", Liczba drzwi: {LiczbaDrzwi}";
    }
}

public class Motocykl : PojazdSilnikowy
{
    public string TypMotocykla { get; set; }

    public Motocykl(string marka, string model, int rokProdukcji, double pojemnoscSilnika, string typMotocykla)
        : base(marka, model, rokProdukcji, pojemnoscSilnika)
    {
        TypMotocykla = typMotocykla;
    }

    public override double KosztPaliwa(int km)
    {
        return (4.0 * km / 100) * 6;
    }

    public override string OpisPojazdu()
    {
        return base.OpisPojazdu() + $", Typ motocykla: {TypMotocykla}";
    }
}

public class PojazdElektryczny : Pojazd
{
    public int Zasieg { get; set; }
    public double CzasLadowania { get; set; }

    public PojazdElektryczny(string marka, string model, int rokProdukcji, int zasieg, double czasLadowania)
        : base(marka, model, rokProdukcji)
    {
        Zasieg = zasieg;
        CzasLadowania = czasLadowania;
    }

    public double CzasNaPrzejazd(int km)
    {
        if (km <= Zasieg) return 0;
        int liczbaLadowan = (km - 1) / Zasieg;
        return liczbaLadowan * CzasLadowania;
    }

    public override string OpisPojazdu()
    {
        return base.OpisPojazdu() + $", Zasięg: {Zasieg} km, Czas ładowania: {CzasLadowania} h";
    }
}

public class Hybryda : PojazdSilnikowy
{
    public int ZasiegNaPradzie { get; set; }
    public string TrybAktualny { get; set; }

    public Hybryda(string marka, string model, int rokProdukcji, double pojemnoscSilnika, int zasiegNaPradzie, string trybAktualny)
        : base(marka, model, rokProdukcji, pojemnoscSilnika)
    {
        ZasiegNaPradzie = zasiegNaPradzie;
        TrybAktualny = trybAktualny.ToLower();
    }

    public void ZmienTryb()
    {
        TrybAktualny = TrybAktualny == "elektryczny" ? "spalinowy" : "elektryczny";
    }

    public override double KosztPaliwa(int km)
    {
        if (TrybAktualny == "elektryczny" && km <= ZasiegNaPradzie)
        {
            return 0;
        }
        return (5.0 * km / 100) * 6;
    }

    public override string OpisPojazdu()
    {
        return base.OpisPojazdu() + $", Zasięg na prądzie: {ZasiegNaPradzie} km, Tryb: {TrybAktualny}";
    }
}

public class Flota
{
    private List<Pojazd> pojazdy = new List<Pojazd>();

    public void DodajPojazd(Pojazd pojazd)
    {
        pojazdy.Add(pojazd);
    }

    public void WyswietlFlote()
    {
        foreach (var pojazd in pojazdy)
        {
            Console.WriteLine(pojazd.OpisPojazdu());
        }
    }

    public double ObliczCalkowityKoszt(int km)
    {
        double suma = 0;
        foreach (var pojazd in pojazdy)
        {
            if (pojazd is PojazdSilnikowy ps)
            {
                suma += ps.KosztPaliwa(km);
            }
        }
        return suma;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Flota flota = new Flota();

        flota.DodajPojazd(new Samochod("Toyota", "Corolla", 2020, 1.8, 4));
        flota.DodajPojazd(new Motocykl("Honda", "CBR500R", 2019, 0.5, "sportowy"));
        flota.DodajPojazd(new PojazdElektryczny("Tesla", "Model S", 2022, 600, 8.5));

        Hybryda hybryda = new Hybryda("Toyota", "Prius", 2021, 1.5, 50, "elektryczny");
        flota.DodajPojazd(hybryda);

        Console.WriteLine("Flota pojazdów:");
        flota.WyswietlFlote();

        int dystans = 200;
        Console.WriteLine($"\nCałkowity koszt przejazdu {dystans} km: {flota.ObliczCalkowityKoszt(dystans)} PLN");

        hybryda.ZmienTryb();
        Console.WriteLine($"\nKoszt po zmianie trybu hybrydy: {flota.ObliczCalkowityKoszt(dystans)} PLN");
    }
}
