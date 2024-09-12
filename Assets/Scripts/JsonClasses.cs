using System;

[Serializable]
public class TradeOptionInstanceData {
    public string buyPrice;
    public string sellPrice;
    public string currencyIn;
    public string currencyOut;
}

[Serializable]
public class InvestorUpgradeData {
    public string whatYouGetText;
    public string whatYouMultiply;
    public ulong price;
    public string multiplier;
}

[Serializable]
public class ShopItemPriceData {
    public string name;
    public ulong value;
    public ulong unlockCount;
}

[Serializable]
public class ShopItemResultData {
    public string type;
    public ulong value;
}

[Serializable]
public class ShopItemData {
    public string name;
    public ulong unlockLevel;
    public ShopItemPriceData[] price;
    public ShopItemResultData result;
}

[Serializable]
public class WorldLocationData {
    public string name;
    public string mainResource;
    public ulong price;
    public float posX;
    public float posY;
}

[Serializable]
public class WorldMapData {
    public float sizeX;
    public float sizeY;
    public WorldLocationData[] data;
};

[Serializable]
public class InventoryItemData {
    public string name;
    public string iconPath;
    public uint clicksToPop;
    public string type;
    public ulong price;
    public ulong maxStackCount;
}

[Serializable]
public class FactoryPriceItemData {
    public string name;
    public ulong value;
}

[Serializable]
public class FactoryResultData {
    public string type;
    public ulong value;
}

[Serializable]
public class FactoryItemData {
    public string name;
    public string toUnlock;
    public FactoryPriceItemData[] price;
    public FactoryResultData result;
    public float duration;
}

[Serializable]
public class InstanceWrapperData<T> {
    public T[] data;
}
