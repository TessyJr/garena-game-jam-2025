using UnityEngine;

public class PlayerKeyComponent : MonoBehaviour
{
    public bool _keyObtained = false;
    [SerializeField] private GameObject _keyIcon;
    public void ObtainKey(){
        _keyObtained = true;
        _keyIcon.SetActive(true);
    }
    public void DropKey(){
        _keyObtained = false;
        _keyIcon.SetActive(false);
    }
}