using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TechnologyRewardUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI techNameText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI abilitiesText;
    public TextMeshProUGUI passivesText;

    public Transform robotButtonParent;
    public GameObject robotButtonPrefab;

    public Button mergeButton;

    private Technology generatedTech;
    private RobotInstance selectedRobot;
    private List<Button> robotButtons = new List<Button>();

    void Start()
    {
        GenerateTech();
        PopulateRobotButtons();
        mergeButton.onClick.AddListener(OnMergePressed);
    }

    void GenerateTech()
    {
        // Create a tech with value 100
        generatedTech = FindFirstObjectByType<TechnologyCreator>().CreateTechnology(100);

        techNameText.text = generatedTech.techName;

        // Stats
        statsText.text = "";
        string[] statNames = { "Health", "Damage", "Ability Damage", "Attack Speed", "Cast Speed", "Ability Count", "Luck" };
        for (int i = 0; i < generatedTech.stats.Length; i++)
            statsText.text += $"{statNames[i]}: {generatedTech.stats[i]}\n";

        // Abilities
        abilitiesText.text = "";
        foreach (int id in generatedTech.abilityIDs)
        {
            var ability = AbilityDatabase.GetAbility(id);
            abilitiesText.text += $"{ability.abilityName}\n";
        }

        // Passives
        passivesText.text = "";
        foreach (int id in generatedTech.passiveIDs)
        {
            var passive = PassiveDatabase.GetPassive(id);
            passivesText.text += $"{passive.passiveName}\n";
        }
    }

    void PopulateRobotButtons()
    {
        var robots = PlayerManager.Instance.GetRobots();

        foreach (var robot in robots)
        {
            GameObject btnObj = Instantiate(robotButtonPrefab, robotButtonParent);
            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = robot.data.robotName;

            robotButtons.Add(btn);

            btn.onClick.AddListener(() =>
            {
                selectedRobot = robot;
                HighlightSelectedButton(btn);
                Debug.Log($"Selected robot: {robot.data.robotName}");
            });
        }
    }

    void OnMergePressed()
    {
        if (selectedRobot == null)
        {
            Debug.LogWarning("No robot selected!");
            return;
        }

        selectedRobot.MergeTech(generatedTech);
        Debug.Log($"Merged {generatedTech.techName} into {selectedRobot.data.robotName}");
    }

    public void OnContinue()
    {
        //SceneManager.LoadScene("MapScene");
    }

    void HighlightSelectedButton(Button selected)
    {
        foreach (var btn in robotButtons)
        {
            // Reset all buttons
            var img = btn.GetComponent<Image>();
            var outline = btn.GetComponent<Outline>();

            if (img != null)
                img.color = Color.white;

            if (outline != null)
                outline.enabled = false;
        }

        // Highlight selected button
        var selectedImg = selected.GetComponent<Image>();
        var selectedOutline = selected.GetComponent<Outline>();

        if (selectedImg != null)
            selectedImg.color = new Color(0.8f, 0.8f, 1f); // light blue highlight

        if (selectedOutline != null)
            selectedOutline.enabled = true;
    }
}