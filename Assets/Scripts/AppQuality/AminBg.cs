using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICouldGames
{
    public class AminBg : MonoBehaviour
    {
        private Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        
        }
        public void Play()
        {
            anim.SetTrigger("State");
        }

       
    }
}
