using UnityEngine;

public class TechController : MonoBehaviour // old script, used for early testing of tech creation and merging. Can be removed later.
{
    public Robot robot;                 // The MonoBehaviour
    public TechnologyCreator techCreator;
    public TechnologyManager techManager;

    private Technology lastCreatedTech;

    public void OnCreateTechButton()
    {
        lastCreatedTech = techCreator.CreateTechnology(100);
        //techManager.AddTech(lastCreatedTech);
        Debug.Log("Created technology!");
    }

    public void OnMergeTechButton()
    {
        if (lastCreatedTech == null)
        {
            Debug.LogWarning("No tech created yet.");
            return;
        }

        robot.instance.MergeTech(lastCreatedTech);

        //techManager.RemoveFirstTech();
        lastCreatedTech = null;

        Debug.Log("Merged tech into robot.");
    }

    public void OnDisplayRobotStatsButton()
    {
        robot.DisplayStats();
    }
}