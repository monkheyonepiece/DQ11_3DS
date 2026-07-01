using System;
using System.Collections.Generic;

namespace DQ11
{
    class Item
    {
        private static Item mThis;
        public ItemInfo None { get; private set; } = new ItemInfo(0xFFFF, LocalizationManager.Get("Msg_None"), 1);
        public List<ItemInfo> Tools { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Equipments { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Hats { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Titles { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Zooms { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Importants { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Recipes { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Quests { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Storys { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Techniques { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> Monsters { get; private set; } = new List<ItemInfo>();
        public List<ItemInfo> WatchWords { get; private set; } = new List<ItemInfo>();
        // #で始まる文字はコメント扱い
        // タブ区切りで解釈
        // ID\t名前で区切られている前提として扱う

        private Item()
        { }

        public static Item Instance()
        {
            if (mThis == null)
            {
                mThis = new Item();
                mThis.Init();
            }
            return mThis;
        }

        private void Init()
        {
            // On passe le nom brut ; la langue (et le repli JP) sont résolus dans AppendList.
            AppendList("tool.txt", Tools);
            AppendList("equipment.txt", Equipments);
            AppendList("hat.txt", Hats);
            AppendList("title.txt", Titles);
            AppendList("zoom.txt", Zooms);
            AppendList("important.txt", Importants);
            AppendList("recipe.txt", Recipes);
            AppendList("quest.txt", Quests);
            AppendList("story.txt", Storys);
            AppendList("technique.txt", Techniques);
            AppendList("monster.txt", Monsters);
            AppendList("watchword.txt", WatchWords);
            Tools.Sort((a, b) => (int)(a.ID - b.ID));
            Equipments.Sort((a, b) => (int)(a.ID - b.ID));
        }

        /// <summary>
        /// Recharge les noms depuis les .txt de la langue courante en écrasant le
        /// Name des ItemInfo EXISTANTS (appariés par ID) : aucune référence cassée,
        /// l'ordre/tri et la recherche binaire restent valides (seul le Name change).
        /// </summary>
        public void Reload()
        {
            ReloadList("tool.txt", Tools);
            ReloadList("equipment.txt", Equipments);
            ReloadList("hat.txt", Hats);
            ReloadList("title.txt", Titles);
            ReloadList("zoom.txt", Zooms);
            ReloadList("important.txt", Importants);
            ReloadList("recipe.txt", Recipes);
            ReloadList("quest.txt", Quests);
            ReloadList("story.txt", Storys);
            ReloadList("technique.txt", Techniques);
            ReloadList("monster.txt", Monsters);
            ReloadList("watchword.txt", WatchWords);
            None.Name = LocalizationManager.Get("Msg_None");
        }

        private static void ReloadList(String filename, List<ItemInfo> items)
        {
            String path = ResolveDataPath(filename);
            if (path == null) return;

            // File des ItemInfo par ID : gère les IDs DUPLIQUÉS (même adresse, bits
            // différents -> ex. watchword/technique/story). On dé-empile dans l'ordre
            // du fichier pour mettre à jour TOUTES les entrées, pas seulement la 1ʳᵉ.
            Dictionary<uint, Queue<ItemInfo>> map = new Dictionary<uint, Queue<ItemInfo>>();
            foreach (ItemInfo info in items)
            {
                Queue<ItemInfo> queue;
                if (!map.TryGetValue(info.ID, out queue))
                {
                    queue = new Queue<ItemInfo>();
                    map[info.ID] = queue;
                }
                queue.Enqueue(info);
            }

            String[] lines = System.IO.File.ReadAllLines(path);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length < 2) continue;
                if (String.IsNullOrEmpty(values[0])) continue;
                if (String.IsNullOrEmpty(values[1])) continue;
                uint id = 0;
                if (values[0].Length > 1 && values[0][1] == 'x') id = Convert.ToUInt32(values[0], 16);
                else id = Convert.ToUInt32(values[0]);

                Queue<ItemInfo> queue;
                if (map.TryGetValue(id, out queue) && queue.Count > 0) queue.Dequeue().Name = values[1];
            }
        }

        /// <summary>
        /// Résout le chemin d'un fichier de données selon la langue active.
        ///   1) item\<langue>\<fichier>  (ex. item\fr\tool.txt)
        ///   2) item\<fichier>           (japonais, repli universel)
        /// Renvoie null si aucun n'existe.
        /// </summary>
        private static String ResolveDataPath(String filename)
        {
            String lang = LocalizationManager.CurrentLanguage;
            if (!String.IsNullOrEmpty(lang) &&
                !String.Equals(lang, LocalizationManager.BaseLang, StringComparison.OrdinalIgnoreCase))
            {
                String langPath = System.IO.Path.Combine("item", lang, filename);
                if (System.IO.File.Exists(langPath)) return langPath;
            }
            String basePath = System.IO.Path.Combine("item", filename);
            if (System.IO.File.Exists(basePath)) return basePath;
            return null;
        }

        public ItemInfo SearchBinaryNear(List<ItemInfo> items, uint id)
        {
            int min = 0;
            int max = items.Count;
            for (; min < max;)
            {
                int mid = (min + max) / 2;
                if (items[mid].ID == id) return items[mid];
                else if (items[mid].ID > id) max = mid;
                else min = mid + 1;
            }
            if (min < 1) return null;
            ItemInfo info = items[min - 1];
            if (info.ID < id && info.ID + info.Count > id) return info;
            return null;
        }

        public ItemInfo SearchBinary(List<ItemInfo> items, uint id)
        {
            int min = 0;
            int max = items.Count;
            for (; min < max;)
            {
                int mid = (min + max) / 2;
                if (items[mid].ID == id) return items[mid];
                else if (items[mid].ID > id) max = mid;
                else min = mid + 1;
            }
            return null;
        }

        public ItemInfo GetToolItemInfo(uint id)
        {
            if (None.ID == id) return None;
            return SearchBinary(Tools, id);
        }

        public ItemInfo GetEquipmentInfo(uint id)
        {
            if (None.ID == id) return None;
            return SearchBinaryNear(Equipments, id);
        }

        public ItemInfo GetHatItemInfo(uint id)
        {
            if (None.ID == id) return None;
            return SearchBinary(Tools, id);
        }

        public ItemInfo GetItemInfo(uint id)
        {
            ItemInfo info = GetToolItemInfo(id);
            if (info != null) return info;
            return GetEquipmentInfo(id);
        }

        private void AppendList(String filename, List<ItemInfo> items)
        {
            String path = ResolveDataPath(filename);
            if (path == null) return;
            String[] lines = System.IO.File.ReadAllLines(path);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length < 2) continue;
                if (String.IsNullOrEmpty(values[0])) continue;
                if (String.IsNullOrEmpty(values[1])) continue;
                uint id = 0;
                if (values[0].Length > 1 && values[0][1] == 'x') id = Convert.ToUInt32(values[0], 16);
                else id = Convert.ToUInt32(values[0]);

                uint count = 1;
                if (values.Length >= 3) count = Convert.ToUInt32(values[2]);
                items.Add(new ItemInfo(id, values[1], count));
            }
        }
    }
}