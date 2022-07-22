using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class DbPlayer
    {
		public Player Client { get; set; }

		public string Name { get; set; }

		public int Id { get; set; }

		public string Password { get; set; }

		public int Money { get; set; }

		public AccountStatus AccountStatus { get; set; }

		public Faction Faction { get; set; }

		/*public bool SpielerFraktion { get; set; }*/

		public HashSet<int> HouseKeys { get; set; } = new HashSet<int>();
		public Dictionary<int, string> VehicleKeys { get; set; } = new Dictionary<int, string>();

		public Dictionary<int, string> OwnVehicles { get; set; } = new Dictionary<int, string>();

		public int Factionrank { get; set; }

		public Business Business { get; set; }

		public int Businessrank { get; set; }

		public int Level { get; set; }
		public int warns { get; set; }

		public string VoiceHash { get; set; }

		public int ForumId { get; set; }

		public bool IsCuffed { get; set; }

		public bool IsFarming { get; set; }

		public DeathData DeathData { get; set; }

		public bool __ActionsDisabled { get; set; }

		public bool AllActionsDisabled
		{
			get
			{
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return false;
				}
				return __ActionsDisabled;
			}
			set
			{
				if (NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					__ActionsDisabled = value;
					Client.TriggerEvent("disableAllPlayerActions", new object[1] { value });
				}
			}
		}

		public int Health
		{
			get
			{
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return 0;
				}
				return Client.Health;
			}
			set
			{
				if (NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					Client.Health = (value);
				}
			}
		}

		public int Armor
		{
			get
			{
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return 0;
				}
				return Client.Armor;
			}
			set
			{
				if (NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					Client.Armor = (value);
				}
			}
		}

		public Vector3 Position
		{
			get
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return new Vector3();
				}
				return ((Entity)Client).Position;
			}
			set
			{
				if (NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					((Entity)Client).Position = (value);
				}
			}
		}

		public float Heading
		{
			get
			{
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return 0f;
				}
				return ((Entity)Client).Heading;
			}
		}

		public int Dimension
		{
			get
			{
				if (!NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					return 0;
				}
				int result = 0;
				if (!int.TryParse(((Entity)Client).Dimension.ToString(), out result))
				{
					return 0;
				}
				return result;
			}
			set
			{
				if (NAPI.Pools.GetAllPlayers().Contains(Client))
				{
					uint result = 0u;
					if (uint.TryParse(value.ToString(), out result))
					{
						((Entity)Client).Dimension = (result);
					}
				}
			}
		}

		public PlayerClothes PlayerClothes { get; set; } = new PlayerClothes();


		public List<WeaponHash> Loadout { get; set; } = new List<WeaponHash>();

		public List<Decoration> Decoration { get; set; } = new List<Decoration>();

		public Adminrank Adminrank { get; set; }

		public bool Medic { get; set; } = false;


		public DateTime OnlineSince { get; set; }

		public DateTime LastInteracted { get; set; }

		public DateTime LastEInteract { get; set; }

		public DateTime LastDeath { get; set; }

		public bool HasData(string key)
		{
			return ((Entity)Client).HasData(key);
		}

		public void ResetData(string key)
		{
			((Entity)Client).ResetData(key);
		}

		public dynamic GetData(string key)
		{
			return ((Entity)Client).GetData<object>(key);
		}

		public void SetData(string key, object value)
		{
			((Entity)Client).SetData(key, value);
		}

		public bool HasSharedData(string key)
		{
			return ((Entity)Client).HasSharedData(key);
		}

		public void ResetSharedData(string key)
		{
			((Entity)Client).ResetSharedData(key);
		}

		public dynamic GetSharedData(string key)
		{
			return ((Entity)Client).GetSharedData<int>(key);
		}

		public void SetSharedData(string key, object value)
		{
			((Entity)Client).SetSharedData(key, value);
		}

		public void RefreshData(DbPlayer dbPlayer)
		{
			if ((Entity)(object)dbPlayer.Client == null)
			{
				return;
			}
			try
			{
				if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
				{
					((Entity)Client).ResetData("player");
					((Entity)Client).SetData("player", (object)dbPlayer);
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION RefreshData] " + ex.Message);
				Logger.Print("[EXCEPTION RefreshData] " + ex.StackTrace);
			}
		}

		public void TriggerEvent(string eventName, params object[] args)
		{
			Client.TriggerEvent(eventName, args);
		}

		public void StopAnimation()
		{
			Client.StopAnimation();
		}

        internal void clearDecorations() => throw new NotImplementedException();
    }
}
