using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static AshVectors;


public class AshTransformTranslator : MonoBehaviour
{

    public AshVector3 userReadablePosition;


    public AshVector3 userReadableEulerAngles; //DO NOT USE
    public AshVector3 eulerAngles_LastFrame;

    public AshQuat userReadableRotation;
    public AshVector3 userReadableScale = new AshVector3(1, 1, 1);
    //public AshVector3 oneFrameVelocity;


    public AshVector3 position_LastFrame;
    //public AshVector3 rotation_LastMoved;
    public bool overrideTransform;

    public Mesh baseMesh;




    // Update is called once per frame
    public virtual void ManualUpdate()
    {


        //current list of crappy coding practice:



        //planets (determinate):

        //eliminated -> //EulerToRight relies on base Quaternion * Euler operator implementation 
        //eliminated -> //dirtoeuler just uses internal code, since I could find NO SOURCES ONLINE about how to fix the issue of infinite theoretical "ups" for a direction that is only horizontal. you know what i mean
        //ashfloat ^ ashfloat uses internals, although all operators kinda do
        //camera moves using the built-in transform, which is because i'm NOT coding my own camera, and the transform i made only moves mesh vertices. I considered making a new class, but in the end it would just be copying custom transformational data to the built-in one 1:1, which shows little skill not worth my time.



        //planets (freebody):

        //eliminated -> //EulerToRight relies on base Quaternion * Euler operator implementation 
        //eliminated -> //dirtoeuler just uses internal code, since I could find NO SOURCES ONLINE about how to fix the issue of infinite theoretical "ups" for a direction that is only horizontal. you know what i mean
        //ashfloat ^ ashfloat uses internals, although all operators kinda do
        //camera moves using the built-in transform, which is because i'm NOT coding my own camera, and the transform i made only moves mesh vertices. I considered making a new class, but in the end it would just be copying custom transformational data to the built-in one 1:1, which shows little skill not worth my time.



        //ships:
        //eliminated -> //slerp is taken from the source code of unity's quaternion implementation, since the one i was taught went thoroughly wrong
        //eliminated -> //dirtoeuler just uses internal code, since I could find NO SOURCES ONLINE about how to fix the issue of infinite theoretical "ups" for a direction that is only horizontal. you know what i mean
        //ashfloat ^ ashfloat uses internals, although all operators kinda do







        if (userReadableEulerAngles.x != eulerAngles_LastFrame.x || userReadableEulerAngles.y != eulerAngles_LastFrame.y || userReadableEulerAngles.z != eulerAngles_LastFrame.z)
        {
            userReadableRotation = AshMaths.EulerToQuat(userReadableEulerAngles);
        }

        eulerAngles_LastFrame = userReadableEulerAngles;



    }

    public void UpdateMeshes()
    {

        position_LastFrame = userReadablePosition;


        AshVector3 usingPosition = userReadablePosition;
        AshQuat usingRotation = userReadableRotation;
        AshVector3 usingScale = userReadableScale;


        //Debug.DrawRay(userReadablePosition.ReturnBase(), AshMaths.EulerToDir(AshMaths.QuatToEuler(userReadableRotation)).ReturnBase(), Color.green);
        //Debug.DrawRay(userReadablePosition.ReturnBase(), oneFrameVelocity.GetDirection().ReturnBase(), Color.yellow);


        if (!overrideTransform)
        {
            transform.localPosition = usingPosition.ReturnBase();
            transform.localRotation = usingRotation.ReturnBase();
            transform.localScale = usingScale.ReturnBase();
        } else
        {

            userReadablePosition += new AshVector3(transform.localPosition);
            transform.localPosition = Vector3.zero;
            userReadableEulerAngles += new AshVector3(transform.localEulerAngles);
            transform.localEulerAngles = Vector3.zero;
            //userReadableRotation = new AshQuat(userReadableRotation.x.value + transform.localRotation.x, userReadableRotation.y.value + transform.localRotation.y, userReadableRotation.z.value + transform.localRotation.z, userReadableRotation.w.value + transform.localRotation.w - 1);
            //transform.localRotation = Quaternion.identity;
            userReadableScale += new AshVector3(transform.localScale - Vector3.one);
            transform.localScale = Vector3.one;


            if (GetComponent<MeshFilter>())
            {
                Mesh transformedMesh = new Mesh();
                transformedMesh.SetVertices(TransformPositionArray(baseMesh.vertices, usingPosition, AshMaths.QuatToEuler(userReadableRotation), usingScale));
                transformedMesh.triangles = baseMesh.triangles;
                transformedMesh.uv = baseMesh.uv;
                transformedMesh.normals = baseMesh.normals;
                transformedMesh.RecalculateBounds();
                transformedMesh.RecalculateNormals();
                GetComponent<MeshFilter>().mesh = transformedMesh;
            }
        }

    }


