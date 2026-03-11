using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public static IEnumerator LoadSceneWithSaveData(string scene) {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;

        // 0.9 (90%) is used as a magic number, scene activation makes up the last 10%
        while (!op.isDone && op.progress < 0.9f) {
            yield return null;
        }

        

        // At this point, `scene` is loaded but *not* active.
        // Do your save logic here.
        
        // And finally... enable the scene.
        op.allowSceneActivation = true;
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}