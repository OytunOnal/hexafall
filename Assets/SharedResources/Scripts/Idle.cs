
using UnityEngine;

    public class Idle : MonoBehaviour
    {
        private Animator m_Animator;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void NewIdleState()
        {
            m_Animator.SetInteger("Idle", Random.Range(0, 3));
        }
    }
