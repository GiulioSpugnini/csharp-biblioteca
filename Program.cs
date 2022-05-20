
using System;
using System.IO;

/*
Si vuole progettare un sistema per la gestione di una biblioteca.
Gli utenti registrati al sistema, fornendo 
- cognome, 
- nome, 
- email, 
- password, 
- recapito telefonico,
Possono effettuare dei prestiti sui documenti che sono di vario tipo (libri, DVD).
I documenti sono caratterizzati da 
- un codice identificativo di tipo stringa (ISBN per i libri, numero seriale per i DVD), 
- titolo, 
- anno, 
- settore (storia, matematica, economia, …),
- stato (In Prestito, Disponibile), 
- uno scaffale in cui è posizionato, 
- un elenco di autori (Nome, Cognome).
Per i libri si ha in aggiunta
- il numero di pagine,
mentre per i dvd 
- la durata.
L’utente deve poter eseguire delle ricerche per 
- codice o per 
- titolo
e, eventualmente, effettuare dei prestiti registrando 
- il periodo (Dal/Al) del prestito 
- il documento.
Il sistema per ogni prestito determina
- un numero progressivo di tipo alfanumerico.
Deve essere possibile effettuare la ricerca dei prestiti dato 
- nome e cognome di un utente.
 */

//Da completare nell'esercizio
//NB: la chiave di un dizionario non deve necessarimente essere una stringa. ome abbiamo visto nelle procedure di ordinamento,
//questa può essere anche una tupl
//Esempio:  var dict = new Dictionary<Tuple<int, double, string>, string>();


