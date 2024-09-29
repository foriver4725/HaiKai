using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IA
{
    #region

    public enum InputType
    {
        /// <summary>
        /// �ynull�z�f�t�H���g�l�B�����Ӗ����Ȃ�
        /// </summary>
        Null,

        /// <summary>
        /// �ybool�z���̃t���[�����A�����ꂽ�u�Ԃ̃t���[���ł��邩
        /// </summary>
        Click,

        /// <summary>
        /// �ybool�z���̃t���[�����A���b�������ꂽ�u�Ԃ̃t���[���ł��邩
        /// </summary>
        Hold,

        /// <summary>
        /// �ybool�z���̃t���[���ɂ�����A������Ă��邩�̃t���O
        /// </summary>
        Value0,

        /// <summary>
        /// �yfloat�z���̃t���[���ɂ�����A1���̓��͂̒l(�P�ʐ� �ȓ�)
        /// </summary>
        Value1,

        /// <summary>
        /// �yVector2�z���̃t���[���ɂ�����A2���̓��͂̒l(�P�ʉ~ �ȓ�)
        /// </summary>
        Value2,

        /// <summary>
        /// �yVector3�z���̃t���[���ɂ�����A3���̓��͂̒l(�P�ʋ� �ȓ�)
        /// </summary>
        Value3
    }

    public sealed class InputInfo : IDisposable
    {
        private InputAction _inputAction;
        private readonly InputType _type;
        private ReadOnlyCollection<Action<InputAction.CallbackContext>> _action;

        public bool Bool { get; private set; } = false;
        public float Float { get; private set; } = 0;
        public Vector2 Vector2 { get; private set; } = Vector2.zero;
        public Vector3 Vector3 { get; private set; } = Vector3.zero;

        public InputInfo(InputAction inputAction, InputType type)
        {
            this._inputAction = inputAction;
            this._type = type;

            this._action = this._type switch
            {
                InputType.Null => null,

                InputType.Click => new List<Action<InputAction.CallbackContext>>()
                {
                    _ => { Bool = true; }
                }
                .AsReadOnly(),

                InputType.Hold => new List<Action<InputAction.CallbackContext>>()
                {
                    _ => { Bool = true; }
                }
                .AsReadOnly(),

                InputType.Value0 => new List<Action<InputAction.CallbackContext>>()
                {
                    _ => { Bool = true; },
                    _ => { Bool = false; }
                }
                .AsReadOnly(),

                InputType.Value1 => new List<Action<InputAction.CallbackContext>>()
                {
                    e => { Float = e.ReadValue<float>(); }
                }
                .AsReadOnly(),

                InputType.Value2 => new List<Action<InputAction.CallbackContext>>()
                {
                    e => { Vector2 = e.ReadValue<Vector2>(); }
                }
                .AsReadOnly(),

                InputType.Value3 => new List<Action<InputAction.CallbackContext>>()
                {
                    e => { Vector3 = e.ReadValue<Vector3>(); }
                }
                .AsReadOnly(),

                _ => null
            };
        }

        public void Dispose()
        {
            _inputAction = null;
            _action = null;
        }

        public void Link(bool isLink)
        {
            if (_inputAction == null) return;
            if (_action == null) return;

            if (isLink)
            {
                switch (_type)
                {
                    case InputType.Null:
                        break;

                    case InputType.Click:
                        _inputAction.performed += _action[0];
                        break;

                    case InputType.Hold:
                        _inputAction.performed += _action[0];
                        break;

                    case InputType.Value0:
                        _inputAction.performed += _action[0];
                        _inputAction.canceled += _action[1];
                        break;

                    case InputType.Value1:
                        _inputAction.started += _action[0];
                        _inputAction.performed += _action[0];
                        _inputAction.canceled += _action[0];
                        break;

                    case InputType.Value2:
                        _inputAction.started += _action[0];
                        _inputAction.performed += _action[0];
                        _inputAction.canceled += _action[0];
                        break;

                    case InputType.Value3:
                        _inputAction.started += _action[0];
                        _inputAction.performed += _action[0];
                        _inputAction.canceled += _action[0];
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (_type)
                {
                    case InputType.Null:
                        break;

                    case InputType.Click:
                        _inputAction.performed -= _action[0];
                        break;

                    case InputType.Hold:
                        _inputAction.performed -= _action[0];
                        break;

                    case InputType.Value0:
                        _inputAction.performed -= _action[0];
                        _inputAction.canceled -= _action[1];
                        break;

                    case InputType.Value1:
                        _inputAction.started -= _action[0];
                        _inputAction.performed -= _action[0];
                        _inputAction.canceled -= _action[0];
                        break;

                    case InputType.Value2:
                        _inputAction.started -= _action[0];
                        _inputAction.performed -= _action[0];
                        _inputAction.canceled -= _action[0];
                        break;

                    case InputType.Value3:
                        _inputAction.started -= _action[0];
                        _inputAction.performed -= _action[0];
                        _inputAction.canceled -= _action[0];
                        break;

                    default:
                        break;
                }
            }
        }

        public void OnLateUpdate()
        {
            if (_type == InputType.Click && Bool) Bool = false;
            else if (_type == InputType.Hold && Bool) Bool = false;
        }
    }

    public static class InputEx
    {
        public static InputInfo Add(this InputInfo inputInfo, List<InputInfo> list)
        {
            list.Add(inputInfo);
            return inputInfo;
        }
    }

    #endregion

    public sealed class InputGetter : MonoBehaviour
    {
        #region

        private IA _ia;
        private List<InputInfo> _inputInfoList;
        public static InputGetter Instance { get; set; } = null;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            _ia = new();
            _inputInfoList = new();

            Init();

            foreach (InputInfo e in _inputInfoList) e.Link(true);
        }
        private void OnDestroy()
        {
            foreach (InputInfo e in _inputInfoList) e.Link(false);

            _ia.Dispose();
            foreach (InputInfo e in _inputInfoList) e.Dispose();

            _ia = null;
            _inputInfoList = null;
        }

        private void OnEnable() => _ia.Enable();
        private void OnDisable() => _ia.Disable();

        private void LateUpdate()
        {
            foreach (InputInfo e in _inputInfoList) e.OnLateUpdate();
        }

        #endregion

        public InputInfo PlayerMove { get; private set; }
        public InputInfo PlayerLook { get; private set; }
        public InputInfo PlayerInteract { get; private set; }
        public InputInfo Submit { get; private set; }
        public InputInfo Cancel { get; private set; }
        public InputInfo Select { get; private set; }

        private void Init()
        {
            PlayerMove = new InputInfo(_ia.Player.Move, InputType.Value2).Add(_inputInfoList);
            PlayerLook = new InputInfo(_ia.Player.Look, InputType.Value2).Add(_inputInfoList);
            PlayerInteract = new InputInfo(_ia.Player.Interact, InputType.Click).Add(_inputInfoList);
            Submit = new InputInfo(_ia.General.Submit, InputType.Click).Add(_inputInfoList);
            Cancel = new InputInfo(_ia.General.Cancel, InputType.Click).Add(_inputInfoList);
            Select = new InputInfo(_ia.General.Select, InputType.Value1).Add(_inputInfoList);
        }
    }
}