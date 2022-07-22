using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
	public class ReasonModel
	{
		public string name
		{
			get;
			set;
		}

		public int id
		{
			get;
			set;
		}

		public int jailcost
		{
			get;
			set;
		}

		public int jailtime
		{
			get;
			set;
		}

		public ReasonModel(string name, int id, int jailcost, int jailtime)
		{
			this.name = name;
			this.id = id;
			this.jailcost = jailcost;
			this.jailtime = jailtime;
		}
	}
}
