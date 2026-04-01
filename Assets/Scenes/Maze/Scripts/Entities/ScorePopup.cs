using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : ScorecardCommonsMesh
{
    public PopupTheme skin;
    public GameObject valueDisplay;

    public enum PopupBehavior
    {
        MoveUp,
        Stationary
    }

    public class MovementValues
    {
        public static readonly float height = 15f;
        public static readonly float speed = 2f;
    }

    public float lifespan = 2f;
    public PopupBehavior behavior = PopupBehavior.Stationary;
    private bool _valueActive = false;
    private float _startY = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshSkin();
        UpdateScore(true);
    }

    void OnValidate()
    {
        Invoke(nameof(OnHardValidate), 0);
    }

    void OnHardValidate()
    {
        RefreshSkin();
        UpdateScore(true);
    }

    public void Spawn()
    {
        _startY = transform.position.y;
        _valueActive = true;
        StartCoroutine(AfterSpawn());
    }

    private IEnumerator AfterSpawn()
    {
        yield return new WaitForSeconds(lifespan);
        _valueActive = false;
        Destroy(gameObject);
    }

    public void RefreshSkin()
    {
        if (!skin) return;
        if (skin.numbers == null) return;

        charWidth = skin.numberWidth;
        charHeight = skin.numberHeight;
        charSpacing = skin.spacing;
        charSizeOverrides = skin.sizeOverrides;
        charPixelsPerUnit = skin.pixelsPerUnit;
        UpdateSkinBase(skin.numbers.ToArray());
        UpdateScore(true);
    }

    void OnActive()
    {
        switch (behavior)
        {
            case PopupBehavior.MoveUp:
                var pos = transform.position;
                var newY = _startY + MovementValues.height;
                var target = new Vector3(transform.position.x, newY, transform.position.z);
                float step = MovementValues.speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(pos, target, step);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_valueActive) OnActive();
        UpdateScore();
    }
}
