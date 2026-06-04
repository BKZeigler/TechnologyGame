using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    public void SetValue(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        fill.rectTransform.localScale = new Vector3(normalized, 1f, 1f);
        Debug.Log("HPBar normalized = " + normalized);
    }
}
