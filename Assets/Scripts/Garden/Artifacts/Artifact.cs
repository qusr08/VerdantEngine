using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.WSA; - idk what this is but it's causing a build error

public enum ArtifactType {
    WHEELBARROW,
    COMPOST,
    FLAMINGO,
    NONE
}
public enum ArtifactAbilityType
{
    OnHeal,
    OnRoundStart,
    OnDestroy,
    OnRoundEnd,
    OnPlacePlant,
    Passive
}

/// <summary>
/// This class holds all data for each artifact that is placed in the garden
/// </summary>
public abstract class Artifact : GardenPlaceable {
	[Header("Artifact")]
	[SerializeField] private ArtifactType _artifactType;
    [SerializeField] private ArtifactAbilityType _artifactAbilityType;

    /// <summary>
    /// The type of this artifact
    /// </summary>
    public ArtifactType ArtifactType => _artifactType;
    public ArtifactAbilityType ArtifactAbilityType => _artifactAbilityType;

    public override void OnKilled ( ) {
		base.OnKilled( );
		if(_artifactAbilityType == ArtifactAbilityType.Passive)
		{
			DeactivateAction();
        }
		gardenManager.UprootArtifact(this);
	}
	//when incoming valueis not needed
	public virtual void ActivateAction ()
	{

	}
	//When incoming value is needed
    public virtual void ActivateAction(int value)
    {

    }
	public virtual void DeactivateAction() { }
}
