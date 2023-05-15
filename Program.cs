using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace AxesAndBoots
{
    class Program
    {

        struct enemy
        {
            public static int silaAI = 2; 
            public static int zdraviAI = 2;
            public static int ZakladZdraviAI = 10;
            public static int HPEN1 = ZakladZdraviAI * zdraviAI;
            public static string Jmeno = "Nepřítel1"; //Jména se mění podle levelu hráče
        }
        struct postava
        {
            public static int sila = 1;
            public static int zdravi = 1;
            //Základní talenty ^^^^^^^^^^
            public static int ZakladZdravi = 10; //Základ pro zdraví - lze změnit
            public static int HP = ZakladZdravi * zdravi; //Vysledné zdraví
            public static int Damage = sila * 2; //Damage bez randomu
            public static int level = 1;
            public static int XP = 0; //XP v případě prohry měnit, že hráč dostane menší obnos XP
            public static int Zlataky = 100; //Počet zlaťáků 
            public static string Zbran = ""; //Proměnné pro názvy zbraní
            public static string Zbroj = "";
            public static int Lektvary = 1; //Intová proměnná pro počet lekvarů
            public static void NactiInfo() //Metoda pro načtení informací v průběhu hry
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(5, 2);
                Console.WriteLine("Síla: {0}", sila);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(5, 3);
                Console.WriteLine("Zdraví: {0}", HP);
                Console.SetCursorPosition(5, 4);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("LEVEL: {0}", level);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(5, 6);
                Console.WriteLine("Zlaťáky: {0} g", Zlataky);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(5, 7);
                Console.WriteLine("Počet lektvarů: {0}", Lektvary);
                Console.SetCursorPosition(5, 9);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Zbraň: {0}", Zbran);
                Console.SetCursorPosition(5, 10);
                Console.WriteLine("Zbroj: {0}", Zbroj);
                Console.SetCursorPosition(5, 12);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Zmáčkni cokoliv pro návrat do města!");
                Console.ReadKey();
                Mesto();
            }
            public static void Level() //Vyvolaná metoda pro zvýšení XP mimo metodu a následná kontrola zda je XP = 100
            {
                if (XP >= 100)
                {
                    level++;
                    XP = 0;
                    sila++; //Navýšení statistik při novém levelu
                    zdravi++;
                    HP = ZakladZdravi * zdravi;
                }
            }
            public static void RozdelTalent() //Rozdělení talentů před zahájením hry
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                int i = 1;
                sila = 1; zdravi = 1;
                for (i = 1; i < 4; i++)
                {
                    Ramecek(45, 15, 45, 0);
                    Console.SetCursorPosition(52, 2); Console.WriteLine("Zvolte si základní statistiky.");
                    Console.SetCursorPosition(52, 3); Console.WriteLine("Napište 1 nebo 2 a potvrďte Entrem");
                    Console.ForegroundColor = ConsoleColor.Red; Console.SetCursorPosition(52, 5); Console.WriteLine("1 - Zdraví - celkové zdraví postavy");
                    Console.ForegroundColor = ConsoleColor.Blue; Console.SetCursorPosition(52, 6); Console.WriteLine("2 - Síla - poškození proti nepřátelům");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; Console.SetCursorPosition(52, 8);
                    Console.WriteLine("Počet talentů k rozdání: {0}", 4 - i);
                    Console.SetCursorPosition(52, 10); Console.Write("Váš výběr: ");
                    string s = Console.ReadLine();
                    int a;
                    bool b = Int32.TryParse(s, out a);
                    if (b == false) {
                        Console.ForegroundColor = ConsoleColor.Red; Console.SetCursorPosition(48, 12); Console.WriteLine("Neplatný volba-Stiskni libovolnou klávesu");
                        Console.ReadKey(); Console.Clear(); RozdelTalent(); }
                    if (a == 1) { zdravi++; HP = ZakladZdravi * zdravi; }
                    else if (a == 2) { sila++; }
                    else { Console.SetCursorPosition(48, 12); Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Neplatný volba-Stiskni libovolnou klávesu");
                        Console.ReadKey(); Console.Clear(); RozdelTalent(); }
                    Console.Clear();
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                ZakladniZbranAZbroj(); //musí být až po volbě talentů
                Mesto();
            }
            public static void ZakladniZbranAZbroj() { Zbran = "Dřevěný meč (Síla + 1)"; sila = sila + 1; Zbroj = "Základní zbroj (Zdraví + 1)"; zdravi = zdravi + 1; HP = ZakladZdravi * zdravi; }
            public static void VykresliPostavu(int souradniceX, int souradniceY) { //podle zadaných souřadnic vykreslí postavu
                Console.SetCursorPosition(souradniceX, souradniceY);
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.WriteLine(" ");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(souradniceX, souradniceY - 1);
                Console.WriteLine(" ");
                Console.BackgroundColor = ConsoleColor.DarkYellow; //samozřejmě ve finále lze změnit barvu 
            }

        }
        //_________________________________________________________________________________________________________________________________
        static int x, y = 1;
        static ConsoleKeyInfo keyInfo; //Proměnná do které se uloží stisk klávesy
        //Vše ohledně menu______________________________________________________________________________
        static void Menu()
        {
            Ramecek(140, 40, 0, 0);
            Console.ForegroundColor = ConsoleColor.DarkYellow; //Barvu lze upřesnit
            StreamReader title = new StreamReader("Title.txt"); //Soubor musí být v debugu
            int krok = 0;
            while (krok < 20) //načtení 20 řádků textového souboru
            {
                Console.SetCursorPosition(28, 2 + krok);
                string text = title.ReadLine();
                Console.WriteLine(text);
                krok++;
            }
            Console.SetCursorPosition(60, 15); Console.WriteLine("Press ENTER to continue...");
            Console.SetCursorPosition(3, 37); Console.WriteLine("Autor: Jan Pejřil");
            Console.SetCursorPosition(3, 38); Console.WriteLine("Předmět: IAZPE");
            Console.SetCursorPosition(3, 39); Console.WriteLine("Rok: 2019");
            keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Ramecek(140, 40, 0, 0); //Vypsání príběhu v rámci menu
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(73, 3); Console.WriteLine("Příběh");
                Console.SetCursorPosition(50, 5);
                Console.WriteLine("Hlavní postavou hry je Varian Wrynn - král Stormwindu.");
                Console.SetCursorPosition(35, 6);
                Console.WriteLine("Varian je syn bývalého krále Stormwidnu po jehož smrti zaujal místo krále právě on.");
                Console.SetCursorPosition(25, 7);
                Console.WriteLine("Celý život se Varian věnoval boji mezi jeho lidem a orky, kteří přišli na Azeroth skrz Temný Portál.");
                Console.SetCursorPosition(20, 8);
                Console.WriteLine("Po skončení Třetí Války byl unesen na ostrov Alcaz z něhož unikl za cenu ztráty paměti a padl do zajetí Orků.");
                Console.SetCursorPosition(35, 9);
                Console.WriteLine("Varian byl převezen do hlavní města orků do Orgrimmaru, kde se z něj stal gladiátor.");
                Console.SetCursorPosition(45, 10);
                Console.WriteLine("Po několika bitvách si vysloužil pžezdívku Lo'gosh (duch vlka).");
                Console.SetCursorPosition(25, 11);
                Console.WriteLine("Výzvou Variana je momentálně přežití, kolik toho ještě vydrží a zda se něco dozví o jeho minulosti...");
                Console.SetCursorPosition(50, 15);
                Console.WriteLine("Stistkni libovolnou klávesu pro zahájení hry...");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey(); //Stisknutím libovolné klávesy se dostaneme dál
                Console.Clear();
                postava.RozdelTalent();

            }
            else { Menu(); } //Vrácení do menu při jiné klávese
        }
        static void VytvorPostavy() {
            Console.WriteLine("");
        }
        //Vše ohledně hlavního prostranství___________________________________________________________
        //vyska=100, sirka = 50
        static void Ramecek(int vyska, int sirka, int zacatekX, int zacatekY) //metoda pro vykreslení rámečku se vstupními parametry
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow; //rozměry lze doladit
            for (int i = 0; i <= vyska; i++) { Console.SetCursorPosition(zacatekX + x + i, zacatekY + y); Console.Write(" "); }
            for (int i = 0; i <= sirka; i++) { Console.SetCursorPosition(zacatekX + x, zacatekY + y + i); Console.Write("  "); }
            for (int i = 0; i <= vyska; i++) { Console.SetCursorPosition(zacatekX + x + i, zacatekY + y + sirka); Console.Write(" "); }
            for (int i = 0; i <= sirka; i++) { Console.SetCursorPosition(zacatekX + vyska, zacatekY + y + i); Console.Write("  "); }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        static void Lecitel()
        {
            Ramecek(100, 25, 0, 0);
            StreamReader lekar = new StreamReader("Lekar.txt"); //Soubor musí být v debugu
            int krok2 = 0;
            while (krok2 < 20)
            {
                Console.SetCursorPosition(x + 2, y + krok2);
                string text2 = lekar.ReadLine();
                Console.WriteLine(text2);
                krok2++;
            }
            Console.SetCursorPosition(60, 13); Console.WriteLine("                         ");
            Console.SetCursorPosition(60, 4); Console.WriteLine("Lektvar léčení tě v boji vyléčí");
            Console.SetCursorPosition(60, 5); Console.WriteLine("na maximální hodnotu zdraví.");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(60, 7); Console.WriteLine("1 - Lektvar léčení (50g)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(60, 9); Console.WriteLine("2 - Vrácení do města");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(60, 13); Console.WriteLine("Dostupné zlaťáky: {0}", postava.Zlataky);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(60, 11); Console.Write("Váš výběr: "); string volba = Console.ReadLine();
            int b;
            bool c = Int32.TryParse(volba, out b);
            if (c == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(60, 12); Console.WriteLine("Neplatná volba - Stiskni cokoliv");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.ReadKey();Console.Clear(); Lecitel();
            }
            switch (b)
            {
                case 1: if (postava.Zlataky >= 50) { postava.Zlataky = postava.Zlataky - 50; postava.Lektvary++; break; }
                    else { Console.SetCursorPosition(60, 12); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv");
                        Console.ReadKey();Console.Clear(); Lecitel(); break; }
                case 2: Mesto(); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(60, 12); Console.WriteLine("Neplatná volba - Stiskni cokoliv");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; Console.ReadKey(); Console.Clear(); Lecitel();
                    break;
            }
            Lecitel();

        }
        static void Mesto()
        {
            Console.Clear();
            Ramecek(100, 40, 0, 0);
            //______________________________________________________
            StreamReader mesto = new StreamReader("mesto.txt"); //Soubor musí být v debugu
            int krok = 0;
            string hodnoty = mesto.ReadLine();
            while (hodnoty != null)
            {
                Console.SetCursorPosition(x + 2, y + krok);
                foreach (Char s in hodnoty)
                {
                    char barva = s;
                    switch (barva) //Vybarvení podle znaků v texťáku + nahrazení mezerou
                    {
                        case 'X': Console.BackgroundColor = ConsoleColor.DarkGray; break;
                        case '#': Console.BackgroundColor = ConsoleColor.Black; break;
                        case '_': Console.BackgroundColor = ConsoleColor.Red; break;
                        case '/': Console.BackgroundColor = ConsoleColor.Red; break;
                        case '\\': Console.BackgroundColor = ConsoleColor.Red; break; //Lze doladit
                        case '0': Console.BackgroundColor = ConsoleColor.Gray; break;
                        default: Console.BackgroundColor = ConsoleColor.Black; break;
                    }
                    Console.Write(" ");

                }
                hodnoty = mesto.ReadLine(); Console.WriteLine();
                krok++;
            }
            Ramecek(50, 40, 100, 0); //Rámečky v boční části
            Ramecek(50, 10, 100, 0);
            Console.SetCursorPosition(105, 12); Console.WriteLine("NÁPOVĚDA");
            Console.SetCursorPosition(105, 17); Console.WriteLine("Kovář - Slouží k nákupu zbraní a zbroje,");
            Console.SetCursorPosition(105, 18); Console.WriteLine("      které navyšují statistiky(zdraví,síla)");
            Console.SetCursorPosition(105, 14); Console.WriteLine("Aréna - Souboj s nepřáteli. V případě výhry");
            Console.SetCursorPosition(105, 15); Console.WriteLine("        ziskání levelu,statistik a zlaťáků");
            Console.SetCursorPosition(105, 20); Console.WriteLine("Léčitel - Slouží k nakupování lektvarů, které");
            Console.SetCursorPosition(105, 21); Console.WriteLine("          slouží k vyléčení postavy v boji");
            Console.SetCursorPosition(105, 23); Console.WriteLine("Domov - Slouží k zobrazení statistik postavy,");
            Console.SetCursorPosition(105, 24); Console.WriteLine("         zbraní,zbroje a počtu zlaťáků");
            Console.SetCursorPosition(105, 2);
            Console.WriteLine("Vyberte si místo, které chcete navštívit!");
            Console.SetCursorPosition(105, 3);
            Console.WriteLine("Zadejte číslo 1-4 a potvrďte Entrem");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(105, 5);
            Console.WriteLine("1. Aréna");
            Console.SetCursorPosition(105, 6);
            Console.WriteLine("2. Kovář ");
            Console.SetCursorPosition(105, 7);
            Console.WriteLine("3. Léčitel ");
            Console.SetCursorPosition(105, 8);
            Console.WriteLine("4. Domov ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(105, 10); Console.Write("Váš výběr: ");
            string volba = Console.ReadLine();
            int vyber;
            bool b = Int32.TryParse(volba, out vyber);
            if (b == false)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.SetCursorPosition(105, 9); Console.WriteLine("Neplatný datový typ");
                Console.SetCursorPosition(105, 10); Console.WriteLine("Stistkni libovolnou klávesu"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey(); Console.Clear(); Mesto();
            }
            if (vyber > 5 || vyber < 1) {
                Console.ForegroundColor = ConsoleColor.Red; Console.SetCursorPosition(105, 9); Console.WriteLine("Neplatný výběr");
                Console.SetCursorPosition(105, 10); Console.WriteLine("Stistkni libovolnou klávesu"); Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey(); Console.Clear(); Mesto();
            }
            switch (vyber)
            {
                case 1: Console.Clear(); ZacatekArena(); break;
                case 2: Console.Clear(); Kovar(); break;
                case 3: Console.Clear(); Lecitel(); break;
                case 4: Console.Clear(); Domov(); break;
                default:
                    Console.SetCursorPosition(110, 5); Console.ReadLine(); Mesto(); 
                    break;
            }
            Console.ReadLine();
        }
        static void ZacatekArena(){
            Ramecek(150, 30, 0, 0);
            Console.SetCursorPosition(4, 2); Console.WriteLine("NÁPOVĚDA K ARÉNĚ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(4, 3); Console.WriteLine("KONEC TAHU:"); Console.SetCursorPosition(4, 4); Console.WriteLine("Souboj probíhá tahově");
            Console.SetCursorPosition(4, 5); Console.WriteLine("Tah ukončíte buď úmyslně (MEZERNÍK), vyčerpáním všech kroků a nebo útokem.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(4, 7); Console.WriteLine("ÚTOK:");
            Console.SetCursorPosition(4, 8); Console.WriteLine("Pokud stojíte 1 krok od nepřítele - útok provedete stisknutím klávesy Q.");
            Console.SetCursorPosition(4, 9); Console.WriteLine("Nepřítel bude odhozen o počet kroků rovnající se vašemu poškození.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(4, 11); Console.WriteLine("POHYB:");
            Console.SetCursorPosition(4, 12); Console.WriteLine("Pohyb postavy lze provést stisknutím LEVÉ ŠIPKY - pohyb doleva nebo PRAVÉ ŠIPKY - pohyb doprava");
            Console.SetCursorPosition(4, 13); Console.WriteLine("Po každém pohybu se posunete o jeden krok a odečte se vám jeden krok z celkového počtu kroků,");
            Console.SetCursorPosition(4, 14); Console.WriteLine("které jsou dostupné pro toto kolo. Každé vaše kolo se vám vygeneruje nový počet kroků.");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(4, 16); Console.WriteLine("LEKTVARY:");
            Console.SetCursorPosition(4, 17); Console.WriteLine("Během vašehu tahu máte možnost použít lekvat léčení (E) a tím se vyléčit na maximální hodnotu");
            Console.SetCursorPosition(4, 18); Console.WriteLine("Pokud počet lekvarů klesne na 0 můžete si nové lekvary pořídit u LÉČITELE");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(4, 20); Console.WriteLine("ÚTĚK Z ARÉNY:");
            Console.SetCursorPosition(4, 21); Console.WriteLine("Kdykoliv během boje můžete opustit arénu - klávesa ESCAPE. Útěkem postava nic neztratí.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(4, 23); Console.WriteLine("KONEC ARÉNY:");
            Console.SetCursorPosition(4, 24); Console.WriteLine("Cílem je porazit protivníka snížením jeho zdraví na nebo pod 0 pomocí vašich útoků - VÝHRA");
            Console.SetCursorPosition(4, 25); Console.WriteLine("Výhrou v aréně získáte nový level (navýšení zdraví a síly o 1 bod) a zlaťáky.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(4, 26); Console.WriteLine("V případě poklesu vašich životů na nebo pod 0 - PROHRA");
            Console.SetCursorPosition(4, 27); Console.WriteLine("Prohrou v aréně nic neztratíte a získáte 50 zlaťáků za účast.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(4, 28); Console.Write("HODNĚ ŠTĚSTÍ! - Pokračujte stisknutím klávesy ENTER nebo odejděte z arény klávesou ESCAPE");
            keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter) { Console.Clear(); Arena();}
            if (keyInfo.Key == ConsoleKey.Escape) { Console.Clear(); Mesto();}
        }
        //_________________________________________________________________________________________________________
        static int souradniceX = 40; //Výchozí souřadnice pro vykreslení postavy!
        static int souradniceY = 30;
        static void Arena() {
            StreamReader arena = new StreamReader("Arena.txt");
            int krok = 0;
            string hodnoty = arena.ReadLine();
            while (hodnoty != null) //načtení až do konce - pouze krátký obrazec
            {
                Console.SetCursorPosition(x + 2, y + krok + 4);
                foreach (Char s in hodnoty)
                {
                    char barva = s;
                    switch (barva)
                    {
                        case 'X': Console.BackgroundColor = ConsoleColor.DarkGray; break;
                        case 'T': Console.BackgroundColor = ConsoleColor.Gray; break;
                        case '1': Console.BackgroundColor = ConsoleColor.Blue; break;
                        case '2': Console.BackgroundColor = ConsoleColor.DarkBlue; break;
                        case '3': Console.BackgroundColor = ConsoleColor.Red; break;
                        case '4': Console.BackgroundColor = ConsoleColor.DarkRed; break;
                        case '5': Console.BackgroundColor = ConsoleColor.Green; break;
                        case '6': Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                        case '7': Console.BackgroundColor = ConsoleColor.Yellow; break;
                        case '8': Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                        case '9': Console.BackgroundColor = ConsoleColor.White; break;
                        default: Console.BackgroundColor = ConsoleColor.Black; break;
                    }
                    Console.Write(" ");

                }
                hodnoty = arena.ReadLine(); Console.WriteLine();
                krok++;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            VyberEnemy();
            int AktualniHP = postava.HP; //nastavení proměnné zdraví před bojem
            int EnemyHP = enemy.HPEN1;
            Tah(AktualniHP, EnemyHP);
        }
        static void Kovar() {
            Console.Clear();
            Ramecek(100, 25, 0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            StreamReader kovar = new StreamReader("kovar.txt"); //Soubor musí být v debugu
            int krok2 = 0;
            while (krok2 < 20)
            {
                Console.SetCursorPosition(x + 2, y + krok2);
                string text2 = kovar.ReadLine();
                Console.WriteLine(text2);
                krok2++;
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(60, 4);
            Console.WriteLine("Vyber si zbraň kterou chceš koupit (1-4)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(60, 6);
            Console.WriteLine("1. Jehla - Síla + 2  (200g)");
            Console.SetCursorPosition(60, 7);
            Console.WriteLine("2. Ocelový meč - Síla + 3  (300g)");
            Console.SetCursorPosition(60, 8);
            Console.WriteLine("3. Stříbrný meč - Síla + 4 (400g)");
            Console.SetCursorPosition(60, 9);
            Console.WriteLine("4. Shalamayne - Síla+10 (1000g)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(60, 11); Console.WriteLine("Vyber si zbroj kterou chceš koupit(5-8)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(60, 13); Console.WriteLine("5. Plechová zbroj - Zdraví + 2 (200g)");
            Console.SetCursorPosition(60, 14); Console.WriteLine("6. Ocelová zbroj - Zdraví + 3 (300g)");
            Console.SetCursorPosition(60, 15); Console.WriteLine("7. Stříbrná zbroj - Zdraví + 4 (400)g");
            Console.SetCursorPosition(60, 16); Console.WriteLine("8. Zbroj Aliance - Zdraví +10 (1000g)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(60, 22); Console.Write("Dostupné zlaťáky: {0}g", postava.Zlataky);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(60, 18); Console.WriteLine("Pro navrácení do města (9)");
            Console.SetCursorPosition(60, 19); Console.Write("Váš výběr: ");
            string volba = Console.ReadLine();
            int b;
            bool c = Int32.TryParse(volba, out b);
            if (c == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(60, 21); Console.WriteLine("Neplatná volba - Stiskni cokoliv");Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey(); Kovar();
            }
            switch (b) //kontrola počtu zlaťáků, přepsání jména zbraně/zbroje + navýšení statistik za cenu zlaťáků
            {
                case 1: if (postava.Zlataky >= 200) { postava.Zbran = "Jehla (Síla + 2)"; postava.Zlataky = postava.Zlataky - 200; postava.sila = postava.sila + 2; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 2: if (postava.Zlataky >= 300) { postava.Zbran = "Ocelový meč (Síla + 3)"; postava.Zlataky = postava.Zlataky - 300; postava.sila = postava.sila + 3; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 3: if (postava.Zlataky >= 400) { postava.Zbran = "Stříbrný meč (Síla + 4)"; postava.Zlataky = postava.Zlataky - 400; postava.sila = postava.sila + 4; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 4: if (postava.Zlataky >= 1000) { postava.Zbran = "Shalamayne (Síla + 10)"; postava.Zlataky = postava.Zlataky - 1000; postava.sila = postava.sila + 10; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 5: if (postava.Zlataky >= 200) { postava.Zbroj = "Plechová zbroj (Zdraví + 2)"; postava.Zlataky = postava.Zlataky - 200; postava.zdravi = postava.zdravi + 2; postava.HP = postava.ZakladZdravi * postava.zdravi; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 6: if (postava.Zlataky >= 300) { postava.Zbroj = "Ocelová zbroj (Zdraví + 3)"; postava.Zlataky = postava.Zlataky - 300; postava.zdravi = postava.zdravi + 3; postava.HP = postava.ZakladZdravi * postava.zdravi; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 7: if (postava.Zlataky >= 400) { postava.Zbroj = "Stříbrná zbroj (Zdraví + 4)"; postava.Zlataky = postava.Zlataky - 400; postava.zdravi = postava.zdravi + 4; postava.HP = postava.ZakladZdravi * postava.zdravi; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 8: if (postava.Zlataky >= 1000) { postava.Zbroj = "Zbroj Aliance (Zdraví + 10)"; postava.Zlataky = postava.Zlataky - 1000; postava.zdravi = postava.zdravi + 10; postava.HP = postava.ZakladZdravi * postava.zdravi; break; }
                    else { Console.SetCursorPosition(60, 21); Console.WriteLine("Nedostatek zlaťáků - stiskni cokoliv"); Console.ReadKey(); Kovar(); break; }
                case 9: Mesto(); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(60, 21); Console.WriteLine("Neplatná volba - Stiskni cokoliv");Console.ForegroundColor = ConsoleColor.DarkYellow; Console.ReadKey(); Kovar(); //default načte znova metodu Kováře
                    break;
            }
            Mesto(); //Vrácení do města na konci nákupu
        }
        static void Domov() {
            Console.Clear();
            Ramecek(50, 12, 0, 0);
            postava.NactiInfo();
        }
        static void Main(string[] args)
        {
            ////V případě,že se v novém projektu neprojeví přednastavení konzole odkomentovat od až po hvězdičky
            ////*************************************************************************************
            ////Console.WindowWidth = 210;  //příkazy pro konzoly.
            ////Console.WindowHeight = 50;
            //Console.Title = "Axes and Boots";
            //Console.WriteLine("Prosím MAXIMALIZUJTE si konzoly před spustěním hry."); 
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
            //Console.Clear();
            ////*************************************************************************************
            Menu();
            Console.ReadLine(); //Pracovní ReadLine
        }
        //Jádro souboje!!!____________________________________________________________________________________________________________
        //1) Resetovat souřadnice po dalším vstupu do areny - done
        //2) Nefunční HP když skončí while v tahu - done
        //3) Dodělat timer pro AI
        //4) Dodělat útok a kroky pro AI - done
        //5) Při každé další arene udělat větší odměny a s větším levelem těžší AI - done
        public static void Konec() //Metoda pro konec hry + nová hra od znova
        {
            Console.Clear(); //resetování všech statistik na začátek
            postava.level = 1;
            postava.XP = 0;
            postava.Zlataky = 100;
            postava.Zbran = "";
            postava.Zbroj = "";
            postava.Lektvary = 1;
            Ramecek(75, 10, 25, 0);
            Console.SetCursorPosition(50, 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ÚSPĚŠNĚ JSTE DOKONČIL HRU!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(40, 4);
            Console.WriteLine("Varian se dozvěděl pravdu o své minulosti.");
            Console.SetCursorPosition(40, 5);
            Console.WriteLine("Následující noc se proplížil z města a sehnal loď.");
            Console.SetCursorPosition(40, 6);
            Console.WriteLine("Ve Stormwindu ho okamžitě poznali a vřele přivítali.");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(50, 8); Console.WriteLine("Stiskni cokoliv pro konec hry");
            Console.ReadKey();
            Console.Clear();
            Menu();
        } //Hlavní metoda programu pro souboj hráče
        public static void Tah(int AktualniHP, int EnemyHP) {
            do
            { //Přepis na začátku kola starých údajů co se mohli stát v průběhu tahů
                Console.SetCursorPosition(105, 9); Console.WriteLine("                                   ");
                Console.SetCursorPosition(105, 7); Console.WriteLine("                 ");
                Console.SetCursorPosition(105, 6); Console.WriteLine("                 ");
                //Mazání údajů z předešlého kola!
                Random dmg = new Random();
                int Damage = dmg.Next(1, (postava.sila * 2)); //náhodný damage měnící se sílou postavy
                Console.BackgroundColor = ConsoleColor.Black;
                //Console.Clear();
                Ramecek(100, 30, 0, 0);
                Ramecek(50, 10, 100, 0); //Ramecek pro Hráče
                Ramecek(50, 10, 100, 10); //Ramecek AI
                Ramecek(50, 10, 100, 20); //Ramecek Nápověda
                postava.VykresliPostavu(souradniceX, souradniceY);
                VykresliAI(souAIX, souAIY);
                Random kr = new Random();
                int kroky = kr.Next(1, (postava.level * 2) + 1); //Generování random kroků podle levelu
                if (postava.level > 4) { kroky = kr.Next(1, 10); } //Omezení kroků z důvodu vysokého levelu
                while (kroky != 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(42, 2); Console.WriteLine("LEVEL: {0}", postava.level); Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(2, 2); Console.WriteLine("Varian"); Console.SetCursorPosition(2, 3); Console.WriteLine("HP: {0}", AktualniHP);
                    Console.SetCursorPosition(90, 2); Console.WriteLine(enemy.Jmeno); Console.SetCursorPosition(90, 3); Console.WriteLine("HP: {0}", EnemyHP);
                    Console.SetCursorPosition(105, 2); Console.WriteLine("Jsi na tahu! Vyber jakou akci chceš provést: "); //souřadnice posléze doladit v metodě
                    Console.SetCursorPosition(120, 23); Console.WriteLine("NÁPOVĚDA");
                    Console.SetCursorPosition(110, 25); Console.WriteLine("Left Arrow - Pohyb doleva");
                    Console.SetCursorPosition(110, 26); Console.WriteLine("Right Arrow - Pohyb doprava");
                    Console.SetCursorPosition(110, 27); Console.WriteLine("Spacebar - Úmyslný konec kola");
                    Console.SetCursorPosition(110, 28); Console.WriteLine("Escape - Návrat do města");
                    Console.SetCursorPosition(110, 29); Console.WriteLine("E - Použití lektvaru na vyléčení");
                    Console.SetCursorPosition(110, 30); Console.WriteLine("Q - Útok");
                    Console.SetCursorPosition(105, 4); Console.WriteLine("Počet dostupných kroků: {0}", kroky);
                    Console.ForegroundColor = ConsoleColor.DarkRed; Console.SetCursorPosition(105, 5); Console.WriteLine("Počet dostupných lektvarů: {0}", postava.Lektvary);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(105, 10);
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Escape) { souradniceX = 30; souradniceY = 30; souAIX = 70; souAIY = 30; Mesto(); } //Návrat do města + reset arény
                    if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        kroky = 0; Console.SetCursorPosition(105, 4); Console.WriteLine("Počet dostupných kroků: {0}", kroky); //Úmyslné ukončení kola
                    }
                    if (keyInfo.Key == ConsoleKey.LeftArrow && souradniceX > 2) //Omezení abychom nemohli jít až do rámečku
                    {
                        souradniceX--; kroky--; //ubrání jednoho kroku po provedení pohybu + přepis souřadnice postavy (stejné u dalšího pohybu)
                        Console.SetCursorPosition(souradniceX + 1, souradniceY); Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine(" "); //Přepis staré pozice
                        Console.SetCursorPosition(souradniceX + 1, souradniceY - 1); Console.WriteLine(" ");
                    }
                    if (keyInfo.Key == ConsoleKey.RightArrow && souradniceX < 99)
                    {

                        souradniceX++; kroky--;
                        Console.SetCursorPosition(souradniceX - 1, souradniceY); Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine(" ");
                        Console.SetCursorPosition(souradniceX - 1, souradniceY - 1); Console.WriteLine(" ");
                        if (souradniceX > souAIX - 2 || souradniceX > souAIX - 1 || souradniceX == souAIX) { souradniceX--; kroky++; } //ošetření proti překročení protivníka
                    }
                    if (keyInfo.Key == ConsoleKey.E) { if (postava.Lektvary > 0) { AktualniHP = postava.HP; postava.Lektvary--; } //Vyléčení postavy
                        Console.ForegroundColor = ConsoleColor.DarkRed; Console.SetCursorPosition(105, 5); Console.WriteLine("Počet dostupných lektvarů: {0}", postava.Lektvary);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    if (souradniceX + 1 == souAIX || souradniceX + 2 == souAIX) //utok 1x na konci kola
                    {
                        if (kroky == 0) { kroky++; if (keyInfo.Key == ConsoleKey.LeftArrow) { kroky--; } } //Možnost útočit pokud jsme hned u protivníka a nemáme kroky
                        Console.SetCursorPosition(105, 6); Console.WriteLine("Lze provést útok!");
                        if (keyInfo.Key == ConsoleKey.Q) //Útok provedením klávesy Q aby to nebylo automatické
                        {
                            Console.ForegroundColor = ConsoleColor.Green; Console.SetCursorPosition(105, 7); Console.WriteLine("Tvůj damage: {0}", Damage);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            EnemyHP = EnemyHP - Damage; Console.SetCursorPosition(90, 3); Console.WriteLine("        "); //Přepis starých údajů
                            Console.SetCursorPosition(90, 3); Console.WriteLine("HP: {0}", EnemyHP); 
                            kroky = 0;  Console.SetCursorPosition(105, 4); Console.WriteLine("Počet dostupných kroků: {0}", kroky); //Přepis kroků
                            souAIX = souAIX + Damage; Maz(souAIX - Damage, souAIY);
                            if (souAIX < 99) //Ošetření vylétnutí z mapy při odhození
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.SetCursorPosition(souAIX - Damage, souAIY - 1);
                                Console.WriteLine(" ");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.SetCursorPosition(souAIX - Damage, souAIY);
                                Console.WriteLine(" ");
                            }
                            else { Maz(souAIX - Damage, souAIY); souAIX = 98;VykresliAI(souAIX, souAIY); }
                            if (EnemyHP <= 0)
                            {
                                for (int i = 1; i < 39; i++) { for (int y = 1; y < 7; y++) { Console.SetCursorPosition(38 + i, 13 + y); Console.Write(" "); } Console.WriteLine(); }
                                Ramecek(40, 6, 37, 13);
                                Console.SetCursorPosition(54, 15); Console.WriteLine("VICTORY!");
                                postava.XP = postava.XP + 100;
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.SetCursorPosition(40, 19); Console.WriteLine("Stiskni cokoliv pro návrat do města!");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(50, 17); Console.WriteLine("Dostáváš {0} XP", postava.XP); Console.SetCursorPosition(50, 18);
                                int goldy = 100; if (postava.level >= 5) { goldy = 200; }
                                if (postava.level >= 10) { goldy = 300; }
                                postava.Zlataky = postava.Zlataky + goldy; Console.WriteLine("Dostáváš {0} goldů", goldy);
                                postava.Level(); Console.SetCursorPosition(54, 16); Console.WriteLine("Level:{0}", postava.level);
                                if (postava.level == 13) { Konec(); } //Konec hry
                                souradniceX = 40; souradniceY = 30; souAIX = 60; souAIY = 30; //nastavení počátečních souřadnic zpět
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.ReadKey(); Mesto();
                            }
                        }
                    }
                    else { Console.SetCursorPosition(105, 6); Console.WriteLine("               "); }
                    postava.VykresliPostavu(souradniceX, souradniceY);
                    VykresliAI(souAIX, souAIY);
                }
                VykresliAI(souAIX, souAIY);
                Console.SetCursorPosition(105, 4); Console.WriteLine("Počet dostupných kroků: {0}", kroky);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(105, 9); Console.WriteLine("Zmáčkni MEZERNÍK pro ukončení kola.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(105, 10); Console.Write(" ");
                bool konec = false;
                do //Ošetření pro konec kola pouze stisknutím mezerníku
                {
                    Console.SetCursorPosition(105, 10); Console.Write(" ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        AI(AktualniHP, EnemyHP);
                        konec = true;
                    }else { konec = false; }
                } while (kroky == 0 && konec == false);
                
            } while (true); //Aby souboj probíhal neustále ( Hlavní cykl v metodě "Tah")
        }
        static int souAIX = 60; //souřadnice pro AI
        static int souAIY = 30;
        public static void VykresliAI(int souAIX, int souAIY) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(souAIX, souAIY - 1);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" ");
            Console.SetCursorPosition(souAIX, souAIY);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Black;
        } 
          public static void Maz(int souradniceX, int souradniceY) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(souAIX, souAIY - 1);
            Console.WriteLine(" ");
            Console.SetCursorPosition(souAIX, souAIY);
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void AI(int AktualniHP, int EnemyHP) { //Metoda chování nepřítele
            VykresliAI(souAIX, souAIY);
            bool tah = true; 
            Random dmgAI = new Random();
            int DamageAI = dmgAI.Next(1, (enemy.silaAI * 2));
            Random krAI = new Random();
            int krokAI = krAI.Next(1, (postava.level * 2) + 1);
            if (postava.level > 4) { krokAI = krAI.Next(1, 10); } //^^^^^^Stejné jako pro hráče
            while (tah == true && krokAI != 0)
            {
                if (souradniceX + 2 < souAIX && EnemyHP > (EnemyHP / 4)) { //přibližování k hráčovi na určitou vdálenost
                    souAIX--; krokAI--;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(souAIX+1,souAIY-1);
                    Console.WriteLine(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(souAIX+1, souAIY);
                    Console.WriteLine(" ");

                    if (EnemyHP <= (EnemyHP / 4)) { souAIX++; krokAI--; }

                    if (souAIX == souradniceX + 2 || souAIX == souradniceX + 1) //Útok nepřítele
                    {
                        AktualniHP = AktualniHP - DamageAI; Console.SetCursorPosition(2, 3); Console.WriteLine("       ");
                        Console.SetCursorPosition(2, 3); Console.WriteLine("HP: {0}", AktualniHP);
                        souradniceX = souradniceX - DamageAI; //odhození silou jakou zaútočil 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(105, 12); Console.WriteLine("                                ");
                        Console.SetCursorPosition(105, 12); Console.WriteLine("Poslední nepřítelův útok: {0}", DamageAI);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        if (souradniceX > 2) //Omezení vyhození z rámečku hráče
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(souradniceX + DamageAI, souAIY - 1);
                            Console.WriteLine(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(souradniceX + DamageAI, souAIY);
                            Console.WriteLine(" ");
                        }
                        else { Maz(souradniceX - DamageAI, souradniceY); souradniceX = 2; postava.VykresliPostavu(souradniceX, souradniceY); }
                        tah = false;
                    }
                    if (AktualniHP <= 0) //Vyhodnocení a určení prohry hráče 
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        for (int i = 1; i < 38; i++) { for (int y = 1; y < 6; y++) { Console.SetCursorPosition(28 + i, 13 + y); Console.Write(" "); } Console.WriteLine(); }
                        Ramecek(38, 5, 28, 13); Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(44, 15); Console.WriteLine("PROHRA!");
                        Console.SetCursorPosition(38, 16);
                        int goldy = 50; postava.Zlataky = postava.Zlataky + goldy; Console.WriteLine("Dostáváš {0} goldů", goldy); //Obnos zlaťáku lze změnit s levelem
                        Console.SetCursorPosition(30, 18); Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("Stiskni cokoliv pro vrácení do města");
                        souradniceX = 40; souradniceY = 30; souAIX = 60; souAIY = 30; Console.ForegroundColor = ConsoleColor.DarkYellow; Console.ReadKey(); Mesto();
                    }
                }
            }
            
            VykresliAI(souAIX, souAIY);
            Tah(AktualniHP, EnemyHP);
        }
        
    //Obtížnost hry
    //Možné zavedení dalšího parametru - Kondička, která by určovala počet kroků nepřítele
    public static void VyberEnemy() {
            switch (postava.level)
            {
                case 1: enemy.Jmeno = "Žump"; enemy.silaAI = 2; enemy.zdraviAI = 2; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 2: enemy.Jmeno = "Guldan"; enemy.silaAI = 3; enemy.zdraviAI = 3; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 3: enemy.Jmeno = "Saurfang"; enemy.silaAI = 4; enemy.zdraviAI = 5; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 4: enemy.Jmeno = "Drak'thul"; enemy.silaAI = 5; enemy.zdraviAI = 5; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 5: enemy.Jmeno = "Cho'gall"; enemy.silaAI = 7; enemy.zdraviAI = 7; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 6: enemy.Jmeno = "Garona"; enemy.silaAI = 9; enemy.zdraviAI = 8; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 7: enemy.Jmeno = "Samuro"; enemy.silaAI = 9; enemy.zdraviAI = 9; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 8: enemy.Jmeno = "Kilrogg"; enemy.silaAI = 10; enemy.zdraviAI = 10; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 9: enemy.Jmeno = "Drek'Thar"; enemy.silaAI = 11; enemy.zdraviAI = 12; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 10: enemy.Jmeno = "Grommash"; enemy.silaAI = 14; enemy.zdraviAI = 14; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 11: enemy.Jmeno = "Blackhand"; enemy.silaAI = 15; enemy.zdraviAI = 15; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                case 12: enemy.Jmeno = "Garrosh"; enemy.silaAI = 15; enemy.zdraviAI = 25; enemy.HPEN1 = enemy.ZakladZdraviAI * enemy.zdraviAI; break;
                default:
                    break;
            }
        }
        
    }
    
}
