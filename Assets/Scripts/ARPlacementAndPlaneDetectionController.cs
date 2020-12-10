using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


// with this script we easily can get access to 
// AR Plane Manager and AR Placement Manager Scripts
public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public TextMeshProUGUI informUIPanelText;
    public GameObject scaleSlider;
    public GameObject battleArenaGameObject;

    private ARPlaneManager _arPlaneManager;
    private ARPlacementManager _arPlacementManager;

    private void Awake()
    {
        // access to ARPlaneManager to disable/enable plane detection
        // adn activate or deactivate existing planes
        _arPlaneManager = GetComponent<ARPlaneManager>();
        // access to ARPlacementManager to stop or continuing Raycasting from center of the screen
        _arPlacementManager = GetComponent<ARPlacementManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        // placeWithoutArButton.SetActive(true);
        scaleSlider.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanelText.text = "Move phone to detect planes and place the Battle Arena!";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickDisableARPlacementAndPlaneDetection()
    {
        _arPlaneManager.enabled = false;
        _arPlacementManager.enabled = false;
        
        // hide all existing planes
        SetAllPlanesActive(false);
        
        placeButton.SetActive(false);
        scaleSlider.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);

        // battleArenaGameObject.transform.position = new Vector3(0, -0.45F, 1.5F);
        informUIPanelText.text = "Great! You placed ARENA... Now, search games to BATTLE!";
    }

    public void OnClickEnableARPlacementAndPlaneDetection()
    {
        _arPlaneManager.enabled = true;
        _arPlacementManager.enabled = true;

        // show all existing planes
        SetAllPlanesActive(true);

        placeButton.SetActive(true);
        scaleSlider.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanelText.text = "Move phone to detect planes and place the Battle Arena!";
    }

    #region PRIVATE methods

    private void SetAllPlanesActive(bool isActive)
    {
        // access to all detected planes
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(isActive);
        }
    }

    #endregion
}