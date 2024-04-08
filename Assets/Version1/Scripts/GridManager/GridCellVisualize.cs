using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;

public class GridCellVisualize : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer InnerSprite;
    [SerializeField]
    SpriteRenderer OuterSprite;

    public GridCell gridCell;

    public void SetInnerColor(Color col)
    {
        InnerSprite.color = col;
    }
    public void SetOuterColor(Color col)
    {
        OuterSprite.color = col;
    }
}
