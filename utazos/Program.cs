using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace utazos
{
    class Utas
    {
        public string Nev { get; set; }
        public string Cim { get; set; }
        public string Telefonszam { get; set; }
    }

    class Utazas
    {
        public string Uticel { get; set; }
        public decimal Ar { get; set; }
        public int MaxLetszam { get; set; }
        public List<Utas> Utasok { get; set; }
        public Dictionary<string, decimal> Eloleg { get; set; }

        public Utazas()
        {
            Utasok = new List<Utas>();
            Eloleg = new Dictionary<string, decimal>();
        }

        public int ElerhetoHelyekSzama()
        {
            int jelentkezettekSzama = Utasok.Count;
            int elerhetoHelyek = MaxLetszam - jelentkezettekSzama;
            return elerhetoHelyek;
        }
    }


    class Program
    {
        static List<Utas> utasok = new List<Utas>();
        static List<Utazas> utazasok = new List<Utazas>();
        static string adminPassword = "admin";

        static void Main(string[] args)
        {
            Betoltes(); // Előzőleg mentett adatok betöltése

            // Főmenü megjelenítése és kezelése
            bool kilepes = false;
            while (!kilepes)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ha rossz opciót választ ki akkor csak üssen le egy enter és akkor a program majd ki fogja dobni!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Utasok kezelése");
                Console.WriteLine("2. Utazások adatainak kezelése");
                Console.WriteLine("3. Utazásokra jelentkezés");
                Console.WriteLine("4. Utazasokról leiratkozas");
                Console.WriteLine("5. Összes adat megtekintése");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Kilépés");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Válasszon menüpontot: ");
                string valasztas = Console.ReadLine();

                switch (valasztas)
                {
                    case "1":
                        Console.Clear();
                        UtaskozpontKezelese();
                        break;
                    case "2":
                        Console.Clear();
                        UtazasokKezelese();
                        break;
                    case "3":
                        Console.Clear();
                        UtazasokraJelentkezes();
                        break;
                    case "4":
                        UtazasokraLeiratkozas();
                        break;
                    case "5":
                        Console.Clear();
                        if (!AdminJelszoEllenorzes())
                        {
                            break;
                        }
                        OsszesUtasKiirasa();
                        break;
                    case "6":
                        kilepes = true;
                        break;
                    default:
                        Console.WriteLine("Nem létező menüpont!");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                }

            }

            Mentés(); // Adatok mentése kilépéskor
        }


        static void Betoltes()
        {
            BetoltUtasok();
            BetoltUtazasok();
        }

        static void BetoltUtasok()
        {
            if (File.Exists("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utaslista.txt"))
            {
                using (StreamReader reader = new StreamReader("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utaslista.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        string sor = reader.ReadLine();
                        string[] adatok = sor.Split(',');
                        if (adatok.Length == 3)
                        {
                            Utas ujUtas = new Utas();
                            ujUtas.Nev = adatok[0];
                            ujUtas.Cim = adatok[1];
                            ujUtas.Telefonszam = adatok[2];
                            utasok.Add(ujUtas);
                        }
                    }
                }
            }
        }

        static void BetoltUtazasok()
        {
            if (File.Exists("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utazasok.txt"))
            {
                using (StreamReader reader = new StreamReader("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utazasok.txt"))
                {
                    string sor;
                    Utazas ujUtazas;
                    while ((sor = reader.ReadLine()) != null)
                    {
                        ujUtazas = new Utazas();
                        string[] adatok = sor.Split(',');
                        if (adatok.Length >= 3)
                        {
                            ujUtazas.Uticel = adatok[0];
                            ujUtazas.Ar = Convert.ToDecimal(adatok[1]);
                            ujUtazas.MaxLetszam = Convert.ToInt32(adatok[2]);

                            while (!reader.EndOfStream)
                            {
                                string elolegSor = reader.ReadLine();
                                if (string.IsNullOrEmpty(elolegSor))
                                {
                                    break;
                                }

                                string[] elolegAdatok = elolegSor.Split(',');
                                if (elolegAdatok.Length == 2)
                                {
                                    ujUtazas.Eloleg.Add(elolegAdatok[0], Convert.ToDecimal(elolegAdatok[1]));
                                }
                            }

                            utazasok.Add(ujUtazas);
                        }
                    }
                }
            }
        }

        static void Mentés()
        {
            MentUtasokat();
            MentUtazasokat();
            Console.WriteLine("Az adatok sikeresen mentve.");
            Thread.Sleep(1500);
            Console.Clear();
        }

        static void MentUtasokat()
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utaslista.txt"))
            {
                foreach (Utas utas in utasok)
                {
                    writer.WriteLine($"{utas.Nev},{utas.Cim},{utas.Telefonszam}");
                }
            }
        }

        static void MentUtazasokat()
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\utazasok.txt"))
            {
                foreach (Utazas utazas in utazasok)
                {
                    writer.WriteLine($"{utazas.Uticel},{utazas.Ar},{utazas.MaxLetszam}");
                    foreach (var jelentkezo in utazas.Eloleg)
                    {
                        writer.WriteLine($"{jelentkezo.Key},{jelentkezo.Value}");
                    }
                    writer.WriteLine(); // Üres sor a választóvonalhoz
                }
            }
        }

        static void OsszesUtasKiirasa()
        {
            Console.Clear();
            foreach (var utas in utasok)
            {
                Console.WriteLine($"Név: {utas.Nev}, Cím: {utas.Cim}, Telefonszám: {utas.Telefonszam}");
                // Ellenőrzés, hogy van-e jelentkezése
                var jelentkezes = utazasok.Find(ut => ut.Utasok.Exists(u => u.Nev == utas.Nev));
                if (jelentkezes != null)
                {
                    Console.WriteLine($"Utazás célja: {jelentkezes.Uticel}, Előleg összege: {jelentkezes.Eloleg[utas.Nev]}");
                }
                else
                {
                    Console.WriteLine("Nem utazik sehova.");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Nyomjon meg egy gombot a visszatéréshez a menübe!");
            Console.ReadKey();
            Console.Clear();

        }


        static void UtaskozpontKezelese()
        {
            Console.WriteLine("1. Új utas felvétele");
            Console.WriteLine("2. Utas adatainak módosítása");
            Console.Write("Válasszon menüpontot: ");
            string valasztas = Console.ReadLine();

            switch (valasztas)
            {
                case "1":
                    Console.Clear();
                    UjUtasFelvele();
                    break;
                case "2":
                    Console.Clear();
                    UtasAdatokModositasa();
                    break;
                default:
                    Console.WriteLine("Nem létező menüpont!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }
        }

        static void UjUtasFelvele()
        {
            Console.Clear();
            Utas ujUtas = new Utas();

            Console.Write("Adja meg az utas nevét(Kis-Nagy betű számít!): ");
            string nev = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nev))
            {
                Console.WriteLine("A név nem lehet üres!");
                Thread.Sleep(1000);
                Console.Clear();
                return; // Kilépés a függvényből
            }

            // Ellenőrizzük, hogy van-e már olyan nevű utas
            if (utasok.Exists(x => x.Nev == nev))
            {
                Console.WriteLine("Már létezik ilyen nevű utas!");
                Thread.Sleep(1000);
                Console.Clear();
                return; // Kilépés a függvényből
            }

            ujUtas.Nev = nev;

            Console.Write("Adja meg az utas címét: ");
            ujUtas.Cim = Console.ReadLine();

            string telefonszam = BekeresTelefonszam();
            if (!string.IsNullOrWhiteSpace(telefonszam))
            {
                ujUtas.Telefonszam = telefonszam;
                utasok.Add(ujUtas);
                MentUtasokat(); // Mentjük az új utast is
                Console.WriteLine("Az új utas sikeresen hozzáadva!");
            }
            else
            {
                Console.WriteLine("Hibás telefonszám!");
            }
            Mentés();
        }


        static string BekeresTelefonszam()
        {
            string telefonszam;
            bool ervenyes = false;

            do
            {
                Console.Write("Adja meg a telefonszámot(csak számok): ");
                telefonszam = Console.ReadLine();

                // Ellenőrizzük, hogy csak számokat tartalmaz-e
                if (CsakSzamokatTartalmaz(telefonszam))
                {
                    ervenyes = true;
                }
                else
                {
                    Console.WriteLine("Hibás formátum. Kérjük, csak számokat adjon meg!");
                }

            } while (!ervenyes);

            return telefonszam;
        }

        static bool CsakSzamokatTartalmaz(string input)
        {
            foreach (char character in input)
            {
                if (!char.IsDigit(character))
                {
                    return false;
                }
            }
            return true;
        }

        static void UtasAdatokModositasa()
        {
            Console.Clear();
            Console.Write("Adja meg a módosítani kívánt utas nevét: ");
            string nev = Console.ReadLine();
            Utas modositandoUtas = utasok.Find(x => x.Nev == nev);

            if (modositandoUtas != null)
            {
                // Készítsünk egy másolatot az eredeti adatokról
                Utas elozoUtas = new Utas
                {
                    Nev = modositandoUtas.Nev,
                    Cim = modositandoUtas.Cim,
                    Telefonszam = modositandoUtas.Telefonszam
                };

                Console.Write("Adja meg az új címet: ");
                string ujCim = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(ujCim))
                {
                    Console.WriteLine("Az új cím nem lehet üres!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return; // Kilépés a függvényből
                }

                string ujTelefonszam = BekeresTelefonszam();

                // Ellenőrizzük, hogy a módosítások sikeresek
                if (MentesMegfelelo(modositandoUtas, ujCim, ujTelefonszam))
                {
                    // Ha minden módosítás sikeres volt, frissítsük az adatokat
                    modositandoUtas.Cim = ujCim;
                    modositandoUtas.Telefonszam = ujTelefonszam;
                    Mentés();
                    Console.WriteLine("Az utas adatai sikeresen módosítva!");
                }
                else
                {
                    // Ha valamelyik módosítás nem sikerült, visszaállítjuk az eredeti adatokat
                    modositandoUtas.Nev = elozoUtas.Nev;
                    modositandoUtas.Cim = elozoUtas.Cim;
                    modositandoUtas.Telefonszam = elozoUtas.Telefonszam;
                    Console.WriteLine("Hiba történt a módosítás során. Az adatok nem lettek megváltoztatva.");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Nem található ilyen nevű utas!");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        // Ellenőrzi, hogy a módosítások érvényesek-e
        static bool MentesMegfelelo(Utas utas, string ujCim, string ujTelefonszam)
        {
            if (ujCim.Length > 0 && ujTelefonszam.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void UtazasokKezelese()
        {
            Console.WriteLine("1. Új út felvétele");
            Console.WriteLine("2. Utazás módosítása");
            Console.WriteLine("3. Utazásokkal kapcsolatos információk");
            Console.WriteLine("4. Jelentkezett utazasok modosítása");
            Console.WriteLine("5. Utazás törlése");
            Console.Write("Válasszon menüpontot: ");
            string valasztas = Console.ReadLine();

            switch (valasztas)
            {
                case "1":
                    Console.Clear();
                    UjUtFelvetele();
                    break;
                case "2":
                    Console.Clear();
                    UtazasModositasa();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Elérhető utazások:");
                    foreach (Utazas utazas in utazasok)
                    {
                        Console.WriteLine($"- {utazas.Uticel}");
                    }
                    Console.Write("Adja meg az utazás célját: ");
                    string uticel = Console.ReadLine();
                    UtazasInformaciokMegtekintese(uticel); // Átadva az úticélt
                    break;
                case "4":
                    Console.Clear();
                    UtazasokraJelentkezesModositas();
                    break;
                case "5":
                    Console.Clear();
                    UtazasTorles();
                    break;
                default:
                    Console.WriteLine("Nem létező menüpont!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }
        }


        static void UjUtFelvetele()
        {
            Console.Clear();
            Console.Write("Adja meg az úticélt: ");
            string uticel = Console.ReadLine();

            // Ellenőrizzük, hogy az úticél nem üres
            if (string.IsNullOrWhiteSpace(uticel))
            {
                Console.WriteLine("Az úticél nem lehet üres!");
                Thread.Sleep(1000);
                Console.Clear();
                return; // Kilépés a függvényből
            }

            // Ellenőrizzük, hogy az adott úticél már létezik-e
            if (utazasok.Exists(x => x.Uticel == uticel))
            {
                Console.WriteLine("Már létezik ilyen úticélú utazás!");
                Thread.Sleep (1500);
                Console.Clear();
                return; // Kilépés a függvényből
            }

            Utazas ujUtazas = new Utazas();
            ujUtazas.Uticel = uticel;

            Console.Write("Adja meg az árat: ");
            string arInput = Console.ReadLine();
            decimal ar;
            // Ellenőrizzük, hogy csak számot adott-e meg az árnál
            if (!decimal.TryParse(arInput, out ar))
            {
                Console.WriteLine("Hibás formátum. Kérjük, csak számot adjon meg az árnál!");
                Thread.Sleep(1500);
                Console.Clear();
                return; // Kilépés a függvényből
            }
            ujUtazas.Ar = ar;

            Console.Write("Adja meg a maximális létszámot: ");
            string letszamInput = Console.ReadLine();
            int letszam;
            // Ellenőrizzük, hogy csak számot adott-e meg a létszámnál
            if (!int.TryParse(letszamInput, out letszam))
            {
                Console.WriteLine("Hibás formátum. Kérjük, csak számot adjon meg a létszámnál!");
                Thread.Sleep(1500);
                Console.Clear();
                return; // Kilépés a függvényből
            }
            ujUtazas.MaxLetszam = letszam;

            utazasok.Add(ujUtazas);
            MentUtazasokat(); // Mentjük az új utazást is
            Console.WriteLine("Az új út sikeresen hozzáadva!");
            Mentés();
        }


        static void UtazasModositasa()
        {
            Console.Clear();
            Console.WriteLine("Elérhető utazások:");
            foreach (Utazas utazas in utazasok)
            {
                Console.WriteLine($"- {utazas.Uticel} Ár:{utazas.Ar} Maximális fő:{utazas.MaxLetszam}");
            }

            Console.Write("Adja meg a módosítani kívánt utazás célját: ");
            string uticel = Console.ReadLine();
            Utazas modositandoUtazas = utazasok.Find(x => x.Uticel == uticel);

            if (modositandoUtazas != null)
            {
                // Check if a utazás with the same name already exists
                if (utazasok.Exists(x => x.Uticel == uticel && x != modositandoUtazas))
                {
                    Console.WriteLine("Már létezik ugyanilyen nevű utazás!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }

                // Készítsünk egy másolatot az eredeti adatokról
                Utazas elozoUtazas = new Utazas
                {
                    Uticel = modositandoUtazas.Uticel,
                    Ar = modositandoUtazas.Ar,
                    MaxLetszam = modositandoUtazas.MaxLetszam,
                    Utasok = modositandoUtazas.Utasok,
                    Eloleg = new Dictionary<string, decimal>(modositandoUtazas.Eloleg) // Másoljuk a Dictionary-t is
                };

                Console.Write("Adja meg az új úticélt: ");
                string ujUticel = Console.ReadLine();

                Console.Write("Adja meg az új árat: ");
                string arInput = Console.ReadLine();
                decimal ujAr;
                if (!decimal.TryParse(arInput, out ujAr))
                {
                    Console.WriteLine("Hibás formátum. Kérjük, csak számot adjon meg az árnál!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }

                Console.Write("Adja meg az új maximális létszámot: ");
                string letszamInput = Console.ReadLine();
                int ujLetszam;
                if (!int.TryParse(letszamInput, out ujLetszam))
                {
                    Console.WriteLine("Hibás formátum. Kérjük, csak számot adjon meg a létszámnál!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }

                // Ellenőrizzük, hogy a módosítások sikeresek
                if (MentesMegfelelo(modositandoUtazas, ujUticel, ujAr, ujLetszam))
                {
                    // Ha minden módosítás sikeres volt, frissítsük az adatokat
                    modositandoUtazas.Uticel = ujUticel;
                    modositandoUtazas.Ar = ujAr;
                    modositandoUtazas.MaxLetszam = ujLetszam;
                    Mentés();
                    Console.WriteLine("Az utazás adatai sikeresen módosítva!");
                }
                else
                {
                    // Ha valamelyik módosítás nem sikerült, visszaállítjuk az eredeti adatokat
                    modositandoUtazas.Uticel = elozoUtazas.Uticel;
                    modositandoUtazas.Ar = elozoUtazas.Ar;
                    modositandoUtazas.MaxLetszam = elozoUtazas.MaxLetszam;
                    modositandoUtazas.Eloleg = new Dictionary<string, decimal>(elozoUtazas.Eloleg); // Visszaállítjuk a Dictionary-t is
                    Console.WriteLine("Hiba történt a módosítás során. Az adatok nem lettek megváltoztatva.");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Nem található ilyen célú utazás!");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        // Ellenőrzi, hogy a módosítások érvényesek-e
        static bool MentesMegfelelo(Utazas utazas, string ujUticel, decimal ujAr, int ujLetszam)
        {
            if (ujAr > 0 && ujLetszam > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void UtazasokraJelentkezes()
        {
            Console.Clear();
            if (utazasok.Count > 0)
            {
                Console.WriteLine("Elérhető utazások:");
                foreach (Utazas utazas in utazasok)
                {
                    if (utazas.ElerhetoHelyekSzama() > 0) // Ellenőrizze az elérhető helyek számát
                    {
                        Console.WriteLine($"- {utazas.Uticel} Ár:{utazas.Ar} Maximális fő:{utazas.MaxLetszam} Elérhető helyek: {utazas.ElerhetoHelyekSzama()}");
                    }
                }

                Console.Write("Adja meg az utazás célját: ");
                string uticel = Console.ReadLine();
                Utazas kivalasztottUtazas = utazasok.Find(x => x.Uticel == uticel);

                if (kivalasztottUtazas != null && kivalasztottUtazas.ElerhetoHelyekSzama() > 0)
                {
                    Console.Write("Adja meg az utas nevét: ");
                    string nev = Console.ReadLine();
                    Utas jelentkezo = utasok.Find(x => x.Nev == nev);

                    if (jelentkezo != null)
                    {
                        // Check if the passenger has already signed up for the trip
                        if (!kivalasztottUtazas.Eloleg.ContainsKey(nev))
                        {
                            Console.Write("Adja meg az előleget: ");
                            decimal eloleg;
                            bool isValidInput = decimal.TryParse(Console.ReadLine(), out eloleg);

                            if (isValidInput)
                            {
                                if (eloleg <= kivalasztottUtazas.Ar)
                                {
                                    // Add the passenger to the Utasok list
                                    kivalasztottUtazas.Utasok.Add(jelentkezo);
                                    // Add the deposit to the Eloleg dictionary
                                    kivalasztottUtazas.Eloleg.Add(nev, eloleg);
                                    Console.WriteLine("Jelentkezés sikeres!");
                                    Mentés();
                                }
                                else
                                {
                                    Console.WriteLine("Az előleg nem lehet nagyobb az út áránál!");
                                    Thread.Sleep(1000);
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Csak számot fogadunk el az előleg megadásakor!");
                                Thread.Sleep(1500);
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Már jelentkezett erre az útra!");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen nevű utas!");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("A célú út vagy már megtelt, vagy nem létezik.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Jelenleg nincsenek elérhető utazások.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }


        static void UtazasokraJelentkezesModositas()
        {
            Console.Clear();
            if (utazasok.Count > 0)
            {
                Console.WriteLine("Elérhető utazások:");
                foreach (Utazas utazas in utazasok)
                {
                    Console.WriteLine($"- {utazas.Uticel} Ár:{utazas.Ar} Maximális fő:{utazas.MaxLetszam}");
                }

                Console.Write("Adja meg az utazás célját: ");
                string uticel = Console.ReadLine();
                Utazas kivalasztottUtazas = utazasok.Find(x => x.Uticel == uticel);

                if (kivalasztottUtazas != null)
                {
                    Console.Write("Adja meg az utas nevét: ");
                    string nev = Console.ReadLine();
                    Utas jelentkezo = utasok.Find(x => x.Nev == nev);

                    if (jelentkezo != null && kivalasztottUtazas.Eloleg.ContainsKey(nev))
                    {
                        decimal ujEloleg;
                        bool ervenyes = false;

                        do
                        {
                            Console.Write("Adja meg az új előleget: ");
                            string ujElolegInput = Console.ReadLine();

                            // Ellenőrizzük, hogy csak számot tartalmaz-e az input
                            if (decimal.TryParse(ujElolegInput, out ujEloleg))
                            {
                                ervenyes = true;
                            }
                            else
                            {
                                Console.WriteLine("Hibás formátum. Kérjük, csak számot adjon meg az előleget!");
                            }

                        } while (!ervenyes);

                        if (ujEloleg <= kivalasztottUtazas.Ar)
                        {
                            // Módosítjuk az előleget
                            kivalasztottUtazas.Eloleg[nev] = ujEloleg;
                            Console.WriteLine("Előleg sikeresen módosítva!");
                            Mentés();
                        }
                        else
                        {
                            Console.WriteLine("Az új előleg nem lehet nagyobb az út áránál!");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen nevű utas, vagy az adott utas még nem jelentkezett erre az útra!");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Nem található ilyen célú út!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Jelenleg nincsenek elérhető utazások.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void UtazasokraLeiratkozas()
        {
            Console.Clear();
            if (utazasok.Count > 0)
            {
                Console.WriteLine("Elérhető utazások, amelyekről le lehet iratkozni:");
                foreach (Utazas utazas in utazasok)
                {
                    Console.WriteLine($"- {utazas.Uticel} Ár:{utazas.Ar} Maximális fő:{utazas.MaxLetszam}");
                }

                Console.Write("Adja meg az utazás célját, amelyről le szeretne iratkozni: ");
                string uticel = Console.ReadLine();
                Utazas kivalasztottUtazas = utazasok.Find(x => x.Uticel == uticel);

                if (kivalasztottUtazas != null)
                {
                    Console.Write("Adja meg az utas nevét: ");
                    string nev = Console.ReadLine();
                    Utas jelentkezo = kivalasztottUtazas.Utasok.Find(x => x.Nev == nev);

                    if (jelentkezo != null)
                    {
                        // Töröljük az utast az utazásról
                        kivalasztottUtazas.Utasok.Remove(jelentkezo);
                        // Töröljük az előleget is
                        kivalasztottUtazas.Eloleg.Remove(nev);
                        Console.WriteLine("Leiratkozás sikeres!");
                        Mentés();
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen nevű utas a kiválasztott úton!");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Nem található ilyen célú út!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Jelenleg nincsenek elérhető utazások.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }



        static void UtazasInformaciokMegtekintese(string uticel) 
        {
            Console.Clear();
            Utazas kivalasztottUtazas = utazasok.Find(x => x.Uticel == uticel);

            if (kivalasztottUtazas != null)
            {
                Console.WriteLine("Úticél: " + kivalasztottUtazas.Uticel);
                Console.WriteLine("Ár: " + kivalasztottUtazas.Ar);
                Console.WriteLine("Maximális létszám: " + kivalasztottUtazas.MaxLetszam);
                Console.WriteLine("Jelentkezettek:");

                if (kivalasztottUtazas.Eloleg.Count > 0)
                {
                    foreach (var jelentkezo in kivalasztottUtazas.Eloleg)
                    {
                        Console.WriteLine(jelentkezo.Key + ", Befizetett előleg: " + jelentkezo.Value);
                    }
                    Console.WriteLine("Nyomjon meg egy gombot a visszatéréshez a menübe!");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Jelenleg nincsenek jelentkezők.");
                    Console.WriteLine("Nyomjon meg egy gombot a visszatéréshez a menübe!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Nem található ilyen célú út!");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        static void UtazasTorles()
        {
            Console.Clear();
            if (utazasok.Count > 0)
            {
                Console.WriteLine("Elérhető utazások:");
                foreach (Utazas utazas in utazasok)
                {
                    Console.WriteLine($"- {utazas.Uticel} Ár:{utazas.Ar} Maximális fő:{utazas.MaxLetszam}");
                }

                Console.Write("Adja meg az utazás célját, amelyet törölni szeretne: ");
                string uticel = Console.ReadLine();
                Utazas kivalasztottUtazas = utazasok.Find(x => x.Uticel == uticel);

                if (kivalasztottUtazas != null)
                {
                    utazasok.Remove(kivalasztottUtazas);
                    MentUtazasokat();
                    Console.WriteLine("Az utazás sikeresen törölve!");
                }
                else
                {
                    Console.WriteLine("Nem található ilyen célú utazás!");
                }
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Jelenleg nincsenek elérhető utazások.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static bool AdminJelszoEllenorzes()
        {
            Console.Write("Adja meg az jelszót: ");
            string jelszo = Console.ReadLine();

            if (jelszo != adminPassword)
            {
                Console.WriteLine("Helytelen jelszó!");
                Thread.Sleep(1000);
                Console.Clear();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
