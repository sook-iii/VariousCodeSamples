

using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AshVectors
{


    [Serializable]
    public struct AshFloat //vector one
    {
        
        //passVector

        public float value;

        public AshFloat(float passValue)
        {
            value = passValue;
        }

        //basic maths

        public static AshFloat operator +(AshFloat left, AshFloat right)
        {

            return (new AshFloat(left.value + right.value));

        }

        public static AshFloat operator -(AshFloat left, AshFloat right)
        {

            return (new AshFloat(left.value - right.value));

        }

        public static AshFloat operator *(AshFloat left, AshFloat right)
        {

            return (new AshFloat(left.value * right.value));

        }

        public static AshFloat operator /(AshFloat left, AshFloat right)
        {

            return (new AshFloat(left.value / right.value));

        }

        public static AshFloat operator ^(AshFloat passBase, AshFloat passExponent)
        {

            /*if (passExponent.value.) {  
                AshFloat runningTotal = 1;

                for (AshInt i = 0; i < passExponent; i++)
                {
                    runningTotal *= passBase;
                }

                return runningTotal;
            }*/

            return new AshFloat(Mathf.Pow(passBase.value, passExponent.value));

        }

        //runners

        public static AshFloat operator ++(AshFloat left)
        {

            return (new AshFloat(left.value + 1));

        }

        public static AshFloat operator --(AshFloat left)
        {

            return (new AshFloat(left.value - 1));

        }

        //comparators

        public static bool operator ==(AshFloat left, AshFloat right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(AshFloat left, AshFloat right)
        {
            return left.value != right.value;
        }

        public static bool operator <(AshFloat left, AshFloat right)
        {
            return left.value < right.value;
        }

        public static bool operator >(AshFloat left, AshFloat right)
        {
            return left.value > right.value;
        }

        public static bool operator <=(AshFloat left, AshFloat right)
        {
            return left.value <= right.value;
        }

        public static bool operator >=(AshFloat left, AshFloat right)
        {
            return left.value >= right.value;
        }

        //conversions

        public static implicit operator AshFloat(float v)
        {
            return new AshFloat(v);
        }

        public static implicit operator AshFloat(AshInt v)
        {
            return new AshFloat(v.value);
        }

        public override bool Equals(object obj)
        {
            return obj is AshFloat @float &&
                   value == @float.value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value);
        }
    }




    [Serializable]
    public struct AshInt //vector one 
    {

        //passVector

        public int value;

        public AshInt(int left)
        {
            value = left;
        }

        //basic maths

        public static AshInt operator +(AshInt left, AshInt right)
        {

            return (new AshInt(left.value + right.value));

        }

        public static AshInt operator -(AshInt left, AshInt right)
        {

            return (new AshInt(left.value - right.value));

        }

        public static AshInt operator *(AshInt left, AshInt right)
        {

            return (new AshInt(left.value * right.value));

        }

        public static AshInt operator /(AshInt left, AshInt right)
        {

            return (new AshInt(left.value / right.value));

        }

        public static AshInt operator ^(AshInt passBase, AshInt passExponent)
        {

            AshInt runningTotal = 1;

            for (AshInt i = 0; i < passExponent; i++)
            {
                runningTotal *= passBase;
            }

            return runningTotal;

        }

        //runners

        public static AshInt operator ++(AshInt left)
        {

            return (new AshInt(left.value + 1));

        }
        
        public static AshInt operator --(AshInt left)
        {

            return (new AshInt(left.value - 1));

        }

        //comparators

        public static bool operator ==(AshInt left, AshInt right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(AshInt left, AshInt right)
        {
            return left.value != right.value;
        }

        public static bool operator <(AshInt left, AshInt right)
        {
            return left.value < right.value;
        }

        public static bool operator >(AshInt left, AshInt right)
        {
            return left.value > right.value;
        }

        public static bool operator <=(AshInt left, AshInt right)
        {
            return left.value <= right.value;
        }

        public static bool operator >=(AshInt left, AshInt right)
        {
            return left.value >= right.value;
        }

        //conversions

        public static implicit operator AshInt(int v)
        {
            return new AshInt(v);
        }

        public static implicit operator AshInt(AshFloat v)
        {
            return new AshInt(Mathf.RoundToInt(v.value));
        }

        public override bool Equals(object obj) //i believe visual studio generated this function for me. I never needed it, don't recognise it, nor did I copy it from anywhere.
        {
            return obj is AshInt @int &&
                   value == @int.value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value);
        }
    }





    /*public struct AshVector2
    {

        //passVector

        public AshFloat x;
        public AshFloat y;

        public AshVector2(AshFloat passValueX, AshFloat passValueY)
        {
            x = passValueX;
            y = passValueY;
        }

        //basic maths

        public static AshVector2 operator +(AshVector2 left, AshVector2 right) {        
            
            return (new AshVector2(left.x + right.x, left.y + right.y));
            
        }

        public static AshVector2 operator -(AshVector2 left, AshVector2 right)
        {

            return (new AshVector2(left.x - right.x, left.y - right.y));

        }

        public static AshVector2 operator *(AshVector2 left, AshVector2 right)
        {

            return (new AshVector2(left.x * right.x, left.y * right.y));

        }

        public static AshVector2 operator /(AshVector2 left, AshVector2 right)
        {

            return (new AshVector2(left.x / right.x, left.y / right.y));

        }

        //vector-specific

        public AshFloat GetMagnitude()
        {
            return ((x ^ new AshFloat(2)) + (y ^ new AshFloat(2))) ^ new AshFloat(0.5f); //bugged unless i either casted or used an "explicit" operator, which made every line error. for another day.
        }

        public AshVector2 GetDirection()
        {
            return new AshVector2(x / GetMagnitude(), y / GetMagnitude());
        }
    }*/




    [Serializable]
    public struct AshVector3
    {

        //passVector

        public AshFloat x;
        public AshFloat y;
        public AshFloat z;

        public AshVector3(AshFloat passValueX, AshFloat passValueY, AshFloat passValueZ)
        {
            x = passValueX;
            y = passValueY;
            z = passValueZ;
        }

        public AshVector3(Vector3 passVector)
        {
            x = passVector.x;
            y = passVector.y;
            z = passVector.z;
        }

        public Vector3 ReturnBase()
        {
            return new Vector3(x.value, y.value, z.value);
        }

        //basic maths

        public static AshVector3 operator +(AshVector3 left, AshVector3 right)
        {

            return (new AshVector3(left.x + right.x, left.y + right.y, left.z + right.z));

        }

        public static AshVector3 operator -(AshVector3 left, AshVector3 right)
        {

            return (new AshVector3(left.x - right.x, left.y - right.y, left.z - right.z));

        }

        public static AshVector3 operator *(AshVector3 left, AshVector3 right)
        {

            return (new AshVector3(left.x * right.x, left.y * right.y, left.z * right.z));

        }

        public static AshVector3 operator *(AshVector3 passValue, AshFloat mag)
        {

            return (new AshVector3(passValue.x * mag, passValue.y * mag, passValue.z * mag));

        }

        public static AshVector3 operator /(AshVector3 left, AshVector3 right)
        {

            return (new AshVector3(left.x / right.x, left.y / right.y, left.z / right.z));

        }

        public static AshVector3 operator /(AshVector3 passValue, AshFloat mag)
        {

            return (new AshVector3(passValue.x / mag, passValue.y / mag, passValue.z / mag));

        }

        //vector-specific

        public AshFloat GetMagnitude()
        {
            return ((x ^ new AshFloat(2)) + (y ^ new AshFloat(2)) + (z ^ new AshFloat(2))) ^ new AshFloat(0.5f); //bugged unless i either casted or used an "explicit" operator, which made every line error. for another day.
        }

        public AshVector3 GetDirection()
        {
            return this / GetMagnitude();
        }

        


    }

    [Serializable]
    public struct AshVector4
    {

        //passVector

        public AshFloat x;
        public AshFloat y;
        public AshFloat z;
        public AshFloat w;

        public AshVector4(AshFloat passValueX, AshFloat passValueY, AshFloat passValueZ, AshFloat passValueW)
        {
            x = passValueX;
            y = passValueY;
            z = passValueZ;
            w = passValueW;
        }

        public AshVector4(Vector4 passVector)
        {
            x = passVector.x;
            y = passVector.y;
            z = passVector.z;
            w = passVector.w;
        }

        public Vector4 ReturnBase()
        {
            return new Vector4(x.value, y.value, z.value, w.value);
        }

        //basic maths

        public static AshVector4 operator +(AshVector4 left, AshVector4 right)
        {

            return (new AshVector4(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w));

        }

        public static AshVector4 operator -(AshVector4 left, AshVector4 right)
        {

            return (new AshVector4(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w));

        }

        public static AshVector4 operator *(AshVector4 left, AshVector4 right)
        {

            return (new AshVector4(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w));

        }

        public static AshVector4 operator *(AshVector4 passValue, AshFloat mag)
        {

            return (new AshVector4(passValue.x * mag, passValue.y * mag, passValue.z * mag, passValue.w * mag));

        }

        public static AshVector4 operator /(AshVector4 left, AshVector4 right)
        {

            return (new AshVector4(left.x / right.x, left.y / right.y, left.z / right.z, left.w / right.w));

        }

        public static AshVector4 operator /(AshVector4 passValue, AshFloat mag)
        {

            return (new AshVector4(passValue.x / mag, passValue.y / mag, passValue.z / mag, passValue.w / mag));

        }

        //vector-specific

        public AshFloat GetMagnitude()
        {
            return ((x ^ new AshFloat(2)) + (y ^ new AshFloat(2)) + (z ^ new AshFloat(2))) ^ new AshFloat(0.5f); //bugged unless i either casted or used an "explicit" operator, which made every line error. for another day.
        }

        public AshVector4 GetDirection()
        {
            return this / GetMagnitude();
        }




    }

    public struct AshMatrix
    {
        public AshFloat[,] values;

        public static AshMatrix identity
        {
            get { 
                return new AshMatrix(
                    new AshVector4(1, 0, 0, 0), 
                    new AshVector4(0, 1, 0, 0), 
                    new AshVector4(0, 0, 1, 0), 
                    new AshVector4(0, 0, 0, 1) 
                );
            }
        }

        public AshMatrix(AshVector4 column1, AshVector4 column2, AshVector4 column3, AshVector4 column4)
        {
            values = new AshFloat[4, 4];

            values[0, 0] = column1.x;
            values[1, 0] = column1.y;
            values[2, 0] = column1.z;
            values[3, 0] = column1.w;

            values[0, 1] = column2.x;
            values[1, 1] = column2.y;
            values[2, 1] = column2.z;
            values[3, 1] = column2.w;

            values[0, 2] = column3.x;
            values[1, 2] = column3.y;
            values[2, 2] = column3.z;
            values[3, 2] = column3.w;

            values[0, 3] = column4.x;
            values[1, 3] = column4.y;
            values[2, 3] = column4.z;
            values[3, 3] = column4.w;
        }

        public AshMatrix(AshVector3 column1, AshVector3 column2, AshVector3 column3, AshVector3 column4) { 
            values = new AshFloat[4,4];

            values[0, 0] = column1.x;
            values[1, 0] = column1.y;
            values[2, 0] = column1.z;
            values[3, 0] = 0;

            values[0, 1] = column2.x;
            values[1, 1] = column2.y;
            values[2, 1] = column2.z;
            values[3, 1] = 0;

            values[0, 2] = column3.x;
            values[1, 2] = column3.y;
            values[2, 2] = column3.z;
            values[3, 2] = 0;

            values[0, 3] = column4.x;
            values[1, 3] = column4.y;
            values[2, 3] = column4.z;
            values[3, 3] = 1;
        }

        public static AshVector4 operator *(AshMatrix passMatrix, AshVector4 passVector)
        {
            return new AshVector4(
                (passMatrix.values[0, 0] * passVector.x) + (passMatrix.values[0, 1] * passVector.y) + (passMatrix.values[0, 2] * passVector.z) + (passMatrix.values[0, 3] * passVector.w),
                (passMatrix.values[1, 0] * passVector.x) + (passMatrix.values[1, 1] * passVector.y) + (passMatrix.values[1, 2] * passVector.z) + (passMatrix.values[1, 3] * passVector.w),
                (passMatrix.values[2, 0] * passVector.x) + (passMatrix.values[2, 1] * passVector.y) + (passMatrix.values[2, 2] * passVector.z) + (passMatrix.values[2, 3] * passVector.w),
                (passMatrix.values[3, 0] * passVector.x) + (passMatrix.values[3, 1] * passVector.y) + (passMatrix.values[3, 2] * passVector.z) + (passMatrix.values[3, 3] * passVector.w)
            );
        }

        public static AshVector3 operator *(AshMatrix passMatrix, AshVector3 passVector)
        {
            return new AshVector3(
                (passMatrix.values[0, 0] * passVector.x) + (passMatrix.values[0, 1] * passVector.y) + (passMatrix.values[0, 2] * passVector.z) + (passMatrix.values[0, 3]),
                (passMatrix.values[1, 0] * passVector.x) + (passMatrix.values[1, 1] * passVector.y) + (passMatrix.values[1, 2] * passVector.z) + (passMatrix.values[1, 3]),
                (passMatrix.values[2, 0] * passVector.x) + (passMatrix.values[2, 1] * passVector.y) + (passMatrix.values[2, 2] * passVector.z) + (passMatrix.values[2, 3])
            );
        }


        public static AshMatrix operator *(AshMatrix left, AshMatrix right)
        {
            return new AshMatrix(
                new AshVector4(        (left.values[0, 0] * right.values[0, 0]) + (left.values[1, 0] * right.values[0, 1]) + (left.values[2, 0] * right.values[0, 2]) + (left.values[3, 0] * right.values[0, 3]),        
                                       (left.values[0, 0] * right.values[1, 0]) + (left.values[1, 0] * right.values[1, 1]) + (left.values[2, 0] * right.values[1, 2]) + (left.values[3, 0] * right.values[1, 3]),        
                                       (left.values[0, 0] * right.values[2, 0]) + (left.values[1, 0] * right.values[2, 1]) + (left.values[2, 0] * right.values[2, 2]) + (left.values[3, 0] * right.values[2, 3]),        
                                       (left.values[0, 0] * right.values[3, 0]) + (left.values[1, 0] * right.values[3, 1]) + (left.values[2, 0] * right.values[3, 2]) + (left.values[3, 0] * right.values[3, 3])    ),


                new AshVector4(        (left.values[0, 1] * right.values[0, 0]) + (left.values[1, 1] * right.values[0, 1]) + (left.values[2, 1] * right.values[0, 2]) + (left.values[3, 1] * right.values[0, 3]),        
                                       (left.values[0, 1] * right.values[1, 0]) + (left.values[1, 1] * right.values[1, 1]) + (left.values[2, 1] * right.values[1, 2]) + (left.values[3, 1] * right.values[1, 3]),        
                                       (left.values[0, 1] * right.values[2, 0]) + (left.values[1, 1] * right.values[2, 1]) + (left.values[2, 1] * right.values[2, 2]) + (left.values[3, 1] * right.values[2, 3]),        
                                       (left.values[0, 1] * right.values[3, 0]) + (left.values[1, 1] * right.values[3, 1]) + (left.values[2, 1] * right.values[3, 2]) + (left.values[3, 1] * right.values[3, 3])    ),


                new AshVector4(        (left.values[0, 2] * right.values[0, 0]) + (left.values[1, 2] * right.values[0, 1]) + (left.values[2, 2] * right.values[0, 2]) + (left.values[3, 2] * right.values[0, 3]),        
                                       (left.values[0, 2] * right.values[1, 0]) + (left.values[1, 2] * right.values[1, 1]) + (left.values[2, 2] * right.values[1, 2]) + (left.values[3, 2] * right.values[1, 3]),        
                                       (left.values[0, 2] * right.values[2, 0]) + (left.values[1, 2] * right.values[2, 1]) + (left.values[2, 2] * right.values[2, 2]) + (left.values[3, 2] * right.values[2, 3]),        
                                       (left.values[0, 2] * right.values[3, 0]) + (left.values[1, 2] * right.values[3, 1]) + (left.values[2, 2] * right.values[3, 2]) + (left.values[3, 2] * right.values[3, 3])    ),


                new AshVector4(        (left.values[0, 3] * right.values[0, 0]) + (left.values[1, 3] * right.values[0, 1]) + (left.values[2, 3] * right.values[0, 2]) + (left.values[3, 3] * right.values[0, 3]),        
                                       (left.values[0, 3] * right.values[1, 0]) + (left.values[1, 3] * right.values[1, 1]) + (left.values[2, 3] * right.values[1, 2]) + (left.values[3, 3] * right.values[1, 3]),        
                                       (left.values[0, 3] * right.values[2, 0]) + (left.values[1, 3] * right.values[2, 1]) + (left.values[2, 3] * right.values[2, 2]) + (left.values[3, 3] * right.values[2, 3]),        
                                       (left.values[0, 3] * right.values[3, 0]) + (left.values[1, 3] * right.values[3, 1]) + (left.values[2, 3] * right.values[3, 2]) + (left.values[3, 3] * right.values[3, 3])    )
            );
        }

        public AshMatrix InvertTranslation()
        {
            AshMatrix returnID = identity;
            returnID.values[0, 3] *= -1;
            returnID.values[1, 3] *= -1;
            returnID.values[2, 3] *= -1;
            return returnID;
        }

    }



    [Serializable]
    public struct AshQuat
    {

        //passVector

        /*private AshFloat backingX;
        private AshFloat backingY;
        private AshFloat backingZ;
        private AshFloat backingW;

        public AshFloat x { get { return backingX; } set { backingX = value; } }
        public AshFloat y { get { return backingY; } set { backingY = value; } }
        public AshFloat z { get { return backingZ; } set { backingZ = value; } }
        public AshFloat w { get { return backingW; } set { backingW = value; } }*/

        public AshFloat x;
        public AshFloat y;
        public AshFloat z;
        public AshFloat w;

        public AshQuat(AshFloat angle, AshVector3 axis)
        {
            x = axis.x * Mathf.Sin(angle.value / 2f);
            y = axis.y * Mathf.Sin(angle.value / 2f);
            z = axis.z * Mathf.Sin(angle.value / 2f);
            w = Mathf.Cos(angle.value / 2f);
            //ManuallyNormalizeQuat();
        }

        public AshQuat(AshVector3 passVector)
        {
            x = passVector.x;
            y = passVector.y;
            z = passVector.z;
            w = 0;
            //ManuallyNormalizeQuat();
        }

        public AshQuat(AshFloat px, AshFloat py, AshFloat pz, AshFloat pw)
        {
            x = px;
            y = py;
            z = pz;
            w = pw;
            //ManuallyNormalizeQuat();
        }

        public AshQuat(Quaternion passQuaternion)
        {
            x = passQuaternion.x;
            y = passQuaternion.y;
            z = passQuaternion.z;
            w = passQuaternion.w;
            //ManuallyNormalizeQuat();
        }


        public void SetAxis(AshVector3 newAxis)
        {
            x = newAxis.x;
            y = newAxis.y;
            z = newAxis.z;
        }
        
        public AshVector3 GetAxis()
        {
            return new AshVector3(x,y,z);
        }

        public Quaternion ReturnBase()
        {
            return new Quaternion(x.value, y.value, z.value, w.value);
        }
        
        public static AshQuat operator*(AshQuat left, AshQuat right)
        {

            return new AshQuat() {

                x = (left.w * right.x) + (left.x * right.w) + (left.y * right.z) - (left.z * right.y),
                y = (left.w * right.y) + (left.y * right.w) + (left.x * right.z) - (left.z * right.x),
                z = (left.w * right.z) + (left.z * right.w) + (left.x * right.y) - (left.y * right.x),
                w = (left.w * right.w) - (left.x * right.x) - (left.y * right.y) - (left.z * right.z)

            };


        }
        
        /*public static AshQuat operator *(AshQuat left, AshQuat right) //CHEATS !!!
        {

            return new AshQuat(left.ReturnBase() * right.ReturnBase());

        }*/

        public AshQuat GetInverse()
        {

            AshQuat inverse = new AshQuat();
            inverse.w = w;
            inverse.SetAxis(new AshVector3(0f,0f,0f) - GetAxis());
            return inverse;
        }

        /*public AshQuat GetInverse() //cheats by using internals.
        {
            return new AshQuat(Quaternion.Inverse(ReturnBase()));
        }*/


        public AshFloat GetMagnitude()
        {
            return ((x ^ new AshFloat(2)) + (y ^ new AshFloat(2)) + (z ^ new AshFloat(2)) + (w ^ new AshFloat(2))) ^ new AshFloat(0.5f);
        }

        public void ManuallyNormalizeQuat()
        {

            AshFloat magnitude = GetMagnitude();
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            w /= magnitude;

        }

        public AshVector4 GetAxisAngle()
        {

            /*AshFloat nancheckX = new AshFloat(Mathf.Sin(2 * Mathf.Acos(Mathf.Clamp01(x.value))));
            AshFloat nancheckY = new AshFloat(Mathf.Sin(2 * Mathf.Acos(Mathf.Clamp01(y.value))));
            AshFloat nancheckZ = new AshFloat(Mathf.Sin(2 * Mathf.Acos(Mathf.Clamp01(z.value))));
            AshFloat nancheckW = Mathf.Acos(Mathf.Clamp(w.value, 0f, 1f));

           

            if (Mathf.Approximately(nancheckX.value, 0)) { nancheckX = 1; }
            if (Mathf.Approximately(nancheckY.value, 0)) { nancheckY = 1; }
            if (Mathf.Approximately(nancheckZ.value, 0)) { nancheckZ = 1; }
            if (Mathf.Approximately(nancheckW.value, 0)) { nancheckW = 1; }



            return new AshVector4(
                
                x / nancheckX,
                y / nancheckY,
                z / nancheckZ,
                nancheckW

            );*/


            AshVector4 rv = new AshVector4();
            AshFloat halfAngle = Mathf.Acos(w.value);
            rv.w = halfAngle * 2;

            rv.x = x / Mathf.Sin(halfAngle.value);
            rv.y = y / Mathf.Sin(halfAngle.value);
            rv.z = z / Mathf.Sin(halfAngle.value);

            return rv;

        }

        /*public AshVector4 GetAxisAngle()
        {

            AshVector4 rv = new AshVector4();
            AshFloat halfAngle = Mathf.Acos(w.value);
            rv.w = halfAngle * 2;

            rv.x = x / Mathf.Sin(halfAngle.value);
            rv.y = y / Mathf.Sin(halfAngle.value);
            rv.z = z / Mathf.Sin(halfAngle.value);

            return rv;

        }*/

        /*public AshVector4 GetAxisAngle() //Cheats by using internals.
        {

            Vector3 outAxis;
            float outAngle;

            ReturnBase().ToAngleAxis(out outAngle, out outAxis);


            AshVector4 rv = new AshVector4(outAxis.x, outAxis.y, outAxis.z, outAngle);

            return rv;

        }*/



        /*public AshVector3 GetEuler() //from (Conversion between quaternions and Euler angles, 2024, January 28)
        {
            AshVector3 radians = new AshVector3(

                //pitch (x-axis rotation)
                2 * Mathf.Atan2(Mathf.Sqrt((1 + 2 * (w * y - x * z)).value), Mathf.Sqrt((1 - 2 * (w * y - x * z)).value)) - Mathf.PI / 2f,


                //yaw (y-axis rotation)
                Mathf.Atan2(2 * (w * z + x * y).value, 1 - 2 * (y * y + z * z).value),


                //roll (z-axis rotation)
                Mathf.Atan2(2 * (w * x + y * z).value, 1 - 2 * (x * x + y * y).value)

            );

            return AshMaths.RadiansToDegrees(radians);
        }*/


    }




    public struct AshMaths { 

        public static AshFloat DotProduct(AshVector3 left, AshVector3 right, bool normalise = true)
        {

            AshVector3 usingLeft;
            AshVector3 usingRight;

            switch (normalise) {

                case true:

                    usingLeft = left.GetDirection();
                    usingRight = right.GetDirection();

                    break;

                case false:

                    usingLeft = left;
                    usingRight = right;

                    break;

            }

            return (usingLeft.x * usingRight.x) + (usingLeft.y * usingRight.y) + (usingLeft.z * usingRight.z);

        }

        //rotations

        public static AshVector3 DegreesToRadians(AshVector3 degreeVector)
        {
            AshVector3 radianVector = new AshVector3();
            
            radianVector = degreeVector / (180f/Mathf.PI);

            return radianVector;
        }

        public static AshVector3 RadiansToDegrees(AshVector3 radianVector)
        {
            AshVector3 degreeVector = new AshVector3();

            degreeVector = radianVector * (180f / Mathf.PI);

            return degreeVector;
        }

        public static AshVector3 EulerToDir(AshVector3 eulerVector)
        { //uses builtin cos and sin

            AshVector3 radianVector = DegreesToRadians(eulerVector);

            AshVector3 dirVector = new AshVector3();

            //dirVector.x = Mathf.Cos(radianVector.x.interp) * Mathf.Cos(radianVector.y.interp);
            //dirVector.y = Mathf.Sin(radianVector.x.interp);
            //dirVector.z = Mathf.Cos(radianVector.x.interp) * Mathf.Sin(radianVector.y.interp);

            dirVector.x = Mathf.Cos(-radianVector.x.value) * Mathf.Cos((0.5f *  Mathf.PI) - radianVector.y.value);
            dirVector.y = Mathf.Sin(-radianVector.x.value);
            dirVector.z = Mathf.Cos(-radianVector.x.value) * Mathf.Sin((0.5f * Mathf.PI) - radianVector.y.value);

            //Debug.Log(dirVector.ReturnBase());
            return dirVector;

        }

        public static AshVector3 EulerToUp(AshVector3 eulerVector)
        { //uses builtin cos and sin

            AshVector3 radianVector = DegreesToRadians(eulerVector);

            AshVector3 dirVector = new AshVector3();

            //dirVector.x = Mathf.Cos(radianVector.x.interp) * Mathf.Cos(radianVector.y.interp);
            //dirVector.y = Mathf.Sin(radianVector.x.interp);
            //dirVector.z = Mathf.Cos(radianVector.x.interp) * Mathf.Sin(radianVector.y.interp);

            dirVector.x = Mathf.Cos(-radianVector.x.value + (Mathf.PI * 0.5f)) * Mathf.Cos((0.5f * Mathf.PI) - radianVector.y.value);
            dirVector.y = Mathf.Sin(-radianVector.x.value + (Mathf.PI * 0.5f));
            dirVector.z = Mathf.Cos(-radianVector.x.value + (Mathf.PI * 0.5f)) * Mathf.Sin((0.5f * Mathf.PI) - radianVector.y.value);

            //Debug.Log(dirVector.ReturnBase());
            return dirVector;

        }

        public static AshVector3 EulerToRight(AshVector3 eulerVector)
        { //uses builtin cos and sin

            if (Organizer.Orrery.depatch_outOfScope_bugs)
            {
                AshVector3 dirVector;
                AshVector3 radianVector = DegreesToRadians(eulerVector);
                dirVector.x = Mathf.Cos(-radianVector.x.value) * Mathf.Cos((0.5f * Mathf.PI) - radianVector.y.value + (Mathf.PI * 0.5f));
                dirVector.y = Mathf.Sin(-radianVector.x.value);
                dirVector.z = Mathf.Cos(-radianVector.x.value) * Mathf.Sin((0.5f * Mathf.PI) - radianVector.y.value + (Mathf.PI * 0.5f));
                return dirVector;
            } 
            
            return new AshVector3();
        }

        public static AshVector3 DirToEuler(AshVector3 dirVector, AshVector3 upVector) 
        {
            if (Organizer.Orrery.depatch_outOfScope_bugs)
            {

                dirVector = dirVector.GetDirection();

                AshVector3 radianVector = new AshVector3();

                //dirVector.x = Mathf.Cos(-radianVector.x.interp) * Mathf.Cos((0.5f * Mathf.PI) - radianVector.y.interp);
                //dx = Mathf.Cos(-rx) * Mathf.Cos((0.5f * Mathf.PI) - ry);
                //dx / Mathf.Cos(-rx) = Mathf.Cos((0.5f * Mathf.PI) - ry);
                //Mathf.Acos(dx / Mathf.Cos(-rx)) = (0.5f * Mathf.PI) - ry;
                //ry = (0.5f * Mathf.PI) - Mathf.Acos(dx / Mathf.Cos(-rx));

                //dirVector.y = Mathf.Sin(-radianVector.x.interp);
                //dirVector.z = Mathf.Cos(-radianVector.x.interp) * Mathf.Sin((0.5f * Mathf.PI) - radianVector.y.interp);

                AshVector3 crossedDirections = VectorCrossProduct(upVector, dirVector);

                radianVector.x = -Mathf.Asin(Mathf.Clamp(dirVector.y.value, -1f, 1f));
                radianVector.y = (0.5f * Mathf.PI) - Mathf.Acos(Mathf.Clamp((dirVector.x.value) / Mathf.Cos(radianVector.x.value), -1f, 1f)) * Mathf.Sign(dirVector.z.value);
                radianVector.z = (0.5f * Mathf.PI) - Mathf.Acos(Mathf.Clamp((upVector.x.value) / Mathf.Cos(radianVector.x.value), -1f, 1f)) * Mathf.Sign(upVector.z.value);


                //fuck, how are we gonna figure out z with an up vector
                //i imagine it would be much the same, just with axes flipped. if (x,y) are the axes relating to the forward vector ... then surely (z,?) are the axes of the up vector, since we're offsetting them all.
                //more specifically, x is the "right" regarding forward, y is the "up", and z is the "forward". 
                //so if y was "forward" (which is the case for an up axis) ... either x or z would be right or up, but it's impossible to tell which. i think. given the way it's rotated around the up axis, yk?
                //however, it might not matter. i imagine that we can figure out z by using the up vector's ... forward. in the same formula
                //so i'll try:
                //-Mathf.Asin(Mathf.Clamp(upVector.y.value, -1f, 1f));
                //tried that. it was consistently better but absolutely incorrect. 
                //giving up here, out of scope





                AshVector3 eulerVector = RadiansToDegrees(radianVector);
                //if (float.IsNaN(eulerVector.x.value)) { eulerVector.x = 0; }
                //if (float.IsNaN(eulerVector.y.value)) { eulerVector.y = 0; }
                //if (float.IsNaN(eulerVector.z.value)) { eulerVector.z = 0; }
                //eulerVector.y = (Quaternion.LookRotation(dirVector.ReturnBase(), Vector3.up)).eulerAngles.y; //sucks! ask about this

                //Debug.Log(eulerVector.ReturnBase());
                return eulerVector;
            } 
            
            else //Cheats by using internals. this was to act as a reference for what the intended behaviour SHOULD look like, for bug detection. it is not to be used in the final project under any circumstances.
            {
                Quaternion cheatDirQuat = Quaternion.LookRotation(dirVector.ReturnBase(), upVector.ReturnBase());
                return new AshVector3(cheatDirQuat.eulerAngles);
            }

        }

        public static AshVector3 VectorCrossProduct(AshVector3 left, AshVector3 right)
        {
            return new AshVector3((left.y * right.z) - (left.z * right.y), (left.z * right.x) - (left.x * right.z), (left.x * right.y) - (left.y * right.x));
        }

        public static AshFloat Clamp(AshFloat min, AshFloat max, AshFloat value)
        {
            if (value < min)
            {
                return min;
            }

            else if (value > max)
            {
                return max;
            }

            else
            {
                return value;
            }
        }

        public static AshFloat Lerp(AshFloat min, AshFloat max, AshFloat value)
        {
            return (min * (1-Clamp(0, 1, value))) + (max * Clamp(0, 1, value));
        }

        public static AshVector3 Lerp(AshVector3 min, AshVector3 max, AshFloat value)
        {
            return (min * (1 - Clamp(0, 1, value))) + (max * Clamp(0, 1, value));
        }

        /*public static AshQuat Slerp(AshQuat left, AshQuat right, AshFloat interp)
        {  
            Debug.Log(left.ReturnBase().ToString() + " " + right.ReturnBase().ToString() + " " + interp.value.ToString());

            interp = Mathf.Clamp01(interp.value);


            //edge case exception detected by (Unity, 2015)
            if ((left.x * right.x + left.y * right.y + left.z * right.z + left.w * right.w).value > (1.0f - 1e-6f))
            {
                // Too close, do straight linear interpolation.
                //s1 = 1.0f - interp.value;
                //s2 = (flip) ? -interp.value : interp.value;
                
                return right;

            }

            else
            {
                AshQuat multiplierMultiplier = right * left.GetInverse();
                AshVector4 axisAngle = multiplierMultiplier.GetAxisAngle();
                AshQuat resultMultiplier = new AshQuat(axisAngle.w * interp, new AshVector3(axisAngle.x, axisAngle.y, axisAngle.z));

                return resultMultiplier * left;
            }
            

        }*/

        public static AshQuat Slerp(AshQuat left, AshQuat right, AshFloat interp) 
        {

            if (Organizer.Orrery.depatch_outOfScope_bugs)
            {
                interp = Mathf.Clamp01(interp.value);
                AshQuat multiplierMultiplier = right * left.GetInverse();
                AshVector4 axisAngle = multiplierMultiplier.GetAxisAngle();
                AshQuat resultMultiplier = new AshQuat(axisAngle.w * interp, new AshVector3(axisAngle.x, axisAngle.y, axisAngle.z));

                return resultMultiplier * left;
            } 
            
            else //the following code snippet was adapted from (Unity, 2015). this was attempting to solve several bugs with SLERP, but ultimately i didn't understand the math enough for it to be considered my own work. i leave it here out of posterity; it is not to be used in the artefact, nor should it be marked as if it were mine.
            {
                float cosOmega = (left.x * right.x + left.y * right.y +
                             left.z * right.z + left.w * right.w).value;

                bool flip = false;

                if (cosOmega < 0.0f)
                {
                    flip = true;
                    cosOmega = -cosOmega;
                }

                float s1, s2;

                if (cosOmega > (1.0f - 1e-6f))
                {
                    // Too close, do straight linear interpolation.
                    s1 = 1.0f - interp.value;
                    s2 = (flip) ? -interp.value : interp.value;
                } else
                {
                    float omega = (float)Math.Acos(cosOmega);
                    float invSinOmega = (float)(1 / Math.Sin(omega));

                    s1 = (float)Math.Sin((1.0f - interp.value) * omega) * invSinOmega;
                    s2 = (flip)
                        ? (float)-Math.Sin(interp.value * omega) * invSinOmega
                        : (float)Math.Sin(interp.value * omega) * invSinOmega;
                }

                return new AshQuat(
                    s1 * left.x + s2 * right.x,
                    s1 * left.y + s2 * right.y,
                    s1 * left.z + s2 * right.z,
                    s1 * left.w + s2 * right.w
                );
            }

            



        }

        /*public static AshQuat Slerp(AshQuat left, AshQuat right, AshFloat interp) //cheats by using internals!!!
        {

            return new AshQuat(Quaternion.Slerp(left.ReturnBase(), right.ReturnBase(), interp.value));



        }*/

        /*static public AshQuat EulerToQuat(AshVector3 passVector) //adapted from (Conversion between quaternions and Euler angles, 2024, January 28) 
        {

            Debug.Log(passVector.ReturnBase().ToString());

            //weird bypass redux
            //if (passVector.x > 90f)
            //{
            //    passVector.x *= -1;
            //}

            //else if (passVector.x < -90f)
            //{
            //    passVector.x *= -1;
            //}



            passVector = DegreesToRadians(passVector);

            float cr = Mathf.Cos(passVector.z.value * 0.5f);
            float sr = Mathf.Sin(passVector.z.value * 0.5f);
            float cp = Mathf.Cos(passVector.x.value * 0.5f);
            float sp = Mathf.Sin(passVector.x.value * 0.5f);
            float cy = Mathf.Cos(passVector.y.value * 0.5f);
            float sy = Mathf.Sin(passVector.y.value * 0.5f);

            return new AshQuat(
                sr * cp * cy - cr * sp * sy,
                cr * sp * cy + sr * cp * sy,
                cr * cp * sy - sr * sp * cy,
                cr * cp * cy + sr * sp * sy
            );


        }*/

        /*static public AshQuat EulerToQuat(AshVector3 passVector) //from (Andrea S., 2002), though thats what we're meant to use
        {
            AshVector3 vx = new AshVector3(1, 0, 0);
            AshVector3 vy = new AshVector3(0, 1, 0);
            AshVector3 vz = new AshVector3(0, 0, 1);
            AshQuat qx;
            AshQuat qy;
            AshQuat qz;
            AshQuat qt;

            

            quaternion_from_axisangle(qx, &vx, rx);
            quaternion_from_axisangle(qy, &vy, ry);
            quaternion_from_axisangle(qz, &vz, rz);

            quaternion_multiply(&qt, &qx, &qy);
            quaternion_multiply(&q, &qt, &qz);

            //this code segment is totally incomplete, and the other reference is a dead link. This demonstrates nothing. Worthless junk.

        }*/

        /*static public AshQuat EulerToQuat(AshVector3 passVector) //cheats by using internals.
        {

            Vector3 cheatEuler = passVector.ReturnBase();
            Quaternion cheatQuat = Quaternion.Euler(cheatEuler);

            return new AshQuat(cheatQuat);

        }*/





        public static AshVector3 QuatToEuler(AshQuat q) //from (Conversion between quaternions and Euler angles, 2024, January 28)
        {


            AshVector3 euler = new AshVector3();

            // roll (x-axis rotation)
            AshFloat sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            AshFloat cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            euler.x = new AshFloat((float)Math.Atan2(sinr_cosp.value, cosr_cosp.value));

            // pitch (y-axis rotation)
            AshFloat sinp = 2 * (q.w * q.y - q.z * q.x);
            if (Math.Abs(sinp.value) >= 1)
            {
                euler.y = new AshFloat((float)(Math.PI / 2 * Math.Sign(sinp.value)));
            } 
            
            else
            {
                euler.y = new AshFloat((float)Math.Asin(sinp.value));
            }

            // yaw (z-axis rotation)
            AshFloat siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            AshFloat cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            euler.z = new AshFloat((float)Math.Atan2(siny_cosp.value, cosy_cosp.value));

            return RadiansToDegrees(euler);
        }


        /*public static AshQuat EulerToQuat(AshVector3 xyz)
        {


            // return mul(rotateY(xyz.y), mul(rotateX(xyz.x), rotateZ(xyz.z)));
            float3 s = new float3(Mathf.Sin(0.5f * xyz.x.value), Mathf.Sin(0.5f * xyz.y.value), Mathf.Sin(0.5f * xyz.z.value));
            float3 c = new float3(Mathf.Cos(0.5f * xyz.x.value), Mathf.Cos(0.5f * xyz.y.value), Mathf.Cos(0.5f * xyz.z.value));


            // s.x * c.y * c.z + s.y * s.z * c.x,
            // s.y * c.x * c.z - s.x * s.z * c.y,
            // s.z * c.x * c.y - s.x * s.y * c.z,
            // c.x * c.y * c.z + s.y * s.z * s.x
            float4 result = new float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * new float4(c.xyz, s.x) * new float4(1.0f, -1.0f, -1.0f, 1.0f);

            return new AshQuat(result.x, result.y, result.z, result.w);


        }*/

        public static AshQuat EulerToQuat(AshVector3 euler) //from (Sha, 2021)
        {
            euler = DegreesToRadians(euler);

            //xxyyzz N
            //xxzzyy N
            //yyxxzz N
            //yyzzxx N
            //zzxxyy N 
            //zzyyxx Y

            AshFloat cy = new AshFloat((float)Math.Cos(euler.z.value * 0.5f));
            AshFloat sy = new AshFloat((float)Math.Sin(euler.z.value * 0.5f));
            AshFloat cp = new AshFloat((float)Math.Cos(euler.y.value * 0.5f));
            AshFloat sp = new AshFloat((float)Math.Sin(euler.y.value * 0.5f));
            AshFloat cr = new AshFloat((float)Math.Cos(euler.x.value * 0.5f));
            AshFloat sr = new AshFloat((float)Math.Sin(euler.x.value * 0.5f));

            return new AshQuat(
                (sr * cp * cy - cr * sp * sy),
                (cr * sp * cy + sr * cp * sy),
                (cr * cp * sy - sr * sp * cy),
                (cr * cp * cy + sr * sp * sy)
            );

        }



    }

}

//Andrea S., 2002: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q60
//Sha, 2021: https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
//Unity, 2015: https://github.com/microsoft/referencesource/blob/master/System.Numerics/System/Numerics/Quaternion.cs
//Conversion between quaternions and Euler angles, 2024, January 28: https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles


