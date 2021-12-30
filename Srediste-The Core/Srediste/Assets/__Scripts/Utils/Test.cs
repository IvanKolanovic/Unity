using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public float radius;
    public Color InitialColor;

    private RaycastHit2D hitInfo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo)
            {
                UpdateTexture();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Resources.UnloadUnusedAssets();
        }
    }

    public Texture2D CopyTexture2D(Texture2D copiedTexture2D)
    {
        float differenceX;
        float differenceY;

        //Create a new Texture2D, which will be the copy
        Texture2D texture = new Texture2D(copiedTexture2D.width, copiedTexture2D.height);

        //Choose your filtermode and wrapmode
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        //Center of hit point circle 
        int m1 = (int)((hitInfo.point.x - hitInfo.collider.bounds.min.x) * (copiedTexture2D.width / hitInfo.collider.bounds.size.x));
        int m2 = (int)((hitInfo.point.y - hitInfo.collider.bounds.min.y) * (copiedTexture2D.height / hitInfo.collider.bounds.size.y));

        //Vector2 extremeScreenPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        //Debug.Log("extremeScreenPoint= " + extremeScreenPoint.x
        //                 + " hitInfo.point.x =" +  hitInfo.point.x 

        //    //+ "  mousePosition =" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x
        //                + "  bounds.min =" +  hitInfo.collider.bounds.min .x
        //                + "  bounds.max =" +  hitInfo.collider.bounds.max .x
        //                                        + "  size =" + hitInfo.collider.bounds.size.x
        //                                        + "  hit =" + (hitInfo.point.x - hitInfo.collider.bounds.min.x)
        //                                        + "  pixels =" + (hitInfo.point.x - hitInfo.collider.bounds.min.x) * (copiedTexture2D.width / hitInfo.collider.bounds.size.x)
        //    );



        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                differenceX = x - m1;
                differenceY = y - m2;



                //INSERT YOUR LOGIC HERE
                if (differenceX * differenceX + differenceY * differenceY <= radius * radius)
                {
                    //This line of code and if statement, turn all texture pixels within radius to zero alpha
                    texture.SetPixel(x, y, InitialColor);
                }
                else
                {
                    //This line of code is REQUIRED. Do NOT delete it. This is what copies the image as it was, without any change
                    texture.SetPixel(x, y, copiedTexture2D.GetPixel(x, y));
                }
            }
        }

        //This finalizes it. If you want to edit it still, do it before you finish with Apply(). Do NOT expect to edit the image after you have applied.
        texture.Apply();
        //DestroyImmediate(copiedTexture2D, true);
        return texture;
    }

    public void UpdateTexture()
    {
        SpriteRenderer mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Texture2D newTexture2D = CopyTexture2D(mySpriteRenderer.sprite.texture);

        //Get the name of the old sprite
        string tempName = mySpriteRenderer.sprite.name;
        //Create a new sprite
        mySpriteRenderer.sprite = Sprite.Create(newTexture2D, mySpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));
        //Name the sprite, the old name
        mySpriteRenderer.sprite.name = tempName;

        //Update the material
        //If you have multiple sprites, you will want to do this in a loop
        //mySpriteRenderer.material.mainTexture = newTexture2D;
        //mySpriteRenderer.material.shader = Shader.Find("Unlit/Transparent");

    }
}
