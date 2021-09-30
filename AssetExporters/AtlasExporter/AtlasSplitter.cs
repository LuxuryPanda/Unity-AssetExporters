/***
 *
 * @Author: Roman Boryslavskyy
 * @Created on: 30/09/21
 *
 ***/

#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetExporters.AtlasExporter
{
    public static class AtlasSplitter
    {
        #region ## Variables ##

        private static bool _enableProgressBar = false;

        #endregion

        #region ## Menu Items ##

        [MenuItem("Assets/Asset Exporters/AtlasExporter/Slip this Atlas", false, 0)]
        public static void SlipSelectedAtlas()
        {
            ExtractSprites(false);
        }

        [MenuItem("Assets/Asset Exporters/AtlasExporter/Slip this Atlas to folder", false, 0)]
        public static void SlipSelectedAtlasToFolder()
        {
            ExtractSprites(true);
        }

        #endregion

        #region ## Core ##

        private static void ExtractSprites(bool toFolder = false)
        {
            Object file = Selection.activeObject;
            EditorUtility.DisplayProgressBar("Atlas Splitter", $"Splitting {file.name}", 1f);

            var rootPath = AssetDatabase.GetAssetPath(file);

            // check if the texture has R/W permission, if not then set it to true
            TextureImporter tImporter = AssetImporter.GetAtPath(rootPath) as TextureImporter;

            // the file isn't a Texture so just quit
            if (!tImporter) return;


            if (tImporter && !tImporter.isReadable)
            {
                tImporter.isReadable = true;
                AssetDatabase.ImportAsset(rootPath);
                AssetDatabase.Refresh();

                EditorUtility.DisplayProgressBar("Atlas Splitter", "Set the Atlas with R/W permission", 5f);
            }

            string fullPath = Path.GetFullPath(AssetDatabase.GetAssetPath(file));
            string folder = GetMyFolder(fullPath, Selection.activeObject.name);


            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath);

            string newFolder = folder;
            if (toFolder)
            {
                newFolder = folder + Selection.activeObject.name + "/";
                Directory.CreateDirectory(newFolder);
            }


            EditorUtility.DisplayProgressBar("Atlas Splitter", "Starting splitting sprites!", 10f);
            for (int i = 0; i < sprites.Length; i++)
            {
                string name = sprites[i].name + ".png";
                string new_name = newFolder + name;


                Sprite s = (Sprite)sprites[i];
                EditorUtility.DisplayProgressBar($"Atlas Splitter - {i + 1}/{sprites.Length}", $"Exporting {s.name}",
                    100f / sprites.Length);

                // Convert the sprite to a Texture2D and then write it to disk
                Texture2D tex2d = ConvertFromSprite(s);
                File.WriteAllBytes(new_name, tex2d.EncodeToPNG());
                AssetDatabase.Refresh();


                string folderPathToSprite = "";
                folderPathToSprite = AssetDatabase.GetAssetPath(file).Replace(".png", toFolder ? "/" : "");
                folderPathToSprite += sprites[i].name + ".png";

                // get the saved image and try to import it as a sprite
                TextureImporter textureImporter = AssetImporter.GetAtPath(folderPathToSprite) as TextureImporter;
                if (textureImporter)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteBorder = s.border;
                    textureImporter.spritePivot = s.pivot;
                    AssetDatabase.ImportAsset(folderPathToSprite);
                }

                AssetDatabase.Refresh();
            }

            EditorUtility.DisplayProgressBar("Atlas Splitter", "That's all folks!", 100f);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        private static string GetMyFolder(string full, string name)
        {
            string s = full.Replace(".png", "");
            s = s.Substring(0, s.Length - name.Length);
            return s;
        }

        private static Texture2D ConvertFromSprite(Sprite sprite)
        {
            // Convert the sprite to a Texture2D using its pixels
            var newTexture2d = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width,
                (int)sprite.rect.height);

            newTexture2d.SetPixels(pixels);
            newTexture2d.Apply();
            return newTexture2d;
        }

        #endregion
    }
}

#endif