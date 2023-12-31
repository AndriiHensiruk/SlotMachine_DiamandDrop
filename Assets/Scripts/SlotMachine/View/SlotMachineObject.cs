﻿using System.Collections;
using ICouldGames.Dependency.Singly;
using ICouldGames.Extensions.System.Collections.Generic;
using ICouldGames.SlotMachine.Controller;
using ICouldGames.SlotMachine.Spin.Outcome.Info;
using ICouldGames.SlotMachine.View.Column;
using ICouldGames.SlotMachine.View.Column.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace ICouldGames.SlotMachine.View
{
    public class SlotMachineObject : MonoBehaviour
    {
        [SerializeField] private Button spinButton;
        [SerializeField] private SlotMachineColumn column1;
        [SerializeField] private SlotMachineColumn column2;
        [SerializeField] private SlotMachineColumn column3;
        [SerializeField] private ParticleSystem coinParticleSystem;
        [SerializeField] private GameObject linkHit;

        [Header("Settings")]
        [SerializeField] private float nextColumnSpinDelay;
        [Range(0.5f, 500f)]
        [SerializeField] private float startingSpinSpeed;
        [Range(0.05f, 10f)]
        [SerializeField] private float startingSpinDuration;
        [Range(0.05f, 10f)]
        [SerializeField] private float fastSpinStopDuration;
        [Range(0.05f, 10f)]
        [SerializeField] private float[] criticalLastSpinStopDurations;

        private SlotMachineController _slotMachineController;
        private WaitForSeconds _nextColumnSpinDelayWait;
        private GameObject _canvas;

        private void Awake()
        {
            _slotMachineController = SingletonProvider.Instance.Get<SlotMachineController>();
        }

        private void Start()
        {
            _nextColumnSpinDelayWait = new WaitForSeconds(nextColumnSpinDelay);
            _canvas = GameObject.FindGameObjectWithTag("Canvas");
            spinButton.onClick.AddListener(() => StartCoroutine(Spin()));
        }

        private void OnValidate()
        {
            _nextColumnSpinDelayWait = new WaitForSeconds(nextColumnSpinDelay);
        }

        private IEnumerator Spin()
        {
            spinButton.interactable = false;
            
            var spinOutcome = _slotMachineController.GetNextSpin(true).OutcomeInfo;
            spinOutcome.SpinItemTypes.Shuffle();

            StartCoroutine(column1.Spin(GenerateSpinSettings(spinOutcome, 0)));
            yield return _nextColumnSpinDelayWait;
            StartCoroutine(column2.Spin(GenerateSpinSettings(spinOutcome, 1)));
            yield return _nextColumnSpinDelayWait;
            yield return StartCoroutine(column3.Spin(GenerateSpinSettings(spinOutcome, 2)));

            TryEmittingCoins(spinOutcome);
           

            spinButton.interactable = true;
        }

        private ColumnSpinSettings GenerateSpinSettings(SpinOutcomeInfo outcomeInfo, int spinNumber)
        {
            var columnSpinSettings = new ColumnSpinSettings();
            columnSpinSettings.StartingSpinSpeed = startingSpinSpeed;
            columnSpinSettings.StartingSpinDuration = startingSpinDuration;
            columnSpinSettings.ResultItemType = outcomeInfo.SpinItemTypes[spinNumber];

            if(spinNumber == 2 && outcomeInfo.SpinItemTypes[0] == outcomeInfo.SpinItemTypes[1])
            {
                columnSpinSettings.SpinStopDuration = criticalLastSpinStopDurations[Random.Range(0, criticalLastSpinStopDurations.Length)];
                columnSpinSettings.SlowingTweenType = LeanTweenType.easeOutSine;
            }
            else
            {
                columnSpinSettings.SpinStopDuration = fastSpinStopDuration;
                columnSpinSettings.SlowingTweenType = LeanTweenType.easeOutBack;
            }

            return columnSpinSettings;
        }

        private void TryEmittingCoins(SpinOutcomeInfo spinOutcome)
        {
            const int baseParticleCount = 4;
            const int extraParticleCoefficient = 2;

            if (spinOutcome.IsPrizeAvailable())
            {
                coinParticleSystem.Emit(baseParticleCount + extraParticleCoefficient * spinOutcome.PrizeTier);
                displayHint();
            }
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveAllListeners();
        }

        private void displayHint()
        {
            GameObject tempLink = linkHit;
            int msg = Random.Range(1000,500000);

            LeanTween.scale(linkHit, new Vector3(100f, 100f, 100f), 1.5f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.scale(linkHit, new Vector3(0f, 0f, 0f), 0.8f).setDelay(4f).setEase(LeanTweenType.easeOutElastic);
            tempLink.GetComponentInChildren<Text>().text = msg.ToString();
            tempLink.transform.SetParent(_canvas.transform);
            
        }
    }
}