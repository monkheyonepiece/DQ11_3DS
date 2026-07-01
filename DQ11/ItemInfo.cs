using System;
using System.ComponentModel;

namespace DQ11
{
	public class ItemInfo : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ItemInfo(uint id, String name, uint count)
		{
			ID = id;
			mName = name;
			Count = count;
		}

		public uint ID { get; set; }

		private String mName;
		public String Name
		{
			get { return mName; }
			set
			{
				if (mName == value) return;
				mName = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
			}
		}

		public uint Count { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
