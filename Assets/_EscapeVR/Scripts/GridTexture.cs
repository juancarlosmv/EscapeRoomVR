using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTexture : MonoBehaviour
{
    Texture3D texture;
    [SerializeField]
    Vector3Int texSz;
    [SerializeField]
    Vector3 objSz;
    [SerializeField]
    float gridSize = 0.2f;
    [SerializeField]
    float lineWidth = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        texture = CreateTexture3D(texSz.x, texSz.y, texSz.z,
            objSz.x, objSz.y, objSz.z,
            gridSize, lineWidth);
        //GetComponent<MeshRenderer>().material.SetTexture("_MainTex3D", texture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sx">Texture x size</param>
    /// <param name="sy">Texture y size</param>
    /// <param name="sz">Texture z size</param>
    /// <param name="sox">Object x size</param>
    /// <param name="soy">Object y size</param>
    /// <param name="soz">Object z size</param>
    /// <param name="sep">Grid size</param>
    /// <param name="wid">Grid line width</param>
    /// <returns></returns>
    Texture3D CreateTexture3D(int sx, int sy, int sz, float sox, float soy, float soz, float sep, float wid)
    {
        Color[] colorArray = new Color[sx * sy * sz];
        texture = new Texture3D(sx, sy, sz, TextureFormat.RGBA32, true);
        for (int x = 0; x < sx; x++){
            for (int y = 0; y < sy; y++){
                for (int z = 0; z < sz; z++){
                    Color c = new Color(1.0f, 1.0f, 0.0f, 0.0f);
                    bool xok = Mathf.Abs((x / (sx - 1.0f) * sox + sep / 2.0f) % sep - sep / 2.0f) < wid;
                    bool yok = Mathf.Abs((y / (sy - 1.0f) * soy + sep / 2.0f) % sep - sep / 2.0f) < wid;
                    bool zok = Mathf.Abs((z / (sz - 1.0f) * soz + sep / 2.0f) % sep - sep / 2.0f) < wid;
                    //Debug.Log(Mathf.Abs((x / (sx - 1.0f) * sox + sep / 2.0f) % sep - sep / 2.0f));
                    //Debug.Log(xok.ToString() + " " + yok.ToString() + " " + zok.ToString() + ((sep / 2.0f) % sep).ToString());
                    if(xok && yok && zok)
                    {
                        c.r = 0.0f;
                        c.a = 1.0f;
                    }
                    //Debug.Log(c);
                    colorArray[x + (y * sx) + (z * sx * sy)] = c;
                }
            }
        }
        texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }
}
