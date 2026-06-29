using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DQ11
{
	class Party : IListItem ,INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private const uint mSize = 1;
		private readonly ObservableCollection<Character> mChars;
		private readonly uint mBaseAddress;

		public String Name
		{
			get
			{
				return mChars[ID].Name;
			}
		}

		/// <summary>Nom d'affichage traduit, délégué au Character occupant ce slot.</summary>
		public String DisplayName
		{
			get
			{
				return mChars[ID].DisplayName;
			}
		}

		/// <summary>Notifie l'UI que DisplayName a pu changer (bascule de langue).</summary>
		public void RefreshDisplayName()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
		}
		public int ID
		{
			get
			{
				return (int)SaveData.Instance().ReadNumber(mBaseAddress, mSize);
			}

			set
			{
				SaveData.Instance().WriteNumber(mBaseAddress, mSize, (uint)value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ID"));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
			}
		}
		public Party(ObservableCollection<Character> chars, uint address)
		{
			mChars = chars;
			mBaseAddress = address;
		}

		public uint Address()
		{
			return mBaseAddress;
		}

		public void Remove(IListItem item)
		{
			SaveData savedate = SaveData.Instance();
			savedate.WriteNumber(mBaseAddress, mSize, savedate.ReadNumber(item.Address(), mSize));
		}

		public void Create()
		{
			SaveData.Instance().WriteNumber(mBaseAddress, mSize, 0x00);
		}

		public void Clear()
		{
			SaveData.Instance().WriteNumber(mBaseAddress, mSize, 0xFF);
		}

		public void Swap(IListItem item)
		{
			SaveData savedate = SaveData.Instance();
			savedate.Swap(Address(), item.Address(), mSize);
		}
	}
}
