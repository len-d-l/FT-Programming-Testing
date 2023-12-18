using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.FPS.Game;

namespace Unity.FPS.Game
{
    public class LevelSystem : MonoBehaviour
    {
        public int Level;
        public float CurrentExp;
        public float RequiredExp;
        public int SkillPoints;

        private float lerpTimer;
        private float delayTimer;

        [Header("UI")]
        public Image ExpBar;
        public Image ExpBarBack;
        public TextMeshProUGUI LevelTxt;

        [Header("Multipliers")]
        [Range(1f, 300f)]
        public float AdditionMultiplier = 300;
        [Range(2f, 4f)]
        public float PowerMultiplier = 2;
        [Range(7f, 14f)]
        public float DivisionMultiplier = 7;


        // Start is called before the first frame update
        void Start()
        {
            ExpBar.fillAmount = CurrentExp / RequiredExp;
            ExpBarBack.fillAmount = CurrentExp / RequiredExp;
            RequiredExp = CalculateRequiredExp();
            LevelTxt.text = "" + Level;

            EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);

            /*StartCoroutine(DisplayLevelNumber());*/
        }

        // Update is called once per frame
        void Update()
        {
            UpdateExpUI();

            if (CurrentExp > RequiredExp)
                LevelUp();
        }

        public void UpdateExpUI()
        {
            float expFraction = CurrentExp / RequiredExp;

            if (ExpBar.fillAmount < expFraction)
            {
                delayTimer += Time.deltaTime;
                ExpBarBack.fillAmount = expFraction;

                if (delayTimer > 0)
                {
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / 4;
                    ExpBar.fillAmount = Mathf.Lerp(ExpBar.fillAmount, ExpBarBack.fillAmount, percentComplete);
                }
            }
        }

        public void GainExperienceFlatRate(float expGained)
        {
            CurrentExp += expGained;
            lerpTimer = 0f;
        }

        public void LevelUp()
        {
            Level++;
            ExpBar.fillAmount = 0f;
            ExpBarBack.fillAmount = 0f;
            CurrentExp = Mathf.RoundToInt(CurrentExp - RequiredExp);
            RequiredExp = CalculateRequiredExp();
            LevelTxt.text = "" + Level;

            /*StartCoroutine(DisplayLevelNumber());*/

            SkillPoints++;
        }

        // Fade in level 
        /*IEnumerator DisplayLevelNumber()
        {
            LevelTxt.text = "" + Level;
            LevelTxt.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);

            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.deltaTime;
                Color textColor = LevelTxt.color;
                textColor.a = Mathf.Lerp(1f, 0f, timer);
                LevelTxt.color = textColor;
                yield return null;
            }

            LevelTxt.gameObject.SetActive(false);
            LevelTxt.color = Color.white;
        }*/

        private int CalculateRequiredExp()
        {
            int solveForRequiredExp = 0;
            for (int levelCycle = 1; levelCycle <= Level; levelCycle++)
            {
                solveForRequiredExp += (int)Mathf.Floor(levelCycle + AdditionMultiplier * Mathf.Pow(PowerMultiplier, levelCycle / DivisionMultiplier));
            }

            return solveForRequiredExp / 4;
        }

        public void OnEnemyKilled(EnemyKillEvent evt)
        {
            GainExperienceFlatRate(evt.Exp);
        }
    }
}
