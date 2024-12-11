using TMPro;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public float lifetime;

    public TextMeshPro textMeshPro;

    public static DamageIcon Instantiate(DamageIcon original, Vector3 position, int damage)
    {
        DamageIcon damageIcon = Instantiate(original, new Vector3(position.x, position.y - 0.2f, -2), Quaternion.identity);
        TextMeshPro scr = damageIcon.GetComponentInChildren<TextMeshPro>();
        scr.text = damage.ToString();
        return damageIcon;
    }

    public void Destroy()
    {
        Destroy(gameObject, 1f);
    }
}
