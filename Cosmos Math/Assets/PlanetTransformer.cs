using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AshVectors;



public class PlanetTransformer : AshTransformTranslator
{



    public AshFloat mass;

    public float approximateSize;

    public AshFloat hoursToOrbit;
    public AshFloat hoursToSpin;

    public AshVector3 linearVelocity;
    public AshVector3 angularVelocity;

    public AshInt previousPositionsToStore;

    public Color representativeColour = Color.white;

    public Color rightColour = Color.red;
    public Color upColour = Color.green;
    public Color forwardColour = Color.blue;

    public PlanetTransformer manuallyTargetedPlanet;

    private AshFloat distanceFromTargetedPlanet;
    private AshFloat initialOffsetFromTargetedPlanet;
    private AshVector3 position_LastPhysicsFrame;

    public AshFloat orbitPercentage;

    public List<AshVector3> previousPositions;

    public AshFloat orbitDisplayMultiplier = 0.9f;

    private void Start()
    {
        if (manuallyTargetedPlanet != null)
        {
            distanceFromTargetedPlanet = (manuallyTargetedPlanet.userReadablePosition - userReadablePosition).GetMagnitude();

            initialOffsetFromTargetedPlanet = 180 + AshMaths.DirToEuler(manuallyTargetedPlanet.userReadablePosition - userReadablePosition, new AshVector3(0, 1, 0)).y;

        }
    }


    // Update is called once per frame
    public override void ManualUpdate()
    {

        base.ManualUpdate();


        //now handled by organizer central clock

        /*PFPS_accumulator += Time.deltaTime;
        
        while (PFPS_accumulator >= 1f / Organizer.Orrery.PFPS_target)
        {
            PFPS_accumulator -= 1f / Organizer.Orrery.PFPS_target;

            PhysicsUpdate();

        }*/


        Debug.DrawLine(userReadablePosition.ReturnBase(), (userReadablePosition + (AshMaths.EulerToRight(userReadableEulerAngles)) * approximateSize).ReturnBase(), rightColour, 0f);
        Debug.DrawLine(userReadablePosition.ReturnBase(), (userReadablePosition + (AshMaths.EulerToUp(userReadableEulerAngles)) * approximateSize).ReturnBase(), upColour, 0f);
        Debug.DrawLine(userReadablePosition.ReturnBase(), (userReadablePosition + (AshMaths.EulerToDir(userReadableEulerAngles)) * approximateSize).ReturnBase(), forwardColour, 0f);
        
        for (int i = 0; i < previousPositions.Count - 1; i++)
        {
            Debug.DrawLine(previousPositions[i].ReturnBase(), previousPositions[i + 1].ReturnBase(), representativeColour, 0);
        }

        UpdateMeshes();

    }

    public void PhysicsUpdate(AshFloat intervalMultiplier)
    {

        //angular velocity
        //AshVector3 eulerStorage = AshMaths.QuatToEuler(userReadableRotation);
        //eulerStorage += angularVelocity;
        //userReadableRotation = AshMaths.EulerToQuat(eulerStorage);
        
        userReadableEulerAngles += angularVelocity.GetDirection() * (360f * intervalMultiplier / hoursToSpin);
        userReadableRotation = AshMaths.EulerToQuat(userReadableEulerAngles);



        //linear velocity and orbits


        if (Organizer.Orrery.freebodyPhysics)
        {

            foreach (PlanetTransformer foundPlanet in Organizer.Orrery.planetsInOrrery)
            {

                AshVector3 distance = foundPlanet.userReadablePosition - userReadablePosition;

                if ((distance.GetMagnitude() ^ Organizer.Orrery.gravitationalExponent) * mass >= 1)
                {
                    AshVector3 distanceNormalised = distance.GetDirection();
                    AshFloat positiveMultipliers = Organizer.Orrery.gravitationalConstant * foundPlanet.mass;
                    AshFloat negativeMultipliers = (distance.GetMagnitude() ^ Organizer.Orrery.gravitationalExponent) * mass;

                    linearVelocity += distanceNormalised * (positiveMultipliers / negativeMultipliers);

                    //Debug.Log(linearVelocity.x.value.ToString() + " " + linearVelocity.y.value.ToString() + " " + linearVelocity.z.value.ToString());
                }
            }

        }

        else
        {

            if (manuallyTargetedPlanet != null)
            {

                orbitPercentage += intervalMultiplier / hoursToOrbit;

                AshVector3 targetPosition = manuallyTargetedPlanet.userReadablePosition + (AshMaths.EulerToDir(new AshVector3(0, initialOffsetFromTargetedPlanet + orbitPercentage * 360f, 0)) * distanceFromTargetedPlanet);

                linearVelocity = targetPosition - userReadablePosition;







            }

        }


        userReadablePosition += linearVelocity;

        previousPositions.Add(userReadablePosition);

        AshFloat totalTester = (orbitDisplayMultiplier * hoursToOrbit);

        if (totalTester > 0 && totalTester < Mathf.Infinity) 
        {  
            while (previousPositions.Count > (AshInt)(totalTester / intervalMultiplier))
            {
                previousPositions.RemoveAt(0);
            }
        }

    }
}
