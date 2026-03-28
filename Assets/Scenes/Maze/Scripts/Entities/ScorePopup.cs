using System.Collections;
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
        
        void SetCharSize(LayoutElement el)
        {
            el.preferredHeight = (float)skin.numberHeight / skin.pixelsPerUnit;
            el.preferredWidth = (float)skin.numberWidth / skin.pixelsPerUnit;
        }

        if (!skin) return;
        if (skin.numbers == null) return;

        charWidth = (float)skin.numberWidth / skin.pixelsPerUnit;
        charHeight = (float)skin.numberHeight / skin.pixelsPerUnit;

        SetCharSize(digit1.GetComponent<LayoutElement>());
        SetCharSize(digit2.GetComponent<LayoutElement>());
        SetCharSize(digit3.GetComponent<LayoutElement>());
        SetCharSize(digit4.GetComponent<LayoutElement>());
        SetCharSize(digit5.GetComponent<LayoutElement>());

        UpdateSkinBase(skin.numbers.ToArray());
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
