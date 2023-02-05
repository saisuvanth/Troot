// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Coherence.Toolkit;
	using Coherence.Toolkit.Bindings;
	using Coherence.Entity;
	using Coherence.ProtocolDef;
	using Coherence.Brook;
	using Coherence.Toolkit.Bindings.ValueBindings;
	using Coherence.Toolkit.Bindings.TransformBindings;
	using Coherence.Connection;
	using Coherence.Log;
	using Logger = Coherence.Log.Logger;
	using UnityEngine.Scripting;

	public class Binding_979025f5a787e6a43b86dc71f2bf290b_489fe2bd_4883_45bc_aa1a_c8529eb7a3f2 : ReferenceBinding
	{
		private GameManager CastedUnityComponent;		

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (GameManager)UnityComponent;
		}
		public override string CoherenceComponentName => "GameManager__char_32_1_GameManager_5598769809711555312";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override SerializeEntityID Value
		{
			get => (SerializeEntityID)coherenceSync.MonoBridge.UnityObjectToEntityId(CastedUnityComponent.Ground);
			set => CastedUnityComponent.Ground = coherenceSync.MonoBridge.EntityIdToGameObject(value);
		}

		protected override SerializeEntityID ReadComponentData(ICoherenceComponentData coherenceComponent)
		{
			var update = (GameManager__char_32_1_GameManager_5598769809711555312)coherenceComponent;
			return update.Ground;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent)
		{
			var update = (GameManager__char_32_1_GameManager_5598769809711555312)coherenceComponent;
			update.Ground = Value;
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new GameManager__char_32_1_GameManager_5598769809711555312();
		}
	}


	[Preserve]
	[AddComponentMenu("coherence/Baked/Baked 'GameManager 1' (auto assigned)")]
	[RequireComponent(typeof(CoherenceSync))]
	public class CoherenceSyncGameManager__char_32_1 : CoherenceSyncBaked
	{
		private CoherenceSync coherenceSync;
		private Logger logger;

		// Cached targets for commands

		private IClient client;
		private CoherenceMonoBridge monoBridge => coherenceSync.MonoBridge;

		protected void Awake()
		{
			coherenceSync = GetComponent<CoherenceSync>();
			coherenceSync.usingReflection = false;

			logger = coherenceSync.logger.With<CoherenceSyncGameManager__char_32_1>();
			if (coherenceSync.TryGetBindingByGuid("489fe2bd-4883-45bc-aa1a-c8529eb7a3f2", "Ground", out Binding InternalGameManager__char_32_1_GameManager_5598769809711555312_GameManager__char_32_1_GameManager_5598769809711555312_Ground))
			{
				var clone = new Binding_979025f5a787e6a43b86dc71f2bf290b_489fe2bd_4883_45bc_aa1a_c8529eb7a3f2();
				InternalGameManager__char_32_1_GameManager_5598769809711555312_GameManager__char_32_1_GameManager_5598769809711555312_Ground.CloneTo(clone);
				coherenceSync.Bindings[coherenceSync.Bindings.IndexOf(InternalGameManager__char_32_1_GameManager_5598769809711555312_GameManager__char_32_1_GameManager_5598769809711555312_Ground)] = clone;
			}
			else
			{
				logger.Error("Couldn't find binding (GameManager).Ground");
			}
		}

		public override List<ICoherenceComponentData> CreateEntity()
		{
			if (coherenceSync.UsesLODsAtRuntime && (Archetypes.IndexForName.TryGetValue(coherenceSync.Archetype.ArchetypeName, out int archetypeIndex)))
			{
				var components = new List<ICoherenceComponentData>()
				{
					new ArchetypeComponent
					{
						index = archetypeIndex
					}
				};

				return components;
			}
			else
			{
				logger.Warning($"Unable to find archetype {coherenceSync.Archetype.ArchetypeName} in dictionary. Please, bake manually (coherence > Bake)");
			}

			return null;
		}

		public override void Initialize(CoherenceSync sync, IClient client)
		{
			if (coherenceSync == null)
			{
				coherenceSync = sync;
			}
			this.client = client;
		}

		public override void ReceiveCommand(IEntityCommand command)
		{
			switch(command)
			{
				default:
					logger.Warning($"[CoherenceSyncGameManager__char_32_1] Unhandled command: {command.GetType()}.");
					break;
			}
		}
	}
}
