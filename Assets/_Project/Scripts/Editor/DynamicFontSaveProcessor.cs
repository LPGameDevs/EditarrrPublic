using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Prevents TextMeshPro dynamic font atlas changes from constantly triggering a git change and causing fonts to break at runtime
/// https://forum.unity.com/threads/tmpro-dynamic-font-asset-constantly-changes-in-source-control.1227831/
/// </summary>
public class DynamicFontSaveProcessor : AssetModificationProcessor
{
    private const string MenuNameAllowSavingDynamicFonts = "Fonts/Allow saving dynamic fonts";
    public const string AllowSavingDynamicFontsPref = "AllowSavingDynamicFonts";

    private static string[] OnWillSaveAssets(string[] paths)
    {
        if (EditorPrefs.GetBool(AllowSavingDynamicFontsPref))
        {
            return paths;
        }
        else
        {
            return paths.Where(path => !IsDynamicFontAsset(path)).ToArray();
        }
    }

    private static bool IsDynamicFontAsset(string assetPath)
    {
        var assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
        if (!typeof(TMP_FontAsset).IsAssignableFrom(assetType)) return false;

        var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
        if (fontAsset == null) return false;

        if (fontAsset.atlasPopulationMode != AtlasPopulationMode.Dynamic) return false;

        return true;
    }

    [MenuItem(MenuNameAllowSavingDynamicFonts)]
    private static void MenuAllowSavingDynamicFonts()
    {
        EditorPrefs.SetBool(AllowSavingDynamicFontsPref, !EditorPrefs.GetBool(AllowSavingDynamicFontsPref));
    }

    [MenuItem(MenuNameAllowSavingDynamicFonts, isValidateFunction: true)]
    private static bool MenuAllowSavingDynamicFontsValidate()
    {
        Menu.SetChecked(MenuNameAllowSavingDynamicFonts, EditorPrefs.GetBool(AllowSavingDynamicFontsPref));
        return true;
    }
}