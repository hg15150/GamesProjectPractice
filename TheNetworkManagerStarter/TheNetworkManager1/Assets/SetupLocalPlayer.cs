using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour {

    public Text namePrefab;
    public Text nameLabel;
    public Transform namePos;
    string textBoxName = "";

    [SyncVar (hook = "OnChangeName")]
    public string pName = "player";



	public override void OnStartServer()
	{
		Debug.Log("Starting Server"  + isLocalPlayer);
	}


	public override void OnStartClient()
	{
        Debug.Log("Starting Client" + isLocalPlayer);
        base.OnStartClient();
        Invoke("UpdateStates", 1);
	}

    void UpdateStates()
    {
        OnChangeName(pName);
    }

	void Awake()
	{
		Debug.Log("Awaking Player " + isLocalPlayer);
	}

	// Use this for initialization
	void Start () 
	{
		if(isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
            CameraFollow360.player = this.gameObject.transform;
		}
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}
		Debug.Log("Starting Player " + isLocalPlayer);

        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as Text;
        nameLabel.transform.SetParent(canvas.transform);
	}

    void Update()
    {
        //determine if the object is inside the camera's viewing volume
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
                   screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //if it is on screen draw its label attached to is name position
        if (onScreen)
        {
            Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(namePos.position);
            nameLabel.transform.position = nameLabelPos;
        }
        else //otherwise draw it WAY off the screen 
            nameLabel.transform.position = new Vector3(-1000, -1000, 0);
    }

    public void OnDestroy()
    {
        if (nameLabel != null) Destroy(nameLabel.gameObject);
    }


    [Command]
    public void CmdChangeName(string newName)
    {
        pName = newName;
        nameLabel.text = pName;
    }


    void OnChangeName(string newName)
    {
        pName = newName;
        nameLabel.text = pName;
    }


    void OnGUI()
    {
        if (isLocalPlayer)
        {
            textBoxName = GUI.TextField(new Rect(25, 25, 100, 25), textBoxName);
            if (GUI.Button(new Rect(130, 15, 35, 25), "Set")) CmdChangeName(textBoxName); 
        }    
    }


}
