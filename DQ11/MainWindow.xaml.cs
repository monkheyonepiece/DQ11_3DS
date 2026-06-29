using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DQ11
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<AllStatus> mAllStatusList;

		Bag mBagTool;
		Bag mBagEquipment;

		ButtonCheckObserver mHatButtonCheck;
		ButtonCheckObserver mTitleButtonCheck;
		ButtonCheckObserver mSmithButtonCheck;
		ButtonCheckObserver mCollectionButtonCheck;
		public MainWindow()
		{
			InitializeComponent();
			// Abonnement unique (durée de vie = fenêtre) : pas de fuite, et le
			// DataContext recréé à chaque chargement est relu à chaud dans le handler.
			LocalizationManager.LanguageChanged += OnLanguageChanged;
		}

		/// <summary>
		/// Rafraîchit le nom AFFICHÉ des personnages (onglet Character et onglet Party)
		/// lors d'un changement de langue, sans recharger ni perdre la sélection.
		/// </summary>
		private void OnLanguageChanged()
		{
			// Re-localise le libellé "aucun fichier" même si aucune save n'est chargée.
			UpdateFilePathDisplay();

			DataContext context = DataContext as DataContext;
			if (context == null) return;

			foreach (Character ch in context.Char)
			{
				ch.RefreshDisplayName();
			}
			foreach (IListItem item in context.Party.List)
			{
				(item as Party)?.RefreshDisplayName();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Item.Instance();
			SaveData.Instance();

			// 全体の設定.
			mAllStatusList = new List<AllStatus>();

			// れんけい・スキル.
			mAllStatusList.Add(new Technique(ListBoxTechnique, ButtonTechniqueCheck, ButtonTechniqueUnCheck));

			// モンスター図鑑.
			mAllStatusList.Add(new Monster(StackPanelMonster, RadioButtonAll, RadioButtonNone, RadioButtonHave, TextBoxMonsterCount, ButtonMonsterDecision, TextBoxMonsterSearch));

			// ふくろ.
			mBagTool = new Bag(mAllStatusList, StackPanelBagTool, ItemSelectWindow.eType.Tool, ComboBoxBagToolPage, Util.BagToolStartAddress, Util.BagToolCount);
			mBagEquipment = new Bag(mAllStatusList, StackPanelBagEquipment, ItemSelectWindow.eType.Equipment, ComboBoxBagEquipmentPage, Util.BagEquipmentStartAddress, Util.BagEquipmentCount);

			// だいじなもの.
			mAllStatusList.Add(new CheckBoxListItem(ListBoxImportant, ButtonImportantCheck, ButtonImportantUnCheck, Item.Instance().Importants, 0x65C4, 90));

			// レシピ.
			mAllStatusList.Add(new CheckBoxListItem(ListBoxRecipe, ButtonRecipeCheck, ButtonRecipeUnCheck, Item.Instance().Recipes, 0x6678, 105));

			// 帽子.
			mHatButtonCheck = new ButtonCheckObserver(ButtonHatCheck, ButtonHatUnCheck);
			CreateHat(mAllStatusList, StackPanelHat);

			// 冒険の書の合言葉.
			mAllStatusList.Add(new WatchWord(ListBoxWatchWorld, ButtonWatchWorldCheck, ButtonWatchWorldUnCheck));

			// 鍛冶.
			mSmithButtonCheck = new ButtonCheckObserver(ButtonSmithCheck, ButtonSmithUnCheck);
			CreateSmith(mAllStatusList, ListBoxSmith);

			// アイテム収集.
			mCollectionButtonCheck = new ButtonCheckObserver(ButtonCollectionCheck, ButtonCollectionUnCheck);
			CreateCollection(mAllStatusList, ListBoxCollection);

			// 称号.
			mTitleButtonCheck = new ButtonCheckObserver(ButtonTitleCheck, ButtonTitleUnCheck);
			CreateTitle(mAllStatusList, ListBoxTitle);

			// クエスト.
			mAllStatusList.Add(new Quest(ListBoxQuest, ComboBoxQuestState, ButtonQuestPatch));

			// ルーラ.
			mAllStatusList.Add(new Zoom(ListBoxZoom, ButtonZoomCheck, ButtonZoomUnCheck));

			// ストーリー.
			mAllStatusList.Add(new Story(ListBoxStory, ButtonStoryCheck, ButtonStoryUnCheck));
			
			mAllStatusList.ForEach(x => x.Init());

			UpdateLanguageMenu();
			UpdateFilePathDisplay();
		}

		private void Lang_Click(object sender, RoutedEventArgs e)
		{
			var item = sender as System.Windows.Controls.MenuItem;
			if (item == null) return;
			LocalizationManager.SetLanguage((string)item.Tag);
			UpdateLanguageMenu();
		}

		private void UpdateLanguageMenu()
		{
			string lang = LocalizationManager.CurrentLanguage;
			LangEnglish.IsChecked = (lang == "en");
			LangFrench.IsChecked = (lang == "fr");
			LangJapanese.IsChecked = (lang == "ja");
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;
			if (!System.IO.File.Exists(files[0])) return;

			SaveData saveData = SaveData.Instance();
			if (saveData.Open(files[0], false) == false)
			{
				MessageBox.Show(LocalizationManager.Get("Msg_LoadFail"));
				return;
			}

			Init();
			MessageBox.Show(LocalizationManager.Get("Msg_LoadOk"));
		}

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void ToolBarFileOpen_Click(object sender, RoutedEventArgs e)
		{
			Load(false);
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			Load(false);
		}

		private void MenuItemFileOpenForce_Click(object sender, RoutedEventArgs e)
		{
			Load(true);
		}

		private void ToolBarFileSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
		{
			mAllStatusList.ForEach(x => x.Save());

			SaveFileDialog dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			if (SaveData.Instance().SaveAs(dlg.FileName) == true)
			{
				UpdateFilePathDisplay();
				MessageBox.Show(LocalizationManager.Get("Msg_SaveOk"));
			}
			else MessageBox.Show(LocalizationManager.Get("Msg_SaveFail"));
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			new AboutWindow().ShowDialog();
		}

		private void ButtonCharItemChange_Click(object sender, RoutedEventArgs e)
		{
			CharItem item = (sender as Button)?.DataContext as CharItem;
			if (item == null) return;
			ItemSelectWindow window = new ItemSelectWindow();
			window.ID = item.ID;
			window.ShowDialog();
			item.ID = window.ID;
		}

		private void ButtonCharStatusMin_Click(object sender, RoutedEventArgs e)
		{
			Character ch = ListBoxChar.SelectedItem as Character;
			ch?.Min();
		}

		private void ButtonCharStatusMax_Click(object sender, RoutedEventArgs e)
		{
			Character ch = ListBoxChar.SelectedItem as Character;
			ch?.Max();
		}

		private void ButtonYochiWeaponChange_Click(object sender, RoutedEventArgs e)
		{
			Yochi yochi = ListBoxYochi.SelectedItem as Yochi;
			if (yochi == null) return;
			ItemSelectWindow window = new ItemSelectWindow();
			window.Type = ItemSelectWindow.eType.Equipment;
			window.ID = yochi.Weapon;
			window.ShowDialog();
			yochi.Weapon = window.ID;
		}

		private void ButtonBagChange_Click(object sender, RoutedEventArgs e)
		{
			SaveData save = SaveData.Instance();
			uint address = Util.BagToolStartAddress;
			uint count = Util.BagToolCount;
			List<ItemInfo> infos = Item.Instance().Tools;
			if (RadioButtonBagTypeEquipment.IsChecked == true)
			{
				address = Util.BagEquipmentStartAddress;
				count = Util.BagEquipmentCount;
				infos = Item.Instance().Equipments;
			}


			if (RadioButtonBagClear.IsChecked == true)
			{
				for (uint i = 0; i < count; i++)
				{
					save.WriteNumber(address + i * 4, 4, 0xFFFF);
				}
			}
			else if (RadioButtonBagAppend.IsChecked == true)
			{
				uint number;
				if (!uint.TryParse(TextBoxBagItemCount.Text, out number))
				{
					return;
				}

				for (uint i = 0; i < infos.Count; i++)
				{
					ItemInfo info = infos[(int)i];

					for (uint j = 0; j < info.Count; j++)
					{
						save.WriteNumber(address, 2, info.ID + j);
						save.WriteNumber(address + 2, 2, number);
						address += 4;
					}
				}
			}
			else
			{
				uint number;
				if (!uint.TryParse(TextBoxBagItemCount.Text, out number))
				{
					return;
				}
				for (uint i = 0; i < count; i++)
				{
					uint id = save.ReadNumber(address + i * 4, 4);
					if (id == Item.Instance().None.ID) break;
					save.WriteNumber(address + i * 4 + 2, 2, number);
				}
			}
			Init();
		}

		private void ButtonPartyUp_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxParty.SelectedIndex < 0) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Party.Up(ListBoxParty.SelectedIndex);
			context.PartyInit();
		}

		private void ButtonPartyDown_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxParty.SelectedIndex < 0) return;
			if (ListBoxParty.SelectedIndex >= ListBoxParty.Items.Count - 1) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Party.Down(ListBoxParty.SelectedIndex);
			context.PartyInit();
		}

		private void ButtonPartyAppend_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxParty.Items.Count >= Util.CharCount) return;

			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.PartyAppend();
			SaveData.Instance().WriteNumber(0x6580, 1, (uint)ListBoxParty.Items.Count);
		}

		private void ButtonPartyRemove_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxParty.SelectedIndex < 0) return;
			if (ListBoxParty.Items.Count <= 1) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Party.Remove(ListBoxParty.SelectedIndex);
			context.PartyInit();
			SaveData.Instance().WriteNumber(0x6580, 1, (uint)ListBoxParty.Items.Count);
		}

		private void ButtonYochiUp_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxYochi.SelectedIndex < 0) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Yochi.Up(ListBoxYochi.SelectedIndex);
			context.YochiInit();
		}

		private void ButtonYochiDown_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxYochi.SelectedIndex < 0) return;
			if (ListBoxYochi.SelectedIndex >= ListBoxYochi.Items.Count - 1) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Yochi.Down(ListBoxYochi.SelectedIndex);
			context.YochiInit();
		}

		private void ButtonYochiAppend_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxYochi.Items.Count >= Util.YochiCount) return;

			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.YochiAppend();
		}

		private void ButtonYochiRemove_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxYochi.SelectedIndex < 0) return;
			DataContext context = DataContext as DataContext;
			if (context == null) return;
			context.Yochi.Remove(ListBoxYochi.SelectedIndex);
			context.YochiInit();
		}

		private void ButtonPatchReflection_Click(object sender, RoutedEventArgs e)
		{
			String patch = TextBoxPatchCode.Text;
			patch = patch.Replace("\t", " ");
			patch = patch.Replace("\r\n", "\n");
			String[] lines = patch.Split('\n');

			SaveData save = SaveData.Instance();
			for(int i = 0; i < lines.Length; i++)
			{
				if (String.IsNullOrEmpty(lines[i])) continue;

				uint size = 0;
				uint address = 0;
				uint value = 0;
				uint loop = 1;
				uint move = 0;
				uint add = 0;

				String[] code = lines[i].Split(' ');
				if(code.Length != 2)
				{
					MessageBox.Show((i + 1).ToString() + LocalizationManager.Get("Msg_LineError"));
					return;
				}

				address = Convert.ToUInt32(code[0].Substring(1), 16);
				value = Convert.ToUInt32(code[1], 16);
				switch (code[0][0])
				{
					case '0':
						size = 1;
						break;

					case '1':
						size = 2;
						break;

					case '2':
						size = 4;
						break;

					case '4':
						if(i + 1 >= lines.Length)
						{
							MessageBox.Show((i + 1).ToString() + LocalizationManager.Get("Msg_LineError"));
							return;
						}
						switch(code[1][0])
						{
							case '2':
								size = 1;
								break;

							case '1':
								size = 2;
								break;

							case '0':
								size = 4;
								break;
						}
						loop = Convert.ToUInt32(code[1].Substring(1, 3), 16);
						move = Convert.ToUInt32(code[1].Substring(4), 16);

						i++;
						code = lines[i].Split(' ');
						value = Convert.ToUInt32(code[0], 16);
						add = Convert.ToUInt32(code[1], 16);
						break;

					default:
						MessageBox.Show((i + 1).ToString() + LocalizationManager.Get("Msg_LineUndecodable"));
						return;
				}

				for(uint j = 0; j < loop; j++)
				{
					save.WriteNumber(address + move * j, size, value + add * j);
				}
			}
			Init();
			MessageBox.Show(LocalizationManager.Get("Msg_Apply"));
		}

		private void Load(bool force)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			if (SaveData.Instance().Open(dlg.FileName, force) == false)
			{
				MessageBox.Show(LocalizationManager.Get("Msg_LoadFail"));
				return;
			}

			Init();
			MessageBox.Show(LocalizationManager.Get("Msg_LoadOk"));
		}

		private void Save()
		{
			mAllStatusList.ForEach(x => x.Save());
			if (SaveData.Instance().Save() == true) MessageBox.Show(LocalizationManager.Get("Msg_SaveOk"));
			else MessageBox.Show(LocalizationManager.Get("Msg_SaveFail"));
		}

		private void Init()
		{
			DataContext = new DataContext();
			mAllStatusList.ForEach(x => x.Open());
			UpdateFilePathDisplay();
		}

		/// <summary>
		/// Affiche le chemin complet de la save chargée dans la barre d'outils,
		/// ou le libellé "aucun fichier" (localisé) si aucune n'est chargée.
		/// </summary>
		private void UpdateFilePathDisplay()
		{
			if (TextBlockFilePath == null) return;
			string path = SaveData.Instance().FileName;
			if (String.IsNullOrEmpty(path))
			{
				TextBlockFilePath.Text = LocalizationManager.Get("Str_0493");
				TextBlockFilePath.ToolTip = null;
			}
			else
			{
				TextBlockFilePath.Text = ShortenPath(path);
				TextBlockFilePath.ToolTip = path;   // chemin complet au survol
			}
		}

		/// <summary>
		/// Réduit un chemin à "…\grand-parent\parent\fichier". Le préfixe "…"
		/// n'est ajouté que s'il existe des dossiers au-dessus du grand-parent.
		/// </summary>
		private static string ShortenPath(string fullPath)
		{
			try
			{
				string file = System.IO.Path.GetFileName(fullPath);
				string parent = System.IO.Path.GetDirectoryName(fullPath);
				string parentName = System.IO.Path.GetFileName(parent);
				string grand = System.IO.Path.GetDirectoryName(parent);
				string grandName = System.IO.Path.GetFileName(grand);

				var parts = new List<string>();
				if (!String.IsNullOrEmpty(grandName)) parts.Add(grandName);
				if (!String.IsNullOrEmpty(parentName)) parts.Add(parentName);
				parts.Add(file);
				string tail = String.Join("\\", parts);

				bool hasMore = !String.IsNullOrEmpty(grand)
					&& !String.IsNullOrEmpty(System.IO.Path.GetDirectoryName(grand));
				return hasMore ? "…\\" + tail : tail;
			}
			catch
			{
				return fullPath;
			}
		}

		private void CreateHat(List<AllStatus> status, StackPanel panel)
		{
			Item item = Item.Instance();
			foreach (ItemInfo info in item.Hats)
			{
				Grid grid = new Grid();
				grid.ColumnDefinitions.Add(new ColumnDefinition());
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(45) });

				CheckBox obtain = new CheckBox();
				status.Add(new HatObtain(obtain, info.ID));
				mHatButtonCheck.Append(obtain);
				obtain.Content = info.Name;
				obtain.VerticalAlignment = VerticalAlignment.Center;
				grid.Children.Add(obtain);

				TextBox ticket = new TextBox();
				status.Add(new AllNumberStatus(ticket, Util.HatStartAddress + info.ID, 1, 0, 99));
				ticket.SetValue(Grid.ColumnProperty, 1);

				grid.Children.Add(ticket);
				panel.Children.Add(grid);
			}
		}

		private void CreateTitle(List<AllStatus> status, ListBox list)
		{
			Item item = Item.Instance();
			foreach (ItemInfo info in item.Titles)
			{
				CheckBox obtain = new CheckBox();
				status.Add(new TitleObtain(obtain, info.ID));
				mTitleButtonCheck.Append(obtain);
				obtain.Content = info.Name;
				list.Items.Add(obtain);
			}
		}

		private void CreateSmith(List<AllStatus> status, ListBox list)
		{
			uint number = 4;
			foreach (var info in Item.Instance().Equipments)
			{
				StackPanel panel = new StackPanel();
				panel.Orientation = Orientation.Horizontal;

				Label label = new Label();
				label.Content = info;
				label.Width = 150;
				panel.Children.Add(label);

				for (int i = 0; i < info.Count; i++)
				{
					CheckBox check = new CheckBox();
					panel.Children.Add(check);
					mSmithButtonCheck.Append(check);
					status.Add(new AllCheckBoxBitStatus(check, 0x9644 + number / 8, number % 8));
					number++;
				}

				list.Items.Add(panel);
			}
		}

		private void CreateCollection(List<AllStatus> status, ListBox list)
		{
			Item item = Item.Instance();
			List<ItemInfo>[] itemInfoList = { item.Tools, item.Equipments, item.Importants, item.Recipes };
			foreach (var itemInfos in itemInfoList)
			{
				foreach (var info in itemInfos)
				{
					StackPanel panel = new StackPanel();
					panel.Orientation = Orientation.Horizontal;

					Label label = new Label();
					label.Content = info;
					label.Width = 150;
					panel.Children.Add(label);

					for (uint j = 0; j < info.Count; j++)
					{
						CheckBox check = new CheckBox();
						panel.Children.Add(check);
						mCollectionButtonCheck.Append(check);
						status.Add(new AllCheckBoxBitStatus(check, 0x8930 + (info.ID + j) / 8, (info.ID + j) % 8));
					}

					list.Items.Add(panel);
				}
			}
		}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
