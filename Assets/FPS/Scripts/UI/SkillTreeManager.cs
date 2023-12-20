using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class SkillTreeManager : MonoBehaviour
    {
        public static SkillTreeManager SkillTree;

        private void Awake()
        {
            SkillTree = this;
        }

        public int[] SkillLevels;
        public int[] SkillCaps;
        public string[] SkillNames;
        public string[] SkillDescription;

        public List<SkillManager> SkillList;
        public GameObject SkillHolder;

        public List<GameObject> ConnectorList;
        public GameObject ConnectorHolder;

        private void Start()
        {
            SkillLevels = new int[6];
            SkillCaps = new[] {2, 2, 1, 1, 1, 1,};

            SkillNames = new[] {"Health Up", "Damage Up", "Dash", "Grapple", "Jump Up", "Jetpack",};
            SkillDescription = new[]
            {
                "Increase amount of health",
                "Increase damage",
                "Gain ability to dash (Q)",
                "Unlock grappling hook (E)",
                "Increase jump height",
                "Unlock jetpack (Space)",
            };

            foreach (var skill in SkillHolder.GetComponentsInChildren<SkillManager>())
            {
                SkillList.Add(skill);
            }

            foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
            {
                ConnectorList.Add(connector.gameObject);
            }

            for (var i = 0; i < SkillList.Count; i++)
            {
                SkillList[i].SkillId = i;
            }

            SkillList[4].ConnectedSkills = new[] {5};

            UpdateAllSkillUI();
        }

        public void UpdateAllSkillUI()
        {
            foreach (var skill in SkillList)
            {
                skill.UpdateUI();
            }
        }
    }
}
