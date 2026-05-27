using UnityEngine;

public class RobotSelectionManager : MonoBehaviour
{
    // WILL BE USED TO DECIDE WHICH ROBOT IS SELECTED FOR TECH MERGING, THATS IT
    
    public Robot currentRobot { get; private set; }

    public void SelectRobot(Robot robot)
    {
        currentRobot = robot;
    }

}
