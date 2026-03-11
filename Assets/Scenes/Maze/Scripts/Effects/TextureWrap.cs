using UnityEngine;

public class SetTextureWrapMode : MonoBehaviour
{
    [Tooltip("Assign the material whose texture wrap mode you want to change.")]
    public Material targetMaterial;

    [Tooltip("Name of the texture property in the shader (default is _MainTex).")]
    public string texturePropertyName = "_MainTex";

    [Tooltip("Wrap mode to apply.")]
    public TextureWrapMode wrapMode = TextureWrapMode.Repeat;

    void Start()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("No material assigned. Please assign a material in the inspector.");
            return;
        }

        // Get the texture from the material
        Texture tex = targetMaterial.GetTexture(texturePropertyName);

        if (tex == null)
        {
            Debug.LogError($"No texture found in material for property '{texturePropertyName}'.");
            return;
        }

        // Set the wrap mode
        tex.wrapMode = wrapMode;

        // If you want to apply immediately in editor mode
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(tex);
#endif

        Debug.Log($"Texture wrap mode set to {wrapMode} for '{texturePropertyName}'.");
    }
}
