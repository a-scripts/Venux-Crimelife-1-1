using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Venux.Module;

namespace Venux
{
    class TuningModule : Module<TuningModule>
    {
		public static List<Tuner> tuningList = new List<Tuner>();
		public static List<TuningCategory> tuningCategories = new List<TuningCategory>();

		public static List<TuningColor> tuningColors = new List<TuningColor>();
		public static List<NeonColor> tuningNeons = new List<NeonColor>();
		public static List<TuningPart> tuningEngines = new List<TuningPart>();
		public static List<TuningPart> tuningTurbo = new List<TuningPart>();
		public static List<TuningPart> tuningBrakes = new List<TuningPart>();
		public static List<TuningPart> tuningHorns = new List<TuningPart>();
		public static List<TuningPart> tuningTrails = new List<TuningPart>();
		public static List<TuningColor> tuningLights = new List<TuningColor>();
		public static List<TuningPart> tuningSpoilers = new List<TuningPart>();
		public static List<TuningColor> tuningWindows = new List<TuningColor>();
		public static List<TuningPart> tuningFrontbumper = new List<TuningPart>();
		public static List<TuningPart> tuningRearbumper = new List<TuningPart>();
		public static List<TuningPart> tuningSideskirt = new List<TuningPart>();
		public static List<TuningPart> tuningFrame = new List<TuningPart>();
		public static List<TuningPart> tuningExhaust = new List<TuningPart>();
		public static List<TuningPart> tuningGrille = new List<TuningPart>();
		public static List<TuningPart> tuningHood = new List<TuningPart>();
		public static List<TuningPart> tuningFender = new List<TuningPart>();
		public static List<TuningPart> tuningRoof = new List<TuningPart>();
		public static List<TuningPart> tuningLifery = new List<TuningPart>();
		public static List<TuningPart> tuningHydraulics = new List<TuningPart>();

