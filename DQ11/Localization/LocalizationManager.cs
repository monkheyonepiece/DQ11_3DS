using System;
using System.IO;
using System.Windows;

namespace DQ11
{
	/// <summary>
	/// Gestion du multilingue par superposition de ResourceDictionary.
	/// - "ja" est toujours chargé en base (secours complet) ;
	/// - la langue choisie est superposée par-dessus (les clés absentes
	///   retombent automatiquement sur le japonais) ;
	/// - le choix est persisté dans lang.config à côté de l'exécutable.
	/// </summary>
	static class LocalizationManager
	{
		public const string BaseLang = "ja";
		public const string DefaultLang = "en";

		private static ResourceDictionary mBase;
		private static ResourceDictionary mOverlay;

		public static string CurrentLanguage { get; private set; } = DefaultLang;

		/// <summary>Levé à la fin de SetLanguage, après superposition du dictionnaire.</summary>
		public static event Action LanguageChanged;

		private static string ConfigPath
		{
			get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lang.config"); }
		}

		public static void Initialize()
		{
			mBase = Merge(DictUri(BaseLang));   // base japonaise, toujours présente

			string saved = DefaultLang;
			try
			{
				if (File.Exists(ConfigPath))
				{
					string v = File.ReadAllText(ConfigPath).Trim();
					if (!String.IsNullOrEmpty(v)) saved = v;
				}
			}
			catch { /* fichier illisible -> défaut */ }

			SetLanguage(saved);
		}

		public static void SetLanguage(string lang)
		{
			if (mOverlay != null)
			{
				Application.Current.Resources.MergedDictionaries.Remove(mOverlay);
				mOverlay = null;
			}

			// On ne superpose que si ce n'est pas la langue de base.
			if (!String.Equals(lang, BaseLang, StringComparison.OrdinalIgnoreCase))
			{
				mOverlay = Merge(DictUri(lang));
			}

			CurrentLanguage = lang;
			try { File.WriteAllText(ConfigPath, lang); } catch { }
			LanguageChanged?.Invoke();
		}

		/// <summary>Pour le code-behind : récupère une chaîne par sa clé.</summary>
		public static string Get(string key)
		{
			object v = Application.Current.TryFindResource(key);
			return v as string ?? key;
		}

		private static Uri DictUri(string lang)
		{
			return new Uri("Localization/Strings." + lang + ".xaml", UriKind.Relative);
		}

		private static ResourceDictionary Merge(Uri uri)
		{
			var d = new ResourceDictionary { Source = uri };
			Application.Current.Resources.MergedDictionaries.Add(d);
			return d;
		}
	}
}
