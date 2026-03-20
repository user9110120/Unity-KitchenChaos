using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEvents> OnProgressChanged;
    public class OnProgressChangedEvents : EventArgs
    {
        public float progressNormalized;
    }


}
