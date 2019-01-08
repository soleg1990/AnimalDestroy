using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Delay
    {
        private bool isCoroutineExecuting;
        public IEnumerator DelayAndProcessAction(Action afterDelayAction, float delaySeconds)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(delaySeconds);

            afterDelayAction();

            isCoroutineExecuting = false;
        }
    }
}
