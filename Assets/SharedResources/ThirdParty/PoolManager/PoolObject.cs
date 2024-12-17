#region Using Statements
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

public class PoolObject : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The prefab name for this object. GameObject name can be changed
    /// at runtime, and the prefab name is stored to facilitate pooling.
    /// </summary>
    public string PrefabName;

    /// <summary>
    /// Called when the object is spawned.
    /// </summary>
    public EventHandler Spawned;
	
	/// <summary>
	/// Called when the object is despawned.
	/// </summary>
	public EventHandler Despawned;
	
	/// <summary>
	/// Is the object currently considered 'spawned'.
	/// </summary>
	private bool m_isSpawned = false;
    private Vector3 m_position;
	public bool IsSpawned
	{
		get { return m_isSpawned; }
		private set { m_isSpawned = value; }
	}
	
	#endregion
	
	#region Methods
	
	/// <summary>
	/// Local initialize.
	/// </summary>
	private void Awake()
    {
        gameObject.SetActive(false);

		if (PrefabName == string.Empty) { PrefabName = name; }
    }
	
	
	/// <summary>
	/// Enable the GameObject and all children.
	/// </summary>
	public void OnSpawn()
	{
		if (IsSpawned)
			return;
		
		IsSpawned = true;
	    m_position = transform.position;

        gameObject.SetActive(true);

        if (Spawned != null)
			Spawned(this, null);

//        SendMessage("OnSpawnCalled");
	}
	
	/// <summary>
	/// Disable the GameObject and all children.
	/// </summary>
	public void OnDespawn()
	{
		if (!IsSpawned)
			return;
		
		IsSpawned = false;
		StopAllCoroutines();
	    transform.position = m_position;
		
		gameObject.SetActive(false);
		
		if (Despawned != null)
			Despawned(this, null);
	}
	
	/// <summary>
	/// Despawn this gameObject.
	/// </summary>
	public void Despawn()
	{
		PoolManager.Despawn(gameObject);
	}
	
	/// <summary>
	/// Despawn in the specified number of seconds.
	/// </summary>
	public void DespawnAfterSeconds(float delay)
	{
		if (!IsSpawned)
			return;
		
		Invoke("Despawn",delay);
	}	
	
	#endregion
}