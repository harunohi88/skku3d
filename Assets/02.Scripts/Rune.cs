using UnityEngine;

public class Rune : MonoBehaviour
{
    public int TID;
    private RuneData _data;

    private int currentTier;

    private void Start()
    {
        if (_data == null) LoadDtata();
    }

    private void LoadDtata()
    {
        if(TID == 0)
        {
            Debug.Log("TID를 넣어주세요");
            return;
        }

        _data = DataTable.instance.GetRuneData(TID);
    }

    public void EquipRune()
    {

    }

    public void UnEquipRune()
    {

    }

    public void OnSkill()
    {

    }
}
