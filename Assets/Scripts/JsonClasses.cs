using System;

[Serializable]
public class OreInstanceData {
    public string name;
    public float minDrop;
    public float maxDrop;
    public ulong digIncrement;
    public string iconName;
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
