using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChoiceCharacterControl : MonoBehaviour
{
    [SerializeField] private Transform cameraShowCharacter;
    [SerializeField] private CharacterDefine[] _characterDefines;
    [SerializeField] private int currentIndex = 1;
    [SerializeField] private float speedLerp = 50;
    [SerializeField] private TextMeshProUGUI skill1DescriptionText;
    [SerializeField] private TextMeshProUGUI skill2DescriptionText;
    [SerializeField] private TextMeshProUGUI skill3DescriptionText;
    [SerializeField] private Button canPlayBtn;
    [SerializeField] private GameObject lockPanel;
    private CharacterDefine currentCharacterChoice;
    private Vector3 camreaPosition;
    private void Start()
    {
        OnSwitchCharacter();
    }

    private int CurrentIndex
    {
        set
        {
            currentIndex = value;
            if (currentIndex < 0)
            {
                currentIndex = _characterDefines.Length - 1;
            }
            else if(currentIndex >= _characterDefines.Length)
            {
                currentIndex = 0;
            }

            OnSwitchCharacter();
        }
        get => currentIndex;
    }
    // Start is called before the first frame update
    public void OnBack()
    {
        CurrentIndex--;
    }

    public void OnNext()
    {
        CurrentIndex++;
    }

    private void OnSwitchCharacter()
    {
        _characterDefines[currentIndex].character.gameObject.SetActive(true);
        currentCharacterChoice = _characterDefines[currentIndex];
        skill1DescriptionText.text = currentCharacterChoice.kill1Description;
        skill2DescriptionText.text = currentCharacterChoice.kill2Description;
        skill3DescriptionText.text = currentCharacterChoice.kill3Description;
        canPlayBtn.interactable = !currentCharacterChoice.isLock;
        lockPanel.SetActive(currentCharacterChoice.isLock);
        camreaPosition.x = currentCharacterChoice.character.position.x;
        camreaPosition.y = 0;
        camreaPosition.z = 6;
        for (int i = 0; i < _characterDefines.Length; i++)
        {
            if(i == currentIndex) continue;
            _characterDefines[i].character.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        cameraShowCharacter.position = Vector3.Lerp(cameraShowCharacter.position,
            camreaPosition, Time.deltaTime * speedLerp);
    }
}
[Serializable]
public class CharacterDefine
{
    public Transform character;
    public bool isLock;
    public string kill1Description;
    public string kill2Description;
    public string kill3Description;
}