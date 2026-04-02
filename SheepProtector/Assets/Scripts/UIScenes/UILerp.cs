using UnityEngine;
using Unity.Mathematics;

public class UILerp : MonoBehaviour
{
    private Vector3 obj;
    [SerializeField] Vector3 target;
    [SerializeField] int duration;
    int interpolationFrames = 100;

    public void SlideOut()
    {
        float interpolationRatio = (float)duration / interpolationFrames;

        obj = this.transform.position;

        Vector3.Lerp(obj, target, interpolationRatio);
    }

    public void SlideIn()
    {
        float interpolationRatio = (float)duration / interpolationFrames;

        obj = this.transform.position;

        Vector3.Lerp(obj, target, interpolationRatio);
    }
}
