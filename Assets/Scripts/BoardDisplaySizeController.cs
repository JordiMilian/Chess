using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoardDisplaySizeController : MonoBehaviour
{
    [SerializeField] float DesiredMaxSize;
    [SerializeField] Transform boardRotTf, boardBGTf, BgChild, borderChild, cameraTf, wholeBoard;
    const float tileSize = 5.12f;
    [SerializeField] float verticalBoardOffset;

    public void SetSize(Board board)
    {
        int largestSideTiles = Mathf.Max(board.Width, board.Height);
        int smallestSideTiles = Mathf.Min(board.Width, board.Height);

        float largestSideLenght = (float)largestSideTiles * tileSize;
        float desiredRootMultiplier = DesiredMaxSize/ largestSideLenght;
        float desideredRootMultiplierShort = (float)smallestSideTiles / (float)largestSideTiles;

        boardRotTf.localScale = new Vector3(desiredRootMultiplier, desiredRootMultiplier, desiredRootMultiplier);

        
        SetUpBG();
        
        SetUpCenterOfBoard();
        SetUpBorder();


        void SetUpBG()
        {
            if (isWiderThanHeighter())
            {
                boardBGTf.localScale = new Vector3(1, desideredRootMultiplierShort, 1);
            }
            else
            {
                boardBGTf.localScale = new Vector3(desideredRootMultiplierShort, 1, 1);
            }
        }
        void SetUpBorder()
        {
            borderChild.transform.position = BgChild.position;
            borderChild.localScale = BgChild.localScale;

            borderChild.localScale = new Vector3(
                (borderChild.localScale.x * boardBGTf.localScale.x) + 6,
                (borderChild.localScale.y * boardBGTf.localScale.y) + 6,
                1);
        }
        void SetUpCenterOfBoard()
        {
            wholeBoard.position = new Vector3(
                cameraTf.position.x - (BgChild.localPosition.x * boardBGTf.localScale.x),
                cameraTf.position.y - (BgChild.localPosition.y * boardBGTf.localScale.y) - verticalBoardOffset,
                1);
        }
        bool isWiderThanHeighter()
        {
            return board.Width >= board.Height;
        }
    }

    public void GetBasicSize()
    {
        boardRotTf.localScale = Vector3.one;
    }

}
