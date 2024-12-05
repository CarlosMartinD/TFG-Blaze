using TMPro;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public float lifetime;

    public GameObject particleEffect;

    public TextMeshPro textMeshPro;

    void Start()
    {
        
    }

    public static DamageIcon Instantiate(DamageIcon original, Vector3 position, int damage)
    {
        DamageIcon damageIcon = Instantiate(original, new Vector3(position.x, position.y, -20), Quaternion.identity);
        TextMeshPro scr = damageIcon.GetComponentInChildren<TextMeshPro>();
        scr.text = damage.ToString();
        return damageIcon;
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