namespace csharp_biblioteca
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Comunale");
            

                // List<string> list = new List<string>();
                // if (File.Exists(vPublicEnv + "biblioteca.txt"))
                // b.RestoreUtenti("biblioteca.txt");
                // else
                //    Console.WriteLine("Il file non esiste");

                Scaffale s1 = new Scaffale("A001");
                Scaffale s2 = new Scaffale("A002");
                Scaffale s3 = new Scaffale("A003");

                #region "Libro 1"
                Libro l1 = new Libro("ASBS1", "Titolo 1", 2000, "Storia", 200);
                Autore a1 = new Autore("Nome 1", "Cognome 1");
                Autore a2 = new Autore("Nome 2", "Cognome 2");
                l1.Autori.Add(a1);
                l1.Autori.Add(a2);
                l1.Scaffale = s1;

                b.Documenti.Add(l1);
                #endregion

                #region "Libro 2"
                Libro l2 = new Libro("ASBS2", "Titolo 2", 1996, "Storia", 312);
                Autore a3 = new Autore("Nome 3", "Cognome 3");
                Autore a4 = new Autore("Nome 4", "Cognome 4");
                l2.Autori.Add(a3);
                l2.Autori.Add(a4);
                l2.Scaffale = s2;

                b.Documenti.Add(l2);
                #endregion

                #region "DVD"
                DVD dvd1 = new DVD("Codice1", "Titolo 3", 2016, "Storia", 112);
                dvd1.Autori.Add(a3);
                dvd1.Scaffale = s3;

                b.Documenti.Add(dvd1);
                #endregion

                Utente u1 = new Utente("Nome 1", "Cognome 1", "Telefono 1", "Email 1", "Password 1");

                b.Utenti.Add(u1);
            // GESTIONE FILE DI CONFIGURAZIONE
            string vPublic = Environment.GetEnvironmentVariable("PUBLIC");
            if (vPublic != null)
                Console.WriteLine("variabile d'ambiente {0}", vPublic);

            vPublic += "\\Biblioteca";
            string mioPath = @"C:\Users\Giulio\Biblioteca\biblioteca.txt";
            //Verifico se il file biblioteca esiste nella variabile d'ambiente, se non esiste creo la cartella Biblioteca


            if (!Directory.Exists(vPublic))
            {

                Console.WriteLine("Dove vuoi storare i file?");
                Console.WriteLine("Sul computer centrale (C3): premi 1");
                Console.WriteLine("Sul sul tuo computer: premi 2");

                string choise = Console.ReadLine();
                int userChoice = Convert.ToInt32(choise);


                if (userChoice == 1)
                {
                    Directory.CreateDirectory(vPublic);
                    Console.WriteLine("Ho creato la cartella");

                    //CREO LA CARTELLA BIBLIOTECA NEL PC REMOTO E SCRIVO I FILE
                    string pathCompleto = vPublic + @"\biblioteca.txt";
                    b.SaveUtenti(pathCompleto);



                    //CREO IL FILE biblioteca.txt NELLA CARTELLA BIN DOVE MI SEGNO IL PATH DELLA CARTELLA DEL COMPUTER REMOTO
                    StreamWriter sr = new StreamWriter("biblioteca.txt");
                    sr.WriteLine(pathCompleto);

                    sr.Close();

                }
                else if (userChoice == 2)
                {
                    //CREO IL FILE NEL MIO COMPUTER E LO USO COME DATABASE PER SCRIVERE I DATI 
                    b.SaveUtenti("biblioteca.txt");

                }
                else
                {
                    Environment.Exit(0);
                }

            }
            else
            {

                Console.WriteLine("la directory esiste");
                if (File.Exists(vPublic + @"\biblioteca.txt"))
                {
                    //QUI LEGGERO I DATI DAL FILE e mi estrapolo il percorso per andare a leggere i file
                    string pathCompleto = vPublic + @"\biblioteca.txt";
                    b.RestoreUtenti(pathCompleto);
                }
                else if (File.Exists(mioPath))
                {
                    b.RestoreUtenti(mioPath);
                }
                else
                {
                    Environment.Exit(0);
                }
                StreamWriter sw = new StreamWriter("lista.utenti");

                Prestito p1 = new Prestito("A89321", new DateTime(2018, 2, 22), new DateTime(2022, 1, 13), u1, l1);
                Prestito p2 = new Prestito("B39393", new DateTime(2018, 4, 18), new DateTime(2020, 6, 28), u1, l2);

                b.Prestiti.Add(p1);
                b.Prestiti.Add(p2);

                Console.WriteLine("\n\nSearchByCodice: ASBS1\n\n");

                List<Documento> results = b.SearchByCodice("ASBS1");

                foreach (Documento doc in results)
                {
                    Console.WriteLine(doc.ToString());

                    if (doc.Autori.Count > 0)
                    {

                        Console.WriteLine("Autori");

                        foreach (Autore a in doc.Autori)
                        {
                            Console.WriteLine(a.ToString());
                        }
                    }
                }

                Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");

                List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");

                foreach (Prestito p in prestiti)
                {
                    Console.WriteLine(p.ToString());
                }
                //Come ultima istruzione del programma o ogni volta che aggiungete un nuovo utente,salvo gli utenti sul file
                //b.SaveUtenti("biblioteca.txt");
                Config.ReadAllSettings();
            }
        }

        enum Stato { Disponibile, Prestito }

        class Persona
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }

            public Persona(string Nome, string Cognome)
            {
                this.Nome = Nome;
                this.Cognome = Cognome;
            }

            public override string ToString()
            {
                return string.Format("Nome:{0}\nCognome:{1}",
                    this.Nome,
                    this.Cognome);
            }
        }


        class ListaUtenti
        {
            private Dictionary<Tuple<string, string, string>, Utente> MyDictionary;

            public ListaUtenti()
            {
                MyDictionary = new Dictionary<Tuple<string, string, string>, Utente>();
            }


            //Tutti i metodi per inserire, cercare e fare tutto nel dizionario
            public void AggiungiUtente(Utente uUtente)
            {
                var chiave = new Tuple<string, string, string>(uUtente.Nome, uUtente.Cognome, uUtente.Email);
                MyDictionary.Add(chiave, uUtente);
            }
        }

        class Biblioteca
        {
            public string Nome { get; set; }
            public List<Documento> Documenti { get; set; }
            public List<Prestito> Prestiti { get; set; }

            public List<Utente> Utenti { get; set; }

            public ListaUtenti MiaListaUtenti { get; set; }

            public Biblioteca(string Nome)
            {
                this.Nome = Nome;
                this.Documenti = new List<Documento>();
                this.Prestiti = new List<Prestito>();
                this.Utenti = new List<Utente>();
            }
            public bool SaveUtenti(string filename)
            {
                //salva gli utenti sul filename
                StreamWriter sw = new StreamWriter(filename);
                {
                    foreach (Utente utente in Utenti)
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4}", utente.Nome, utente.Cognome, utente.Telefono, utente.Email, utente.Password);
                    }
                }
                sw.Close();
                return true;
            }
            public bool RestoreUtenti(string filename)
            {
                //ricostruisce la lista degli utenti leggendo il file su cui sono stati scritti

                StreamReader sr = new StreamReader(filename);
                string line = sr.ReadLine();
                List<Utente> listU = new List<Utente>();
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] v = line.Split(',');
                        string Nome = v[0];
                        string Cognome = v[1];
                        string Telefono = v[2];
                        string Email = v[3];
                        string Password = v[4];

                        Utente utente = new Utente(Nome, Cognome, Telefono, Email, Password);
                    }
                }
                sr.Close();
                return true;
            }

            public List<Documento> SearchByCodice(string Codice)
            {
                return this.Documenti.Where(d => d.Codice == Codice).ToList();
            }

            public List<Documento> SearchByTitolo(string Titolo)
            {
                return this.Documenti.Where(d => d.Titolo == Titolo).ToList();
            }

            public List<Prestito> SearchPrestiti(string Numero)
            {
                return this.Prestiti.Where(p => p.Numero == Numero).ToList();
            }

            public List<Prestito> SearchPrestiti(string Nome, string Cognome)
            {
                return this.Prestiti.Where(p => p.Utente.Nome == Nome && p.Utente.Cognome == Cognome).ToList();
            }
        }

        class Scaffale
        {
            public string Numero { get; set; }

            public Scaffale(string Numero)
            {
                this.Numero = Numero;
            }
        }

        class Documento
        {
            public string Codice { get; set; }
            public string Titolo { get; set; }
            public int Anno { get; set; }
            public string Settore { get; set; }
            public Stato Stato { get; set; }
            public List<Autore> Autori { get; set; }
            public Scaffale Scaffale { get; set; }

            public Documento(string Codice, string Titolo, int Anno, string Settore)
            {
                this.Codice = Codice;
                this.Titolo = Titolo;
                this.Settore = Settore;
                this.Autori = new List<Autore>();
                this.Stato = Stato.Disponibile;
            }

            public override string ToString()
            {
                return string.Format("Codice:{0}\nTitolo:{1}\nSettore:{2}\nStato:{3}\nScaffale numero:{4}",
                    this.Codice,
                    this.Titolo,
                    this.Settore,
                    this.Stato,
                    this.Scaffale.Numero);
            }

            public void ImpostaInPrestito()
            {
                this.Stato = Stato.Prestito;
            }

            public void ImpostaDisponibile()
            {
                this.Stato = Stato.Disponibile;
            }
        }

        class Libro : Documento
        {
            public int NumeroPagine { get; set; }
            public Libro(string Codice, string Titolo, int Anno, string Settore, int NumeroPagine)
                : base(Codice, Titolo, Anno, Settore)
            {
                this.NumeroPagine = NumeroPagine;
            }
            public override string ToString()
            {
                return string.Format("{0}\nNumeroPagine:{1}", base.ToString(), this.NumeroPagine);
            }
        }

        class DVD : Documento
        {
            public int Durata { get; set; }
            public DVD(string Codice, string Titolo, int Anno, string Settore, int Durata)
                : base(Codice, Titolo, Anno, Settore)
            {
                this.Durata = Durata;
            }
            public override string ToString()
            {
                return string.Format("{0}\nDurata:{1}", base.ToString(), this.Durata);
            }
        }

        class Autore : Persona
        {
            public Autore(string Nome, string Cognome) : base(Nome, Cognome)
            {
            }
        }

        class Utente : Persona
        {
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public Utente(string Nome, string Cognome, string Telefono, string Email, string Password) : base(Nome, Cognome)
            {
                this.Telefono = Telefono;
                this.Email = Email;
                this.Password = Password;
            }

            public override string ToString()
            {
                return string.Format("Nome:{0}\nCognome:{1}\nTelefono:{2}\nEmail:{3}\nPassword:{4}",
                    this.Nome,
                    this.Cognome,
                    this.Telefono,
                    this.Email,
                    this.Password);
            }
        }

        class Prestito
        {
            public String Numero { get; set; }
            public DateTime Inizio { get; set; }
            public DateTime Fine { get; set; }
            public Utente Utente { get; set; }
            public Documento Documento { get; set; }

            public Prestito(String Numero, DateTime Inizio, DateTime Fine, Utente Utente, Documento Documento)
            {
                this.Numero = Numero;
                this.Inizio = Inizio;
                this.Fine = Fine;
                this.Utente = Utente;
                this.Documento = Documento;
                this.Documento.Stato = Stato.Prestito;
            }

            public override string ToString()
            {
                return string.Format("Numero:{0}\nDal:{1}\nAl:{2}\nStato:{3}\nUtente:\n{4}\nDocumento:\n{5}",
                    this.Numero,
                    this.Inizio,
                    this.Fine,
                    this.Documento.Stato,
                    this.Utente.ToString(),
                    this.Documento.ToString());
            }
        }
    }
}