    public Vector3[] TransformPositionArray(Vector3[] positionArray, AshVector3 position, AshVector3 rotation, AshVector3 scale)
    {

        rotation = AshMaths.DegreesToRadians(rotation);



        //Debug.Log(rotation.x.value.ToString() + " " + rotation.y.value.ToString() + " " + rotation.z.value.ToString());


        //desperate bypass attempt please fucking god
        /*if (rotation.x.value > (Mathf.PI * 0.5f))
        {
            Debug.Log("true");
        } 

        else
        {
            Debug.Log("fal;se");
        }*/



        //mysteries for another day...
        //get "spotmatrix" to represent correctly based off "userReadablePosition"
        //get "spinmatrix" to represent correctly based off "userReadableEulerAngles"
        //get "sizematrix" to represent correctly based off "userReadableScale"
        //get transformationMatrix to multiply by a position in order to return a transformed position



        AshMatrix translationMatrix = new AshMatrix(
            new AshVector3(1, 0, 0),
            new AshVector3(0, 1, 0),
            new AshVector3(0, 0, 1), 
            position
        );

        AshMatrix pitchMatrix = new AshMatrix(
            new AshVector3(1, 0, 0),
            new AshVector3(0, Mathf.Cos(rotation.x.value), Mathf.Sin(rotation.x.value)),
            new AshVector3(0, -Mathf.Sin(rotation.x.value), Mathf.Cos(rotation.x.value)),
            new AshVector3(0, 0, 0)
        );

        AshMatrix yawMatrix = new AshMatrix(
            new AshVector3(Mathf.Cos(rotation.y.value), 0, -Mathf.Sin(rotation.y.value)),
            new AshVector3(0, 1, 0),
            new AshVector3(Mathf.Sin(rotation.y.value), 0, Mathf.Cos(rotation.y.value)),
            new AshVector3(0, 0, 0)
        );

        AshMatrix rollMatrix = new AshMatrix(
            new AshVector3(Mathf.Cos(rotation.z.value), Mathf.Sin(rotation.z.value), 0),
            new AshVector3(-Mathf.Sin(rotation.z.value), Mathf.Cos(rotation.z.value), 0),
            new AshVector3(0, 0, 1),
            new AshVector3(0, 0, 0)
        );

        AshMatrix scaleMatrix = new AshMatrix(
            new AshVector3(scale.x, 0, 0),
            new AshVector3(0, scale.y, 0),
            new AshVector3(0, 0, scale.z),
            new AshVector3(0, 0, 0)
        );

        AshMatrix transformationMatrix = pitchMatrix * yawMatrix * rollMatrix * scaleMatrix;
        
        transformationMatrix *= translationMatrix;

        Vector3[] transformedArray = new Vector3[positionArray.Length];


        for (int i = 0; i < positionArray.Length; i++)
        {
            transformedArray[i] = (transformationMatrix * new AshVector3(positionArray[i])).ReturnBase();
        }


        return transformedArray;
    }
}
