using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisplaySizeController : MonoBehaviour
{
    [SerializeField] float minSize;
    [SerializeField] int maxBoardTiles, minBoardTiles;
    [SerializeField] Transform boardRotTf;

    public void SetSize(Board board)
    {
        int largestSide = Mathf.Max(board.Width, board.Height);
        float boardSizeNormalized = Mathf.InverseLerp(minBoardTiles, maxBoardTiles, largestSide);
        float rootSize = Mathf.Lerp(minSize,1,boardSizeNormalized); 
        boardRotTf.localScale =  new Vector3(rootSize,rootSize,rootSize);
    }
    public void GetBasicSize()
    {
        boardRotTf.localScale = Vector3.one;
    }
}
