[System.Serializable]
public class ItemTimeflowModify
{
    public int tics;
    public Item modifiedItem;

    public bool IsTimeFlowModifiable() 
    {
        return modifiedItem != null;
    }
}
