#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Génère Localization/Strings.{ja,en,fr}.xaml à partir de keys.json.
- ja : source complète (secours / fallback).
- en : anglais complet (langue par défaut).
- fr : cœur de l'UI ; les clés absentes retombent sur ja.
"""
import json
from pathlib import Path
from xml.sax.saxutils import escape

ROOT = Path(__file__).resolve().parent.parent
KEYS = json.load(open(ROOT / "tools" / "keys.json", encoding="utf-8"))
JA = {x["key"]: x["ja"] for x in KEYS}
ORDER = [x["key"] for x in KEYS]

# ------------------------------------------------------------------ ANGLAIS
EN = {
"Str_0001":"DQ11 Save Editor (Nintendo 3DS)","Str_0002":"_File","Str_0003":"_Open",
"Str_0004":"Force O_pen","Str_0005":"_Save","Str_0006":"Save _As","Str_0007":"E_xit",
"Str_0008":"_About","Str_0009":"Open","Str_0010":"Save","Str_0011":"Basic Settings",
"Str_0012":"Play Time","Str_0013":"Hours","Str_0014":"Min","Str_0015":"Sec",
"Str_0016":"Gold on Hand","Str_0017":"Bank","Str_0018":"Casino Coins","Str_0019":"pcs",
"Str_0020":"Mini Medals","Str_0021":"Deposited Medals","Str_0022":"Reset Orbs","Str_0023":"pcs",
"Str_0024":"Vehicle","Str_0025":"Cleared","Str_0026":"None","Str_0027":"Horse",
"Str_0028":"Pearlmobile","Str_0029":"Skull Rider","Str_0030":"Bee Rider","Str_0031":"Dragon Rider",
"Str_0032":"Dullahan Knight","Str_0033":"Baron Knight","Str_0034":"Ship","Str_0035":"Battle Wins",
"Str_0036":"times","Str_0037":"Times Forged","Str_0038":"Total Gold Earned",
"Str_0039":"Total Mini Medals Earned","Str_0040":"Pots/Barrels Broken","Str_0041":"Camp",
"Str_0042":"Times Rested at Inn","Str_0043":"Pep Powers Used","Str_0044":"Slots Played",
"Str_0045":"Poker Played","Str_0046":"Roulette Played","Str_0047":"Character","Str_0048":"Name",
"Str_0049":"EXP","Str_0050":"Current HP","Str_0051":"Current MP","Str_0052":"Max HP+",
"Str_0053":"Max MP+","Str_0054":"Magical Might+","Str_0055":"Magical Mending+","Str_0056":"Strength+",
"Str_0057":"Defence+","Str_0058":"Agility+","Str_0059":"Deftness+","Str_0060":"Charm+",
"Str_0061":"Skill+","Str_0062":"Tactics","Str_0063":"Status","Str_0064":"Show No Mercy",
"Str_0065":"Fight Wisely","Str_0066":"Mixed Bag","Str_0067":"Focus on Healing","Str_0068":"Don't Use MP",
"Str_0069":"Follow Orders","Str_0070":"Weakest","Str_0071":"Strongest","Str_0072":"Equipment",
"Str_0073":"Right Hand","Str_0074":"Left Hand","Str_0075":"Head","Str_0076":"Body",
"Str_0077":"Accessory 1","Str_0078":"Accessory 2","Str_0079":"Pep / Skills","Str_0080":"Bag",
"Str_0081":"Item Bag","Str_0082":"Equipment Bag","Str_0083":"\u25a0 Bag Type","Str_0084":"Items",
"Str_0085":"Equipment","Str_0086":"\u25a0 What to do?","Str_0087":"Change Quantity","Str_0088":"Remove All",
"Str_0089":"Add All","Str_0090":"Quantity","Str_0091":"ct","Str_0092":"Execute",
"Str_0093":"Important Items","Str_0094":"Recipes","Str_0095":"Titles","Str_0096":"Quests",
"Str_0097":"All","Str_0098":"Details Unknown","Str_0099":"Details Known","Str_0100":"In Progress",
"Str_0101":"Complete","Str_0102":"Hidden","Str_0103":"Indeterminate","Str_0104":"to",
"Str_0105":"Set","Str_0106":"Zoom","Str_0107":"Party","Str_0108":"List","Str_0109":"Target",
"Str_0110":"Monster List","Str_0111":"Defeated: 0","Str_0112":"Defeated: not 0","Str_0113":"Set to",
"Str_0114":"Story","Str_0115":"Tockles","Str_0116":"Basic","Str_0117":"Colour","Str_0118":"Rank",
"Str_0119":"Motivation","Str_0120":"Weapon","Str_0121":"Hat","Str_0122":"Info","Str_0123":"Boost",
"Str_0124":"First Person","Str_0125":"First Finder","Str_0126":"StreetPass 2nd","Str_0127":"StreetPass 3rd",
"Str_0128":"StreetPass 4th","Str_0129":"Unique ID","Str_0130":"Red","Str_0131":"Blue","Str_0132":"Green",
"Str_0133":"Yellow","Str_0134":"Purple","Str_0135":"White","Str_0136":"Metal",
"Str_0137":"Red-Blue","Str_0138":"Red-Green","Str_0139":"Red-Yellow","Str_0140":"Red-Purple",
"Str_0141":"Red-White","Str_0142":"Blue-Green","Str_0143":"Blue-Yellow","Str_0144":"Blue-Purple",
"Str_0145":"Blue-White","Str_0146":"Green-Yellow","Str_0147":"Green-Purple","Str_0148":"Green-White",
"Str_0149":"Yellow-Purple","Str_0150":"Yellow-White","Str_0151":"Purple-White",
"Str_0152":"Red-Blue v2","Str_0153":"Red-Green v2","Str_0154":"Red-Yellow v2","Str_0155":"Red-Purple v2",
"Str_0156":"Red-White v2","Str_0157":"Blue-Green v2","Str_0158":"Blue-Yellow v2","Str_0159":"Blue-Purple v2",
"Str_0160":"Blue-White v2","Str_0161":"Green-Yellow v2","Str_0162":"Green-Purple v2","Str_0163":"Green-White v2",
"Str_0164":"Yellow-Purple v2","Str_0165":"Yellow-White v2","Str_0166":"Purple-White v2",
"Str_0167":"Not Seen","Str_0168":"Seen",
"Str_0169":"Ore","Str_0170":"Watakushi","Str_0171":"Ora","Str_0172":"Oira","Str_0173":"Boku","Str_0174":"Ore-sama",
"Str_0175":"Profile","Str_0176":"Story Progress","Str_0177":"Gender","Str_0178":"Personality",
"Str_0179":"Age Group","Str_0180":"Hobby","Str_0181":"Hometown","Str_0182":"DQ History",
"Str_0183":"Message","Str_0184":"Restricted Play","Str_0185":"Secret","Str_0186":"Male","Str_0187":"Female",
"Str_0188":"Egghead","Str_0189":"Softie","Str_0190":"Coward","Str_0191":"Lone Wolf","Str_0192":"Daredevil",
"Str_0193":"Scatterbrain","Str_0194":"Big Eater","Str_0195":"Lady","Str_0196":"Meddler","Str_0197":"Show-off",
"Str_0198":"Klutz","Str_0199":"Tomboy","Str_0200":"Strong Woman","Str_0201":"Stubborn","Str_0202":"Hard Worker",
"Str_0203":"Sharp","Str_0204":"Hardened","Str_0205":"Brave","Str_0206":"Lonely","Str_0207":"Lucky",
"Str_0208":"Honest","Str_0209":"Brainy","Str_0210":"Nimble","Str_0211":"Sexy Gal","Str_0212":"Naive",
"Str_0213":"Tough Guy","Str_0214":"Strongman","Str_0215":"Ironman","Str_0216":"Lightning Fast",
"Str_0217":"Crybaby","Str_0218":"Lazybones","Str_0219":"Shrewd","Str_0220":"Hot-Blooded","Str_0221":"Easygoing",
"Str_0222":"Shy","Str_0223":"Contrary","Str_0224":"Ordinary","Str_0225":"Resilient","Str_0226":"Sore Loser",
"Str_0227":"Vain","Str_0228":"Plump Perv","Str_0229":"Kind","Str_0230":"Lucky Man","Str_0231":"Rowdy",
"Str_0232":"Romantic","Str_0233":"Selfish","Str_0234":"Pervert No.2",
"Str_0235":"Under 10s","Str_0236":"Teens","Str_0237":"20s","Str_0238":"30s","Str_0239":"40s",
"Str_0240":"50s","Str_0241":"60s","Str_0242":"70s+",
"Str_0243":"Outdoors","Str_0244":"Swimming","Str_0245":"Baseball","Str_0246":"Shooting","Str_0247":"Workout",
"Str_0248":"Walking","Str_0249":"Cycling","Str_0250":"Canoeing","Str_0251":"Golf","Str_0252":"Surfing",
"Str_0253":"Football","Str_0254":"Horse Riding","Str_0255":"Airsoft","Str_0256":"Diving",
"Str_0257":"Watching Sports","Str_0258":"Hiking","Str_0259":"Basketball","Str_0260":"Dance",
"Str_0261":"Tennis","Str_0262":"Bouldering","Str_0263":"Idols","Str_0264":"Live Shows","Str_0265":"Voice Training",
"Str_0266":"Playing Music","Str_0267":"Karaoke","Str_0268":"Streaming","Str_0269":"Commentary",
"Str_0270":"Anime Songs","Str_0271":"Cosplay","Str_0272":"Drawing","Str_0273":"Anime","Str_0274":"Games",
"Str_0275":"Board Games","Str_0276":"Online Games","Str_0277":"Card Games","Str_0278":"Werewolf Game",
"Str_0279":"Puzzles/Riddles","Str_0280":"Mahjong","Str_0281":"Puzzles","Str_0282":"Smartphone",
"Str_0283":"Blogging","Str_0284":"Internet","Str_0285":"Auctions","Str_0286":"Shopping",
"Str_0287":"House Parties","Str_0288":"Banquets","Str_0289":"Travel","Str_0290":"Driving",
"Str_0291":"Ruins Tours","Str_0292":"Photography","Str_0293":"Reading","Str_0294":"Movies",
"Str_0295":"Go","Str_0296":"Shogi","Str_0297":"Stocks","Str_0298":"Filial Piety","Str_0299":"Soul-Searching",
"Str_0300":"Rock-Paper-Scissors","Str_0301":"Tsuchinoko Hunting","Str_0302":"DIY","Str_0303":"Plastic Models",
"Str_0304":"Full Completion","Str_0305":"Daydreaming","Str_0306":"Eating","Str_0307":"Cooking",
"Str_0308":"History","Str_0309":"Helping People","Str_0310":"Stamp Collecting","Str_0311":"Fortune-Telling",
"Str_0312":"Tea Ceremony","Str_0313":"Saving Money","Str_0314":"Fishing","Str_0315":"Railways",
"Str_0316":"Studying","Str_0317":"Bonsai","Str_0318":"Handicrafts","Str_0319":"Chatting","Str_0320":"Fashion",
"Str_0321":"Cleaning","Str_0322":"Dieting","Str_0323":"Nail Art","Str_0324":"Cats","Str_0325":"Dogs",
"Str_0326":"Sleeping","Str_0327":"Adventure","Str_0328":"Levelling Up","Str_0329":"Alchemy",
"Str_0330":"Puff-Puff","Str_0331":"Metal Hunting","Str_0332":"Roulette","Str_0333":"Poker","Str_0334":"Slots",
"Str_0335":"Solo Camping",
"Str_0336":"Hokkaido","Str_0337":"Aomori","Str_0338":"Iwate","Str_0339":"Miyagi","Str_0340":"Akita",
"Str_0341":"Yamagata","Str_0342":"Fukushima","Str_0343":"Ibaraki","Str_0344":"Tochigi","Str_0345":"Gunma",
"Str_0346":"Saitama","Str_0347":"Chiba","Str_0348":"Tokyo","Str_0349":"Kanagawa","Str_0350":"Niigata",
"Str_0351":"Yamanashi","Str_0352":"Nagano","Str_0353":"Toyama","Str_0354":"Ishikawa","Str_0355":"Fukui",
"Str_0356":"Gifu","Str_0357":"Shizuoka","Str_0358":"Aichi","Str_0359":"Mie","Str_0360":"Shiga",
"Str_0361":"Kyoto","Str_0362":"Osaka","Str_0363":"Hyogo","Str_0364":"Nara","Str_0365":"Wakayama",
"Str_0366":"Tottori","Str_0367":"Shimane","Str_0368":"Okayama","Str_0369":"Hiroshima","Str_0370":"Yamaguchi",
"Str_0371":"Tokushima","Str_0372":"Kagawa","Str_0373":"Kochi","Str_0374":"Fukuoka","Str_0375":"Saga",
"Str_0376":"Nagasaki","Str_0377":"Kumamoto","Str_0378":"Oita","Str_0379":"Miyazaki","Str_0380":"Kagoshima",
"Str_0381":"Okinawa","Str_0382":"Asia","Str_0383":"Europe","Str_0384":"Africa","Str_0385":"America",
"Str_0386":"Oceania","Str_0387":"Antarctica","Str_0388":"Countryside","Str_0389":"City","Str_0390":"Faraway Land",
"Str_0391":"Alefgard","Str_0392":"Erdrea","Str_0393":"Astoltia","Str_0394":"Radatome","Str_0395":"Laurasia",
"Str_0396":"Aliahan","Str_0397":"Mountain Village","Str_0398":"Santa Rosa","Str_0399":"Lifecod",
"Str_0400":"Fishbel","Str_0401":"Trapetta","Str_0402":"Walreau Village","Str_0403":"Eteene Village",
"Str_0404":"Cobblestone","Str_0405":"Puff-Puff Hut","Str_0406":"In the Forest","Str_0407":"Kotatsu",
"Str_0408":"Apprentice","Str_0409":"Rookie","Str_0410":"Skilled","Str_0411":"Veteran","Str_0412":"Old Hand",
"Str_0413":"Can't flee battles","Str_0414":"Can't shop","Str_0415":"Can't equip armour",
"Str_0416":"Embarrassing curse","Str_0417":"Hat","Str_0418":"Tockle Village","Str_0419":"Adventure Log Password",
"Str_0420":"State","Str_0421":"Absent","Str_0422":"Initial","Str_0423":"Talked to Elder",
"Str_0424":"Collecting Logs","Str_0425":"Altar Depths Open","Str_0426":"Conquered","Str_0427":"Floor",
"Str_0428":"Floor 1","Str_0429":"Floor 2","Str_0430":"Floor 3","Str_0431":"Floor 4","Str_0432":"Floor 5",
"Str_0433":"Floor 6","Str_0434":"Floor 7","Str_0435":"Floor 8","Str_0436":"Floor 9","Str_0437":"Floor 10",
"Str_0438":"All Cleared","Str_0439":"DQ1 Hero Statue","Str_0440":"DQ2 Hero Statue","Str_0441":"DQ3 Hero Statue",
"Str_0442":"DQ4 Hero Statue","Str_0443":"DQ5 Hero Statue","Str_0444":"DQ6 Hero Statue","Str_0445":"DQ7 Hero Statue",
"Str_0446":"DQ8 Hero Statue","Str_0447":"DQ9 Hero Statue","Str_0448":"DQ10 Hero Statue",
"Str_0449":"Training Grounds","Str_0450":"Fastest Score","Str_0451":"Reward","Str_0452":"First Trial",
"Str_0453":"Not Attempted","Str_0454":"First Clear","Str_0455":"Within 16 moves","Str_0456":"Within 12 moves",
"Str_0457":"Within 8 moves","Str_0458":"Second Trial","Str_0459":"Within 20 moves","Str_0460":"Within 15 moves",
"Str_0461":"Within 10 moves","Str_0462":"Third Trial","Str_0463":"Within 25 moves","Str_0464":"Fourth Trial",
"Str_0465":"Within 30 moves","Str_0466":"Final Trial","Str_0467":"Within 40 moves","Str_0468":"Within 35 moves",
"Str_0469":"Fun-Size Forge","Str_0470":"Item Collection","Str_0471":"StreetPass","Str_0472":"System",
"Str_0473":"Battle Speed","Str_0474":"Sound Effects","Str_0475":"Camera Rotation","Str_0476":"C-Stick",
"Str_0477":"Display Mode","Str_0478":"Fast","Str_0479":"Slow","Str_0480":"Normal","Str_0481":"Reverse",
"Str_0482":"Both","Str_0483":"Patch Code","Str_0484":"Supports 0, 1, 2, 4 codes","Str_0485":"Apply",
"Str_0486":"Item Selection","Str_0487":"Filter","Str_0488":"OK","Str_0489":"Cancel","Str_0490":"Options",
"Str_0491":"Creator: Kamemushi",
}

# ------------------------------------------------------------------ FRANCAIS (cœur de l'UI)
FR = {
"Str_0001":"Éditeur de sauvegarde DQ11 (Nintendo 3DS)","Str_0002":"_Fichier","Str_0003":"_Ouvrir",
"Str_0004":"Ouvrir de _force","Str_0005":"_Enregistrer","Str_0006":"Enregistrer _sous","Str_0007":"_Quitter",
"Str_0008":"_À propos","Str_0009":"Ouvrir","Str_0010":"Enregistrer","Str_0011":"Réglages de base",
"Str_0012":"Temps de jeu","Str_0013":"Heures","Str_0014":"Min","Str_0015":"Sec","Str_0016":"Or en poche",
"Str_0017":"Banque","Str_0018":"Jetons de casino","Str_0019":"pcs","Str_0020":"Mini-médailles",
"Str_0021":"Médailles déposées","Str_0022":"Orbes de réinitialisation","Str_0023":"pcs","Str_0024":"Monture",
"Str_0025":"Terminé","Str_0026":"Aucun","Str_0034":"Navire","Str_0047":"Personnage","Str_0048":"Nom",
"Str_0049":"EXP","Str_0050":"PV actuels","Str_0051":"PM actuels","Str_0052":"PV max+","Str_0053":"PM max+",
"Str_0056":"Force+","Str_0057":"Défense+","Str_0058":"Agilité+","Str_0059":"Dextérité+","Str_0060":"Charme+",
"Str_0061":"Compétence+","Str_0062":"Tactique","Str_0063":"Statut","Str_0072":"Équipement",
"Str_0073":"Main droite","Str_0074":"Main gauche","Str_0075":"Tête","Str_0076":"Corps",
"Str_0077":"Accessoire 1","Str_0078":"Accessoire 2","Str_0080":"Sac","Str_0084":"Objets","Str_0085":"Équipement",
"Str_0090":"Quantité","Str_0092":"Exécuter","Str_0093":"Objets importants","Str_0094":"Recettes",
"Str_0095":"Titres","Str_0096":"Quêtes","Str_0097":"Tous","Str_0105":"Définir","Str_0107":"Équipe",
"Str_0108":"Liste","Str_0109":"Cible","Str_0110":"Liste des monstres","Str_0114":"Histoire",
"Str_0116":"Base","Str_0117":"Couleur","Str_0118":"Rang","Str_0120":"Arme","Str_0121":"Chapeau","Str_0122":"Infos",
"Str_0130":"Rouge","Str_0131":"Bleu","Str_0132":"Vert","Str_0133":"Jaune","Str_0134":"Violet","Str_0135":"Blanc",
"Str_0175":"Profil","Str_0177":"Sexe","Str_0178":"Personnalité","Str_0180":"Loisir","Str_0181":"Origine",
"Str_0186":"Homme","Str_0187":"Femme","Str_0420":"État","Str_0427":"Étage","Str_0472":"Système",
"Str_0473":"Vitesse de combat","Str_0474":"Effets sonores","Str_0477":"Mode d'affichage","Str_0478":"Rapide",
"Str_0479":"Lent","Str_0480":"Normal","Str_0482":"Les deux","Str_0485":"Appliquer","Str_0486":"Sélection d'objet",
"Str_0487":"Filtre","Str_0488":"Valider","Str_0489":"Annuler","Str_0490":"Options",
}

# messages du code-behind (mêmes clés dans toutes les langues)
MSG_JA = {"Msg_LoadOk":"読込成功","Msg_LoadFail":"読込失敗","Msg_SaveOk":"書込成功",
          "Msg_SaveFail":"書込失敗","Msg_Apply":"適応","Msg_Delete":"削除","Msg_None":"なし",
          "Msg_LineError":"行目に誤りがあります","Msg_LineUndecodable":"行目に解読不可の命令があります",
          "St_Zone":"ゾーン","St_Poison":"どく","St_Curse":"のろい","St_Death":"しに"}
MSG_EN = {"Msg_LoadOk":"Load succeeded","Msg_LoadFail":"Load failed","Msg_SaveOk":"Save succeeded",
          "Msg_SaveFail":"Save failed","Msg_Apply":"Apply","Msg_Delete":"Delete","Msg_None":"None",
          "Msg_LineError":" : error on this line","Msg_LineUndecodable":" : undecodable instruction on this line",
          "St_Zone":"Zone","St_Poison":"Poison","St_Curse":"Curse","St_Death":"Dead"}
MSG_FR = {"Msg_LoadOk":"Chargement réussi","Msg_LoadFail":"Échec du chargement","Msg_SaveOk":"Enregistrement réussi",
          "Msg_SaveFail":"Échec de l'enregistrement","Msg_Apply":"Appliquer","Msg_Delete":"Supprimer","Msg_None":"Aucun",
          "Msg_LineError":" : erreur à cette ligne","Msg_LineUndecodable":" : instruction indéchiffrable à cette ligne",
          "St_Zone":"Zone","St_Poison":"Poison","St_Curse":"Malédiction","St_Death":"Mort"}

HEADER = ('<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"\n'
          '                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"\n'
          '                    xmlns:s="clr-namespace:System;assembly=mscorlib">\n')


def write_dict(lang, ui_map, msg_map):
    out = ["\uFEFF" + HEADER]
    for k in ORDER:
        if k in ui_map:
            out.append(f'    <s:String x:Key="{k}">{escape(ui_map[k])}</s:String>\n')
    out.append("\n    <!-- Messages (code-behind) -->\n")
    for k in MSG_JA:
        if k in msg_map:
            out.append(f'    <s:String x:Key="{k}">{escape(msg_map[k])}</s:String>\n')
    out.append("</ResourceDictionary>\n")
    d = ROOT / "DQ11" / "Localization"
    d.mkdir(exist_ok=True)
    (d / f"Strings.{lang}.xaml").write_text("".join(out), encoding="utf-8")


write_dict("ja", JA, MSG_JA)
write_dict("en", EN, MSG_EN)
write_dict("fr", FR, MSG_FR)

# rapport de couverture
print(f"ja : {len(JA)}/{len(ORDER)} UI")
print(f"en : {len(EN)}/{len(ORDER)} UI")
print(f"fr : {len(FR)}/{len(ORDER)} UI (reste -> fallback ja)")
missing_en = [k for k in ORDER if k not in EN]
print("clés EN manquantes :", missing_en if missing_en else "aucune")
