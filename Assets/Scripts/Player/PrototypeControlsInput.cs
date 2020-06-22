// GENERATED AUTOMATICALLY FROM 'Assets/Controls/PrototypeControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace TUFG.Prototype.Controls
{
    public class @PrototypeControlsInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PrototypeControlsInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PrototypeControls"",
    ""maps"": [
        {
            ""name"": ""World"",
            ""id"": ""29f2758b-94e5-4630-8e22-7ec7c2bc2e10"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2ee9ffd7-db98-48a5-993f-6b54c288c174"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""d5f9c067-ec80-4382-b5db-9e3a76a0b614"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e9753f3c-efeb-4fb6-9831-80dd0e8e4027"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b6fff8ab-b212-4cff-ad82-e4505049461b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""816a467c-bf34-43a3-8d2a-8dce3ca0001e"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6793f58e-e6a6-4250-848f-78afcea2a1f2"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0bef1a1d-ab3b-4e3e-aa8a-e93938337d03"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // World
            m_World = asset.FindActionMap("World", throwIfNotFound: true);
            m_World_Move = m_World.FindAction("Move", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // World
        private readonly InputActionMap m_World;
        private IWorldActions m_WorldActionsCallbackInterface;
        private readonly InputAction m_World_Move;
        public struct WorldActions
        {
            private @PrototypeControlsInput m_Wrapper;
            public WorldActions(@PrototypeControlsInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_World_Move;
            public InputActionMap Get() { return m_Wrapper.m_World; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(WorldActions set) { return set.Get(); }
            public void SetCallbacks(IWorldActions instance)
            {
                if (m_Wrapper.m_WorldActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                }
                m_Wrapper.m_WorldActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                }
            }
        }
        public WorldActions @World => new WorldActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        public interface IWorldActions
        {
            void OnMove(InputAction.CallbackContext context);
        }
    }
}
