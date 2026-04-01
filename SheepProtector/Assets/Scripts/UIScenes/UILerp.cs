using UnityEngine;

public class UILerp : MonoBehaviour
{
    private Vector3 obj;
    [SerializeField] Vector3 target;
    [SerializeField] int interpolationFrames;

    public void SlideLerp()
    {
        obj = this.transform.position;

        
    }
}
