using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    private static MissionManager _instance;

    public static MissionManager Instance { get => _instance; set => _instance = value; }
    public GameObject CurrentTargetMission { get => _currentTargetMission; set => _currentTargetMission = value; }

    [SerializeField] GameObject[] _planets;
    [SerializeField] GameObject _firstMissionPlanet;
    public GameObject _currentTargetMission;
    public int _accomplishedMissionCount = 0;

    [SerializeField] TextMeshProUGUI _deliveryCompletedAlertTxt;
    [SerializeField] TextMeshProUGUI _deliveryCompletedCounterTxt;

    [SerializeField] TextMeshProUGUI _missionDisplayPlanetName;
    [SerializeField] Image _missionDisplayPlanetPreview;
    [SerializeField] TextMeshProUGUI _missionDisplayTimer;

    [SerializeField] PlanetLocator _planetLocator;

    float _timeRemaining = 60f;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Update()
    {
        if (_currentTargetMission == null || _timeRemaining <= -0.5f)
        {
            SetNewMission();
        }

        UpdateTimer();
    }

    private void SetNewMission()
    {
        GameObject previousMissionTarget = _currentTargetMission;
        if (previousMissionTarget != null)
        {
            previousMissionTarget.GetComponent<OrbitalPlanet>().DisableDeliveryZone();
        }
        _currentTargetMission = null;

        // Force first mission
        if (_accomplishedMissionCount == 0)
        {
            _currentTargetMission = _firstMissionPlanet;
        }


        while (_currentTargetMission == null)
        {
            int rngIndex = Random.Range(0, _planets.Length);
            _currentTargetMission = _planets[rngIndex];

            // on ne veut pas de la meme mission
            if (_currentTargetMission == previousMissionTarget)
            {
                _currentTargetMission = null;
            }
        }

        _currentTargetMission.GetComponent<OrbitalPlanet>().EnableDeliveryZone();
        _planetLocator.SetNewTarget(_currentTargetMission);
        // Change le nom
        _missionDisplayPlanetName.text = _currentTargetMission.gameObject.name;
        // Affiche la preview 
        _missionDisplayPlanetPreview.sprite = _currentTargetMission.GetComponent<OrbitalPlanet>().PreviewSprite;
        _missionDisplayPlanetPreview.color = _currentTargetMission.GetComponent<OrbitalPlanet>().PreviewColor;
        // Demarre le timer
        ResetTimer();
    }

    void ResetTimer()
    {
        _timeRemaining = 60f - (Mathf.FloorToInt(_accomplishedMissionCount / 3f) * 5);
        Mathf.Clamp(_timeRemaining, 25f, 60f);
    }

    void UpdateTimer()
    {
        _timeRemaining -= Time.deltaTime;
        int timeRemaining = Mathf.CeilToInt(_timeRemaining);
        _missionDisplayTimer.text = timeRemaining.ToString();
    }

    internal void ValidateDelivery()
    {
        _accomplishedMissionCount++;
        _deliveryCompletedAlertTxt.gameObject.SetActive(true);
        _deliveryCompletedCounterTxt.text = "Deliveries Completed : " + _accomplishedMissionCount.ToString();

        StartCoroutine(HideDeliveryCompletedText());
        SetNewMission();
    }

    IEnumerator HideDeliveryCompletedText()
    {
        yield return new WaitForSeconds(1.5f);
        _deliveryCompletedAlertTxt.gameObject.SetActive(false);
    }
}
