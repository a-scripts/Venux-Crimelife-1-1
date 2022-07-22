/*using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GVMP.Tattoo
{
public class TattooRegister : Script
{
	public static Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();

	[ServerEvent(Event.ResourceStart)]
	public void onResourceStart()
	{
		if (points.Count < 1)
		{
			points.Add("Vespucci Tattooladen", new Vector3(-1155.644, -1427.01, 3.854458));
		}

		foreach (KeyValuePair<string, Vector3> point in points)
		{
			NAPI.Blip.CreateBlip(71, point.Value, 1f, 0, point.Key, 255, 0, true, 0, 0);

			ColShape colShape = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1155.644, -1427.01, 4.854458), 1.4f, 1.4f, uint.MaxValue);
			colShape.SetData("FUNCTION_MODEL", new FunctionModel("openTattooLaden"));
			colShape.SetData("MESSAGE", new Message("Drücke E um den Tattooladen zu benutzen", "TATTOO", "orange", 5000));
		}
	}

	[RemoteEvent("openTattooLaden")]
	public void openTattooLaden(Client c)
	{
		DbPlayer dbPlayer = c.GetPlayer();
		{
			object JSONobject = new
			{
				Besitzer = "   Aspect"
			};

			dbPlayer.TriggerEvent("sendInfocard", "Casino", "lightblue", "storage.jpg", 7500, NAPI.Util.ToJson(JSONobject));
			return;
		}
	}
}
}



using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GVMP.Tattoo
{
public class TattooRegister : Script
{
	public static Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();


	[ServerEvent(Event.PlayerEnterColshape)]
	public void enterColShape(Client c)
	{
		DbPlayer dbPlayer = c.GetPlayer();
		if (c.Position.DistanceTo(new Vector3(1179.315, -330.7011, 68.21653)) < 60f)
		{
			object JSONobject = new
			{
				Besitzer = "   Aspect",
				Kosten = "   50.000$"
			};

			dbPlayer.TriggerEvent("sendInfocard", "Casino", "lightblue", "storage.jpg", 4500, NAPI.Util.ToJson(JSONobject));
			return;
		}
	}
}
}*/
