﻿using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Venux
{
	public class KFZRentApp : Script
	{
		public static List<RentModel> renters = new List<RentModel>();

		[ServerEvent(Event.ResourceStart)]
		public void Start()
		{
			//renters.Add(new RentModel("Johnson_Mendosa", 46642, "18performante", "Lamborghini"));
			//renters.Add(new RentModel("Johnson_Mendosa", 46642, "turismor", "Turismor"));
		}

		[RemoteEvent("requestkfzrent")]
		public void requestkfzrent(Player p)
		{
			try
			{
				p.TriggerEvent("componentServerEvent", new object[3]
				{
				"KFZRentApp",
				"responsekfzrent",
				JsonConvert.SerializeObject(renters)
				});
			}
			catch (Exception e)
			{
				Logger.Print(e.ToString());
			}
		}

		[RemoteEvent("cancelkfzrent")]
		public void cancelkfzrent(Player p, int vehId)
		{
			DbPlayer dbPlayer = p.GetPlayer();
			dbPlayer.SendNotification("Dein Vertrag wurde nun erfolgreich beendet!", 3000, "gray", "KFZ");
			requestkfzrent(p);
			renters.Remove(renters.Find((RentModel renter) => renter.name == p.Name));
		}
	}
}
