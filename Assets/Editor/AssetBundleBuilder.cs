using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetBundleBuilder
{
    static BuildTarget CurrentPlatform {
        get {
#if UNITY_IOS
            return BuildTarget.iPhone;
#elif UNITY_ANDROID
            return BuildTarget.Android;
#else
            return BuildTarget.WebPlayer;
#endif
        }
    }

    static Vector4 RandomParameter ()
    {
        return new Vector4 (
            Random.Range (1.0f, 3.0f),
            Random.Range (0.1f, 0.3f),
            Random.Range (1.0f, 3.0f),
            Random.Range (0.5f, 1.5f)
        );
    }

    [MenuItem("Assets/Build Asset Bundle")]
    static void BuildAssetBundle ()
    {
        var destination = EditorUtility.SaveFolderPanel ("Save Asset Bundle", "", "");
        if (destination.Length == 0) {
            return;
        }

        var prefab = AssetDatabase.LoadMainAssetAtPath ("Assets/Quad.prefab") as GameObject;
        var uanimValue = prefab.renderer.sharedMaterial.GetVector ("_UAnim");
        var vanimValue = prefab.renderer.sharedMaterial.GetVector ("_VAnim");

        for (var i = 0; i < 9; i++) {
            prefab.renderer.sharedMaterial.SetVector ("_UAnim", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_VAnim", RandomParameter ());

            var path = System.IO.Path.Combine (destination, "panel" + i + ".unity3d");
            var options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
            BuildPipeline.BuildAssetBundle (prefab, new Object[] {prefab}, path, options, CurrentPlatform);
        }

        prefab.renderer.sharedMaterial.SetVector ("_UAnim", uanimValue);
        prefab.renderer.sharedMaterial.SetVector ("_VAnim", vanimValue);
    }
}