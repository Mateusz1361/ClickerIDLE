using System;

[Serializable]
public class TradeOptionInstanceDataJson {
    public string buyPrice;
    public string sellPrice;
    public string currencyIn;
    public string currencyOut;
}

[Serializable]
public class InvestorUpgradeDataJson {
    public string whatYouGetText;
    public string whatYouMultiply;
    public ulong price;
    public string multiplier;
}

[Serializable]
public class ShopItemPriceDataJson {
    public string name;
    public ulong value;
    public ulong unlockCount;
}

[Serializable]
public class ShopItemResultDataJson {
    public string type;
    public ulong value;
}

[Serializable]
public class ShopItemDataJson {
    public string name;
    public ulong unlockLevel;
    public bool belongsToMine;
    public ShopItemPriceDataJson[] price;
    public ShopItemResultDataJson result;
}

[Serializable]
public class WorldLocationDataJson {
    public string name;
    public string mainResource;
    public ulong price;
    public float posX;
    public float posY;
}

[Serializable]
public class WorldMapDataJson {
    public float sizeX;
    public float sizeY;
    public WorldLocationDataJson[] data;
};

[Serializable]
public class InventoryItemDataJson {
    public string name;
    public string iconPath;
    public ulong powerOfDynamite;
    public uint clicksToPop;
    public string type;
    public ulong price;
    public ulong maxStackCount;
    public ulong clickPower;
    public ulong damage;
    public double damageReduction;
}

[Serializable]
public class FactoryPriceItemDataJson {
    public string name;
    public ulong value;
}

[Serializable]
public class FactoryResultDataJson {
    public string type;
    public ulong value;
}

[Serializable]
public class FactoryItemDataJson {
    public string name;
    public string toUnlock;
    public FactoryPriceItemDataJson[] price;
    public FactoryResultDataJson result;
    public float duration;
}

[Serializable]
public class MonsterDropDataJson {
    public string name;
    public ulong count;
}

[Serializable]
public class MonsterDataJson {
    public string name;
    public string imagePath;
    public double maxHealth;
    public MonsterDropDataJson[] drops;
    public double hitPoints;
}

[Serializable]
public class InstanceWrapperDataJson<T> {
    public T[] data;
}
