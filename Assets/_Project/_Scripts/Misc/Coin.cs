using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] private int value;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerController _))
            return;

        Manager_Game.OnAddCoinAction(value);

        if (value < 50)
            Manager_Sound.OnGetCoinAction();
        else if (value < 100)
            Manager_Sound.OnGetDiamondAction();

        Destroy(gameObject);
    }

}
