using UnityEngine;

public class TechController : MonoBehaviour
{
    public Robot robot;
    public TechnologyCreator techCreator;
    public TechnologyManager techManager;

    private Technology lastCreatedTech;

    public void OnCreateTechButton()
    {
        lastCreatedTech = techCreator.CreateTechnology(100);
        techManager.AddTech(lastCreatedTech);
        Debug.Log("Created technology!");
    }

    public void OnMergeTechButton()
    {
        if (lastCreatedTech == null)
        {
            Debug.LogWarning("No tech created yet.");
            return;
        }

        robot.MergeTech(lastCreatedTech);
        techManager.RemoveFirstTech();
        lastCreatedTech = null;

        Debug.Log("Merged tech into robot.");
    }

    public void OnDisplayRobotStatsButton()
    {
        robot.DisplayStats();
    }
}
