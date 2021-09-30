/***
 *
 * @Author: Roman Boryslavskyy
 * @Created on: 30/09/21
 *
 ***/

#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetExporters.MeshExporter
{
    public static class FBXExporter
    {
        #region ## Variables ##

        private static string _path = Application.dataPath + "/AssetExporters/MeshExporter/Exported/";

        // General config
        private static bool _enableExport = true;
        private static bool _enableExportLimit = false;
        private static int _exportLimit = 300;

        #endregion

        #region ## Core ##

        [MenuItem("Asset Exporters/Mesh Exporter/Export all in scene")]
        private static void ExportAssetsInScene()
        {
            var scene = SceneManager.GetActiveScene();
            var allGameObjects = scene.GetRootGameObjects();
            var meshesList = new List<MeshFilter>();

            // process all the objects in the scene and find all the meshes to export
            Utils.Log("Processing all the sub-objects");
            foreach (var obj in allGameObjects)
            {
                ProcessGameObject(obj, meshesList);
            }

            if (_enableExport)
            {
                // export all the models
                for (int i = 0; i < meshesList.Count; i++)
                {
                    if (_enableExportLimit && i >= _exportLimit)
                    {
                        Utils.Log("Export limit reached!");
                        break;
                    }

                    var progress = i > 0 ? (meshesList.Count / i) / 100 : 0.01f;

                    EditorUtility.DisplayProgressBar($"FBX Exporter - {i + 1}/{meshesList.Count}",
                        $"Currently exporting: {meshesList[i].name}", progress);

                    ExportGameObject(meshesList[i].gameObject, meshesList[i].sharedMesh.name);
                }

                EditorUtility.ClearProgressBar();
            }

            Utils.Log("That's all folks!");
        }

        private static void ExportGameObject(Object obj, string name)
        {
            string filePath = Path.Combine(_path, name + ".fbx");
            ModelExporter.ExportObject(filePath, obj);
        }

        #endregion

        #region ## Utils ##

        private static void ProcessGameObject(GameObject go, List<MeshFilter> meshes)
        {
            Utils.Log($"Processing {go.name} child.");
            var meshFilters = go.GetComponents<MeshFilter>();

            foreach (var meshFilter in meshFilters)
            {
                if (!meshes.Contains(meshFilter) && meshFilter.sharedMesh != null)
                {
                    meshes.Add(meshFilter);
                }
            }

            foreach (Transform child in go.transform)
            {
                ProcessGameObject(child.gameObject, meshes);
            }
        }

        #endregion
    }
}

#endif