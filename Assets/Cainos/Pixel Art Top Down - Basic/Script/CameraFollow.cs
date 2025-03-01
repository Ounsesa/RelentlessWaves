using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        #region Variables
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 m_offset;
        private Vector3 m_targetPos;
        #endregion

        private void Start()
        {
            if (target == null) return;

            m_offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            m_targetPos = target.position + m_offset;
            transform.position = Vector3.Lerp(transform.position, m_targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
