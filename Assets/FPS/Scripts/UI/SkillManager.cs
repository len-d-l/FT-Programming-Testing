using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.FPS.UI.SkillTreeManager;
using Unity.FPS.Gameplay;
using Unity.FPS.Game;

namespace Unity.FPS.UI
{
    public class SkillManager : MonoBehaviour
    {
        public int SkillId;

        public TMP_Text TitleText;
        public TMP_Text DescriptionText;

        public int[] ConnectedSkills;

        public LevelSystem LevelSystem;

        private void Update()
        {
            GetComponent<Image>().color = SkillTree.SkillLevels[SkillId] >= SkillTree.SkillCaps[SkillId] ? Color.green
                : LevelSystem.SkillPoints >= 1 ? Color.yellow : Color.white;
        }

        public void UpdateUI()
        {
            TitleText.text = $"{SkillTree.SkillLevels[SkillId]}/{SkillTree.SkillCaps[SkillId]}\n{SkillTree.SkillNames[SkillId]}";
            DescriptionText.text = $"{SkillTree.SkillDescription[SkillId]}\nCost: {LevelSystem.SkillPoints}/1SP";
            Debug.Log(LevelSystem.SkillPoints);

            foreach (var connectedSkill in ConnectedSkills)
            {
                SkillTree.SkillList[connectedSkill].gameObject.SetActive(SkillTree.SkillLevels[SkillId] > 0);
                SkillTree.ConnectorList[connectedSkill].SetActive(SkillTree.SkillLevels[SkillId] > 0);
            }
        }

        public void Buy()
        {
            if (LevelSystem.SkillPoints < 1 || SkillTree.SkillLevels[SkillId] >= SkillTree.SkillCaps[SkillId])
            {
                Debug.Log("Not enough skill points");
                return;
            }

            LevelSystem.SkillPoints -= 1;
            SkillTree.SkillLevels[SkillId]++;      

            SkillTree.UpdateAllSkillUI();
        }
    }
}