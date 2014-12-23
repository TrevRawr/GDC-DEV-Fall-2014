/// @file
/// @author Ondrej Mocny http://www.hardwire.cz
/// See LICENSE.txt for license information.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Takes care of the collider mesh data. The mesh is created in a similar way as the fill mesh but is 3d to support
/// MeshCollider. The mesh is stored in a sub-object of the main game object.
public class e2dTerrainColliderMesh : e2dTerrainMesh
{
	// NOTE: any data here are not serialized by Unity.

	/// Default constructor.
	public e2dTerrainColliderMesh(e2dTerrain terrain)
		: base(terrain)
	{
	}

	/// Returns the collider component of the mesh.
    public PolygonCollider2D collider
	{
		get
		{
            EnsureMeshColliderExists(Terrain.gameObject);
			return transform.GetComponent<PolygonCollider2D>();
		}
	}


	/// Rebuilds the mesh from scratch deleting the old one if necessary.
	public void RebuildMesh()
	{
        EnsureMeshColliderExists(Terrain.gameObject);
		// set the result to the mesh
        PolygonCollider2D collider = transform.GetComponent<PolygonCollider2D>();

        Vector2[] tempVectorList = new Vector2[Terrain.FillMesh.GetShapePolygon().Count];
        for (int i = 0; i < tempVectorList.Length; i++)
        {
            tempVectorList[i] = Terrain.FillMesh.GetShapePolygon()[i];
        }
        collider.SetPath(0, tempVectorList);
		ResetMeshObjectsTransforms();
	}

	/// Destroys the mesh data.
	public void DestroyMesh()
	{
		EnsureMeshObjectsExist();

        PolygonCollider2D collider = transform.GetComponent<PolygonCollider2D>();
		if (collider && collider.points.Length > 0)
		{
            collider.points = new Vector2[0];
		}
		// NOTE: can't do that since the objects are used after destroying the mesh
		//Object.DestroyImmediate(collider);
	}
	
}
