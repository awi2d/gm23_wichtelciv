using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldUi
{
    private static LineRenderer lineRenderer;
    private static Quaternion[] hex_directions = new Quaternion[] { Quaternion.Euler(0, 30f, 0), Quaternion.Euler(0, 90f, 0), Quaternion.Euler(0, 150f, 0), Quaternion.Euler(0, 210f, 0), Quaternion.Euler(0, 270f, 0), Quaternion.Euler(0, 330f, 0) };
    // center = 5, -1, linksdrüber = 6, -2, rechtsdrüber = 5, -2, rechts = 4, -1, rechtsdrunter = 4, 0, linksdrunter = 5, 0, links = 6, -1
    // center = (0, 0), linksdrüber = (1, -1), rechtsdrüber = (0, -1), rechts = (-1, 0), rechtsdrunter = (-1, 1), linksdrunter = (0, 1), links = (1, 1)
    public static void draw_lines(int posx, int posy, float radius)
    {
        // get all intpositions of hexes inside the radius.
        // draw line around them, ignoring inlier edges.
        Vector3 center = worldgen.intpos2wordpos(posx, posy) + new Vector3(0, 1, 0);
        Vector3 center_to_edge = new Vector3((float)worldgen.tilesize * 0.5f, 0, 0);
        Vector3[] direction = new Vector3[] { worldgen.world_rotation * InWorldUi.hex_directions[0] * center_to_edge, worldgen.world_rotation * InWorldUi.hex_directions[1] * center_to_edge, worldgen.world_rotation * InWorldUi.hex_directions[2] * center_to_edge, worldgen.world_rotation * InWorldUi.hex_directions[3] * center_to_edge, worldgen.world_rotation * InWorldUi.hex_directions[4] * center_to_edge, worldgen.world_rotation * InWorldUi.hex_directions[5] * center_to_edge };
        if (radius == 0)
        {
            InWorldUi.lineRenderer.positionCount = 7;

            for (int i = 0; i < InWorldUi.hex_directions.Length; i++)
            {
                InWorldUi.lineRenderer.SetPosition(i, center + direction[i]);
            }
            InWorldUi.lineRenderer.SetPosition(6, center + direction[0]); // connection from lower left corner to upper left corner.
        }
        else
        {
            if(radius == 1)
            {
                InWorldUi.lineRenderer.positionCount = 19;// 6*3+1;
                for (int i = 0; i < 6; i++)
                {
                    int ip_index = i + 1;
                    if(ip_index >= 6)
                    {
                        ip_index -= 6;
                    }
                    int im_index = i - 1;
                    if(im_index < 0)
                    {
                        im_index += 6;
                    }
                    InWorldUi.lineRenderer.SetPosition(3*i+0, center + 2 * direction[i]+direction[im_index]);
                    InWorldUi.lineRenderer.SetPosition(3*i+1, center + 2 * direction[i]);
                    InWorldUi.lineRenderer.SetPosition(3*i+2, center + 2 * direction[i]+direction[ip_index]);
                }
                InWorldUi.lineRenderer.SetPosition(18, center + 2*direction[0] + direction[5]);
            }
            else
            {
                if(radius == 2)
                {
                    InWorldUi.lineRenderer.positionCount = 31; //6*5+1
                    for(int i=0; i<6; i++)
                    {
                        int ip_index = i + 1;
                        if (ip_index >= 6)
                        {
                            ip_index -= 6;
                        }
                        int im_index = i - 1;
                        if (im_index < 0)
                        {
                            im_index += 6;
                        }
                        Vector3 base_pos = center + 3 * direction[i];
                        InWorldUi.lineRenderer.SetPosition(5 * i + 0, base_pos + 2 * direction[im_index]);
                        InWorldUi.lineRenderer.SetPosition(5 * i + 1, base_pos + 1 * direction[im_index]);
                        InWorldUi.lineRenderer.SetPosition(5 * i + 2, base_pos + 1 * direction[i]);
                        InWorldUi.lineRenderer.SetPosition(5 * i + 3, base_pos + 1 * direction[ip_index]);
                        InWorldUi.lineRenderer.SetPosition(5 * i + 4, base_pos + 2 * direction[ip_index]);

                    }
                    InWorldUi.lineRenderer.SetPosition(30, center + 3 * direction[0] + 2 * direction[5]);
                }
                else
                {

                }
                //TODO raidus n
                Debug.Log("notImplementedExeption: drawing with radius not in {0, 1}");
            }
        }
        
    }

    public static void remove_lines()
    {
        InWorldUi.lineRenderer.positionCount = 0;
    }
    static InWorldUi()
    {
        //For creating line renderer object
        InWorldUi.lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        InWorldUi.lineRenderer.startColor = Color.black;
        InWorldUi.lineRenderer.endColor = Color.black;
        InWorldUi.lineRenderer.startWidth = 0.1f;
        InWorldUi.lineRenderer.endWidth = 0.1f;
        InWorldUi.lineRenderer.positionCount = 7;
        InWorldUi.lineRenderer.useWorldSpace = true;
        
    }
}
