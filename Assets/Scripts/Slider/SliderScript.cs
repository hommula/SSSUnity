using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private TextMeshProUGUI _sliderText;
    private PlayerController _playerController;
    void Start()
    {
        _playerController = _gameObject.GetComponent<PlayerController>();
        _slider.onValueChanged.AddListener((v) =>
        {
            _playerController.movementSpeed = v;
            _sliderText.text = (v).ToString("0.0");
        });
    }
}
