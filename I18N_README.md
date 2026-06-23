# DQ11_3DS — Multilingual Support (i18n)

Language support has been added to the save editor, with **English as the default**, **French** (UI core), and **Japanese** (source language). A language selector has been added at the top of the menu bar.

## Principle

`ResourceDictionary` overlay with automatic fallback:

- `Strings.ja.xaml` is **always loaded as the base** (complete fallback, 491 strings).
- The selected language (`en` / `fr`) is **layered on top**. Any missing key automatically falls back to Japanese → **no empty text**.
- XAML references every string through `{DynamicResource Str_XXXX}`, so language changes are **instant and do not require restarting**.
- The selected language is persisted in `lang.config`, located next to the executable.

## Files

```text
DQ11/Localization/
  LocalizationManager.cs   <- engine (loads base + overlays + persists)
  Strings.ja.xaml          <- Japanese source, 491 strings (= base/fallback)
  Strings.en.xaml          <- English, 491/491 + 13 messages (default language)
  Strings.fr.xaml          <- French, 100/491 + 13 messages (rest -> JA fallback)

tools/
  extract_strings.py       <- extracts JP from XAML -> {DynamicResource} + keys.json
  build_dicts.py           <- generates the 3 Strings.*.xaml files (embedded translations)
  keys.json                <- key -> source Japanese text table
```

Modified files: `App.xaml.cs`, `MainWindow.xaml`, `MainWindow.xaml.cs`, `ItemSelectWindow.xaml`, `AboutWindow.xaml`, `Bag.cs`, `Character.cs`, `Item.cs`, `DQ11.csproj`.

## Visual Studio Integration

The `.csproj` already declares the new files (`Compile` for `.cs`, `Page` for the three `.xaml` files). If you start from this repository, it should compile as-is.

If integrating manually into your fork:
- add the `Localization` folder to the project,
- verify that all `Strings.*.xaml` files have **Build Action = Page**.

The selector appears in the top-right menu:

**🌐 Language → English / Français / 日本語**

## What is translated

- **EN**: 491/491 UI strings + 13 messages. This is the default language; the app opens in English.
- **FR**: ~100 strings + messages; everything else falls back to Japanese for now.
- **JA**: complete source.

## Known limitations

- English proper names are **best effort** and should later be aligned with the official DQ11 glossary (Erdrea).
- Some labels initialized during construction do not refresh live until UI rebuild.
- `MessageBox` texts are translated at display time.

## Adding a new language (example: Spanish)

1. Copy `Strings.en.xaml` → `Strings.es.xaml` and translate the values.
2. Add:

```xml
<Page Include="Localization\Strings.es.xaml">
```

to the `.csproj`.

3. In `MainWindow.xaml`, add:

```xml
<MenuItem Header="Español" IsCheckable="True" Tag="es" Click="Lang_Click"/>
```

and update `UpdateLanguageMenu()`.

## Regenerate after modifying XAML

```bash
python tools/extract_strings.py
python tools/build_dicts.py
```
