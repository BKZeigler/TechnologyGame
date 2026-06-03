using UnityEngine;

public class DeleteSaveButton : MonoBehaviour
{
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
        Debug.Log("Save deleted.");
    }
}