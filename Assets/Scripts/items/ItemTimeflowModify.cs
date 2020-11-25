[System.Serializable]
public class ItemTimeflowModify
{
    public int tics;
    public Item modifiedItem;

    public bool IsTimeFlowModifiable() 
    {
        return modifiedItem != null;
    }

    //public void OnTicCountAndModif() 
    //{
    //    if (tics == 0) 
    //    { 
    //        // заменть
    //        // отписатся
    //    }

    //    tics--;
    //}
}
