using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour {
  private static CoroutineRunner instance;
  public static void RunCoroutine(IEnumerator coroutine) {
    if (instance == null) {
      instance = new GameObject("runner").AddComponent<CoroutineRunner>();
    }
    instance.StartCoroutine(coroutine);
  }
}