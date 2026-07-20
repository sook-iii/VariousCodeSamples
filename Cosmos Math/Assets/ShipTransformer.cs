using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AshVectors;


[ExecuteAlways]
public class ShipTransformer : AshTransformTranslator
{

    public float turnSpeed = 0.5f;
    public bool faceVelocity;
    public AshVector3 directionToFace;
    public AshVector3 lastHorizontalDirection;


    private void Awake()
    {
        directionToFace = new AshVector3(transform.forward);
    }

    private void Update()
    {
        ManualUpdate();
    }

    // Update is called once per frame
    public override void ManualUpdate()
    {

        base.ManualUpdate();


        if (faceVelocity)
        {


            if (userReadablePosition.x != position_LastFrame.x || userReadablePosition.y != position_LastFrame.y || userReadablePosition.z != position_LastFrame.z)
            {

                directionToFace = (userReadablePosition - position_LastFrame).GetDirection();

            }


            /*Quaternion storedRot = transform.localRotation; //CHEATS!!!
            transform.localRotation = userReadableRotation.ReturnBase();
            Vector3 storedUp = transform.up;
            transform.localRotation = storedRot;*/



            //AshVector3 directionToFace_Euler = AshMaths.DirToEuler(directionToFace, new AshVector3(storedUp));
            //AshQuat directionToFace_Quat = AshMaths.EulerToQuat(directionToFace_Euler);
            AshQuat directionToFace_Quat;

            if (directionToFace.x != 0 || directionToFace.z != 0)
            {
                directionToFace_Quat = AshMaths.EulerToQuat(AshMaths.DirToEuler(directionToFace, new AshVector3(0f, 1f, 0f)));
                lastHorizontalDirection = directionToFace;
            } else if (directionToFace.y > 0)
            {
                directionToFace_Quat = AshMaths.EulerToQuat(AshMaths.DirToEuler(directionToFace, new AshVector3(Vector3.zero) - lastHorizontalDirection));
            } else //if (directionToFace.y < 0)
            {
                directionToFace_Quat = AshMaths.EulerToQuat(AshMaths.DirToEuler(directionToFace, lastHorizontalDirection));
            }




            userReadableRotation = AshMaths.Slerp(userReadableRotation, directionToFace_Quat, Time.deltaTime * turnSpeed);
            //userReadableEulerAngles = AshMaths.QuatToEuler(userReadableRotation);



        }



        /*oneFrameVelocity = userReadablePosition - position_LastFrame;

       if (faceVelocity) { 
           if (oneFrameVelocity.GetMagnitude() > 0)
           {
               //userReadableEulerAngles = AshMaths.DirToEuler(oneFrameVelocity.GetDirection());

               //AshQuat temp = new AshQuat();
               //temp.EulerToQuat(AshMaths.DirToEuler(oneFrameVelocity.GetDirection()));
               //userReadableEulerAngles = temp.GetEuler();

               AshQuat temp1 = userReadableRotation;
               AshQuat temp2 = AshMaths.EulerToQuat(AshMaths.DirToEuler(oneFrameVelocity.GetDirection()));




               AshQuat proposedRotation = AshMaths.Slerp(temp1, temp2, turnSpeed * Time.deltaTime);

               userReadableRotation = proposedRotation;


               //if (!float.IsNaN(proposedRotation.x.value)) { userReadableEulerAngles = new AshVector3(proposedRotation.x, userReadableEulerAngles.y, userReadableEulerAngles.z); }
               //if (!float.IsNaN(proposedRotation.y.value)) { userReadableEulerAngles = new AshVector3(userReadableEulerAngles.x, proposedRotation.y, userReadableEulerAngles.z); }
               //if (!float.IsNaN(proposedRotation.z.value)) { userReadableEulerAngles = new AshVector3(userReadableEulerAngles.x, userReadableEulerAngles.y, proposedRotation.z); }


               //Debug.Log("((" + temp1.x.value + ", " + temp1.y.value + ", " + temp1.z.value + "),  " + temp1.w.value + "),   ((" + temp2.x.value + ", " + temp2.y.value + ", " + temp2.z.value + "),  " + temp2.w.value + ")");
               //Debug.Log("((" + AshMaths.Slerp(temp1, temp2, turnSpeed * Time.deltaTime).x.value + ", " + AshMaths.Slerp(temp1, temp2, turnSpeed * Time.deltaTime).y.value + ", " + AshMaths.Slerp(temp1, temp2, turnSpeed * Time.deltaTime).z.value + "),  " + AshMaths.Slerp(temp1, temp2, turnSpeed * Time.deltaTime).w.value + ")");
               //Debug.Log("((" + userReadableEulerAngles.x.value + ", " + userReadableEulerAngles.y.value + ", " + userReadableEulerAngles.z.value + ")");
           }
       }*/



        //userReadableEulerAngles = AshMaths.EulerToQuat(userReadableRotation);


        UpdateMeshes();

    }
}
