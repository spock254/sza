[System.Serializable]
public class ItemTimeflowModify
{
    public int tics;
    public Item modifiedItem;
    public bool ticsTransition;
    public bool IsTimeFlowModifiable() 
    {
        return modifiedItem != null;
    }
}
