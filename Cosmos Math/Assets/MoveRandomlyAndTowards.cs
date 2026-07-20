using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomlyAndTowards : MonoBehaviour
{
    public GameObject movementTarget;
    public bool inheritKeyboard;
    public int moveTowardsMode = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AshVectors.AshVector3 randomDir = new AshVectors.AshVector3();

        if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.G))
        {
            moveTowardsMode = 0;
        }

        else if (Input.GetKeyDown(KeyCode.T)) {
            moveTowardsMode = 1;
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            moveTowardsMode = -1;
        }


        if (inheritKeyboard)
        {

            if (Input.GetKey(KeyCode.W))
            {
                randomDir += new AshVectors.AshVector3(0, 0, 1) * new AshVectors.AshFloat(Time.deltaTime * 2);
            }

            if (Input.GetKey(KeyCode.S))
            {
                randomDir += new AshVectors.AshVector3(0, 0, -1) * new AshVectors.AshFloat(Time.deltaTime * 2);
            } 
            
            if (Input.GetKey(KeyCode.A))
            {
                randomDir += new AshVectors.AshVector3(-1, 0, 0) * new AshVectors.AshFloat(Time.deltaTime * 2);
            } 
            
            if (Input.GetKey(KeyCode.D))
            {
                randomDir += new AshVectors.AshVector3(1, 0, 0) * new AshVectors.AshFloat(Time.deltaTime * 2);
            } 
            
            if (Input.GetKey(KeyCode.Space))
            {
                randomDir += new AshVectors.AshVector3(0, 1, 0) * new AshVectors.AshFloat(Time.deltaTime * 2);
            } 
            
            if (Input.GetKey(KeyCode.LeftControl))
            {
                randomDir += new AshVectors.AshVector3(0, -1, 0) * new AshVectors.AshFloat(Time.deltaTime * 2);
            }

        }

        else { 
        
            AshVectors.AshFloat dotProduct = AshVectors.AshMaths.DotProduct(movementTarget.GetComponent<ShipTransformer>().directionToFace, (movementTarget.GetComponent<ShipTransformer>().userReadablePosition - GetComponent<ShipTransformer>().userReadablePosition));

            if (moveTowardsMode == 1 && movementTarget)
            {
                if (dotProduct > 0) 
                {              

                    randomDir += (movementTarget.GetComponent<ShipTransformer>().userReadablePosition - GetComponent<ShipTransformer>().userReadablePosition).GetDirection() * new AshVectors.AshFloat(Time.deltaTime * 2 * dotProduct.value);
                }
            }

            else if (moveTowardsMode == -1 && movementTarget)
            {
                if (dotProduct < 0)
                {
                    randomDir += (movementTarget.GetComponent<ShipTransformer>().userReadablePosition - GetComponent<ShipTransformer>().userReadablePosition).GetDirection() * new AshVectors.AshFloat(Time.deltaTime * -2 * -dotProduct.value);
                }
            }

            else if(moveTowardsMode == 0 && movementTarget)
            {
                randomDir += new AshVectors.AshVector3(new AshVectors.AshFloat(Random.Range(-10f, 10f)), new AshVectors.AshFloat(Random.Range(-10f, 10f)), new AshVectors.AshFloat(Random.Range(-10f, 10f))) * new AshVectors.AshFloat(Time.deltaTime);
            }

        }

        GetComponent<ShipTransformer>().userReadablePosition += randomDir;


    }
}
