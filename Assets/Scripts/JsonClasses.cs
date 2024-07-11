using System;

[Serializable]
public class ResourceInstanceData {
    public string name;
    public float minDropChance;
    public float maxDropChance;
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
public class BuyOptionCurrencyInstanceData {
    public string name;
    public ulong value;
    public int unlockQuantity;
}

[Serializable]
public class BuyOptionResultInstanceData {
    public string type;
    public ulong value;
}

[Serializable]
public class BuyOptionInstanceData {
    public int unlockLevel;
    public BuyOptionCurrencyInstanceData[] price;
    public BuyOptionResultInstanceData result;
}

[Serializable]
public class InstanceWrapper<T> {
    public T[] data;
}
