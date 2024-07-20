using System;

[Serializable]
public class ResourceInstanceData {
    public string name;
    public string iconPath;
}

[Serializable]
public class TradeOptionInstanceData {
    public double buyPrice;
    public double sellPrice;
    public string currencyIn;
    public string currencyOut;
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
public class InstanceWrapper<T> {
    public T[] data;
}
