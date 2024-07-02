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
    public ulong price;
    public ulong gain;
    public string currencyIn;
    public string currencyOut;
}

[Serializable]
public class BuyOptionInstanceData {
    public ulong price;
    public ulong power;
    public int unlockLevel;
}

[Serializable]
public class WorkerInstanceData {
    public ulong price;
    public ulong power;
    public int unlockLevel;
}

[Serializable]
public class InstanceWrapper<T> {
    public T[] data;
}
