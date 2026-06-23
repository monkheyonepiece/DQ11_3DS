#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Extracteur i18n pour DQ11_3DS.
- Parcourt les .xaml indiqués.
- Externalise chaque valeur d'attribut UI (Title/Header/Content/Text/ToolTip)
  qui contient du japonais.
- Déduplique les chaînes identiques sous une même clé (Str_0001, ...).
- Réécrit le XAML pour remplacer le littéral par {DynamicResource Cle}.
- Produit un mapping JSON cle -> texte japonais (source).

Usage:
  python extract_strings.py extract   # génère keys.json + .xaml réécrits (.new)
"""
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent / "DQ11"
XAML_FILES = ["MainWindow.xaml", "ItemSelectWindow.xaml", "AboutWindow.xaml"]
ATTRS = ("Title", "Header", "Content", "Text", "ToolTip")

# plage japonaise : hiragana, katakana, kanji
JP = re.compile(r"[\u3040-\u30ff\u3400-\u4dbf\u4e00-\u9fff]")
# attribut="valeur" pour les attributs ciblés ; capture le nom, le préfixe et la valeur
ATTR_RE = re.compile(r'\b(' + "|".join(ATTRS) + r')="([^"]*)"')


def has_jp(s: str) -> bool:
    return bool(JP.search(s))


def extract():
    mapping = {}          # texte_ja -> cle
    order = []            # garde l'ordre d'apparition
    counter = [0]

    def key_for(text):
        if text not in mapping:
            counter[0] += 1
            mapping[text] = f"Str_{counter[0]:04d}"
            order.append(text)
        return mapping[text]

    for fname in XAML_FILES:
        path = ROOT / fname
        src = path.read_text(encoding="utf-8")

        def repl(m):
            attr, val = m.group(1), m.group(2)
            if not has_jp(val):
                return m.group(0)
            if val.startswith("{"):  # binding / markup ext, ne pas toucher
                return m.group(0)
            k = key_for(val)
            return f'{attr}="{{DynamicResource {k}}}"'

        new = ATTR_RE.sub(repl, src)
        (path.parent / (fname + ".new")).write_text(new, encoding="utf-8")

    out = [{"key": mapping[t], "ja": t} for t in order]
    (ROOT.parent / "tools" / "keys.json").write_text(
        json.dumps(out, ensure_ascii=False, indent=2), encoding="utf-8")
    print(f"{len(out)} chaînes uniques externalisées.")
    return out


if __name__ == "__main__":
    extract()
