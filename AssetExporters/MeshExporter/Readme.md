# Mesh Exporter

This tool allows to convert Unity's native meshes (.asset) to fbx.


## Installation

First of all in your Unity project you need to install "FBX Exporter" package from the PackageManager, after that download the [unitypackage](https://github.com/LuxuryPanda/Unity-AssetExporters/raw/main/AssetExporters.unitypackage) and import it.

Before using the tool you'll need to configure Unity's FBX exporter at least once so, open the scene "MeshExporting" in the folder "AssetExporters/MeshExporter/", then right click on the gameobject that is in there and select "Export to FBX".
You'll be prompted with an editor window in which you can configure the exporter.

![Config Window](https://raw.githubusercontent.com/LuxuryPanda/Unity-AssetExporters/main/%23Screenshots/ExportPanel.png)

## Usage

After following the installation step you should be ready to export the meshes.

Simply drop in the "MeshExporting" scene the meshes that you want to export and click on the menu item "Asset Exporters/Mesh Exporter/Export all in scene",
the tool wil automatically export every mesh and put them into the "Exported" folder.


To get the best out of the export I suggest you to not only import the meshes but also apply them a material so it will be included in the fbx.
  
