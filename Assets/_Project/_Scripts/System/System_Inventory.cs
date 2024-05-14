using System.Collections.Generic;

public static class System_Inventory
{

    private static readonly List<SO_BodyPart> m_inventory = new();
    public static IList<SO_BodyPart> Inventory => m_inventory.AsReadOnly();

    private static bool m_inventoryStarted = false;

    public static void Clear()
    {
        m_inventory.Clear();
    }

    public static void StartInventory(SO_BodyPart[] soBodyParts)
    {
        if (m_inventoryStarted)
            return;

        foreach (var soBodyPart in soBodyParts)
            AddItem(soBodyPart);

        m_inventoryStarted = true;
    }

    public static void AddItem(SO_BodyPart soBodyPart)
    {
        if (m_inventory.Contains(soBodyPart))
            return;

        if (soBodyPart.sprite == null && soBodyPart.leftSprite == null)
            return;

        m_inventory.Add(soBodyPart);
    }

    public static void RemoveItem(SO_BodyPart soBodyPart)
    {
        if (!m_inventory.Contains(soBodyPart))
            return;

        m_inventory.Remove(soBodyPart);
    }

}
