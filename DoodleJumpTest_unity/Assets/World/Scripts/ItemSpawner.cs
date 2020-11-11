using UnityEngine;

public enum ItemType { Spring, Booster };

[CreateAssetMenu(fileName = "ItemSpawner", menuName = "Scriptable Objects/Item Spawner")]
public class ItemSpawner : ScriptableObject
{
    [SerializeField]
    private Spring _springPrefab = default;

    [SerializeField]
    private Booster _boosterPrefab = default;

    public GameObject SpawnItem(ItemType type, Transform parent)
    {
        GameObject newItem = null;

        switch (type)
        {
            case ItemType.Spring:
                newItem = Instantiate(_springPrefab, parent).gameObject;
                break;

            case ItemType.Booster:
                newItem = Instantiate(_boosterPrefab, parent).gameObject;
                break;
        }

        Debug.Assert(newItem != null, "Failed to create item of type: " + type.ToString());

        return newItem;
    }
}