using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static AshVectors;


public class Organizer : MonoBehaviour
{
    

    public static Organizer Orrery;

    public List<PlanetTransformer> planetsInOrrery;



    private AshFloat PFPS_accumulator = 0;
    public AshFloat PFPS_target = 60;

    public AshFloat gravitationalConstant = 6.67f;
    public AshFloat gravitationalExponent = 2;

    public bool freebodyPhysics;

    public AshFloat galacticTime;
    public PlanetTransformer timeReference;
    public AshFloat timeMultiplier = 1;

    public List<string> fillerText;
    public TextMeshProUGUI timeTextBox;

    public Camera satelliteCamera;
    public Light satellight;
    public AshVector3 satelliteOffset;

    public bool depatch_outOfScope_bugs;
    public bool simplyBe;


    private void Awake()
    {
        if (Orrery == null)
        {
            Orrery = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (!simplyBe) {  
            PFPS_accumulator += Time.deltaTime;

            while (PFPS_accumulator >= 1f / PFPS_target)
            {

                PFPS_accumulator -= 1f / PFPS_target;
                PhysicsUpdate(timeMultiplier);

            }

            //reference planet name
            //year
            //day
            //years/sec
            //days/sec
            //physics/sec
            //orbitstotrace

            string yearsectext;
            string daysectext;

            if (timeReference.hoursToOrbit < Mathf.Infinity)
            {
                yearsectext = ((timeMultiplier * PFPS_target) / timeReference.hoursToOrbit).value.ToString();
            }

            else
            {
                yearsectext = "N/A";
            }

            if (timeReference.hoursToSpin < Mathf.Infinity)
            {
                daysectext = ((timeMultiplier * PFPS_target) / timeReference.hoursToSpin).value.ToString();
            } 
        
            else
            {
                daysectext = "N/A";
            }

            timeTextBox.text = (
                fillerText[0] + 
                timeReference.name //planet name
                + fillerText[1] + 
                (galacticTime / timeReference.hoursToOrbit).value.ToString() //year
                + fillerText[2] + 
                (galacticTime / timeReference.hoursToSpin).value.ToString() //day
                + fillerText[3] +
                yearsectext //years/sec
                + fillerText[4] +
                daysectext //days/sec
                + fillerText[5] +
                (PFPS_target).value.ToString() //physics/sec
                + fillerText[6] +
                timeReference.orbitDisplayMultiplier.value.ToString() //orbitstotrace
                + fillerText[7]
            );

            for (int i = 0; i < planetsInOrrery.Count; i++)
            {
                planetsInOrrery[i].ManualUpdate();
            }



            satelliteCamera.transform.position = (timeReference.userReadablePosition + new AshVector3(0, satelliteOffset.y, 0)).ReturnBase();

            if (timeReference.manuallyTargetedPlanet != null)
            {

                AshVector3 directionToOrbitTarget = (timeReference.manuallyTargetedPlanet.userReadablePosition - timeReference.userReadablePosition).GetDirection();
                satelliteCamera.transform.eulerAngles = (AshMaths.DirToEuler(directionToOrbitTarget, new AshVector3(0, 1, 0))).ReturnBase();
                satelliteCamera.transform.position += (directionToOrbitTarget * satelliteOffset.x).ReturnBase();

            }
        
            else
            {

                AshVector3 eulerToFaceTarget = new AshVector3(timeReference.userReadableEulerAngles.x, timeReference.userReadableEulerAngles.y, timeReference.userReadableEulerAngles.z);
                satelliteCamera.transform.eulerAngles = eulerToFaceTarget.ReturnBase();
                //satelliteCamera.transform.position += (AshMaths.EulerToDir(eulerToFaceTarget) * satelliteOffset.x).ReturnBase();

            }

            satellight.transform.position = satelliteCamera.transform.position;
            satellight.transform.eulerAngles = new Vector3(satellight.transform.eulerAngles.x, satelliteCamera.transform.eulerAngles.y, satellight.transform.eulerAngles.z);
        }
    }

    public void PhysicsUpdate(AshFloat intervalMultiplier)
    {
        galacticTime += intervalMultiplier;

        for (int i = 0; i < planetsInOrrery.Count; i++)
        {
            planetsInOrrery[i].PhysicsUpdate(intervalMultiplier);
        }

    }

    //would combine these into one but unity buttons can only take one argument at a time?? silly
    public void MatchYearButtonPressed(float newInterval)
    {
        if (timeReference.hoursToOrbit < Mathf.Infinity) 
        {  
            timeMultiplier = newInterval * timeReference.hoursToOrbit / PFPS_target;
        }

        foreach (PlanetTransformer planet in planetsInOrrery)
        {
            planet.previousPositions.Clear();
        }
    }

    public void MatchDayButtonPressed(float newInterval)
    {
        if (timeReference.hoursToSpin < Mathf.Infinity)
        {
            timeMultiplier = newInterval * timeReference.hoursToSpin / PFPS_target;
        }

        foreach (PlanetTransformer planet in planetsInOrrery)
        {
            planet.previousPositions.Clear();
        }
    }

    public void PhysicsTargetButtonPressed(float newTarget)
    {
        AshFloat storedFPS = PFPS_target;
        PFPS_target = newTarget;
        timeMultiplier = timeMultiplier * (storedFPS / newTarget);

        foreach (PlanetTransformer planet in planetsInOrrery)
        {
            planet.previousPositions.Clear();
        }

    }

    public void OrbitTraceButtonPressed(float newTarget)
    {

        foreach (PlanetTransformer planet in planetsInOrrery.Where(planet => planet.gameObject.name == timeReference.name))
        {

            planet.orbitDisplayMultiplier = newTarget;

        }

    }

    public void ChangeRefernecePlanet(PlanetTransformer newPlanet)
    {
        timeReference = newPlanet;
    }

}
