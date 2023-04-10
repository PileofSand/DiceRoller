using UnityEngine;

public class MVCBehaviour : MonoBehaviour
{
    private Application app;

    protected Application App
    {
        get { return app ?? (app = FindObjectOfType<Application>()); }
    }
}