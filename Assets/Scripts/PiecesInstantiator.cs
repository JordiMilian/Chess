using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiecesInstantiator : MonoBehaviour
{
    public Board startingBoard;
    
    [Serializable]
    public class PieceCreator
    {
        public int team;
        public Vector2Int Position;
        public Piece.PiecesEnum type;

        public PieceCreator(int cteam, Vector2Int cpos, Piece.PiecesEnum ctype)
        {
            team = cteam;
            Position = cpos;
            type = ctype;
        }
    }
    
    
}
