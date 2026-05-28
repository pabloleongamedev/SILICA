using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public float posX = 0;
    public float posY = 1;
    public float posZ = 0;

    public float rotX = 0;
    public float rotY = 0;
    public float rotZ = 0;
    public float rotW = 1;

    public int health = 100;
    public int maxHealth = 100;
    public float jetpackFuel = 100;

    public static PlayerSaveData CreateDefault()
    {
        return new PlayerSaveData
        {
            posX = 0,
            posY = 1,
            posZ = 0,
            health = 100,
            maxHealth = 100,
            jetpackFuel = 100
        };
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(rotX, rotY, rotZ, rotW);
    }

    public void SetPosition(Vector3 pos)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
    }

    public void SetRotation(Quaternion rot)
    {
        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;
    }
}
