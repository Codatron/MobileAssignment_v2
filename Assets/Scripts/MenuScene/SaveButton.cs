using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour
{
    public TMP_InputField inputName;
    public void SaveName()
    {
        SaveManager.Instance.SaveName(inputName.text);
    }
}
