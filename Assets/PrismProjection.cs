using System;
using UnityEngine;

namespace Blocks
{
    public class PrismProjection
    {
        public Vector3[] vertexes;

        public PrismProjection(Vector3Int dimensions)
        {
            vertexes = new Vector3[8];
            vertexes[0] = new Vector3(-dimensions.x/2, -dimensions.y/2, dimensions.z/2);
            vertexes[1] = new Vector3(dimensions.x/2, -dimensions.y/2, dimensions.z/2);
            vertexes[2] = new Vector3(dimensions.x/2, -dimensions.y/2, -dimensions.z/2);
            vertexes[3] = new Vector3(-dimensions.x/2, -dimensions.y/2, -dimensions.z/2);
            vertexes[4] = new Vector3(-dimensions.x/2, dimensions.y/2, -dimensions.z/2);
            vertexes[5] = new Vector3(dimensions.x/2, dimensions.y/2, -dimensions.z/2);
            vertexes[6] = new Vector3(-dimensions.x/2, dimensions.y/2, dimensions.z/2);
            vertexes[7] = new Vector3(dimensions.x/2, dimensions.y/2, dimensions.z/2);
        }

        public Vector3Int GetDimensions()
        {
            Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
            Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (i)
                    {
                        case 0:
                            min.x = (int) Math.Min(min.x, vertexes[j].x);
                            break;
                        case 1:
                            min.y = (int) Math.Min(min.y, vertexes[j].y);
                            break;
                        case 2:
                            min.z = (int) Math.Min(min.z, vertexes[j].z);
                            break;
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (i)
                    {
                        case 0:
                            max.x = (int) Math.Max(max.x, vertexes[j].x);
                            break;
                        case 1:
                            max.y = (int) Math.Max(max.y, vertexes[j].y);
                            break;
                        case 2:
                            max.z = (int) Math.Max(max.z, vertexes[j].z);
                            break;
                    }
                }
            }
            
            return new Vector3Int(max.x - min.x, max.y - min.y, max.z - min.z);
        }

        public void RotateShape(Vector3Int angles)
        {
            angles = angles * 90;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (i)
                    {
                        case 0:
                            vertexes[j] = RotatePoint(vertexes[j], angles.x, i);
                            break;
                        case 1:
                            vertexes[j] = RotatePoint(vertexes[j], angles.y, i);
                            break;
                        case 2:
                            vertexes[j] = RotatePoint(vertexes[j], angles.z, i);
                            break;
                    }
                    
                }
            }
        }

        private Vector3 RotatePoint(Vector3 point, int angle, int step)
        {
            switch (step)
            {
                case 0:
                    return new Vector3(point.x, (float) (point.y * Math.Cos(angle) - point.z * Math.Sin(angle)),
                        (float) (point.z * Math.Cos(angle) + point.y * Math.Sin(angle)));
                case 1:
                    return new Vector3((float)(point.x*Math.Cos(angle) + point.z*Math.Sin(angle)), point.y, (float)(point.z*Math.Cos(angle) - point.x*Math.Sin(angle)));
                case 2:
                    return new Vector3((float)(point.x*Math.Cos(angle) + point.y*Math.Sin(angle)), (float)(point.y*Math.Cos(angle) - point.x*Math.Sin(angle)), point.z);
            }
            return new Vector3();
        }
    }
}