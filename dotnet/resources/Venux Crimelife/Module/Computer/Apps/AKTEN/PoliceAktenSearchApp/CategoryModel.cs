using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
	class CategoryModel
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

		public CategoryModel(string name, int id)
		{
			this.name = name;
			this.id = id;
		}
	}
}