		protected override bool OnLoad()
		{
			tuningCategories = new List<TuningCategory>
			{
				new TuningCategory("Motor - EMS", "ems"),
				new TuningCategory("Turbo", "turbo"),
				new TuningCategory("Bremsen", "brakes"),
				new TuningCategory("------------", "-"),
				new TuningCategory("Farbe", "color"),
				new TuningCategory("Sekundäre Farbe", "secondcolor"),
				new TuningCategory("Perleffekt", "pearl"),
				new TuningCategory("Neons", "neons"),
				new TuningCategory("Frontstoßstange", "frontbumper"),
				new TuningCategory("Rückstoßstange", "rearbumper"),
				new TuningCategory("Seitenverkleidung", "sideskirt"),
				new TuningCategory("Auspuff", "exhaust"),
				new TuningCategory("Rahmen", "frame"),
				new TuningCategory("Frontgrill", "grille"),
				new TuningCategory("Motorhaube", "hood"),
				new TuningCategory("Fender", "fender"),
				new TuningCategory("Dach", "roof"),
								new TuningCategory("Design", "lifery"),
				new TuningCategory("Hydraulik", "hydraulics"),
				new TuningCategory("Felgen", "trails"),
				new TuningCategory("Fensterscheiben", "glasses"),
				new TuningCategory("Frontlichter", "lights"),
				new TuningCategory("Hupe", "horn"),
				new TuningCategory("Spoiler", "spoiler")
				
			};

			/* tuningColors = new List<TuningColor>
			{
				new TuningColor("Schwarz", "black", 0, 7500),
				new TuningColor("Gelb", "yellow", 89, 7500),
				new TuningColor("Grün", "green", 55, 7500),
				new TuningColor("Lila", "purple", 145, 7500),
				new TuningColor("Blau", "blue", 70, 7500),
				new TuningColor("Rot", "red", 28, 7500),
				new TuningColor("Weiß", "white", 111, 7500)
			}; */

			for (int i = 0; i < 160; i++)
			{
				tuningColors.Add(new TuningColor("Farbe " + i, "color" + i, i, 0));
			}

			tuningEngines = new List<TuningPart>
			{
				new TuningPart("EMS Verbesserung 1", "ems1", 11, 0, 15000),
				new TuningPart("EMS Verbesserung 2", "ems2", 11, 1, 30000),
				new TuningPart("EMS Verbesserung 3", "ems3", 11, 2, 45000),
				new TuningPart("EMS Verbesserung 4", "ems4", 11, 3, 60000)
			};

			tuningTurbo = new List<TuningPart>
			{
				new TuningPart("EMS Verbesserung 1", "turbo1", 18, 0, 50000)
			};

			tuningBrakes = new List<TuningPart>
			{
				new TuningPart("Bremsen Upgrade 1", "brakes1", 12, 0, 8000),
				new TuningPart("Bremsen Upgrade 2", "brakes2", 12, 1, 16000),
				new TuningPart("Bremsen Upgrade 3", "brakes3", 12, 2, 24000)
			};

			tuningHorns = new List<TuningPart>
			{
				new TuningPart("Standart", "horn0", 14, -1, 0),
				new TuningPart("Hupe 1", "horn1", 14, 0, 0),
				new TuningPart("Hupe 2", "horn2", 14, 1, 0),
				new TuningPart("Hupe 3", "horn3", 14, 2, 0),
				new TuningPart("Hupe 4", "horn4", 14, 3, 0),
				new TuningPart("Hupe 5", "horn5", 14, 4, 0),
				new TuningPart("Hupe 6", "horn6", 14, 5, 0),
				new TuningPart("Hupe 7", "horn7", 14, 6, 0),
				new TuningPart("Hupe 8", "horn8", 14, 7, 0),
				new TuningPart("Hupe 9", "horn9", 14, 8, 0),
				new TuningPart("Hupe 10", "horn10", 14, 9, 0),
				new TuningPart("Hupe 11", "horn11", 14, 10, 0),
				new TuningPart("Hupe 12", "horn12", 14, 11, 0)
			};

			tuningLights = new List<TuningColor>
			{
				new TuningColor("Weiß", "light1", 0, 0),
				new TuningColor("Blau", "light2", 1, 5000),
				new TuningColor("Hellblau", "light3", 2, 5000),
				new TuningColor("Grün", "light4", 3, 5000),
				new TuningColor("Hellgrün", "light5", 4, 5000),
				new TuningColor("Helles Gelb", "light6", 5, 5000),
				new TuningColor("Gelb", "light7", 6, 5000),
				new TuningColor("Orange", "light8", 7, 5000),
				new TuningColor("Rot", "light9", 8, 5000),
				new TuningColor("Helles Pink", "light10", 9, 5000),
				new TuningColor("Pink", "light11", 10, 5000),
				new TuningColor("Lila", "light12", 11, 5000),
				new TuningColor("Helles Lila", "light13", 12, 5000)
			};

			tuningSpoilers = new List<TuningPart>
			{
				new TuningPart("Standart", "spoiler0", 0, -1, 0),
				new TuningPart("Spoiler 1", "spoiler1", 0, 0, 0),
				new TuningPart("Spoiler 2", "spoiler2", 0, 1, 0),
				new TuningPart("Spoiler 3", "spoiler3", 0, 2, 0),
				new TuningPart("Spoiler 4", "spoiler4", 0, 3, 0),
				new TuningPart("Spoiler 5", "spoiler5", 0, 4, 0),
				new TuningPart("Spoiler 6", "spoiler6", 0, 5, 0),
				new TuningPart("Spoiler 7", "spoiler7", 0, 6, 0),
				new TuningPart("Spoiler 8", "spoiler8", 0, 7, 0),
				new TuningPart("Spoiler 9", "spoiler9", 0, 8, 0),
				new TuningPart("Spoiler 10", "spoiler10", 0, 9, 0),
				new TuningPart("Spoiler 11", "spoiler11", 0, 10, 0),
				new TuningPart("Spoiler 12", "spoiler12", 0, 11, 0)
			};

			tuningWindows = new List<TuningColor>
			{
				new TuningColor("Standart", "window0", 0, 0),
				new TuningColor("Farbe 1", "window1", 1, 0),
				new TuningColor("Farbe 2", "window2", 2, 0),
				new TuningColor("Farbe 3", "window3", 3, 0),
				new TuningColor("Farbe 4", "window4", 4, 0),
				new TuningColor("Farbe 5", "window5", 5, 0),
				new TuningColor("Farbe 6", "window6", 6, 0)
			};

			tuningFrontbumper = new List<TuningPart>
			{
				new TuningPart("Standart", "fbumper0", 1, -1, 0),
				new TuningPart("Stoßstange 1", "fbumper1", 1, 0, 0),
				new TuningPart("Stoßstange 2", "fbumper2", 1, 1, 0),
				new TuningPart("Stoßstange 3", "fbumper3", 1, 2, 0),
				new TuningPart("Stoßstange 4", "fbumper4", 1, 3, 0),
				new TuningPart("Stoßstange 5", "fbumper5", 1, 4, 0),
				new TuningPart("Stoßstange 6", "fbumper6", 1, 5, 0),
				new TuningPart("Stoßstange 7", "fbumper7", 1, 6, 0),
				new TuningPart("Stoßstange 8", "fbumper8", 1, 7, 0),
			};

			tuningRearbumper = new List<TuningPart>
			{
				new TuningPart("Standart", "rbumper0", 2, -1, 0),
				new TuningPart("Stoßstange 1", "rbumper1", 2, 0, 0),
				new TuningPart("Stoßstange 2", "rbumper2", 2, 1, 0),
				new TuningPart("Stoßstange 3", "rbumper3", 2, 2, 0),
				new TuningPart("Stoßstange 4", "rbumper4", 2, 3, 0),
				new TuningPart("Stoßstange 5", "rbumper5", 2, 4, 0),
				new TuningPart("Stoßstange 6", "rbumper6", 2, 5, 0),
				new TuningPart("Stoßstange 7", "rbumper7", 2, 6, 0),
				new TuningPart("Stoßstange 8", "rbumper8", 2, 7, 0),
			};

			tuningSideskirt = new List<TuningPart>
			{
				new TuningPart("Standart", "skirt0", 3, -1, 0),
				new TuningPart("Seitenverkleidung 1", "skirt1", 3, 0, 0),
				new TuningPart("Seitenverkleidung 2", "skirt2", 3, 1, 0),
				new TuningPart("Seitenverkleidung 3", "skirt3", 3, 2, 0),
				new TuningPart("Seitenverkleidung 4", "skirt4", 3, 3, 0),
				new TuningPart("Seitenverkleidung 5", "skirt5", 3, 4, 0),
				new TuningPart("Seitenverkleidung 6", "skirt6", 3, 5, 0),
				new TuningPart("Seitenverkleidung 7", "skirt7", 3, 6, 0),
				new TuningPart("Seitenverkleidung 8", "skirt8", 3, 7, 0),
			};

			tuningFrame = new List<TuningPart>
			{
				new TuningPart("Standart", "frame0", 5, -1, 0),
				new TuningPart("Rahmen 1", "frame1", 5, 0, 0),
				new TuningPart("Rahmen 2", "frame2", 5, 1, 0),
				new TuningPart("Rahmen 3", "frame3", 5, 2, 0),
				new TuningPart("Rahmen 4", "frame4", 5, 3, 0),
				new TuningPart("Rahmen 5", "frame5", 5, 4, 0),
				new TuningPart("Rahmen 6", "frame6", 5, 5, 0),
				new TuningPart("Rahmen 7", "frame7", 5, 6, 0),
				new TuningPart("Rahmen 8", "frame8", 5, 7, 0),
			};

			tuningGrille = new List<TuningPart>
			{
				new TuningPart("Standart", "grill0", 6, -1, 0),
				new TuningPart("Grill 1", "grill1", 6, 0, 0),
				new TuningPart("Grill 2", "grill2", 6, 1, 0),
				new TuningPart("Grill 3", "grill3", 6, 2, 0),
				new TuningPart("Grill 4", "grill4", 6, 3, 0),
				new TuningPart("Grill 5", "grill5", 6, 4, 0),
				new TuningPart("Grill 6", "grill6", 6, 5, 0),
				new TuningPart("Grill 7", "grill7", 6, 6, 0),
				new TuningPart("Grill 8", "grill8", 6, 7, 0),
			};

			tuningExhaust = new List<TuningPart>
			{
				new TuningPart("Standart", "exhaust0", 4, -1, 0),
				new TuningPart("Auspuff 1", "exhaust1", 4, 0, 0),
				new TuningPart("Auspuff 2", "exhaust2", 4, 1, 0),
				new TuningPart("Auspuff 3", "exhaust3", 4, 2, 0),
				new TuningPart("Auspuff 4", "exhaust4", 4, 3, 0),
				new TuningPart("Auspuff 5", "exhaust5", 4, 4, 0),
				new TuningPart("Auspuff 6", "exhaust6", 4, 5, 0),
				new TuningPart("Auspuff 7", "exhaust7", 4, 6, 0),
				new TuningPart("Auspuff 8", "exhaust8", 4, 7, 0),
			};

			tuningHood = new List<TuningPart>
			{
				new TuningPart("Standart", "hood0", 7, -1, 0),
				new TuningPart("Motorhaube 1", "hood1", 7, 0, 0),
				new TuningPart("Motorhaube 2", "hood2", 7, 1, 0),
				new TuningPart("Motorhaube 3", "hood3", 7, 2, 0),
				new TuningPart("Motorhaube 4", "hood4", 7, 3, 0),
				new TuningPart("Motorhaube 5", "hood5", 7, 4, 0),
				new TuningPart("Motorhaube 6", "hood6", 7, 5, 0),
				new TuningPart("Motorhaube 7", "hood7", 7, 6, 0),
				new TuningPart("Motorhaube 8", "hood8", 7, 7, 0),
			};

			tuningFender = new List<TuningPart>
			{
				new TuningPart("Standart", "fender0", 8, -1, 0),
				new TuningPart("Fender 1", "fender1", 8, 0, 0),
				new TuningPart("Fender 2", "fender2", 8, 1, 0),
				new TuningPart("Fender 3", "fender3", 8, 2, 0),
				new TuningPart("Fender 4", "fender4", 8, 3, 0),
				new TuningPart("Fender 5", "fender5", 8, 4, 0),
				new TuningPart("Fender 6", "fender6", 8, 5, 0),
				new TuningPart("Fender 7", "fender7", 8, 6, 0),
				new TuningPart("Fender 8", "fender8", 8, 7, 0),
			};

			tuningRoof = new List<TuningPart>
			{
				new TuningPart("Standart", "roof0", 10, -1, 0),
				new TuningPart("Dach 1", "roof1", 10, 0, 0),
				new TuningPart("Dach 2", "roof2", 10, 1, 0),
				new TuningPart("Dach 3", "roof3", 10, 2, 0),
				new TuningPart("Dach 4", "roof4", 10, 3, 0),
				new TuningPart("Dach 5", "roof5", 10, 4, 0),
				new TuningPart("Dach 6", "roof6", 10, 5, 0),
				new TuningPart("Dach 7", "roof7", 10, 6, 0),
				new TuningPart("Dach 8", "roof8", 10, 7, 0),
			};

			tuningLifery = new List<TuningPart>
			{
				new TuningPart("Standart", "lifery0", 48, -1, 0),
				new TuningPart("Design 1", "lifery1", 48, 0, 0),
				new TuningPart("Design 2", "lifery2", 48, 1, 0),
				new TuningPart("Design 3", "lifery3", 48, 2, 0),
				new TuningPart("Design 4", "lifery4", 48, 3, 0),
				new TuningPart("Design 5", "lifery5", 48, 4, 0),
				new TuningPart("Design 6", "lifery6", 48, 5, 0),
				new TuningPart("Design 7", "lifery7", 48, 6, 0),
				new TuningPart("Design 8", "lifery8", 48, 7, 0),
			};

			tuningHydraulics = new List<TuningPart>
			{
				new TuningPart("Standart", "hydraulics0", 38, -1, 0),
				new TuningPart("Hydraulik 1", "hydraulics1", 38, 0, 10000),
				new TuningPart("Hydraulik 2", "hydraulics2", 38, 1, 10000),
				new TuningPart("Hydraulik 3", "hydraulics3", 38, 2, 10000)
			};

			/*for (int i = 0; i < 30; i++)
			{
				tuningWindowTint.Add(new TuningPart("Farbe " + i, "windowtint" + i, 46, i, 0));
			}*/

			for (int i = 0; i < 130; i++)
            {
				tuningTrails.Add(new TuningPart("Felge " + i, "felge" + i, 23, i, 0));
            }

			tuningNeons = new List<NeonColor>
			{
				new NeonColor("Aus", "off", new Color(0, 0, 0), 0),
				new NeonColor("Schwarz", "neon1", new Color(0, 0, 0), 10000),
				new NeonColor("Hell Grün", "neon2", new Color(0, 255, 0), 10000),
				new NeonColor("Helles Gelb", "neon3", new Color(255, 255, 0), 10000),
				new NeonColor("Lila", "neon4", new Color(136, 0, 255), 10000),
				new NeonColor("Hell Blau", "neon5", new Color(0, 102, 255), 10000),
				new NeonColor("Rot", "neon6", new Color(255, 0, 0), 10000),
				new NeonColor("Weiß", "neon7", new Color(255, 255, 255), 10000),
				new NeonColor("Grün", "neon8", new Color(48, 243, 214), 10000),
				new NeonColor("Orange", "neon9", new Color(242, 125, 32), 10000),
				new NeonColor("Blau", "neon10", new Color(49, 107, 184), 10000),
				new NeonColor("Gelb", "neon11", new Color(221, 197, 50), 10000), 
				new NeonColor("Pink", "neon12", new Color(171, 125, 151), 10000), 
				new NeonColor("Helles Pink", "neon13", new Color(171, 58, 163), 10000)
			};

			tuningList.Add(new Tuner
            {
                Id = 1,
                Name = "Los Santos Customs",
                Position = new Vector3(-361, -115, 38)
            });



            foreach (Tuner tuner in tuningList)
            {
                ColShape val = NAPI.ColShape.CreateCylinderColShape(tuner.Position, 25f, 23f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openTuner"));
				NAPI.Marker.CreateMarker(1, new Vector3(-361, -115, 15), new Vector3(), new Vector3(), 25.0f, new Color(255, 0, 0), false, 0);
				val.SetData("MESSAGE", new Message("Drücke E um dein Fahrzeug zu tunen.", "TUNER", "red", 3000));

                NAPI.Blip.CreateBlip(72, tuner.Position, 1f, 0, tuner.Name, 255, 0, true, 0, 0);
            }
			return true;
        }

		[RemoteEvent("openTuner")]
		public void openTuner(Player c)
		{
			try
			{
				if (c == null) return;
				
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
					return;

				if (c.IsInVehicle)
				{
					DbVehicle dbVehicle = c.Vehicle.GetVehicle();
					if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
						return;

					if (dbVehicle.OwnerId != dbPlayer.Id) return;

					List<NativeItem> nativeItems = new List<NativeItem>();
					foreach (TuningCategory tuningCategory in tuningCategories)
					{
						nativeItems.Add(new NativeItem(tuningCategory.Label, tuningCategory.Name));
					}

					NativeMenu nativeMenu = new NativeMenu("Tuner", "Angebote", nativeItems);
					dbPlayer.ShowNativeMenu(nativeMenu);
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION openTuner] " + ex.Message);
				Logger.Print("[EXCEPTION openTuner] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Tuner")]
		public void Tuner(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;
				
				if (selection == "color")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningColor tuningColor in tuningColors)
						{
							nativeItems.Add(new NativeItem(tuningColor.Label + " - " + tuningColor.Price + "$",
								tuningColor.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Farbe", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "secondcolor")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningColor tuningColor in tuningColors)
						{
							nativeItems.Add(new NativeItem(tuningColor.Label + " - " + tuningColor.Price + "$",
								tuningColor.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Sekundärfarbe", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}
					else if (selection == "pearl")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningColor tuningColor in tuningColors)
						{
							nativeItems.Add(new NativeItem(tuningColor.Label + " - " + tuningColor.Price + "$",
								tuningColor.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Perleffekt", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}
					else if (selection == "neons")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (NeonColor neonColor in tuningNeons)
						{
							nativeItems.Add(new NativeItem(neonColor.Label + " - " + neonColor.Price + "$",
								neonColor.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Neons", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}
					else if (selection == "ems")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningEngines)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}
					else if (selection == "glasses")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningColor tuningColor in tuningWindows)
						{
							nativeItems.Add(new NativeItem(tuningColor.Label + " - " + tuningColor.Price + "$",
								tuningColor.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Fensterscheiben", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}
					else if (selection == "turbo")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningTurbo)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "brakes")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningBrakes)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "frontbumper")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningFrontbumper)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "rearbumper")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningRearbumper)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "sideskirt")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningSideskirt)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "exhaust")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningExhaust)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "frame")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningFrame)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "grille")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningGrille)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "hood")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningHood)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "fender")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningFender)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "roof")
					{
						DbVehicle dbVehicle = veh.GetVehicle();
						if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
							return;

						if (dbVehicle.Model.ToLower() == "revolter")
						{
							dbPlayer.SendNotification("Du kannst bei dem Revolter nicht das Dach tunen.", 3000, "red",
								"TUNING");
							dbPlayer.CloseNativeMenu();
							return;
						}

						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningRoof)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "hydraulics")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningHydraulics)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
				else if (selection == "lifery")
				{
					List<NativeItem> nativeItems = new List<NativeItem>();
					foreach (TuningPart tuningPart in tuningLifery)
					{
						nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
							tuningPart.Name));
					}

					NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
					dbPlayer.ShowNativeMenu(nativeMenu);

				}
				else if (selection == "horn")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningHorns)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "spoiler")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningSpoilers)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "trails")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningPart tuningPart in tuningTrails)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Tuning", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);

					}
					else if (selection == "lights")
					{
						List<NativeItem> nativeItems = new List<NativeItem>();
						foreach (TuningColor tuningPart in tuningLights)
						{
							nativeItems.Add(new NativeItem(tuningPart.Label + " - " + tuningPart.Price + "$",
								tuningPart.Name));
						}

						NativeMenu nativeMenu = new NativeMenu("Frontlichter", "Angebote", nativeItems);
						dbPlayer.ShowNativeMenu(nativeMenu);
					}


				}
				catch (Exception ex)
				{
					Logger.Print("[EXCEPTION openTuner] " + ex.Message);
					Logger.Print("[EXCEPTION openTuner] " + ex.StackTrace);
				}
			}
		
		[RemoteEvent("nM-Frontlichter")]
		public void Frontlichter(Player c, string selection)
		{
			try
			{
				if (c == null) return;

				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningColor tuningLight =
					tuningLights.Find((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningLight == null) return;

				if (dbPlayer.Money >= tuningLight.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningLight.Price.ToDots() + "$ getuned.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningLight.Price);
					dbVehicle.SetAttribute("HeadlightColor", tuningLight.ColorId);
					veh.SetSharedData("headlightColor", tuningLight.ColorId);
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Frontlichter] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Frontlichter] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Neons")]
		public void Neons(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				NeonColor neonColor = tuningNeons.Find((NeonColor neonColor2) => neonColor2.Name == selection);
				if (neonColor == null) return;

				if (selection == "off")
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + neonColor.Price.ToDots() + "$ getuned.",
						3000, "red", "TUNER");
					dbVehicle.SetAttribute("NeonColor", NAPI.Util.ToJson(neonColor.Color));
					dbVehicle.SetAttribute("Neons", 0);
					veh.NeonColor = neonColor.Color;
					veh.Neons = false;
					return;
				}

				if (dbPlayer.Money >= neonColor.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + neonColor.Price.ToDots() + "$ getuned.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(neonColor.Price);
					dbVehicle.SetAttribute("NeonColor", NAPI.Util.ToJson(neonColor.Color));
					dbVehicle.SetAttribute("Neons", 1);
					veh.NeonColor = neonColor.Color;
					veh.Neons = true;
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Neons] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Neons] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Fensterscheiben")]
		public void Fensterscheiben(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningColor tuningColor =
					tuningWindows.Find((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor == null) return;

				if (dbPlayer.Money >= tuningColor.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningColor.Price.ToDots() + "$ lackiert.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningColor.Price);
					dbVehicle.SetAttribute("WindowTint", tuningColor.ColorId);
					dbVehicle.WindowTint = tuningColor.ColorId;
					dbVehicle.RefreshData(dbVehicle);
					veh.WindowTint = tuningColor.ColorId;
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION nM-Fensterscheiben] " + ex.Message);
				Logger.Print("[EXCEPTION nM-Fensterscheiben] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Farbe")]
		public void FirstColor(Player c, string selection)
		{
			try
			{
				if (c == null) return;

				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningColor tuningColor =
					tuningColors.Find((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor == null) return;

				if (dbPlayer.Money >= tuningColor.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningColor.Price.ToDots() + "$ lackiert.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningColor.Price);
					dbVehicle.SetAttribute("PrimaryColor", tuningColor.ColorId);
					dbVehicle.PrimaryColor = tuningColor.ColorId;
					dbVehicle.RefreshData(dbVehicle);
					veh.PrimaryColor = tuningColor.ColorId;
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION FirstColor] " + ex.Message);
				Logger.Print("[EXCEPTION FirstColor] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Sekundärfarbe")]
		public void SecondColor(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningColor tuningColor =
					tuningColors.Find((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor == null) return;

				if (dbPlayer.Money >= tuningColor.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningColor.Price.ToDots() + "$ lackiert.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningColor.Price);
					dbVehicle.SetAttribute("SecondaryColor", tuningColor.ColorId);
					dbVehicle.SecondaryColor = tuningColor.ColorId;
					dbVehicle.RefreshData(dbVehicle);
					veh.SecondaryColor = tuningColor.ColorId;
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Sekundärfarbe] " + ex.Message);
				Logger.Print("[EXCEPTION Sekundärfarbe] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Perleffekt")]
		public void Pearlescent(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningColor tuningColor =
					tuningColors.Find((TuningColor tuningColor2) => tuningColor2.Name == selection);
				if (tuningColor == null) return;

				if (dbPlayer.Money >= tuningColor.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningColor.Price.ToDots() + "$ lackiert.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningColor.Price);
					dbVehicle.SetAttribute("PearlescentColor", tuningColor.ColorId);
					dbVehicle.PearlescentColor = tuningColor.ColorId;
					dbVehicle.RefreshData(dbVehicle);
					veh.PearlescentColor = tuningColor.ColorId;
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Perleffekt] " + ex.Message);
				Logger.Print("[EXCEPTION Perleffekt] " + ex.StackTrace);
			}
		}

		[RemoteEvent("nM-Tuning")]
		public void Tuning(Player c, string selection)
		{
			try
			{
				if (c == null) return;
				DbPlayer dbPlayer = c.GetPlayer();
				if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

				Vehicle veh = c.Vehicle;
				if (veh == null) return;

				DbVehicle dbVehicle = veh.GetVehicle();
				if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null) return;

				TuningPart tuningPart = tuningBrakes.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningEngines.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningHorns.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningTurbo.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningTrails.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningSpoilers.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningFrontbumper.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningRearbumper.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningSideskirt.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningFrame.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningExhaust.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningGrille.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningHood.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningFender.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningRoof.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningLifery.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null)
					tuningPart = tuningHydraulics.Find((TuningPart tuningPart2) => tuningPart2.Name == selection);
				if (tuningPart == null) return;

				if (dbPlayer.Money >= tuningPart.Price)
				{
					dbPlayer.SendNotification("Du hast dein Fahrzeug für " + tuningPart.Price.ToDots() + "$ getuned.",
						3000, "red", "TUNER");
					dbPlayer.removeMoney(tuningPart.Price);
					Dictionary<int, int> dict = new Dictionary<int, int>();
					string saved = dbVehicle.GetAttributeString("Tuning");
					if (saved != null && saved != "[]") dict = NAPI.Util.FromJson<Dictionary<int, int>>(saved);
					if (!dict.ContainsKey(tuningPart.Index))
					{
						dict.Add(tuningPart.Index, tuningPart.PartId);
					}
					else
					{
						dict[tuningPart.Index] = tuningPart.PartId;
					}

					dbVehicle.SetAttribute("Tuning", NAPI.Util.ToJson(dict));
					dbVehicle.Tuning = dict;
					dbVehicle.RefreshData(dbVehicle);
					veh.SetMod(tuningPart.Index, tuningPart.PartId);
				}
				else
				{
					dbPlayer.SendNotification("Du besitzt nicht genug Geld!", 3000, "red", "TUNER");
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION Tuning] " + ex.Message);
				Logger.Print("[EXCEPTION Tuning] " + ex.StackTrace);
			}
		}
	}
}
