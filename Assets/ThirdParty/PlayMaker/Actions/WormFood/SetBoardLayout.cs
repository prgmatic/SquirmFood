using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

[ActionCategory("Worm Food")]
public class SetBoardLayout : FsmStateAction
{
    [RequiredField]
    public BoardLayout Layout;
}
