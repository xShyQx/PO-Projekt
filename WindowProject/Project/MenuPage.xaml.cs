﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
        private VolleyballPage volleyballPage;
        private TugOfWarPage tugOfWarPage;

        public Volleyball volleyball = new Volleyball();
        public Tug_of_war tug_Of_War = new Tug_of_war();
        public MenuPage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            volleyballPage = new VolleyballPage(this);
            tugOfWarPage = new TugOfWarPage(this);
            teamPage = new TeamPage(this);
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
            MessageBoxResult result = MessageBox.Show("Czy zapisać stan programu?", "Wyjście", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {

            }
            mainWindow.Close();
        }
    }
}
