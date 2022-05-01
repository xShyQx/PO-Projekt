﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Forms;
using Project.RegistrationPages;
using Project.Registrations;
using Project.Games;
using Project.GamePages;

namespace Project
{
    public partial class MenuPage : Page
    {
        private Window mainWindow;
        private TeamPage teamPage;
        private LoadPage loadPage;
        private VolleyballPage volleyballPage;
        private TugOfWarPage tugOfWarPage;

        public Volleyball volleyball = new Volleyball();
        public Tug_of_war tugOfWar = new Tug_of_war();
        public MenuPage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            teamPage = new TeamPage(this);
            loadPage = new LoadPage(this);
            volleyballPage = new VolleyballPage(this);
            tugOfWarPage = new TugOfWarPage(this);
        }

        public static void load(string? fname, Volleyball volleyball, Tug_of_war tugOfWar)
        {
            //Najpierw sprawdzamy czy nazwa pliku jest dobra i czy taki plik istnieje
            if (fname == null || fname.Length == 0 || !File.Exists($@"..\..\..\saved\{fname}.txt"))
            {
                Console.WriteLine("Niepoprawna nazwa pliku");
                return;
            }

            //Usuwamy to co było wcześniej w listach
            volleyball.clearTeams();
            volleyball.clearJudges();
            tugOfWar.clearTeams();
            tugOfWar.clearJudges();

            //Póki co tylko dla siatkówki, sprawdzamy czy dana linijka to drużyna czy sędzia, potem jaki sport, i dodajemy do odpowiedniej listy
            StreamReader loadStream = new StreamReader($@"..\..\..\saved\{fname}.txt");
            while (!loadStream.EndOfStream)
            {
                string[] dane = loadStream.ReadLine().Split(',');
                if (dane[0].Equals("T"))
                {
                    if (dane[1].Equals("V"))
                    {
                        volleyball.addTeam(new Team(dane[2]));
                    }
                    else if (dane[1].Equals("T"))
                    {
                        tugOfWar.addTeam(new Team(dane[2]));
                    }
                }
                else if (dane[0].Equals("J"))
                {
                    if (dane[1].Equals("V"))
                    {
                        volleyball.addJudge(new Judge(dane[2], dane[3]));
                    }
                    else if (dane[1].Equals("T"))
                    {
                        tugOfWar.addJudge(new Judge(dane[2], dane[3]));
                    }
                }
            }
            loadStream.Close();
        }
        public static void save(string? fname, Volleyball volleyball, Tug_of_war tugOfWar)
        {
            //Najpierw sprawdzamy czy nazwa pliku jest dobra
            if (fname == null || fname.Length == 0)
            {
                Console.WriteLine("Niepoprawna nazwa pliku");
                return;
            }

            //Zapisujemy w kodzie T,[sport],[nazwa] dla drużyn i J,[sport],[imie],[nazwisko] dla sędziów
            StreamWriter saveStream = new StreamWriter($@"..\..\..\saved\{fname}.txt");
            volleyball.getTeams().ForEach(team =>
            {
                saveStream.WriteLine($"T,V,{team.getName()}");
            });
            volleyball.getJudges().ForEach(judge =>
            {
                saveStream.WriteLine($"J,V,{judge.getName()},{judge.getSurname()}");
            });
            tugOfWar.getTeams().ForEach(team =>
            {
                saveStream.WriteLine($"T,T.{team.getName()}");
            });
            tugOfWar.getJudges().ForEach(judge =>
            {
                saveStream.WriteLine($"J,T,{judge.getName()}, {judge.getSurname()}");
            });
            saveStream.Close();
        }

        private void TeamButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(teamPage);
        }

        private void JudgeButton_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(loadPage);
        }

        private void VolleyballButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(volleyballPage);
        }

        private void TugOfWarButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(tugOfWarPage);
        }

        private void DodgeballButton_Clicked(object sender, RoutedEventArgs e)
        {

        }
        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show("Czy zapisać stan programu?", "Wyjście", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) //Jeżeli ktoś wybierze, że chce zapisać (wybierze "tak")
            {
                FolderBrowserDialog folder = new FolderBrowserDialog(); //Tworzysz obiekt okna i ewentualnie ustawiasz mu propertisy. My nie potrzebujemy, podstawowe są git
                if (folder.ShowDialog() != DialogResult.OK) return; //Jak Ktoś zamknie okno lub wyjdzie nie zatwierdzając wyboru to ma wrócić do menu bez wychodzenia
                string path = folder.SelectedPath; //Robisz coś z uzyskaną ścieżką
                //Tu wywołaj zapis (i usuń tą linijkę wyżej, bo to ma tylko pokazać gdzie masz wybraną ścieżkę)
            }
            mainWindow.Close();

            //Po usuwaj komentarze potem
        }
    }
}
