using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juto.Misc
{
    public class Misc : MonoBehaviour
    {

        public delegate void TimerOver();

        #region Functions

        /// <summary>
        /// Starts a timer, and calls the callback when its over
        /// </summary>
        /// <param name="time">time before the callback should be called.</param>
        /// <param name="callback">callback to be called when timer is over.</param>
        /// <returns></returns>
        public Coroutine StartTimer(float time, TimerOver callback)
        {
            if (time <= 0)
                Debug.LogError("Timer is set to 0.");

            if (callback == null)
                Debug.LogError("Callback is null");

            return StartCoroutine(_startTimer(time, callback));
        }


        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion


        #region IEnumerators

        private IEnumerator _startTimer(float time, TimerOver callback)
        {
            yield return new WaitForSeconds(time);

            callback();
        }

        #endregion

    }

}

