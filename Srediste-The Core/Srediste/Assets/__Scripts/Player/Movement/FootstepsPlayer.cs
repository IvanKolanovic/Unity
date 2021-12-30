using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsPlayer : MonoBehaviour {
    private SoundManager soundManager;

    private void Start() {
        soundManager = SoundManager.Instance;
    }

    public void PlayFootstepSound() => soundManager.PlayFootStepSound();
}
