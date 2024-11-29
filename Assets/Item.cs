using UnityEngine;

public enum item_level
{
    Easy,
    Medium,
    Hard
}
public class Item : MonoBehaviour
{
    public item_level level;
    public SpriteRenderer sprite;
    public int item_id;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;

        // Deactivate self if no sprite attached by ItemLottery
        if (sprite.sprite == null) this.gameObject.SetActive(false);
    }
}