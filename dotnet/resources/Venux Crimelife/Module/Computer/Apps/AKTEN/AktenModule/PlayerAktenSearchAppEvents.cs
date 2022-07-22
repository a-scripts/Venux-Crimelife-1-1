using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using GTANetworkAPI;
using System.Linq;

namespace Venux
{
    public class PlayerAktenSearchAppEvents : Script
    {
         [RemoteEvent("requestPlayerResults")]
         public void OnRequestPlayerResults(Player client, string queryName)
         {
             try
             {
                /* MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                 connection.Open();

                 MySqlCommand command = connection.CreateCommand();//helpakten
                 command.CommandText = "SELECT * FROM accounts WHERE Username = @name";
                 command.Parameters.AddWithValue("@name", queryName);

                 using (MySqlDataReader reader = command.ExecuteReader())
                 {
                     while (reader.Read())
                     {
                         client.TriggerEvent("componentServerEvent", "PoliceAktenSearchApp", "responsePlayerResults", "[\"" + reader.GetString("Username") + "\"]");
                     }
                 }*/
                Player target = NAPI.Pools.GetAllPlayers().ToList().FirstOrDefault(x => x.Name.Contains(queryName));
                DbPlayer dbPlayer2 = target.GetPlayer();
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                {
                    return;
                }
                client.TriggerEvent("componentServerEvent", "PoliceAktenSearchApp", "responsePlayerResults", "[\"" + dbPlayer2.Name + "\"]");

            }
             catch (Exception exception)
             {
                 NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.StackTrace}");
                 NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.Message}");
             }
         }

    

        [RemoteEvent("requestPersonData")]
        public void OnRequestPersonData(Player client, string personName)
        {
            try
            {
                List<PolicePersonalDataModule> PlayerPersonalData = new List<PolicePersonalDataModule>();
                List<string> PlayerPersonalDataList = new List<string>();

                Player target = NAPI.Player.GetPlayerFromName(personName);
                if (target == null || !target.Exists) return;

                PlayerPersonalData.Add(new PolicePersonalDataModule(PlayerPersonalInformation.GetAddressByName(target.Name), PlayerPersonalInformation.GetPhoneNumberByName(target.Name), PlayerPersonalInformation.GetMembershipByName(target.Name), PlayerPersonalInformation.GetInfoByName(target.Name)));
                PlayerPersonalDataList.Add(NAPI.Util.ToJson(PlayerPersonalData).Replace("[", "").Replace("]", ""));

                client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responsePersonData",
                    NAPI.Util.ToJson(PlayerPersonalDataList).Replace("[\"", "").Replace("\"]", ""));
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.Message}");
            }
        }

        [RemoteEvent("requestJailTime")]
        public void OnRequestJailTime(Player client, string name)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(name);
                if (target == null || !target.Exists) return;

                client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseJailTime",
                    PlayerCrimeInformation.GetJailtimeByName(target.Name));
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestJailTime: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestJailTime: {exception.Message}");
            }
        }

        [RemoteEvent("requestJailCosts")]
        public void OnRequestJailCosts(Player client, string name)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(name);
                if (target == null || !target.Exists) return;

                client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseJailCosts",
                    PlayerCrimeInformation.GetJailcostsByName(target.Name));
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestJailCosts: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestJailCosts: {exception.Message}");
            }
        }

        [RemoteEvent("requestOpenCrimes")]
        public void OnRequestOpenCrimes(Player client, string name)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(name);
                if (target == null || !target.Exists) return;

                //client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseOpenCrimes",
                //    "[{\"name\":\"STGB §12\",\"description\":\"Drogenbesitz an der Person\"}]");

                client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseOpenCrimes",
                    PlayerCrimeInformation.GetJailreasonsByName(target.Name));
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestOpenCrimes: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestOpenCrimes: {exception.Message}");
            }
        }

        [RemoteEvent("requestWantedCategories")]
        public void OnRequestWantedCategories(Player client)
        {
            try
            {
                client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategories",
                    "[{}]");
                client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategories",
                    "[{\"id\":1,\"name\":\"§1 STVO - Geschwindigkeitsüberschreitungen\"},{\"id\":2,\"name\":\"§2 STGB - Lizenzverstösse\"},{\"id\":3,\"name\":\"§3 STVO - Normaler Straßenverkehr\"},{\"id\":4,\"name\":\"§4 STVO - Luftverkehr\"},{\"id\":5,\"name\":\"§5 BTMG - Drogendelikte\"},{\"id\":6,\"name\":\"§6 STGB - Waffendelikte\"},{\"id\":7,\"name\":\"§7 STGB - Körperliche Integrität\"},{\"id\":8,\"name\":\"§8 STGB - Umgang mit Beamten\"},{\"id\":9,\"name\":\"§9 STGB - Sonstige Delikte\"},{\"id\":10,\"name\":\"§10 STGB - Gefangenenbefreiung\"},{\"id\":11,\"name\":\"§11 STGB - Umgang mit staatlichen Behörden\"}]");
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.Message}");
            }
        }

        [RemoteEvent("requestCategoryReasons")]
        public void OnRequestCategoryReasons(Player client, int id)
        {
            try
            {
                switch(id)
                {
                    case 1:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                           client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                                "[{\"id\":1,\"name\":\"1 - 21 KMH Geschwindigkeitsüberschreitung\"},{\"id\":2,\"name\":\"21 - 50 KMH Geschwindigkeitsüberschreitung\"},{\"id\":3,\"name\":\"50 - 100 KMH Geschwindigkeitsüberschreitung\"},{\"id\":4,\"name\":\"101+ KMH Geschwindigkeitsüberschreitung\"}]");
                        break;
                    case 2:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":5,\"name\":\"Führen eines Kraftfahrzeuges ohne Lizenz\"},{\"id\":6,\"name\":\"Führen eines Wasserfahrzeuges ohne Lizenz\"},{\"id\":7,\"name\":\"Führen eines Luftfahrzeuges ohne Lizenz\"},{\"id\":8,\"name\":\"Besitz einer gefälschten Lizenz\"}]");
                        break;
                    case 3:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":9,\"name\":\"Gefährlicher Eingriff in den Straßenverkehr\"},{\"id\":10,\"name\":\"Falschparken / Parken außerhalb zulässiger Parkflächen\"},{\"id\":11,\"name\":\"Allgemeine Verkehrsbehinderung\"},{\"id\":12,\"name\":\"Fahren entgegen der Fahrtrichtung\"},{\"id\":13,\"name\":\"Fahren abseits der Straße\"},{\"id\":14,\"name\":\"Führen unter Alkohol oder Drogeneinfluss\"},{\"id\":15,\"name\":\"Nicht Beachten von Sondersignalen/ Rettungsfahrzeuge\"},{\"id\":16,\"name\":\"Fahrlässiges verursachen eines Unfalls\"},{\"id\":17,\"name\":\"Fahrerflucht / Bei Flucht von einem Unfallort bewusst oder unbewusst\"},{\"id\":18,\"name\":\"Handynutzung während der Fahrt\"},{\"id\":19,\"name\":\"Missachtung der Vorfahrtsregeln\"},{\"id\":20,\"name\":\"Betreiben eines fahruntüchtigen Fahrzeugs\"},{\"id\":21,\"name\":\"Mitführpflicht für Verbandskasten und Reparaturkit missachtet\"},{\"id\":23,\"name\":\"Fahren ohne Beleuchtungseinrichtung\"},{\"id\":24,\"name\":\"Fahren ohne Zulassung\"},{\"id\":25,\"name\":\"Fahren mit gefälschten Kennzeichen \"},{\"id\":26,\"name\":\"Fahren ohne gültige Erstehilfe-Lizenz\"},{\"id\":22,\"name\":\"Rechtsfahrgebot missachtet\"},{\"id\":27,\"name\":\"Kostenpflichtige Entfernung der Parkkralle\"},{\"id\":28,\"name\":\"Fahren mit nicht gültigen Kennzeichen\"}]");
                        break;
                    case 4:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":29,\"name\":\"Landen auf nicht genehmigten Flächen\"},{\"id\":30,\"name\":\"Nicht einhalten der Mindestflughöhe\"},{\"id\":31,\"name\":\"Missachtung der Luftverkehrs Funkpflicht\"},{\"id\":32,\"name\":\"Betreiben eines fluguntüchtigen Luftfahrzeugs\"},{\"id\":33,\"name\":\"Wiederholtes führen eines Luftfahrzeugs ohne staatlich vorgeschriebene Instrumente\"},{\"id\":34,\"name\":\"Störung des Funkverkehrs\"},{\"id\":35,\"name\":\"Schwere Störung des Funkverkehrs\"},{\"id\":36,\"name\":\"Missachtung von Anweisungen der Air-Control\"},{\"id\":37,\"name\":\"Schwere Missachtung der Anweisungen der Air-Control\"},{\"id\":38,\"name\":\"Missachtung der Flughafenrichtlinien\"},{\"id\":39,\"name\":\"Gefährlicher Eingriff in den Luftverkehr\"},{\"id\":40,\"name\":\"Führen eines Luftfahrzeug unter Einfluss psychoaktiver Substanzen.\"},{\"id\":41,\"name\":\"Fahrlässiges Verursachen eines Luftverkehrsunfalls\"}]");
                        break;
                    case 5:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":42,\"name\":\"Drogenbesitz an der Person\"},{\"id\":43,\"name\":\"Drogenbesitz KFZ / Flugfahrzeug / Wasserfahrzeug\"},{\"id\":44,\"name\":\"Drogenbesitz im Haus\"},{\"id\":45,\"name\":\"Drogenbesitz (größere Menge)\"},{\"id\":46,\"name\":\"Drogenhandel\"},{\"id\":47,\"name\":\"Anbau und/oder Herstellung von Drogen\"},{\"id\":48,\"name\":\"Drogenkonsum\"}]");
                        break;
                    case 6:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":49,\"name\":\"Mit gezogener Waffe durch die Öffentlichkeit laufen\"},{\"id\":50,\"name\":\"Besitz einer illegalen Waffe und/oder Munition\"},{\"id\":51,\"name\":\"Unberechtigter Schusswaffengebrauch\"},{\"id\":52,\"name\":\"Waffenhandel\"},{\"id\":53,\"name\":\"Waffenherstellung\"}]");
                        break;
                    case 7:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":54,\"name\":\"Prostitution\"},{\"id\":55,\"name\":\"Belästigung/ Nötigung\"},{\"id\":56,\"name\":\"Schwere Belästigung /Nötigung\"},{\"id\":57,\"name\":\"Freiheitsberaubung\"},{\"id\":58,\"name\":\"Geiselnahme\"},{\"id\":59,\"name\":\"Beleidigung/ Rufmord\"},{\"id\":60,\"name\":\"Schwere Beleidigung/Schwerer Rufmord\"},{\"id\":61,\"name\":\"Drohung\"},{\"id\":62,\"name\":\"Schwere Drohung\"},{\"id\":63,\"name\":\"Unterlassene Hilfeleistung\"},{\"id\":64,\"name\":\"Vorsätzliche Körperverletzung\"},{\"id\":65,\"name\":\"Totschlag\"},{\"id\":66,\"name\":\"Totschlag in mehreren Fällen\"}],{\"id\":67,\"name\":\"Mord\"},{\"id\":68,\"name\":\"Mord in mehreren Fällen (2+)\"},{\"id\":69,\"name\":\"Mord in mehreren Fällen (5+)\"}");
                        break;
                    case 8:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":70,\"name\":\"Umgehung polizeilicher Maßnahmen\"},{\"id\":71,\"name\":\"Widerstand gegen Vollstreckungsbeamte\"},{\"id\":72,\"name\":\"Behinderung eines Beamten\"},{\"id\":73,\"name\":\"Missachtung polizeilicher Anweisungen\"},{\"id\":74,\"name\":\"Behinderung eines Beamten bei einem Einsatz\"},{\"id\":75,\"name\":\"Beamtenbeleidigung\"},{\"id\":76,\"name\":\"Schwere Beamtenbeleidigung\"},{\"id\":77,\"name\":\"Vertuschung von Beweismaterial\"}]");
                        break;
                    case 9:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":78,\"name\":\"Betreten von Sperrzonen/ Nichteinhalten des Platzverweises\"},{\"id\":79,\"name\":\"Durchbrechen von Absperrungen (Fahrzeug Beschlagnamen)\"},{\"id\":80,\"name\":\"Vermummungsverbot\"},{\"id\":81,\"name\":\"Unangemeldete Versammlung\"},{\"id\":82,\"name\":\"Amtsanmaßung\"},{\"id\":83,\"name\":\"Missbrauch des Notrufs\"},{\"id\":84,\"name\":\"Sachbeschädigung\"},{\"id\":85,\"name\":\"Umweltverschmutzung\"},{\"id\":86,\"name\":\"Aufforderung zu Straftaten\"},{\"id\":87,\"name\":\"Angabe falscher Informationen\"},{\"id\":88,\"name\":\"Erregung öffentlichen Ärgernisses\"},{\"id\":90,\"name\":\"Hausfriedensbruch\"},{\"id\":91,\"name\":\"Falsche Aussage vor Gericht\"},{\"id\":92,\"name\":\"Schwere Amtsanmaßung\"},{\"id\":93,\"name\":\"Korruption\"},{\"id\":94,\"name\":\"Schweres Dienstvergehen\"}]");
                        break;
                    case 10:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":95,\"name\":\"Gefangenenbefreiung - Flüchtiger\"},{\"id\":96,\"name\":\"Gefangenenbefreiung - Beihilfe\"}]");
                        break;
                    case 11:
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                        "[{}]");
                        client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "responseCategoryReasons",
                             "[{\"id\":97,\"name\":\"Angriff auf staatliche Einrichtungen\"},{\"id\":98,\"name\":\"Hochverrat\"},{\"id\":99,\"name\":\"Verschwörung\"},{\"id\":100,\"name\":\"Terroristischer Akt\"},{\"id\":101,\"name\":\"Einbruch in staatliche Behörden\"}]");
                        break;
                    default: break;
                }
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnRequestLaptopApps: {exception.Message}");
            }
        }

        [RemoteEvent("savePersonData")]
        public void OnSavePersonData(Player client, string personName, string address, string membership, int phone,
            string info)
        {
            try
            {
                PlayerPersonalInformation.UpdatePlayerPersonalInformation(personName, address, membership, info, client);
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnSavePersonData: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnSavePersonData: {exception.Message}");
            }
        }

        [RemoteEvent("removeAllCrimes")]
        public void OnRemoveAllCrimes(Player client, string personName)
        {
            try
            {
                DbPlayer dbPlayer = client.GetPlayer();
                Player target = NAPI.Player.GetPlayerFromName(personName);

                PlayerCrimeInformation.UpdateAktenCrimes(target.Name, "[]");
                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, 0);
                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, 0);

                foreach (Player c in NAPI.Pools.GetAllPlayers())
                {
                    DbPlayer dbPlayer2 = c.GetPlayer();
                    if (dbPlayer2.Faction.Id == 21)
                    {
                        dbPlayer2.SendNotification(client.Name + " hat die Akten von " + target.Name + " erloschen!", 5000, "lightblue", "Los Santos Police Department");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"OnRemoveAllCrimes: {exception.StackTrace}");
            }
        }

        [RemoteEvent("addPlayerWanteds")]
        public void OnAddPlayerWanteds(Player client, string personName, string newWanteds)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(personName);

                client.TriggerEvent("componentServerEvent", "PoliceEditWantedsApp", "addPlayerWanteds",
                    "[{\"id\":" + newWanteds + "}]");

                string wanted = newWanteds.Replace("[", "").Replace("]", "");

                if (wanted.Length > 0)
                {
                    string[] wanteds = wanted.Split(",");

                    for (int x = 0; x < wanteds.Length; x++)
                    {
                        int wantedIndex = x + 1;
                        List <AktenModule> aktenModuleList = NAPI.Util.FromJson<List<AktenModule>>(PlayerCrimeInformation.GetJailreasonsByName(target.Name));
                        aktenModuleList.Add(new AktenModule(wantedIndex, PlayerCrimeInformation.GetAktenNameById(int.Parse(wanteds[x])), PlayerCrimeInformation.GetAktenDescriptionById(int.Parse(wanteds[x]))));
                        PlayerCrimeInformation.UpdateAktenCrimes(target.Name, NAPI.Util.ToJson(aktenModuleList));

                        switch (int.Parse(wanteds[x])) // WantedId = add jailtime for each WantedID
                        {
                            case 1:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 2:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 3:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                                break;
                            case 4:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 5);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 5:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 6:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                                break;
                            case 7:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                                break;
                            case 8:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                                break;
                            case 9:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 10:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 11:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                                break;
                            case 12:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3500);
                                break;
                            case 13:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4500);
                                break;
                            case 14:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                                break;
                            case 15:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4000);
                                break;
                            case 16:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 17:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                                break;
                            case 18:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2000);
                                break;
                            case 19:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 20:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 21:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3500);
                                break;
                            case 22:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1000);
                                break;
                            case 23:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 24:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 25:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 26:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 27:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4500);
                                break;
                            case 28:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                                break;
                            case 29:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                                break;
                            case 30:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                                break;
                            case 31:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2000);
                                break;
                            case 32:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 11000);
                                break;
                            case 33:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 34:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 35:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 36:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 37:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 38:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 75000);
                                break;
                            case 39:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 40:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                                break;
                            case 41:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                                break;
                            case 42:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 43:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 44:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 45:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 46:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                                break;
                            case 47:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 48:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 49:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 50:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 51:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6000);
                                break;
                            case 52:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 53:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                                break;
                            case 54:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                                break;
                            case 55:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 56:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 57:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 58:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 59:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 60:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 61:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 62:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 63:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 64:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 65:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 66:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 30000);
                                break;
                            case 67:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 40000);
                                break;
                            case 68:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 40);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                                break;
                            case 69:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 50);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                                break;
                            case 70:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 71:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 72:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                                break;
                            case 73:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 74:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 75:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 76:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                                break;
                            case 77:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6000);
                                break;
                            case 78:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 79:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 80:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                                break;
                            case 81:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                                break;
                            case 82:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                                break;
                            case 83:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                                break;
                            case 84:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 85:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                                break;
                            case 86:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                                break;
                            case 87:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 88:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1000);
                                break;
                            case 90:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                                break;
                            case 91:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                                break;
                            case 92:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 35);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                                break;
                            case 93:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 150000);
                                break;
                            case 94:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 80);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 185000);
                                break;
                            case 95:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 35);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 16000);
                                break;
                            case 96:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                                break;
                            case 97:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 90);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 100000);
                                break;
                            case 98:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 140000);
                                break;
                            case 99:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 60);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 100000);
                                break;
                            case 100:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                                break;
                            case 101:
                                PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                                PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 25000);
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (Player c in NAPI.Pools.GetAllPlayers())
                    {
                        DbPlayer dbPlayer = c.GetPlayer();
                        if (dbPlayer.Faction.Id == 21)
                        {
                            dbPlayer.SendNotification("" + client.Name + " hat die Akte von " + target.Name + " bearbeitet", 5000, "lightblue", "Los Santos Police Department");
                        }
                    }
                }
                else
                {
                    int wantedId = int.Parse(wanted);
                    List<AktenModule> aktenModuleList = NAPI.Util.FromJson<List<AktenModule>>(PlayerCrimeInformation.GetJailreasonsByName(target.Name));
                    aktenModuleList.Add(new AktenModule(wantedId, PlayerCrimeInformation.GetAktenNameById(wantedId), PlayerCrimeInformation.GetAktenDescriptionById(wantedId)));
                    PlayerCrimeInformation.UpdateAktenCrimes(target.Name, NAPI.Util.ToJson(aktenModuleList));

                    switch (wantedId) // WantedId = add jailtime for each WantedID
                    {
                        case 1:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 2:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 3:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7500);
                            break;
                        case 4:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 5);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 5:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 6:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 8000);
                            break;
                        case 7:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 8000);
                            break;
                        case 8:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 8000);
                            break;
                        case 9:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 10:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 11:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1500);
                            break;
                        case 12:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3500);
                            break;
                        case 13:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 4500);
                            break;
                        case 14:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7000);
                            break;
                        case 15:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 4000);
                            break;
                        case 16:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 17:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1500);
                            break;
                        case 18:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2000);
                            break;
                        case 19:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 20:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 21:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3500);
                            break;
                        case 22:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1000);
                            break;
                        case 23:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 24:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 25:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 26:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 27:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 4500);
                            break;
                        case 28:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7500);
                            break;
                        case 29:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6500);
                            break;
                        case 30:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6500);
                            break;
                        case 31:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2000);
                            break;
                        case 32:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 11000);
                            break;
                        case 33:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 34:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 35:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 36:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 37:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 38:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 75000);
                            break;
                        case 39:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 40:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 12500);
                            break;
                        case 41:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 12500);
                            break;
                        case 42:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 43:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 44:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 45:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 46:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 12500);
                            break;
                        case 47:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 48:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 49:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 50:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 51:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6000);
                            break;
                        case 52:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 53:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 12500);
                            break;
                        case 54:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1500);
                            break;
                        case 55:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 56:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 57:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 58:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 59:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 60:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 61:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 62:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 63:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 64:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 65:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 66:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 30000);
                            break;
                        case 67:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 40000);
                            break;
                        case 68:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 40);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 60000);
                            break;
                        case 69:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 50);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 60000);
                            break;
                        case 70:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 71:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 72:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 2500);
                            break;
                        case 73:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 74:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 75:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 76:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6500);
                            break;
                        case 77:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6000);
                            break;
                        case 78:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 79:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 80:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 5000);
                            break;
                        case 81:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1500);
                            break;
                        case 82:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7000);
                            break;
                        case 83:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7500);
                            break;
                        case 84:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 85:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 3000);
                            break;
                        case 86:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 6500);
                            break;
                        case 87:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 88:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 1000);
                            break;
                        case 90:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 7000);
                            break;
                        case 91:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 20000);
                            break;
                        case 92:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 35);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 10000);
                            break;
                        case 93:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 150000);
                            break;
                        case 94:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 80);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 185000);
                            break;
                        case 95:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 35);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 16000);
                            break;
                        case 96:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 15000);
                            break;
                        case 97:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 90);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 100000);
                            break;
                        case 98:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 140000);
                            break;
                        case 99:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 60);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 100000);
                            break;
                        case 100:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 60000);
                            break;
                        case 101:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) - 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) - 25000);
                            break;
                        default:
                            break;
                    }


                    foreach (Player c in NAPI.Pools.GetAllPlayers())
                    {
                        DbPlayer dbPlayer = c.GetPlayer();
                        if (dbPlayer.Faction.Id == 21)
                        {
                            dbPlayer.SendNotification(client.Name + " hat die Akte von " + target.Name + " bearbeitet!", 5000, "lightblue", "Los Santos Police Department");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnAddPlayerWanteds: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnAddPlayerWanteds: {exception.Message}");
            }
        }

        [RemoteEvent("removePlayerCrime")]
        public void OnRemovePlayerCrime(Player client, string personName, int wantedId)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(personName);

                List<AktenModule> aktenModuleList = NAPI.Util.FromJson<List<AktenModule>>(PlayerCrimeInformation.GetJailreasonsByName(target.Name));
                if (aktenModuleList.Find(aktenId => aktenId.id == wantedId) != null)
                {
                    AktenModule aktenModule = aktenModuleList.Find(aktenId => aktenId.id == wantedId);
                    aktenModuleList.Remove(aktenModule);
                    PlayerCrimeInformation.UpdateAktenCrimes(target.Name, NAPI.Util.ToJson(aktenModuleList));

                    switch (wantedId)
                    {
                        case 1:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 2:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 3:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                            break;
                        case 4:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 5);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 5:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 6:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                            break;
                        case 7:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                            break;
                        case 8:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 8000);
                            break;
                        case 9:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 10:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 11:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                            break;
                        case 12:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3500);
                            break;
                        case 13:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4500);
                            break;
                        case 14:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                            break;
                        case 15:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4000);
                            break;
                        case 16:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 17:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                            break;
                        case 18:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2000);
                            break;
                        case 19:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 20:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 21:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3500);
                            break;
                        case 22:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1000);
                            break;
                        case 23:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 24:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 25:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 26:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 27:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 4500);
                            break;
                        case 28:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                            break;
                        case 29:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                            break;
                        case 30:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                            break;
                        case 31:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2000);
                            break;
                        case 32:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 11000);
                            break;
                        case 33:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 34:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 35:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 36:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 37:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 38:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 75000);
                            break;
                        case 39:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 40:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                            break;
                        case 41:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                            break;
                        case 42:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 43:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 44:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 45:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 46:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                            break;
                        case 47:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 48:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 49:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 50:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 51:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6000);
                            break;
                        case 52:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 53:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 12500);
                            break;
                        case 54:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                            break;
                        case 55:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 56:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 57:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 58:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 59:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 60:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 61:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 62:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 63:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 64:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 65:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 66:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 30000);
                            break;
                        case 67:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 40000);
                            break;
                        case 68:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 40);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                            break;
                        case 69:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 50);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                            break;
                        case 70:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 71:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 72:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 2500);
                            break;
                        case 73:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 10);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 74:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 75:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 76:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                            break;
                        case 77:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6000);
                            break;
                        case 78:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 79:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 15);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 80:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 5000);
                            break;
                        case 81:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1500);
                            break;
                        case 82:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                            break;
                        case 83:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7500);
                            break;
                        case 84:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 85:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 3000);
                            break;
                        case 86:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 20);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 6500);
                            break;
                        case 87:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 88:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 1000);
                            break;
                        case 90:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 0);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 7000);
                            break;
                        case 91:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 20000);
                            break;
                        case 92:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 35);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 10000);
                            break;
                        case 93:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 150000);
                            break;
                        case 94:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 80);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 185000);
                            break;
                        case 95:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 35);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 16000);
                            break;
                        case 96:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 25);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 15000);
                            break;
                        case 97:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 90);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 100000);
                            break;
                        case 98:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 140000);
                            break;
                        case 99:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 60);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 100000);
                            break;
                        case 100:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 70);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 60000);
                            break;
                        case 101:
                            PlayerCrimeInformation.UpdateAktenJailtime(target.Name, PlayerCrimeInformation.GetJailtimeByName(target.Name) + 30);
                            PlayerCrimeInformation.UpdateAktenJailcost(target.Name, PlayerCrimeInformation.GetJailcostsByName(target.Name) + 25000);
                            break;
                        default:
                            break;
                    }

                    client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseOpenCrimes",
                        PlayerCrimeInformation.GetJailreasonsByName(target.Name));

                    client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseJailCosts",
                        PlayerCrimeInformation.GetJailcostsByName(target.Name));

                    client.TriggerEvent("componentServerEvent", "PoliceEditPersonApp", "responseJailTime",
                        PlayerCrimeInformation.GetJailtimeByName(target.Name));

                    foreach (Player c in NAPI.Pools.GetAllPlayers())
                    {
                        DbPlayer dbPlayer = c.GetPlayer();
                        if (dbPlayer.Faction.Id == 21)
                        {
                            dbPlayer.SendNotification(client.Name + " hat die Akte von " + target.Name + " bearbeitet!", 5000, "lightblue", "Los Santos Police Department");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"OnSavePersonData: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"OnSavePersonData: {exception.Message}");
            }
        }
    }
}
