using UnityEngine;

public class Billboarding : MonoBehaviour
{
    [SerializeField]
    bool isShadow;

    [SerializeField]
    bool lockY; // makes trees only rotate left and right 

    [SerializeField]
    Transform target; // used for camera attachment 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // assign camera to object dynamically so you dont have to assign each one
        if (target == null && Camera.main != null)
        {
            target = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if asset is a shadow
        if (isShadow)
        {
            transform.forward = Vector3.Scale(target.forward, new Vector3(1, 0, 1));

        }
        else
        {
            Vector3 lookDir = target.forward;

            if (lockY)
            {

                lookDir.y = 0;
            }

            transform.forward = lookDir;
        }
    }
}
