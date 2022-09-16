using UnityEngine;
using UnityEngine.Serialization;

public class CrocodileHead : MonoBehaviour
{
    [FormerlySerializedAs("selfLA")] //เมื่อมีการแก้ชื่อตัวแปรในภายหลัง คำสั่งนี้มีไว้กันไม่ให้การตั้งค่าถูก Reset
    public Animator selfAnimator;

    private void OnCollisionEnter(Collision collision)
    {
        selfAnimator.ResetTrigger("OpenMouth");
        selfAnimator.SetTrigger("OpenMouth");
    }
}
