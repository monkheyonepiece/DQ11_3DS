using System.ComponentModel;

namespace DQ11
{
	class CharStatus : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly uint mAddress;
		private readonly uint mBit;
		private readonly string mKey;

		public CharStatus(uint address, uint bit, string key)
		{
			mAddress = address;
			mBit = bit;
			mKey = key;
			mName = LocalizationManager.Get(key);
		}

		private string mName;
		public string Name
		{
			get { return mName; }
			private set
			{
				if (mName == value) return;
				mName = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
			}
		}

		/// <summary>Ré-résout le libellé selon la langue active (refresh à chaud).</summary>
		public void RefreshName()
		{
			Name = LocalizationManager.Get(mKey);
		}

		public bool Value
		{
			get
			{
				return SaveData.Instance().ReadBit(mAddress, mBit);
			}

			set
			{
				SaveData.Instance().WriteBit(mAddress, mBit, value);
			}
		}
	}
}
