using System.Windows.Controls;

namespace DQ11
{
	abstract class AllStatus
	{
		public virtual void Init() { }
		public abstract void Open();
		public abstract void Save();
		public virtual void Page(uint page) { }
		public virtual void Max() { }
		public virtual void Min() { }

		/// <summary>
		/// Réaffiche les noms de données après Item.Reload() (changement de langue).
		/// Par défaut ne fait rien ; surchargé par les écrans qui affichent des noms.
		/// </summary>
		public virtual void ReloadNames() { }

		/// <summary>
		/// Force un ContentControl dont le Content est un ItemInfo à réafficher son
		/// nom : on ré-assigne le Content pour déclencher un nouveau ToString().
		/// (Un ContentControl n'observe pas les changements de sous-propriété.)
		/// </summary>
		public static void Repoke(ContentControl cc)
		{
			if (cc == null) return;
			object content = cc.Content;
			if (!(content is ItemInfo)) return;
			cc.Content = null;
			cc.Content = content;
		}

		/// <summary>
		/// Repoke tous les ContentControl (CheckBox/Label) d'une liste, y compris
		/// ceux contenus dans des Panel (lignes construites en StackPanel).
		/// </summary>
		public static void RepokeItems(ItemsControl list)
		{
			if (list == null) return;
			foreach (var item in list.Items)
			{
				Repoke(item as ContentControl);
				Panel panel = item as Panel;
				if (panel != null)
				{
					foreach (var child in panel.Children) Repoke(child as ContentControl);
				}
			}
		}
	}
}
