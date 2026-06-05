using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TechnologyRewardUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI techNameText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI abilitiesText;
    public TextMeshProUGUI passivesText;

    public Transform robotButtonParent;
    public GameObject robotButtonPrefab;

    [Header("Robot Display")]
    public GameObject robotDisplayPanel;
    public Image robotImage;
    public TextMeshProUGUI robotStatsText;
    public TextMeshProUGUI robotTechListText;

    public Button mergeButton;

    private TechnologyManager techManager;
    private Technology generatedTech;
    private RobotInstance selectedRobot;
    private List<Button> robotButtons = new List<Button>();

    [Header("Navigation")]
    public Button backButton;

    [Header("Robot Paging")]
    public Button leftArrowButton;
    public Button rightArrowButton;

    private List<RobotInstance> robots;
    private int robotStartIndex = 0;
    private const int visibleRobotCount = 2;

    void Start()
    {
        techManager = FindFirstObjectByType<TechnologyManager>();
        robots = PlayerManager.Instance.GetRobots(); // store once
        GenerateTech();
        PopulateRobotButtons();
        mergeButton.onClick.AddListener(OnMergePressed);
        backButton.onClick.AddListener(ReturnToMap);
        leftArrowButton.onClick.AddListener(() => Rotate(-1));
        rightArrowButton.onClick.AddListener(() => Rotate(1));
        robotDisplayPanel.SetActive(false);
    }

    void GenerateTech()
    {
        // Create a tech with value 100
        var creator = FindFirstObjectByType<TechnologyCreator>();
        generatedTech = creator.CreateTechnology(100);

        // Register tech so it gets an ID and is stored
        generatedTech = techManager.RegisterTech(generatedTech);

        // Display name
        techNameText.text = generatedTech.techName;

        // Stats
        statsText.text = "";
        string[] statNames = { "Health", "Damage", "Ability Damage", "Attack Speed", "Cast Speed", "Ability Count", "Luck" };
        for (int i = 0; i < generatedTech.stats.Length; i++)
            statsText.text += $"{statNames[i]}: {generatedTech.stats[i]}\n";

        // Abilities
        abilitiesText.text = "Abilities:\n";
        foreach (int id in generatedTech.abilityIDs)
        {
            var ability = AbilityDatabase.GetAbility(id);
            abilitiesText.text += $"{ability.abilityName}\n";
        }

        // Passives
        passivesText.text = "Passives:\n";
        foreach (int id in generatedTech.passiveIDs)
        {
            var passive = PassiveDatabase.GetPassive(id);
            passivesText.text += $"{passive.passiveName}\n";
        }
    }

    void PopulateRobotButtons()
    {
        foreach (var btn in robotButtons)
        {
            if (btn != null)
                Destroy(btn.gameObject);
        }
        robotButtons.Clear();

        if (robots == null || robots.Count == 0)
        return;

        int countToShow = Mathf.Min(visibleRobotCount, robots.Count);

        for (int i = 0; i < countToShow; i++)
        {
            int index = (robotStartIndex + i) % robots.Count;
            RobotInstance robot = robots[index];

            GameObject btnObj = Instantiate(robotButtonPrefab, robotButtonParent);
            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = robot.data.robotName;
            robotButtons.Add(btn);

            btn.onClick.AddListener(() =>
            {
                selectedRobot = robot;
                HighlightSelectedButton(btn);
                DisplayRobotInfo(robot);
            });
        }

        // Show arrows only if needed
        bool showArrows = robots.Count > visibleRobotCount;
        leftArrowButton.gameObject.SetActive(showArrows);
        rightArrowButton.gameObject.SetActive(showArrows);
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

        DisplayRobotInfo(selectedRobot); // update display to show new stats and tech list

        // 🔒 Prevent merging again
        generatedTech = null;
        mergeButton.interactable = false;

        PlayerManager.Instance.SaveGameAfterReward();
    }

    void HighlightSelectedButton(Button selected)
    {
        foreach (var btn in robotButtons)
        {
            var img = btn.GetComponent<Image>();
            var outline = btn.GetComponent<Outline>();

            if (img != null)
                img.color = Color.white;

            if (outline != null)
                outline.enabled = false;
        }

        var selectedImg = selected.GetComponent<Image>();
        var selectedOutline = selected.GetComponent<Outline>();

        if (selectedImg != null)
            selectedImg.color = new Color(0.8f, 0.8f, 1f);

        if (selectedOutline != null)
            selectedOutline.enabled = true;
    }

    void DisplayRobotInfo(RobotInstance robot)
    {
        robotDisplayPanel.SetActive(true);

        // Sprite from prefab
        robotImage.sprite = robot.data.prefab.GetComponent<SpriteRenderer>().sprite;
        robotImage.color = Color.white;
        robotImage.preserveAspect = true;
        robotImage.rectTransform.sizeDelta = new Vector2(100, 100); // smaller circle

        // Stats
        robotStatsText.text =
            $"Health: {robot.health}\n" +
            $"Damage: {robot.atkdamage}\n" +
            $"Ability Damage: {robot.abilitydamage}\n" +
            $"Attack Speed: {robot.atkspd}\n" +
            $"Cast Speed: {robot.castspd}\n" +
            $"Ability Count: {robot.abilityCount}\n" +
            $"Luck: {robot.luck}";

        // Technologies
        robotTechListText.text = "Technologies:\n";

        if (robot.technologyIDs.Count == 0)
        {
            robotTechListText.text += "- None\n";
        }
        else
        {
            foreach (int techId in robot.technologyIDs)
            {
                if (techManager.TryGetTech(techId, out Technology tech))
                    robotTechListText.text += $"- {tech.techName}\n";
            }
        }
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene");
    }

    void OnDestroy()
    {
        techManager.ClearAll();
    }

    void Rotate(int direction)
    {
        if (robots == null || robots.Count <= visibleRobotCount)
            return;

        int step;

        // If odd number of robots, move by 1
        if (robots.Count % 2 == 1)
            step = 1;
        else
            step = visibleRobotCount; // normally 2

        robotStartIndex = (robotStartIndex + direction * step + robots.Count) % robots.Count;

        PopulateRobotButtons();
    }

}