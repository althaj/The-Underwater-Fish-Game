using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all containers.
/// </summary>
public abstract class ContainerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Is the container currently open?
    /// </summary>
    public bool IsOpen { get; protected set; }

    /// <summary>
    /// Open the container.
    /// </summary>
    public abstract void Open();

    /// <summary>
    /// Hide the container.
    /// </summary>
    public abstract void Close();
}
