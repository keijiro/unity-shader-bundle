using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public string baseURL = "https://github.com/keijiro/unity-shader-bundle/raw/data";
    public GUIStyle labelStyle;
    string stateString;

    static string GetPlatformDirectoryName ()
    {
#if UNITY_IOS
        return "ios";
#elif UNITY_ANDROID
        return "android";
#else
        return "webplayer";
#endif
    }

    static Vector3 GetIndexedPosition (int index)
    {
        return new Vector3 (index % 3 - 1, index / 3 - 1, 0);
    }

    IEnumerator Start ()
    {
		Caching.CleanCache ();

		while (true) {
            var objects = new GameObject[9];

            for (var i = 0; i < 9; i++) {
                var url = baseURL + "/" + GetPlatformDirectoryName () + "/panel" + i + ".unity3d";
                stateString = "Loading " + url;

                var www = WWW.LoadFromCacheOrDownload (url, 0);
                yield return www;

                objects [i] = Instantiate (www.assetBundle.mainAsset, GetIndexedPosition (i), transform.rotation) as GameObject;

                yield return new WaitForSeconds (0.5f);

                www.assetBundle.Unload (false);
            }

            foreach (var o in objects) {
                Destroy (o);
            }
        }
    }

    void OnGUI ()
    {
        var label = "(" + Time.frameCount + ") " + stateString;
        GUI.Label (new Rect (0, 0, Screen.width, Screen.height), label, labelStyle);
    }
}