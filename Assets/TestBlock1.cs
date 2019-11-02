using System;
using UnityEngine;

namespace Blocks
{
    public class TestBlock1 : Block
    {
        private void Awake()
        {
            SetDimensions(new Vector3Int(1, 1, 1));
        }
    }
}