using UnityEngine;

public class Identifier : MonoBehaviour
{
    public string identiferName;


    public static GameObject FindGameObject(GameObject parent, string identiferName, bool includeInactive = false)
    {
        GameObject result = null;
        foreach (var target in parent.GetComponentsInChildren<Identifier>(includeInactive))
        {
            if (target.identiferName != identiferName) continue;
            result = target.gameObject;
            break;
        }
        return result;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
