// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Activates/deactivates a Game Object. Use this to hide/show areas, or enable/disable many Behaviours at once.")]
	public class ActivateGameObject : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject to activate/deactivate.")]
        public FsmOwnerDefault gameObject;
		
		[RequiredField]
        [Tooltip("Check to activate, uncheck to deactivate Game Object.")]
        public FsmBool activate;

        [Tooltip("Recursively activate/deactivate all children.")]
        public FsmBool recursive;

        [Tooltip("Reset the game objects when exiting this state. Useful if you want an object to be active only while this state is active.\nNote: Only applies to the last Game Object activated/deactivated (won't work if Game Object changes).")]
		public bool resetOnExit;
        		
		// store the game object that we activated on enter
		// so we can de-activate it on exit.
		GameObject activatedGameObject;

		public override void Reset()
		{
			gameObject = null;
			activate = true;
            recursive = true;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go != null)
            {
                if (recursive.Value)
                    SetActiveRecursively(go, activate.Value);
                else
                    go.SetActive(activate.Value);
            }
            activatedGameObject = go;
			Finish();
		}

		public override void OnExit()
		{
			// the stored game object might be invalid now
			if (activatedGameObject == null)
				return;
            if (resetOnExit)
            {
                if (recursive.Value)
                    SetActiveRecursively(activatedGameObject, !activate.Value);
                else
                    activatedGameObject.SetActive(!activate.Value);
            }
		}

        private void SetActiveRecursively(GameObject go, bool state)
        {
            go.SetActive(state);
            foreach (Transform child in go.transform)
            {
                SetActiveRecursively(child.gameObject, state);
            }
        }
    }
}