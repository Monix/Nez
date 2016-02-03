﻿using System;
using System.Collections.Generic;
using System.Collections;


namespace Nez
{
	public class ColliderList : IEnumerable<Collider>, IEnumerable
	{
		/// <summary>
		/// the first collider added is considered the main collider
		/// </summary>
		/// <value>The main collider.</value>
		public Collider mainCollider
		{
			get
			{
				if( _colliders.Count == 0 )
					return null;
				return _colliders[0];
			}
		}

		Entity _entity;
		List<Collider> _colliders = new List<Collider>();


		public ColliderList( Entity entity )
		{
			this._entity = entity;
		}


		/// <summary>
		/// adds a Collider to the list and registers it with the Physics system
		/// </summary>
		/// <param name="collider">Collider.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T add<T>( T collider ) where T : Collider
		{
			collider.entity = _entity;
			collider.registerColliderWithPhysicsSystem();
			_colliders.Add( collider );
			return collider;
		}


		/// <summary>
		/// removes the Collider and unregisters it from the Pysics system
		/// </summary>
		/// <param name="collider">Collider.</param>
		public void remove( Collider collider )
		{
			Assert.isTrue( _colliders.Contains( collider ), "Collider {0} is not in the ColliderList", collider );
			removeAt( _colliders.IndexOf( collider ) );
		}


		/// <summary>
		/// removes the Collider and unregisters it from the Pysics system
		/// </summary>
		/// <param name="index">Index.</param>
		public void removeAt( int index )
		{
			var collider = _colliders[index];
			collider.unregisterColliderWithPhysicsSystem();
			collider.entity = null;
			_colliders.RemoveAt( index );
		}


		internal void onEntityAddedToScene()
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].onEntityAddedToScene();
		}


		internal void onEntityRemovedFromScene()
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].onEntityRemovedFromScene();
		}


		internal void onEntityPositionChanged()
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].onEntityPositionChanged();
		}


		internal void onEntityEnabled()
		{
			registerAllCollidersWithPhysicsSystem();
		}


		internal void onEntityDisabled()
		{
			unregisterAllCollidersWithPhysicsSystem();
		}


		internal void debugRender( Graphics graphics )
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].debugRender( graphics );
		}


		public void registerAllCollidersWithPhysicsSystem()
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].registerColliderWithPhysicsSystem();
		}


		public void unregisterAllCollidersWithPhysicsSystem()
		{
			for( var i = 0; i < _colliders.Count; i++ )
				_colliders[i].unregisterColliderWithPhysicsSystem();
		}


		public int Count
		{
			get { return _colliders.Count; }
		}


		#region IEnumerable and array access

		public Collider this[int index]
		{
			get
			{
				return _colliders[index];
			}
		}


		public IEnumerator<Collider> GetEnumerator()
		{
			return _colliders.GetEnumerator();
		}


		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _colliders.GetEnumerator();
		}

		#endregion

	}
}
