using UnityEngine;

public class Billboarding : MonoBehaviour
{
    [SerializeField]
    bool isShadow;

    [SerializeField]
    Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShadow)
        {
            transform.forward = Vector3.Scale(target.forward, new Vector3(1, 0, 1));
        }
        else
        {
            transform.forward = target.forward;
        }
    }
}
