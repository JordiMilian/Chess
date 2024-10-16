using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDebugger : MonoBehaviour
{
    public static void Log(string t, Board board)
    {
        if(!board.isVirtual)
        {
            Debug.Log(t);
        }
        
    }
}
