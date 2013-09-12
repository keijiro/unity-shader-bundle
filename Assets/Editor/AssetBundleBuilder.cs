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

		BuildPipeline.PushAssetDependencies ();

		var shader = AssetDatabase.LoadMainAssetAtPath ("Assets/Aurora.shader");
		var path = System.IO.Path.Combine (destination, "common.unity3d");
		var options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
		BuildPipeline.BuildAssetBundle (shader, new Object[] {shader}, path, options, CurrentPlatform);

		var prefab = AssetDatabase.LoadMainAssetAtPath ("Assets/Plane.prefab") as GameObject;

        for (var i = 0; i < 9; i++) {
			prefab.renderer.sharedMaterial.SetVector ("_UAnimR", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_VAnimR", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_UAnimG", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_VAnimG", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_UAnimB", RandomParameter ());
            prefab.renderer.sharedMaterial.SetVector ("_VAnimB", RandomParameter ());

            path = System.IO.Path.Combine (destination, "panel" + i + ".unity3d");

			BuildPipeline.PushAssetDependencies ();
			BuildPipeline.BuildAssetBundle (prefab, new Object[] {prefab}, path, options, CurrentPlatform);
			BuildPipeline.PopAssetDependencies();
        }

		BuildPipeline.PopAssetDependencies();
	}
}