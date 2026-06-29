using System.Windows;
using System.Windows.Controls;

namespace DQ11
{
	class Monster : AllStatus
	{
		private readonly RadioButton mAll;
		private readonly StackPanel mPanel;
		private readonly TextBox mValue;
		private readonly TextBox mSearch;
		private bool mFilterQueued = false;

		private enum Type
		{
			All,
			None,
			Have,
		};

		public Monster(StackPanel panel, RadioButton all, RadioButton none, RadioButton have, TextBox value, Button decision, TextBox search)
		{
			mPanel = panel;
			mAll = all;
			mValue = value;
			mSearch = search;
			all.Checked += ((x, y) => CreateList(Type.All));
			none.Checked += ((x, y) => CreateList(Type.None));
			have.Checked += ((x, y) => CreateList(Type.Have));
			decision.Click += Decision_Click;
			// Recherche en temps réel : filtre par visibilité les lignes déjà
			// construites (pas de reconstruction -> rapide même avec beaucoup d'entrées).
			// Différé via le Dispatcher pour ne pas perturber le rendu de la TextBox.
			mSearch.TextChanged += ((x, y) => QueueSearchFilter());
		}

		private void Decision_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (var comp in mPanel.Children)
			{
				Grid grid = comp as Grid;
				if (grid == null) continue;
				if (grid.Visibility != Visibility.Visible) continue;   // n'affecte que les monstres affichés (filtre recherche)
				TextBox count = grid.Children[1] as TextBox;
				if (count == null) continue;
				count.Text = mValue.Text;
			}
		}

		public override void Init()
		{
			CreateList(Type.All);
		}

		public override void Open()
		{
			mAll.IsChecked = true;
			CreateList(Type.All);
		}

		public override void Save()
		{
			SaveData savedata = SaveData.Instance();
			foreach (var comp in mPanel.Children)
			{
				Grid grid = comp as Grid;
				if (grid == null) continue;
				TextBox count = grid.Children[1] as TextBox;
				if (count == null) continue;
				var info = count.Tag as ItemInfo;
				if (info == null) continue;
				uint value;
				if (!uint.TryParse(count.Text, out value)) continue;
				savedata.WriteNumber(Util.MonsterStartAddress + info.ID * 4, 4, value);
			}
		}

		private void CreateList(Type type)
		{
			mPanel.Children.Clear();

			SaveData savedata = SaveData.Instance();
			foreach (var info in Item.Instance().Monsters)
			{
				uint value = savedata.ReadNumber(Util.MonsterStartAddress + info.ID * 4, 4);
				bool isAppend = true;
				switch(type)
				{
					case Type.Have:
						if (value == 0) isAppend = false;
						break;

					case Type.None:
						if (value != 0) isAppend = false;
						break;
				}
				if (isAppend)
				{
					Grid grid = new Grid();
					grid.Tag = info;   // pour le filtre de recherche par visibilité
					grid.ColumnDefinitions.Add(new ColumnDefinition());
					grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(45) });

					Label name = new Label();
					name.Content = info.Name;
					name.VerticalAlignment = VerticalAlignment.Center;
					name.Padding = new Thickness(4, 0, 4, 0);   // compact : réduit la hauteur de ligne
					grid.Children.Add(name);

					TextBox count = new TextBox();
					count.Tag = info;
					count.Text = value.ToString();
					count.SetValue(Grid.ColumnProperty, 1);
					grid.Children.Add(count);

					mPanel.Children.Add(grid);
					// Ligne de séparation collée à l'entrée (aucune marge) pour gagner de la place.
					// (ignorée par Decision_Click/Save qui ne traitent que les Grid)
					Separator sep = new Separator();
					sep.Margin = new Thickness(0);
					mPanel.Children.Add(sep);
				}
			}
		}

		/// <summary>
		/// Diffère le filtrage à la prochaine boucle d'inactivité du Dispatcher :
		/// la TextBox de recherche finit d'abord de se redessiner (curseur/sélection),
		/// puis la liste se relayoute -> évite l'artefact de sélection résiduelle.
		/// Coalesce les frappes rapprochées.
		/// </summary>
		private void QueueSearchFilter()
		{
			if (mFilterQueued) return;
			mFilterQueued = true;
			mPanel.Dispatcher.BeginInvoke(
				new System.Action(() => { mFilterQueued = false; ApplySearchFilter(); }),
				System.Windows.Threading.DispatcherPriority.Background);
		}

		/// <summary>
		/// Affiche/masque les lignes déjà construites selon le texte de recherche,
		/// sans recréer aucun contrôle (rapide, même avec beaucoup d'entrées).
		/// </summary>
		private void ApplySearchFilter()
		{
			string search = mSearch != null ? mSearch.Text : "";
			bool showAll = string.IsNullOrEmpty(search);

			UIElementCollection children = mPanel.Children;
			for (int i = 0; i < children.Count; i++)
			{
				Grid grid = children[i] as Grid;
				if (grid == null) continue;

				var info = grid.Tag as ItemInfo;
				bool visible = showAll
					|| (info != null && info.Name != null
						&& info.Name.IndexOf(search, System.StringComparison.CurrentCultureIgnoreCase) >= 0);

				Visibility vis = visible ? Visibility.Visible : Visibility.Collapsed;
				grid.Visibility = vis;

				// Le séparateur qui suit l'entrée se masque avec elle.
				Separator sep = (i + 1 < children.Count) ? children[i + 1] as Separator : null;
				if (sep != null) sep.Visibility = vis;
			}
		}
	}
}
